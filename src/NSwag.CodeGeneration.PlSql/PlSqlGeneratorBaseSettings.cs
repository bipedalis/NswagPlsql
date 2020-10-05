//-----------------------------------------------------------------------
// <copyright file="SwaggerToPlSqlGeneratorSettings.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.PlSql;
using System.Reflection;

namespace NSwag.CodeGeneration.PlSql
{
    /// <summary>Settings for the <see cref="PlSqlGeneratorBase"/>.</summary>
    public abstract class PlSqlGeneratorBaseSettings : ClientGeneratorBaseSettings
    {
        /// <summary>Initializes a new instance of the <see cref="PlSqlClientGeneratorSettings"/> class.</summary>
        protected PlSqlGeneratorBaseSettings()
        {
            PlSqlGeneratorSettings = new PlSqlGeneratorSettings
            {
                Namespace = "MyNamespace",
                SchemaType = SchemaType.Swagger2
            };

            PlSqlGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(PlSqlGeneratorSettings, new[]
            {
                typeof(PlSqlGeneratorSettings).GetTypeInfo().Assembly,
                typeof(PlSqlGeneratorBaseSettings).GetTypeInfo().Assembly,
            });

            ResponseArrayType = "System.Collections.Generic.ICollection";
            ResponseDictionaryType = "System.Collections.Generic.IDictionary";

            ParameterArrayType = "System.Collections.Generic.IEnumerable";
            ParameterDictionaryType = "System.Collections.Generic.IDictionary";

            AdditionalNamespaceUsages = new string[0];
            AdditionalContractNamespaceUsages = new string[0];
        }

        /// <summary>Gets the PlSql generator settings.</summary>
        public PlSqlGeneratorSettings PlSqlGeneratorSettings { get; }

        /// <summary>Gets the code generator settings.</summary>
        [JsonIgnore]
        public override CodeGeneratorSettingsBase CodeGeneratorSettings => PlSqlGeneratorSettings;

        /// <summary>Gets or sets the additional namespace usages.</summary>
        public string[] AdditionalNamespaceUsages { get; set; }

        /// <summary>Gets or sets the additional contract namespace usages.</summary>
        public string[] AdditionalContractNamespaceUsages { get; set; }

        /// <summary>Gets or sets the array type of operation responses (i.e. the method return type).</summary>
        public string ResponseArrayType { get; set; }

        /// <summary>Gets or sets the dictionary type of operation responses (i.e. the method return type).</summary>
        public string ResponseDictionaryType { get; set; }

        /// <summary>Gets or sets the array type of operation parameters.</summary>
        public string ParameterArrayType { get; set; }

        /// <summary>Gets or sets the dictionary type of operation parameters.</summary>
        public string ParameterDictionaryType { get; set; }
    }
}