//-----------------------------------------------------------------------
// <copyright file="PlSqlGenerator.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration.PlSql.Models;
using NJsonSchema.CodeGeneration.Models;

namespace NJsonSchema.CodeGeneration.PlSql
{
    /// <summary>The PlSql code generator.</summary>
    public class PlSqlGenerator : GeneratorBase
    {
        private readonly PlSqlTypeResolver _resolver;

        /// <summary>Initializes a new instance of the <see cref="PlSqlGenerator"/> class.</summary>
        /// <param name="rootObject">The root object to search for all JSON Schemas.</param>
        public PlSqlGenerator(object rootObject)
            : this(rootObject, new PlSqlGeneratorSettings())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PlSqlGenerator"/> class.</summary>
        /// <param name="rootObject">The root object to search for all JSON Schemas.</param>
        /// <param name="settings">The generator settings.</param>
        public PlSqlGenerator(object rootObject, PlSqlGeneratorSettings settings)
            : this(rootObject, settings, new PlSqlTypeResolver(settings))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PlSqlGenerator"/> class.</summary>
        /// <param name="rootObject">The root object to search for all JSON Schemas.</param>
        /// <param name="settings">The generator settings.</param>
        /// <param name="resolver">The resolver.</param>
        public PlSqlGenerator(object rootObject, PlSqlGeneratorSettings settings, PlSqlTypeResolver resolver)
            : base(rootObject, resolver, settings)
        {
            _resolver = resolver;
            Settings = settings;
        }

        /// <summary>Gets the generator settings.</summary>
        public PlSqlGeneratorSettings Settings { get; }

