//-----------------------------------------------------------------------
// <copyright file="SwaggerToCSharpCommand.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NConsole;
//using NJsonSchema.CodeGeneration.CSharp;
//using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.PlSql;

#pragma warning disable 1591

namespace NSwag.Commands.CodeGeneration
{
    public abstract class OpenApiToPlSqlCommandBase<TSettings> : CodeGeneratorCommandBase<TSettings>
         where TSettings : PlSqlGeneratorBaseSettings
    {
        protected OpenApiToPlSqlCommandBase(TSettings settings)
            : base(settings)
        {
        }

        [Argument(Name = "ClassName", IsRequired = false, Description = "The class name of the generated client.")]
        public string ClassName
        {
            get { return Settings.ClassName; }
            set { Settings.ClassName = value; }
        }

        [Argument(Name = "OperationGenerationMode", IsRequired = false, Description = "The operation generation mode ('SingleClientFromOperationId' or 'MultipleClientsFromPathSegments').")]
        public OperationGenerationMode OperationGenerationMode
        {
            get { return OperationGenerationModeConverter.GetOperationGenerationMode(Settings.OperationNameGenerator); }
            set { Settings.OperationNameGenerator = OperationGenerationModeConverter.GetOperationNameGenerator(value); }
        }

        [Argument(Name = "AdditionalNamespaceUsages", IsRequired = false, Description = "The additional namespace usages.")]
        public string[] AdditionalNamespaceUsages
        {
            get { return Settings.AdditionalNamespaceUsages; }
            set { Settings.AdditionalNamespaceUsages = value; }
        }

        [Argument(Name = "AdditionalContractNamespaceUsages", IsRequired = false, Description = "The additional contract namespace usages.")]
        public string[] AdditionalContractNamespaceUsages
        {
            get { return Settings.AdditionalContractNamespaceUsages; }
            set { Settings.AdditionalContractNamespaceUsages = value; }
        }

        [Argument(Name = "GenerateOptionalParameters", IsRequired = false,
                  Description = "Specifies whether to reorder parameters (required first, optional at the end) and generate optional parameters (default: false).")]
        public bool GenerateOptionalParameters
        {
            get { return Settings.GenerateOptionalParameters; }
            set { Settings.GenerateOptionalParameters = value; }
        }

        [Argument(Name = "GenerateJsonMethods", IsRequired = false,
            Description = "Specifies whether to render ToJson() and FromJson() methods for DTOs (default: true).")]
        public bool GenerateJsonMethods
        {
            get { return Settings.PlSqlGeneratorSettings.GenerateJsonMethods; }
            set { Settings.PlSqlGeneratorSettings.GenerateJsonMethods = value; }
        }

        [Argument(Name = "PublicFromJsonMethods", IsRequired = false,
            Description = "Specifies type  names which require public from_json methods (OperationResponseModel).")]
        public string PublicFromJsonMethods
        {
            get { return Settings.PlSqlGeneratorSettings.PublicFromJsonMethods; }
            set { Settings.PlSqlGeneratorSettings.PublicFromJsonMethods = value; }
        }
        [Argument(Name = "LongStrings", IsRequired = false,
    Description = "Field names which contain long (>1024) string.")]
        public string LongStrings
        {
            get { return Settings.PlSqlGeneratorSettings.LongStrings; }
            set { Settings.PlSqlGeneratorSettings.LongStrings = value; }
        }
        [Argument(Name = "ComplexTypes", IsRequired = false,
   Description = "Complex types (depth>8), try to decompose if in param list.")]
        public string ComplexTypes
        {
            get { return Settings.PlSqlGeneratorSettings.ComplexTypes; }
            set { Settings.PlSqlGeneratorSettings.ComplexTypes = value; }
        }

        [Argument(Name = "EnforceFlagEnums", IsRequired = false,
            Description = "Specifies whether enums should be always generated as bit flags (default: false).")]
        public bool EnforceFlagEnums
        {
            get;set;
        }

        [Argument(Name = "ParameterArrayType", IsRequired = false, Description = "The generic array .NET type of operation parameters (default: 'IEnumerable').")]
        public string ParameterArrayType
        {
            get { return Settings.ParameterArrayType; }
            set { Settings.ParameterArrayType = value; }
        }

        [Argument(Name = "ParameterDictionaryType", IsRequired = false, Description = "The generic dictionary .NET type of operation parameters (default: 'IDictionary').")]
        public string ParameterDictionaryType
        {
            get { return Settings.ParameterDictionaryType; }
            set { Settings.ParameterDictionaryType = value; }
        }

        [Argument(Name = "ResponseArrayType", IsRequired = false, Description = "The generic array .NET type of operation responses (default: 'ICollection').")]
        public string ResponseArrayType
        {
            get { return Settings.ResponseArrayType; }
            set { Settings.ResponseArrayType = value; }
        }

        [Argument(Name = "ResponseDictionaryType", IsRequired = false, Description = "The generic dictionary .NET type of operation responses (default: 'IDictionary').")]
        public string ResponseDictionaryType
        {
            get { return Settings.ResponseDictionaryType; }
            set { Settings.ResponseDictionaryType = value; }
        }

        [Argument(Name = "WrapResponses", IsRequired = false, Description = "Specifies whether to wrap success responses to allow full response access.")]
        public bool WrapResponses
        {
            get { return Settings.WrapResponses; }
            set { Settings.WrapResponses = value; }
        }

        [Argument(Name = "WrapResponseMethods", IsRequired = false, Description = "List of methods where responses are wrapped ('ControllerName.MethodName', WrapResponses must be true).")]
        public string[] WrapResponseMethods
        {
            get { return Settings.WrapResponseMethods; }
            set { Settings.WrapResponseMethods = value; }
        }

        [Argument(Name = "GenerateResponseClasses", IsRequired = false, Description = "Specifies whether to generate response classes (default: true).")]
        public bool GenerateResponseClasses
        {
            get { return Settings.GenerateResponseClasses; }
            set { Settings.GenerateResponseClasses = value; }
        }

        [Argument(Name = "ResponseClass", IsRequired = false, Description = "The response class (default 'SwaggerResponse', may use '{controller}' placeholder).")]
        public string ResponseClass
        {
            get { return Settings.ResponseClass; }
            set { Settings.ResponseClass = value; }
        }

        // PlSqlGeneratorSettings

        [Argument(Name = "Namespace", Description = "The package name.")]
        public string Namespace
        {
            get { return Settings.PlSqlGeneratorSettings.Namespace; }
            set { Settings.PlSqlGeneratorSettings.Namespace = value; }
        }
        

        [Argument(Name = "BaseUrl", Description = "The base url.")]
        public string BaseUrl
        {
            get { return Settings.PlSqlGeneratorSettings.BaseUrl; }
            set { Settings.PlSqlGeneratorSettings.BaseUrl = value; }
        }
        

        [Argument(Name = "RequiredPropertiesMustBeDefined", IsRequired = false,
                  Description = "Specifies whether a required property must be defined in JSON (sets Required.Always when the property is required).")]
        public bool RequiredPropertiesMustBeDefined
        {
            get;set;
        }

        [Argument(Name = "DateType", IsRequired = false, Description = "The date .NET type (default: 'DateTimeOffset').")]
        public string DateType
        {
            get { return Settings.PlSqlGeneratorSettings.DateType; }
            set { Settings.PlSqlGeneratorSettings.DateType = value; }
        }

        [Argument(Name = "JsonConverters", IsRequired = false, Description = "Specifies the custom Json.NET converter types (optional, comma separated).")]
        public string[] JsonConverters
        {
            get;set;
        }

        [Argument(Name = "AnyType", IsRequired = false, Description = "The any .NET type (default: 'object').")]
        public string AnyType
        {
            get { return Settings.PlSqlGeneratorSettings.AnyType; }
            set { Settings.PlSqlGeneratorSettings.AnyType = value; }
        }

        [Argument(Name = "DateTimeType", IsRequired = false, Description = "The date time .NET type (default: 'DateTimeOffset').")]
        public string DateTimeType
        {
            get { return Settings.PlSqlGeneratorSettings.DateTimeType; }
            set { Settings.PlSqlGeneratorSettings.DateTimeType = value; }
        }

        [Argument(Name = "TimeType", IsRequired = false, Description = "The time .NET type (default: 'TimeSpan').")]
        public string TimeType
        {
            get { return Settings.PlSqlGeneratorSettings.TimeType; }
            set { Settings.PlSqlGeneratorSettings.TimeType = value; }
        }

        [Argument(Name = "TimeSpanType", IsRequired = false, Description = "The time span .NET type (default: 'TimeSpan').")]
        public string TimeSpanType
        {
            get { return Settings.PlSqlGeneratorSettings.TimeSpanType; }
            set { Settings.PlSqlGeneratorSettings.TimeSpanType = value; }
        }

        [Argument(Name = "ArrayType", IsRequired = false, Description = "The generic array .NET type (default: 'ICollection').")]
        public string ArrayType
        {
            get { return Settings.PlSqlGeneratorSettings.ArrayType; }
            set { Settings.PlSqlGeneratorSettings.ArrayType = value; }
        }

        [Argument(Name = "ArrayInstanceType", IsRequired = false, Description = "The generic array .NET instance type (default: empty = ArrayType).")]
        public string ArrayInstanceType
        {
            get;set;
        }

        [Argument(Name = "DictionaryType", IsRequired = false, Description = "The generic dictionary .NET type (default: 'IDictionary').")]
        public string DictionaryType
        {
            get;set;
        }

        [Argument(Name = "DictionaryInstanceType", IsRequired = false, Description = "The generic dictionary .NET instance type (default: empty = DictionaryType).")]
        public string DictionaryInstanceType
        {
            get;set;
        }

        [Argument(Name = "ArrayBaseType", IsRequired = false, Description = "The generic array .NET type (default: 'Collection').")]
        public string ArrayBaseType
        {
            get;set;
        }

        [Argument(Name = "DictionaryBaseType", IsRequired = false, Description = "The generic dictionary .NET type (default: 'Dictionary').")]
        public string DictionaryBaseType
        {
            get;
        }

        [Argument(Name = "GenerateDefaultValues", IsRequired = false, Description = "Specifies whether to generate default values for properties (may generate CSharp 6 code, default: true).")]
        public bool GenerateDefaultValues
        {
            get { return Settings.PlSqlGeneratorSettings.GenerateDefaultValues; }
            set { Settings.PlSqlGeneratorSettings.GenerateDefaultValues = value; }
        }

        [Argument(Name = "GenerateDataAnnotations", IsRequired = false, Description = "Specifies whether to generate data annotation attributes on DTO classes (default: true).")]
        public bool GenerateDataAnnotations
        {
            get;
        }

        [Argument(Name = "ExcludedTypeNames", IsRequired = false, Description = "The excluded DTO type names (must be defined in an import or other namespace).")]
        public string[] ExcludedTypeNames
        {
            get { return Settings.PlSqlGeneratorSettings.ExcludedTypeNames; }
            set { Settings.PlSqlGeneratorSettings.ExcludedTypeNames = value; }
        }

        [Argument(Name = "ExcludedParameterNames", IsRequired = false, Description = "The globally excluded parameter names.")]
        public string[] ExcludedParameterNames
        {
            get { return Settings.ExcludedParameterNames; }
            set { Settings.ExcludedParameterNames = value; }
        }

        [Argument(Name = "HandleReferences", IsRequired = false, Description = "Use preserve references handling (All) in the JSON serializer (default: false).")]
        public bool HandleReferences
        {
            get;
        }

        [Argument(Name = "GenerateImmutableArrayProperties", IsRequired = false,
                  Description = "Specifies whether to remove the setter for non-nullable array properties (default: false).")]
        public bool GenerateImmutableArrayProperties
        {
            get;
        }

        [Argument(Name = "GenerateImmutableDictionaryProperties", IsRequired = false,
                  Description = "Specifies whether to remove the setter for non-nullable dictionary properties (default: false).")]
        public bool GenerateImmutableDictionaryProperties
        {
            get;
        }

        [Argument(Name = "JsonSerializerSettingsTransformationMethod", IsRequired = false,
            Description = "The name of a static method which is called to transform the JsonSerializerSettings used in the generated ToJson()/FromJson() methods (default: none).")]
        public string JsonSerializerSettingsTransformationMethod
        {
            get;
        }

        [Argument(Name = "InlineNamedArrays", Description = "Inline named arrays (default: false).", IsRequired = false)]
        public bool InlineNamedArrays
        {
            get;
        }

        [Argument(Name = "InlineNamedDictionaries", Description = "Inline named dictionaries (default: false).", IsRequired = false)]
        public bool InlineNamedDictionaries
        {
            get;
        }

        [Argument(Name = "InlineNamedTuples", Description = "Inline named tuples (default: true).", IsRequired = false)]
        public bool InlineNamedTuples
        {
            get;
        }

        [Argument(Name = "InlineNamedAny", Description = "Inline named any types (default: false).", IsRequired = false)]
        public bool InlineNamedAny
        {
            get { return Settings.PlSqlGeneratorSettings.InlineNamedAny; }
            set { Settings.PlSqlGeneratorSettings.InlineNamedAny = value; }
        }

        [Argument(Name = "GenerateDtoTypes", IsRequired = false, Description = "Specifies whether to generate DTO classes.")]
        public bool GenerateDtoTypes
        {
            get { return Settings.GenerateDtoTypes; }
            set { Settings.GenerateDtoTypes = value; }
        }

        [Argument(Name = "GenerateOptionalPropertiesAsNullable", IsRequired = false, Description = "Specifies whether optional schema properties " +
            "(not required) are generated as nullable properties (default: false).")]
        public bool GenerateOptionalPropertiesAsNullable
        {
            get;
        }

        [Argument(Name = "GenerateNullableReferenceTypes", IsRequired = false, Description = "Specifies whether whether to " +
            "generate Nullable Reference Type annotations (default: false).")]
        public bool GenerateNullableReferenceTypes
        {
            get { return Settings.PlSqlGeneratorSettings.GenerateNullableReferenceTypes; }
            set { Settings.PlSqlGeneratorSettings.GenerateNullableReferenceTypes = value; }
        }
    }
}
