using MyToolkit.Mvvm;
using NSwag.Commands;
using NSwag.Commands.CodeGeneration;
using NSwagStudio.ViewModels.CodeGenerators;

namespace NSwagStudio.Views.CodeGenerators
{
    public partial class SwaggerToPlSqlClientGeneratorView
    {
        private readonly NSwagDocument _document;

        public SwaggerToPlSqlClientGeneratorView(NSwagDocument document)
        {
            InitializeComponent();
            ViewModelHelper.RegisterViewModel(Model, this);

            _document = document;
            Model.Command = document.CodeGenerators.OpenApiToPlSqlClientCommand;
        }

        public override string Title => "Oracle Client";

        private SwaggerToPlSqlClientGeneratorViewModel Model => (SwaggerToPlSqlClientGeneratorViewModel)Resources["ViewModel"];

        public override void UpdateOutput(OpenApiDocumentExecutionResult result)
        {
            Model.ClientCode = result.GetGeneratorOutput<OpenApiToPlSqlClientCommand>();
            if (result.IsRedirectedOutput)
                TabControl.SelectedIndex = 1;
        }

        public override bool IsSelected
        {
            get { return _document.CodeGenerators.OpenApiToPlSqlClientCommand != null; }
            set
            {
                if (value != IsSelected)
                {
                    _document.CodeGenerators.OpenApiToPlSqlClientCommand = value ? new OpenApiToPlSqlClientCommand() : null;
                    Model.Command = _document.CodeGenerators.OpenApiToPlSqlClientCommand;
                    OnPropertyChanged();
                }
            }
        }
    }
}
