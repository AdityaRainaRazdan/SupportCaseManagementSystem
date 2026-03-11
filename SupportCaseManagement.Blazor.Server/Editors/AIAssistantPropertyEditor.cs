// Place in SupportCaseManagement.Blazor.Server project

using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using SupportCaseManagement.Blazor.Server.Pages;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Blazor.Server.Editors
{
    [PropertyEditor(typeof(string), "AIAssistantEditor", false)]
    public class AIAssistantPropertyEditor : BlazorPropertyEditorBase
    {
        public AIAssistantPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentModel CreateComponentModel()
        {
            return new AIAssistantComponentModel();
        }

        // No value to read — this is a display-only component
        protected override void ReadValueCore() { }
    }

    public class AIAssistantComponentModel : ComponentModelBase
    {
        // Points to your AIChatAssistant.razor component
        public override Type ComponentType => typeof(AIChatAssistant);
    }
}