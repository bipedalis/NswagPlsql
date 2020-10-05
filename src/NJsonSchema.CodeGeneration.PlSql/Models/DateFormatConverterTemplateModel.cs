//-----------------------------------------------------------------------
// <copyright file="DateFormatConverterTemplateModel.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Linq;

namespace NJsonSchema.CodeGeneration.PlSql.Models
{
    /// <summary>The DateFormatConverterTemplateModel.</summary>
    public class DateFormatConverterTemplateModel
    {
        private readonly PlSqlGeneratorSettings _settings;

        /// <summary>The DateFormatConverterTemplateModel.</summary>
        public DateFormatConverterTemplateModel(PlSqlGeneratorSettings settings)
        {
            _settings = settings;
        }

        /// <summary>Gets or sets a value indicating whether to generate the DateFormatConverter class.</summary>
        public bool GenerateDateFormatConverterClass => _settings.ExcludedTypeNames?.Contains("DateFormatConverter") != true;
    }
}
