//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.Persistent.Base;
//using SupportCaseManagement.Module.AIBackend;
//using SupportCaseManagement.Module.BusinessObjects;
//using SupportCaseManagement.Module.Services;

//namespace SupportCaseManagement.Module.Controllers
//{
//    public class SupportCaseChatController : ObjectViewController<DetailView, SupportCase>
//    {
//        public SupportCaseChatController()
//        {
//            var sendChat = new SimpleAction(this, "SendChatMessage", PredefinedCategory.View);
//            sendChat.Caption = "Send To AI";
//            sendChat.Execute += SendChat_Execute;
//        }

//        private async void SendChat_Execute(object sender, SimpleActionExecuteEventArgs e)
//        {
//            var supportCase = View.CurrentObject as SupportCase;

//            if (supportCase == null || string.IsNullOrWhiteSpace(supportCase.ChatInput))
//                return;

//            var userMessage = supportCase.ChatInput;
//            // Initialize chat container if empty
//            if (string.IsNullOrEmpty(supportCase.ChatHistory))
//            {
//                supportCase.ChatHistory = userMessage;
//            }

//            // USER MESSAGE BUBBLE
//            supportCase.ChatHistory = userMessage;

//            var aiService = new AgenticAIService();


//            var aiReply = await aiService.ChatWithCase(supportCase, userMessage, ObjectSpace);

//            // AI MESSAGE BUBBLE
//            supportCase.ChatHistory = aiReply;
//            // Put AI response in the input box
//            //supportCase.ChatInput = "";

//            // Save AI interaction log

//            var log = ObjectSpace.CreateObject<AIInteractionLog>();
//            log.Case = supportCase;
//            log.UserName = "User";
//            log.UserMessage = userMessage;
//            log.AIResponse = aiReply;
//            log.Timestamp = DateTime.Now;



//            supportCase.ChatInput +=
//                $"\nUser: {userMessage}\nAI: {aiReply}\n";

//            //supportCase.ChatHistory += "<br/>";
//            ObjectSpace.CommitChanges();

//            //supportCase.ChatInput = "";

//            View.Refresh();
//        }
//    }

//}
