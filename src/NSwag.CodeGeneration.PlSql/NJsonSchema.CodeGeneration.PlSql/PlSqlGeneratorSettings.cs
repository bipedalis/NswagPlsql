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
        public PlSqlGeneratorSettings()
        {
            AnyType = "object";
            Namespace = "MyNamespace";
            DateType = "Date";
            DateTimeType = "Timestamp";
            TimeType = "Timestamp";
            TimeSpanType = "number";
            //ArrayType = "System.Collections.Generic.ICollection";
            GenerateNullableReferenceTypes = true;
            PropertyNameGenerator = new CSharp.CSharpPropertyNameGenerator();

        }
        /// <summary>Gets or sets the .NET namespace of the generated types (default: MyNamespace).</summary>
        public string Namespace { get; set; }


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

        /// <summary>Gets or sets a value indicating whether to generate Nullable Reference Type annotations (default: false).</summary>
        public bool GenerateNullableReferenceTypes { get; set; }
        public bool GenerateJsonMethods { get; set; }
    }
}
