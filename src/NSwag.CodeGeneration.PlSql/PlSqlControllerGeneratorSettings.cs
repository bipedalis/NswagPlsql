//-----------------------------------------------------------------------
// <copyright file="SwaggerToPlSqlControllerGeneratorSettings.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NSwag.CodeGeneration.PlSql.Models;

namespace NSwag.CodeGeneration.PlSql
{
    // TODO: Rename to SwaggerToPlSqlControllerGeneratorSettings?

    /// <summary>Settings for the <see cref="PlSqlControllerGenerator"/>.</summary>
    public class PlSqlControllerGeneratorSettings : PlSqlGeneratorBaseSettings
    {
        /// <summary>Initializes a new instance of the <see cref="PlSqlControllerGeneratorSettings"/> class.</summary>
        public PlSqlControllerGeneratorSettings()
        {
            ClassName = "{controller}";
            PlSqlGeneratorSettings.ArrayType = "System.Collections.Generic.List";
            PlSqlGeneratorSettings.ArrayInstanceType = "System.Collections.Generic.List";
            ControllerStyle = PlSqlControllerStyle.Partial;
            ControllerTarget = PlSqlControllerTarget.AspNetCore;
            RouteNamingStrategy = PlSqlControllerRouteNamingStrategy.None;
            GenerateModelValidationAttributes = false;
            UseCancellationToken = false;
        }

        /// <summary>Returns the route name for a controller method.</summary>
        /// <param name="operation">Swagger operation</param>
        /// <returns>Route name.</returns>
        public string GetRouteName(OpenApiOperation operation)
        {
            if (RouteNamingStrategy == PlSqlControllerRouteNamingStrategy.OperationId)
            {
                return operation.OperationId;
            }

            return null;
        }

        /// <summary>Gets or sets the full name of the base class.</summary>
        public string ControllerBaseClass { get; set; }

        /// <summary>Gets or sets the controller generation style (partial, abstract; default: partial).</summary>
        public PlSqlControllerStyle ControllerStyle { get; set; }

        /// <summary>Gets or sets the controller target framework.</summary>
        public PlSqlControllerTarget ControllerTarget { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding cancellation token </summary>
        public bool UseCancellationToken { get; set; }

        /// <summary>Gets or sets the strategy for naming routes (default: PlSqlRouteNamingStrategy.None).</summary>
        public PlSqlControllerRouteNamingStrategy RouteNamingStrategy { get; set; }

        /// <summary>Gets or sets a value indicating whether to add model validation attributes.</summary>
        public bool GenerateModelValidationAttributes { get; set; }

        /// <summary>Gets or sets a value indicating whether ASP.Net Core (2.1) ActionResult type is used (default: false).</summary>
        public bool UseActionResultType { get; set; }

        /// <summary>Gets or sets the base path on which the API is served, which is relative to the Host.</summary>
        public string BasePath { get; set; }
    }
}
