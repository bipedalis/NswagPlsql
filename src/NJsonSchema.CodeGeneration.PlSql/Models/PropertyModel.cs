//-----------------------------------------------------------------------
// <copyright file="PropertyModel.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using NJsonSchema.CodeGeneration.Models;

namespace NJsonSchema.CodeGeneration.PlSql.Models
{
    /// <summary>The PlSql property template model.</summary>
    public class PropertyModel : PropertyModelBase
    {
        private readonly JsonSchemaProperty _property;
        private readonly PlSqlGeneratorSettings _settings;
        private readonly PlSqlTypeResolver _resolver;
        private string _className;
        /// <summary>Initializes a new instance of the <see cref="PropertyModel"/> class.</summary>
        /// <param name="classTemplateModel">The class template model.</param>
        /// <param name="property">The property.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="settings">The settings.</param>
        public PropertyModel(
            ClassTemplateModel classTemplateModel,
            JsonSchemaProperty property,
            PlSqlTypeResolver typeResolver,
            PlSqlGeneratorSettings settings)
            : base(property, classTemplateModel, typeResolver, settings)
        {
            _property = property;
            _settings = settings;
            _resolver = typeResolver;
            _className = classTemplateModel.ClassName;
        }

        /// <summary>Gets the name of the property.</summary>
        public string Name => _property.Name;

        /// <summary>Gets the name of the property.</summary>
        public string JsonName => Name.First().ToString().ToLower() + Name.Substring(1);

        /// <summary>Gets the type of the property.</summary>
        public override string Type
        {
            get
            {
                var t = _resolver.Resolve(_property, _property.IsNullable(_settings.SchemaType), GetTypeNameHint());
                if(t== _className || t== "HearingDtoT" || t== "HearingDto") 
                {
                    t = "clob"; // rekursiju neatbalsta
                }
                return t;
            }
        }

        /// <summary>Gets a value indicating whether the property has a description.</summary>
        public bool HasDescription => !string.IsNullOrEmpty(_property.Description);

        /// <summary>
        /// 
        /// </summary>
        public bool IsComplex
        {
            get
            {
                return (_property.ActualTypeSchema.IsObject || _property.ActualTypeSchema.IsArray) &&
                    Type != "clob";
            }
        }

        /// <summary>Gets the description.</summary>
        public string Description => _property.Description;

        /// <summary>Gets the name of the field.</summary>
        public string FieldName => "m_" + ConversionUtilities.ConvertToLowerCamelCase(PropertyName, true);

        /// <summary>Gets a value indicating whether the property is nullable.</summary>
        public override bool IsNullable => ( !_property.IsRequired) || base.IsNullable;

        /// <summary>Gets or sets a value indicating whether empty strings are allowed.</summary>
        public bool AllowEmptyStrings =>
            _property.ActualTypeSchema.Type.HasFlag(JsonObjectType.String) &&
            (_property.MinLength == null || _property.MinLength == 0);



        /// <summary>Gets the json property required.</summary>
        public string JsonPropertyRequiredCode
        {
            get
            {
                if (_property.IsRequired)
                {
                    if (!_property.IsNullable(_settings.SchemaType))
                    {
                        return "Newtonsoft.Json.Required.Always";
                    }
                    else
                    {
                        return "Newtonsoft.Json.Required.AllowNull";
                    }
                }
                else
                {
                    if (!_property.IsNullable(_settings.SchemaType))
                    {
                        return "Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore";
                    }
                    else
                    {
                        return "Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore";
                    }
                }
            }
        }

        /// <summary>Gets a value indicating whether to render a required attribute.</summary>
        public bool RenderRequiredAttribute
        {
            get
            {
                if ( !_property.IsRequired || _property.IsNullable(_settings.SchemaType))
                {
                    return false;
                }

                return _property.ActualTypeSchema.IsAnyType ||
                       _property.ActualTypeSchema.Type.HasFlag(JsonObjectType.Object) ||
                       _property.ActualTypeSchema.Type.HasFlag(JsonObjectType.String) ||
                       _property.ActualTypeSchema.Type.HasFlag(JsonObjectType.Array);
            }
        }


