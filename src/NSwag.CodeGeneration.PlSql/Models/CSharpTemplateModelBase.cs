//-----------------------------------------------------------------------
// <copyright file="PlSqlTemplateModelBase.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

namespace NSwag.CodeGeneration.PlSql.Models
{
    /// <summary>Base class for the PlSql models</summary>
    public abstract class PlSqlTemplateModelBase
    {
        private readonly string _controllerName;
        private readonly PlSqlGeneratorBaseSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="PlSqlTemplateModelBase"/> class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="settings">The settings.</param>
        protected PlSqlTemplateModelBase(string controllerName, PlSqlGeneratorBaseSettings settings)
        {
            _controllerName = controllerName;
            _settings = settings;
        }

        /// <summary>Gets a value indicating whether to wrap success responses to allow full response access.</summary>
        public bool WrapResponses => _settings.WrapResponses;

        /// <summary>Gets the response class name.</summary>
        public string ResponseClass => _settings.ResponseClass.Replace("{controller}", _controllerName);
    }
}
