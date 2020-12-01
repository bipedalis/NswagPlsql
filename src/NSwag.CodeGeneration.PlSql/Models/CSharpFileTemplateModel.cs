//-----------------------------------------------------------------------
// <copyright file="PlSqlFileTemplateModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.PlSql;

namespace NSwag.CodeGeneration.PlSql.Models
{
    /// <summary>The PlSql file template model.</summary>
    public class PlSqlFileTemplateModel
    {
        private readonly string _clientCode;
        private readonly string _interfaceCode;
        private readonly OpenApiDocument _document;
        private readonly PlSqlGeneratorBaseSettings _settings;
        private readonly PlSqlTypeResolver _resolver;
        private readonly ClientGeneratorOutputType _outputType;
        private readonly PlSqlGeneratorBase _generator;
        private readonly string _baseUrl;

        /// <summary>Initializes a new instance of the <see cref="PlSqlFileTemplateModel" /> class.</summary>
        /// <param name="clientTypes">The client types.</param>
        /// <param name="dtoTypes">The DTO types.</param>
        /// <param name="outputType">Type of the output.</param>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The client generator base.</param>
        /// <param name="resolver">The resolver.</param>
        public PlSqlFileTemplateModel(
            IEnumerable<CodeArtifact> clientTypes,
            IEnumerable<CodeArtifact> dtoTypes,
            ClientGeneratorOutputType outputType,
            OpenApiDocument document,
            PlSqlGeneratorBaseSettings settings,
            PlSqlGeneratorBase generator,
            PlSqlTypeResolver resolver)
        {
            _outputType = outputType;
            _document = document;
            _generator = generator;
            _settings = settings;
            _resolver = resolver;
            var toJsonFunctions = dtoTypes.Where(c => c.Type == CodeArtifactType.Function).ToList();
            var circ1= SortByDependencies(toJsonFunctions);
            _clientCode = toJsonFunctions.Concat( clientTypes.Where(c => c.Category == CodeArtifactCategory.Client))
                .Concatenate();
            _interfaceCode = clientTypes.Where(c => c.Category == CodeArtifactCategory.Contract)
                //.Concat(dtoTypes.Where(c => c.Type == CodeArtifactType.Interface))
                // hack methods to convert create request to update request
                .Concat(dtoTypes.Where(c => c.Type == CodeArtifactType.Interface 
                && (c.TypeName.StartsWith("Create") || c.TypeName.StartsWith("Update")) &&
                !c.TypeName.EndsWith("T")))
                .Concatenate();
            var dto = dtoTypes.Where(c => c.Type != CodeArtifactType.Interface && c.Type != CodeArtifactType.Function).OrderByBaseDependency().ToList();
            var circ2 = SortByDependencies(dto);
            Classes = dto.Concatenate();
            _baseUrl = _document.BaseUrl;
        }

        private static IList<string> SortByDependencies(List<CodeArtifact> dto)
        {
            var i = dto.Count - 1;
            int lastJ =-1;
            IList<string> circular = new List<string>();
            do
            {
                var j = dto.FindIndex(a => a.Code.Contains(" " + dto[i].TypeName + " ") || a.Code.Contains(" " + dto[i].TypeName + "T "));

                if (j >= 0 && j < i)
                {
                    if (lastJ == j)
                    {
                        //throw new System.Exception("Recursive models");
                        if (dto[j].Code.Contains(" " + dto[i].TypeName + "T "))
                        {
                            var t = dto[j];
                            dto[j] = dto[i];
                            dto[i] = t;
                            //circular.Add(dto[j].TypeName);
                        }
                        circular.Add(dto[i].TypeName);
                        dto[j] = new CodeArtifact(dto[j].TypeName,dto[j].Type,dto[j].Language,
                            dto[j].Category, dto[j].Code.Replace(" " + dto[i].TypeName + " ", " nclob "));
                        //i--;
                    }
                    else
                    {
                        var t = dto[j];
                        dto[j] = dto[i];
                        dto[i] = t;
                        lastJ = j;
                    }
                }
                else
                {
                    i--;
                    lastJ = -1;
                }
            } while (i > 0);
            return circular;
        }

        /// <summary>Gets the namespace.</summary>
        public string Namespace => _settings.PlSqlGeneratorSettings.Namespace ?? string.Empty;

        /// <summary>Gets the all the namespace usages.</summary>
        public string[] NamespaceUsages => (_outputType == ClientGeneratorOutputType.Contracts ?
            _settings.AdditionalContractNamespaceUsages?.Where(n => n != null).ToArray() :
            _settings.AdditionalNamespaceUsages?.Where(n => n != null).ToArray()) ?? new string[] { };

        /// <summary>Gets a value indicating whether the C#8 nullable reference types are enabled for this file.</summary>
        public bool GenerateNullableReferenceTypes => _settings.PlSqlGeneratorSettings.GenerateNullableReferenceTypes;

        /// <summary>Gets a value indicating whether to generate contract code.</summary>
        public bool GenerateContracts =>
            _outputType == ClientGeneratorOutputType.Full ||
            _outputType == ClientGeneratorOutputType.Contracts;

