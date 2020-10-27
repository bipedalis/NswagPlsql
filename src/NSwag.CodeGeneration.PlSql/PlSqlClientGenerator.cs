//-----------------------------------------------------------------------
// <copyright file="SwaggerToPlSqlClientGenerator.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.PlSql;
using NSwag.CodeGeneration.PlSql.Models;

namespace NSwag.CodeGeneration.PlSql
{
    /// <summary>Generates the PlSql service client code. </summary>
    public class PlSqlClientGenerator : PlSqlGeneratorBase
    {
        private readonly  OpenApiDocument _document;
        private List<string> _ops = new List<string>();
        /// <summary>Initializes a new instance of the <see cref="PlSqlClientGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public PlSqlClientGenerator(OpenApiDocument document, PlSqlClientGeneratorSettings settings)
            : this(document, settings, CreateResolverWithExceptionSchema(settings.PlSqlGeneratorSettings, document))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PlSqlClientGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public PlSqlClientGenerator(OpenApiDocument document, PlSqlClientGeneratorSettings settings, PlSqlTypeResolver resolver)
            : base(document, settings, resolver)
        {
            Settings = settings;
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <summary>Gets or sets the generator settings.</summary>
        public PlSqlClientGeneratorSettings Settings { get; }

        /// <summary>Gets the base settings.</summary>
        public override ClientGeneratorBaseSettings BaseSettings => Settings;

        /// <summary>Generates the client class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller class.</param>
        /// <param name="operations">The operations.</param>
        /// <returns>The code.</returns>
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string controllerName, string controllerClassName, IEnumerable<PlSqlOperationModel> operations)
        {
            var exceptionSchema = (Resolver as PlSqlTypeResolver)?.ExceptionSchema;

            var model = new PlSqlClientTemplateModel(controllerName, controllerClassName, operations, exceptionSchema, _document, Settings);
            if (model.HasOperations)
            {
                var ops = model.Operations.Select(o => new Tuple<PlSqlOperationModel, string>(o, o.ActualOperationName + o.Parameters.Select(p => p.Type).Aggregate((p1, p2) => p1 + p2)));
                foreach(var op in ops)
                {
                    if(_ops.Contains(op.Item2))
                    {
                        op.Item1.OperationName = op.Item1.OperationName + "2";
                    }
                    _ops.Add(op.Item2);
                }
                
                if (model.GenerateClientInterfaces)
                {
                    var interfaceTemplate = Settings.PlSqlGeneratorSettings.TemplateFactory.CreateTemplate("PlSql", "Client.Interface", model);
                    yield return new CodeArtifact(model.Class, CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Contract, interfaceTemplate);
                }

                var classTemplate = Settings.PlSqlGeneratorSettings.TemplateFactory.CreateTemplate("PlSql", "Client.Class", model);
                yield return new CodeArtifact(model.Class, CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Client, classTemplate);
            }
        }

        /// <summary>Creates an operation model.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The operation model.</returns>
        protected override PlSqlOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings)
        {
            return new PlSqlOperationModel(operation, (PlSqlGeneratorBaseSettings)settings, this, (PlSqlTypeResolver)Resolver);
        }
    }
}
