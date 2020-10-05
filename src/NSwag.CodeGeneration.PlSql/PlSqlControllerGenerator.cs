//-----------------------------------------------------------------------
// <copyright file="SwaggerToPlSqlControllerGenerator.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.PlSql;
using NSwag.CodeGeneration.PlSql.Models;

namespace NSwag.CodeGeneration.PlSql
{
    /// <summary>Generates the PlSql service client code. </summary>
    public class PlSqlControllerGenerator : PlSqlGeneratorBase
    {
        private readonly OpenApiDocument _document;

        /// <summary>Initializes a new instance of the <see cref="PlSqlControllerGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public PlSqlControllerGenerator(OpenApiDocument document, PlSqlControllerGeneratorSettings settings)
            : this(document, settings, CreateResolverWithExceptionSchema(settings.PlSqlGeneratorSettings, document))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PlSqlControllerGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public PlSqlControllerGenerator(OpenApiDocument document, PlSqlControllerGeneratorSettings settings, PlSqlTypeResolver resolver)
            : base(document, settings, resolver)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>Gets or sets the generator settings.</summary>
        public PlSqlControllerGeneratorSettings Settings { get; set; }

        /// <summary>Gets the base settings.</summary>
        public override ClientGeneratorBaseSettings BaseSettings => Settings;

        /// <summary>Generates the client types.</summary>
        /// <returns>The code artifact collection.</returns>
        protected override IEnumerable<CodeArtifact> GenerateAllClientTypes()
        {
            var artifacts = base.GenerateAllClientTypes().ToList();

            if (Settings.ControllerTarget == PlSqlControllerTarget.AspNet &&
                _document.Operations.Count(operation => operation.Operation.ActualParameters.Any(p => p.Kind == OpenApiParameterKind.Header)) > 0)
            {
                var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("PlSql", "Controller.AspNet.FromHeaderAttribute", new object());
                artifacts.Add(new CodeArtifact("FromHeaderAttribute", CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Utility, template));

                template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("PlSql", "Controller.AspNet.FromHeaderBinding", new object());
                artifacts.Add(new CodeArtifact("FromHeaderBinding", CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Utility, template));
            }

            return artifacts;
        }

        /// <summary>Generates the client class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller class.</param>
        /// <param name="operations">The operations.</param>
        /// <returns>The code.</returns>
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string controllerName, string controllerClassName, IEnumerable<PlSqlOperationModel> operations)
        {
            var model = new PlSqlControllerTemplateModel(controllerClassName, operations, _document, Settings);
            var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("PlSql", "Controller", model);
            yield return new CodeArtifact(model.Class, CodeArtifactType.Class, CodeArtifactLanguage.Undefined, CodeArtifactCategory.Client, template);
        }

        /// <summary>Creates an operation model.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The operation model.</returns>
        protected override PlSqlOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings)
        {
            return new PlSqlControllerOperationModel(operation, (PlSqlControllerGeneratorSettings)settings, this, (PlSqlTypeResolver)Resolver);
        }
    }
}