        /// <inheritdoc />
        public override IEnumerable<CodeArtifact> GenerateTypes()
        {
            var baseArtifacts = base.GenerateTypes();
            var artifacts = new List<CodeArtifact>();

            if (baseArtifacts.Any(r => r.Code.Contains("JsonInheritanceConverter")))
            {
                if (Settings.ExcludedTypeNames?.Contains("JsonInheritanceAttribute") != true)
                {
                    var template = Settings.TemplateFactory.CreateTemplate("PlSql", "JsonInheritanceAttribute", new TemplateModelBase());
                    artifacts.Add(new CodeArtifact("JsonInheritanceAttribute", CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Utility, template));
                }

                if (Settings.ExcludedTypeNames?.Contains("JsonInheritanceConverter") != true)
                {
                    var template = Settings.TemplateFactory.CreateTemplate("PlSql", "JsonInheritanceConverter", new TemplateModelBase());
                    artifacts.Add(new CodeArtifact("JsonInheritanceConverter", CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Utility, template));
                }
            }

            if (baseArtifacts.Any(r => r.Code.Contains("DateFormatConverter")))
            {
                if (Settings.ExcludedTypeNames?.Contains("DateFormatConverter") != true)
                {
                    var template = Settings.TemplateFactory.CreateTemplate("PlSql", "DateFormatConverter", new TemplateModelBase());
                    artifacts.Add(new CodeArtifact("DateFormatConverter", CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Utility, template));
                }
            }
            if (baseArtifacts.Any(r => r.Type==CodeArtifactType.Class))
            {
                artifacts.AddRange(GenerateConvertFunctions());               

            }
            List<CodeArtifact> filtered = new List<CodeArtifact>();
            foreach (var art in baseArtifacts.Concat(artifacts))
            {
                string code = art.Code;
                foreach (string t in Settings.ExcludedTypeNames)
                {
                    if (code.Contains(" " + t + " "))
                    {
                        code = code.Replace(" " + t + " ", " nclob ");
                    }
                    if (code.Contains(" " + t + "T "))
                    {
                        code = code.Replace(" " + t + "T ", " nclobT ");
                    }
                }
                if(art.Type != CodeArtifactType.Interface && art.Type != CodeArtifactType.Function
                    && (code.Contains(" " + art.TypeName + "T ,") || code.Contains(" " + art.TypeName + "T  ")))
                {
                    code = code.Replace(" " + art.TypeName + "T ,", " nclobT ,");
                    code = code.Replace(" " + art.TypeName + "T  ", " nclobT  ");
                }
                if (code != art.Code)
                {
                    filtered.Add(CloneCodeArtifact(art, code));
                }
                else
                {
                    filtered.Add(art);
                }
            }
            return filtered;

        }
        private CodeArtifact CloneCodeArtifact(CodeArtifact old, string newCode)
        {
            return new CodeArtifact(old.TypeName, old.Type, old.Language,
                old.Category, newCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  IEnumerable<CodeArtifact> GenerateConvertFunctions()
        {
            var processedTypes = new List<string>();
            var types = new Dictionary<string, CodeArtifact>();
            while (_resolver.Types.Any(t => !processedTypes.Contains(t.Value)))
            {
                foreach (var pair in _resolver.Types.ToList())
                {
                    processedTypes.Add(pair.Value);
                    var result = GenerateConvertFunctionsDeclarations(pair.Key, pair.Value);
                    types[result.TypeName] = result;
                    var result2 = GenerateConvertFunctions(pair.Key, pair.Value);
                    types[result.TypeName+ "impl"] = result2;
                }
            }

            var artifacts = types.Values
                .Where(p => !Settings.ExcludedTypeNames.Contains(p.TypeName));
            return artifacts;
        }
        /// <inheritdoc />
        protected override string GenerateFile(IEnumerable<CodeArtifact> artifactCollection)
        {
            var model = new FileTemplateModel
            {
                Namespace = Settings.Namespace ?? string.Empty,
                GenerateNullableReferenceTypes = Settings.GenerateNullableReferenceTypes,
                TypesCode = artifactCollection.Concatenate()
            };

            var template = Settings.TemplateFactory.CreateTemplate("PlSql", "File", model);
            return ConversionUtilities.TrimWhiteSpaces(template.Render());
        }

        /// <summary>Generates the type.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="typeNameHint">The type name hint.</param>
        /// <returns>The code.</returns>
        protected override CodeArtifact GenerateType(JsonSchema schema, string typeNameHint)
        {
            var typeName = _resolver.GetOrGenerateTypeName(schema, typeNameHint);
            if(typeName.Length >= 30)
            {
                typeName = typeName.Substring(0, 29);
            }
            if (schema.IsEnumeration)
            {
                return GenerateEnum(schema, typeName);
            }
            else
            {
                return GenerateRecord(schema, typeName);
                //return GenerateClass(schema, typeName);
            }
        }

        private CodeArtifact GenerateClass(JsonSchema schema, string typeName)
        {
            var model = new ClassTemplateModel(typeName, Settings, _resolver, schema, RootObject);

            RenamePropertyWithSameNameAsClass(typeName, model.Properties);

            var template = Settings.TemplateFactory.CreateTemplate("PlSql", "Class", model);
            return new CodeArtifact(typeName, model.BaseClassName, CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Contract, template);
        }
        private CodeArtifact GenerateRecord(JsonSchema schema, string typeName)
        {
            var model = new ClassTemplateModel(typeName, Settings, _resolver, schema, RootObject);

            RenamePropertyWithSameNameAsClass(typeName, model.Properties);

            var template = Settings.TemplateFactory.CreateTemplate("PlSql", "Record", model);
            return new CodeArtifact(typeName, model.BaseClassName, CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Contract, template);
        }
        private CodeArtifact GenerateConvertFunctions(JsonSchema schema, string typeName)
        {
            var model = new ClassTemplateModel(typeName, Settings, _resolver, schema, RootObject);

            RenamePropertyWithSameNameAsClass(typeName, model.Properties);

            var template = Settings.TemplateFactory.CreateTemplate("PlSql", "Record.Convert", model);
            return new CodeArtifact(typeName, model.BaseClassName, CodeArtifactType.Function, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Contract, template);
        }
        private CodeArtifact GenerateConvertFunctionsDeclarations(JsonSchema schema, string typeName)
        {
            var model = new ClassTemplateModel(typeName, Settings, _resolver, schema, RootObject);

            RenamePropertyWithSameNameAsClass(typeName, model.Properties);

            var template = Settings.TemplateFactory.CreateTemplate("PlSql", "Record.Convert.decl", model);
            return new CodeArtifact(typeName, model.BaseClassName, CodeArtifactType.Interface, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Contract, template);
        }

        private void RenamePropertyWithSameNameAsClass(string typeName, IEnumerable<PropertyModel> properties)
        {
            var propertyWithSameNameAsClass = properties.SingleOrDefault(p => p.PropertyName == typeName);
            if (propertyWithSameNameAsClass != null)
            {
                var number = 1;
                while (properties.Any(p => p.PropertyName == typeName + number))
                {
                    number++;
                }

                propertyWithSameNameAsClass.PropertyName = propertyWithSameNameAsClass.PropertyName + number;
            }
        }

        private CodeArtifact GenerateEnum(JsonSchema schema, string typeName)
        {
            var model = new EnumTemplateModel(typeName, schema, Settings);
            var template = Settings.TemplateFactory.CreateTemplate("PlSql", "Enum", model);
            return new CodeArtifact(typeName, CodeArtifactType.Enum, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Contract, template);
        }
    }
}