        /// <summary>Gets a value indicating whether to generate implementation code.</summary>
        public bool GenerateImplementation =>
            _outputType == ClientGeneratorOutputType.Full ||
            _outputType == ClientGeneratorOutputType.Implementation;

        /// <summary>Gets or sets a value indicating whether to generate client types.</summary>
        public bool GenerateClientClasses => _settings.GenerateClientClasses;

        /// <summary>Gets the clients code.</summary>
        public string Clients => _settings.GenerateClientClasses ? _clientCode : string.Empty;


        /// <summary>Gets the package body.</summary>
        public string Interfaces => _settings.GenerateClientClasses ? _interfaceCode : string.Empty;


        /// <summary>Gets the classes code.</summary>
        public string Classes { get; }

        /// <summary>Gets the service base URL.</summary>
        public string BaseUrl => _baseUrl;

        /// <summary>Gets a value indicating whether the generated code requires a JSON exception converter.</summary>
        public bool RequiresJsonExceptionConverter => JsonExceptionTypes.Any();

        /// <summary>Gets the exception model class.</summary>
        public string ExceptionModelClass => JsonExceptionTypes.FirstOrDefault(t => t != "Exception") ?? "Exception";

        private IEnumerable<string> JsonExceptionTypes => _document.Operations
            .SelectMany(o => o.Operation.ActualResponses.Where(r => r.Value.Schema?.InheritsSchema(_resolver.ExceptionSchema) == true).Select(r => new { o.Operation, Response = r.Value }))
            .Select(t => _generator.GetTypeName(t.Response.Schema, t.Response.IsNullable(_settings.PlSqlGeneratorSettings.SchemaType), "Response"));

        /// <summary>Gets a value indicating whether the generated code requires the FileParameter type.</summary>
        public bool RequiresFileParameterType =>
            _settings.PlSqlGeneratorSettings.ExcludedTypeNames?.Contains("FileParameter") != true &&
            (_document.Operations.Any(o => o.Operation.ActualParameters.Any(p => p.ActualTypeSchema.IsBinary)) ||
             _document.Operations.Any(o => o.Operation?.RequestBody?.Content?.Any(c => c.Value.Schema?.IsBinary == true ||
                                                                                       c.Value.Schema?.ActualProperties.Any(p => p.Value.IsBinary ||
                                                                                                                                 p.Value.Item?.IsBinary == true ||
                                                                                                                                 p.Value.Items.Any(i => i.IsBinary)
                                                                                                                                 ) == true) == true));

        /// <summary>Gets a value indicating whether [generate file response class].</summary>
        public bool GenerateFileResponseClass =>
            _settings.PlSqlGeneratorSettings.ExcludedTypeNames?.Contains("FileResponse") != true &&
            _document.Operations.Any(o => o.Operation.ActualResponses.Any(r => r.Value.IsBinary(o.Operation) == true));

        /// <summary>Gets or sets a value indicating whether to generate exception classes (default: true).</summary>
        public bool GenerateExceptionClasses => (_settings as PlSqlClientGeneratorSettings)?.GenerateExceptionClasses == true;

        /// <summary>Gets or sets a value indicating whether to wrap success responses to allow full response access.</summary>
        public bool WrapResponses => _settings.WrapResponses;

        /// <summary>Gets or sets a value indicating whether to generate the response class (only applied when WrapResponses == true, default: true).</summary>
        public bool GenerateResponseClasses => _settings.GenerateResponseClasses;

        /// <summary>Gets the response class names.</summary>
        public IEnumerable<string> ResponseClassNames
        {
            get
            {
                if (_settings.OperationNameGenerator.SupportsMultipleClients)
                {
                    return _document.Operations
                        .GroupBy(o => _settings.OperationNameGenerator.GetClientName(_document, o.Path, o.Method, o.Operation))
                        .Select(g => _settings.ResponseClass.Replace("{controller}", g.Key))
                        .Where(a => _settings.PlSqlGeneratorSettings.ExcludedTypeNames?.Contains(a) != true)
                        .Distinct();
                }

                return new[] { _settings.ResponseClass.Replace("{controller}", string.Empty) };
            }
        }

        /// <summary>Gets the exception class names.</summary>
        public IEnumerable<string> ExceptionClassNames
        {
            get
            {
                var settings = _settings as PlSqlClientGeneratorSettings;
                if (settings != null)
                {
                    if (settings.OperationNameGenerator.SupportsMultipleClients)
                    {
                        return _document.Operations
                            .GroupBy(o => settings.OperationNameGenerator.GetClientName(_document, o.Path, o.Method, o.Operation))
                            .Select(g => settings.ExceptionClass.Replace("{controller}", g.Key))
                            .Where(a => _settings.PlSqlGeneratorSettings.ExcludedTypeNames?.Contains(a) != true)
                            .Distinct();
                    }
                    else
                    {
                        return new[] { settings.ExceptionClass.Replace("{controller}", string.Empty) };
                    }
                }
                return new string[] { };
            }
        }
    }
}