using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SupportCaseManagement.Module.BusinessObjects;
using SupportCaseManagement.Module.Services;

namespace SupportCaseManagement.Module.Controllers
{
    public class SupportCaseChatController : ObjectViewController<DetailView, SupportCase>
    {
        public SupportCaseChatController()
        {
            var sendChat = new SimpleAction(this, "SendChatMessage", PredefinedCategory.View);
            sendChat.Caption = "Send To AI";
            sendChat.Execute += SendChat_Execute;
        }

        private async void SendChat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var supportCase = View.CurrentObject as SupportCase;

            if (supportCase == null || string.IsNullOrWhiteSpace(supportCase.ChatInput))
                return;

            var aiService = new AgenticAIService();

            var userMessage = supportCase.ChatInput;

            var aiReply = await aiService.ChatWithCase(supportCase, userMessage);

            var log = ObjectSpace.CreateObject<AIInteractionLog>();
            log.Case = supportCase;
            log.UserName = "User";
            log.UserMessage = userMessage;
            log.AIResponse = aiReply;
            log.Timestamp = DateTime.Now;

            ObjectSpace.CommitChanges();

            supportCase.ChatInput = "";

            View.Refresh();
        }
    }

}
