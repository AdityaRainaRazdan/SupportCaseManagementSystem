using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using Microsoft.EntityFrameworkCore;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Blazor.Server
{
    public class SupportCaseManagementBlazorApplication : BlazorApplication
    {
        public SupportCaseManagementBlazorApplication()
        {
            ApplicationName = "SupportCaseManagement";
            CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            DatabaseVersionMismatch += SupportCaseManagementBlazorApplication_DatabaseVersionMismatch;
        }
        protected override void OnSetupStarted()
        {
            base.OnSetupStarted();

#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        }
        void SupportCaseManagementBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
#if DEBUG
            e.Updater.Update();
            e.Handled = true;
#else
    throw new InvalidOperationException(
        "The application cannot connect to the database. Update the DB or check the connection string."
    );
#endif

        }
    }
}
