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

namespace GetTalkRecods
{
    public partial class GetTalkRecodsService : ServiceBase
    {

        private readonly Thread gettalkcontensthread;
        private readonly string gettalkcontenshour = System.Configuration.ConfigurationManager.AppSettings["gettalkcontenshour"];

        private readonly Thread kftotalthread;
        private readonly string kftotalhour = System.Configuration.ConfigurationManager.AppSettings["kftotalhour"];

        public GetTalkRecodsService()
        {
            InitializeComponent();
            try
            {
                gettalkcontensthread = new Thread(GetTalkContents) { Priority = ThreadPriority.Lowest, IsBackground = true };
                kftotalthread = new Thread(UpdateKefuTotal) { Priority = ThreadPriority.Lowest, IsBackground = true };
            }
            catch (Exception ex)
            {
                ServiceLog.RecodeLog("启动线程出错" + ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            gettalkcontensthread.Start();
            kftotalthread.Start();
        }

        protected override void OnStop()
        {
            gettalkcontensthread.Abort();
            kftotalthread.Abort();
        }

        void GetTalkContents()
        {
            while (true)
            {
                //每多少个小时搜集一次订单
                int hour = 12;
                try
                {
                    hour = int.Parse(gettalkcontenshour);
                }
                catch (Exception ex)
                {
                    LogHelper.ServiceLog.RecodeLog("配置获取客服聊天记录间隔时间错误" + ex.Message);
                }

                TalkRecod tr = new TalkRecod();

                LogHelper.ServiceLog.RecodeLog("正在执行获取客服聊天记录...");

                tr.GetTalkRecordContent();

                LogHelper.ServiceLog.RecodeLog("执行获取客服聊天记录结束.");

                Thread.Sleep(hour * 3600 * 1000);
            }
        }

        void UpdateKefuTotal()
        {
            while (true)
            {
                //每多少个小时搜集一次订单
                int hour = 12;
                try
                {
                    hour = int.Parse(kftotalhour);
                }
                catch (Exception ex)
                {
                    LogHelper.ServiceLog.RecodeLog("配置统计客服绩效间隔时间错误" + ex.Message);
                }

                KefuTotal kft = new KefuTotal();

                LogHelper.ServiceLog.RecodeLog("正在执行统计客服绩效...");

                kft.GetKefuTotal();

                LogHelper.ServiceLog.RecodeLog("执行统计客服绩效结束.");

                Thread.Sleep(hour * 3600 * 1000);
            }
        }
    }
}
