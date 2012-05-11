using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using LogHelper;

namespace QiJiaService
{
    public partial class QiJiaService : ServiceBase
    {
        private readonly Thread checkthread;
        private readonly string checktime = System.Configuration.ConfigurationManager.AppSettings["checkhour"];

        public QiJiaService()
        {
            InitializeComponent();
            try
            {
                checkthread = new Thread(Check) { Priority = ThreadPriority.Lowest, IsBackground = true };
            }
            catch(Exception ex)
            {
                ServiceLog.RecodeLog("启动线程出错" + ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            checkthread.Start();
        }

        protected override void OnStop()
        {
            checkthread.Abort();
        }

        private void Check()
        {
            ShopExpire se = new ShopExpire();
            while (true)
            {
                se.CheckExpire();
                Thread.Sleep(int.Parse(checktime));
            }
        }

    }
}
