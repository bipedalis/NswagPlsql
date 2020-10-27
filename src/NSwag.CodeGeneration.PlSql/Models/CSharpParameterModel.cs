//-----------------------------------------------------------------------
// <copyright file="PlSqlParameterModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.PlSql;
using NSwag.CodeGeneration.Models;

namespace NSwag.CodeGeneration.PlSql.Models
{
    /// <summary>The PlSql parameter model.</summary>
    public class PlSqlParameterModel : ParameterModelBase
    {
        /// <summary>Initializes a new instance of the <see cref="PlSqlParameterModel" /> class.</summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="typeName">The type name.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="allParameters">All parameters.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The client generator base.</param>
        /// <param name="typeResolver">The type resolver.</param>
        public PlSqlParameterModel(
            string parameterName,
            string variableName,
            string typeName,
            OpenApiParameter parameter,
            IList<OpenApiParameter> allParameters,
            CodeGeneratorSettingsBase settings,
            IClientGenerator generator,
            TypeResolverBase typeResolver)
            : base(parameterName, variableName, typeName.IndexOf("(") > 0 ? typeName.Substring(0, typeName.IndexOf("(")) : typeName, parameter, allParameters, settings, generator, typeResolver)
        {
            
        }

        /// <summary>Gets a value indicating whether the type is a Nullable&lt;&gt;.</summary>
        public bool IsSystemNullable => Type.EndsWith("?");

        /// <summary>Gets the type of the parameter when used in a controller interface where we can set default values before calling.</summary>
        public string TypeInControllerInterface => HasDefault ? Type.EndsWith("?") ? Type.Substring(0, Type.Length - 1) : Type : Type;

        /// <summary>Gets a value indicating whether the parameter name is a valid PlSql identifier.</summary>
        public bool IsValidIdentifier => Name.Equals(VariableName, StringComparison.OrdinalIgnoreCase);
        public bool IsVarchar => Type == "VARCHAR2";

    }
}
