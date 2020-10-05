//-----------------------------------------------------------------------
// <copyright file="PlSqlControllerRouteNamingStrategy.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

namespace NSwag.CodeGeneration.PlSql.Models
{
    /// <summary>The PlSql controller routing naming strategy enum.</summary>
    public enum PlSqlControllerRouteNamingStrategy
    {
        /// <summary>Disable route naming.</summary>
        None,

        /// <summary>Use the operationId as the route name, if available.</summary>
        OperationId
    }
}
