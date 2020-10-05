//-----------------------------------------------------------------------
// <copyright file="PlSqlControllerOperationModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NJsonSchema.CodeGeneration.PlSql;

namespace NSwag.CodeGeneration.PlSql.Models
{
    /// <summary>The PlSql controller operation model.</summary>
    public class PlSqlControllerOperationModel : PlSqlOperationModel
    {
        private readonly PlSqlControllerGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="PlSqlControllerOperationModel" /> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        public PlSqlControllerOperationModel(OpenApiOperation operation, PlSqlControllerGeneratorSettings settings,
            PlSqlControllerGenerator generator, PlSqlTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _settings = settings;
        }

        /// <summary>Gets or sets the type of the result.</summary>
        public override string ResultType
        {
            get
            {
                if (_settings.UseActionResultType)
                {
                    switch (SyncResultType)
                    {
                        case "void":
                        case "FileResult":
                            return "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>";
                        default:
                            return "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<" + SyncResultType + ">>";
                    }
                }

                return base.ResultType;
            }
        }
    }
}