        /// <summary>Gets the minimum value of the range attribute.</summary>
        public string RangeMinimumValue
        {
            get
            {
                var schema = _property.ActualSchema;
                var propertyFormat = GetSchemaFormat(schema);
                var format = propertyFormat == JsonFormatStrings.Integer ? JsonFormatStrings.Integer : JsonFormatStrings.Double;
                var type = propertyFormat == JsonFormatStrings.Integer ? "int" : "double";

                var minimum = schema.Minimum;
                if (minimum.HasValue && schema.IsExclusiveMinimum)
                {
                    if (propertyFormat == JsonFormatStrings.Integer || propertyFormat == JsonFormatStrings.Long)
                    {
                        minimum++;
                    }
                    else if (schema.MultipleOf.HasValue)
                    {
                        minimum += schema.MultipleOf;
                    }
                    else
                    {
                        // TODO - add support for doubles, singles and decimals here
                    }
                }
                return minimum.HasValue
                    ? ValueGenerator.GetNumericValue(schema.Type, minimum.Value, format)
                    : type + "." + nameof(double.MinValue);
            }
        }

        /// <summary>Gets the maximum value of the range attribute.</summary>
        public string RangeMaximumValue
        {
            get
            {
                var schema = _property.ActualSchema;
                var propertyFormat = GetSchemaFormat(schema);
                var format = propertyFormat == JsonFormatStrings.Integer ? JsonFormatStrings.Integer : JsonFormatStrings.Double;
                var type = propertyFormat == JsonFormatStrings.Integer ? "int" : "double";

                var maximum = schema.Maximum;
                if (maximum.HasValue && schema.IsExclusiveMaximum)
                {
                    if (propertyFormat == JsonFormatStrings.Integer || propertyFormat == JsonFormatStrings.Long)
                    {
                        maximum--;
                    }
                    else if (schema.MultipleOf.HasValue)
                    {
                        maximum -= schema.MultipleOf;
                    }
                    else
                    {
                        // TODO - add support for doubles, singles and decimals here
                    }
                }

                return maximum.HasValue
                    ? ValueGenerator.GetNumericValue(schema.Type, maximum.Value, format)
                    : type + "." + nameof(double.MaxValue);
            }
        }

        /// <summary>Gets the minimum value of the string length attribute.</summary>
        public int StringLengthMinimumValue => _property.ActualSchema.MinLength ?? 0;

        /// <summary>Gets the maximum value of the string length attribute.</summary>
        public string StringLengthMaximumValue => _property.ActualSchema.MaxLength.HasValue ? _property.ActualSchema.MaxLength.Value.ToString(CultureInfo.InvariantCulture) : $"int.{nameof(int.MaxValue)}";

        /// <summary>Gets the value of the min length attribute.</summary>
        public int MinLengthAttribute => _property.ActualSchema.MinItems;



        /// <summary>Gets the value of the max length attribute.</summary>
        public int MaxLengthAttribute => _property.ActualSchema.MaxItems;



        /// <summary>Gets the regular expression value for the regular expression attribute.</summary>
        public string RegularExpressionValue => _property.ActualSchema.Pattern?.Replace("\"", "\"\"");

        /// <summary>Gets a value indicating whether the property type is string enum.</summary>
        public bool IsStringEnum => _property.ActualTypeSchema.IsEnumeration && _property.ActualTypeSchema.Type.HasFlag(JsonObjectType.String);

        /// <summary>Gets a value indicating whether the property should be formatted like a date.</summary>
        public bool IsDate => _property.ActualSchema.Format == JsonFormatStrings.Date;

        /// <summary>Gets a value indicating whether the property is deprecated.</summary>
        public bool IsDeprecated => _property.IsDeprecated;

        /// <summary>Gets a value indicating whether the property has a deprecated message.</summary>
        public bool HasDeprecatedMessage => !string.IsNullOrEmpty(_property.DeprecatedMessage);

        /// <summary>Gets the deprecated message.</summary>
        public string DeprecatedMessage => _property.DeprecatedMessage;

        private string GetSchemaFormat(JsonSchema schema)
        {
            if (Type == "long" || Type == "long?")
            {
                return JsonFormatStrings.Long;
            }

            if (schema.Format == null)
            {
                switch (schema.Type)
                {
                    case JsonObjectType.Integer:
                        return JsonFormatStrings.Integer;

                    case JsonObjectType.Number:
                        return JsonFormatStrings.Double;
                }
            }

            return schema.Format;
        }
    }
}
