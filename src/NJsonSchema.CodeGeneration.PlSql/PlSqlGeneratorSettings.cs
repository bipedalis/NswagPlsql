//-----------------------------------------------------------------------
// <copyright file="CSharpGeneratorSettings.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Reflection;

namespace NJsonSchema.CodeGeneration.PlSql
{
    /// <summary>The generator settings.</summary>
    public class PlSqlGeneratorSettings : CodeGeneratorSettingsBase
    {
        /// <summary>Initializes a new instance of the <see cref="PlSqlGeneratorSettings"/> class.</summary>
        public PlSqlGeneratorSettings()
        {
            AnyType = "clob";
            Namespace = "rest_api_client_pck";
            DateType = "Date";
            DateTimeType = "Timestamp";
            TimeType = "Timestamp";
            TimeSpanType = "number";
            // ArrayType = "System.Collections.Generic.ICollection";
            GenerateNullableReferenceTypes = true;
            ValueGenerator = new PlSqlValueGenerator(this);
            PropertyNameGenerator = new PlSqlPropertyNameGenerator();
            TemplateFactory = new DefaultTemplateFactory(this, new Assembly[]
{
                typeof(PlSqlGeneratorSettings).GetTypeInfo().Assembly
});
            TypeNameGenerator = new OracleTypeNameGenerator();
        }
        /// <summary>Gets or sets the package name.</summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the api base url
        /// </summary>
        public string BaseUrl { get; set; }


        /// <summary>Gets or sets the any type (default: "object").</summary>
        public string AnyType { get; set; }

        /// <summary>Gets or sets the date .NET type (default: 'DateTimeOffset').</summary>
        public string DateType { get; set; }

        /// <summary>Gets or sets the date time .NET type (default: 'DateTimeOffset').</summary>
        public string DateTimeType { get; set; }

        /// <summary>Gets or sets the time .NET type (default: 'TimeSpan').</summary>
        public string TimeType { get; set; }

        /// <summary>Gets or sets the time span .NET type (default: 'TimeSpan').</summary>
        public string TimeSpanType { get; set; }

        /// <summary>Gets or sets the generic array .NET type (default: 'ICollection').</summary>
        public string ArrayType { get; set; }
        /// <summary>Gets or sets the generic dictionary .NET type (default: 'IDictionary').</summary>
        public string DictionaryType { get; set; }


        /// <summary>Gets or sets a value indicating whether to generate Nullable Reference Type annotations (default: false).</summary>
        public bool GenerateNullableReferenceTypes { get; set; }
        /// <summary>Gets or sets a value indicating whether to render ToJson() and FromJson() methods (default: true).</summary>
        public bool GenerateJsonMethods { get; set; }
    }
}
