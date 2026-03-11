// Place in SupportCaseManagement.Module/BusinessObjects/
// If using the FALLBACK property editor approach, this version has a dummy string property

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    [NavigationItem("Default")]
    public class AIAssistantObject : NonPersistentBaseObject
    {
        // Dummy property — the property editor will render AIChatAssistant here
        // Set EditorAlias to "AIAssistantEditor" in the Model Editor,
        // OR use the approach below with [EditorAlias]
        [EditorAlias("AIAssistantEditor")]
        public string AIChat { get; set; }
    }
}