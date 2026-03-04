using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using D = IAPR_Data;
namespace InsurexService
{
    public partial class InsurexMailer : ServiceBase
    {
        public InsurexMailer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {
        }
        //public void time_elapsed(object sender, ElapsedEventArgs e)

        //{

        //    MyLogEvent.WriteEntry("Mail Sending on " + DateTime.Now.ToString());

        //    D.Providers.Notification_Provider nP = new D.Providers.Notification_Provider();
        //    nP.SendAllAwaitingNotifications();
        //}
    }
}
