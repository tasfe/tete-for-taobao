using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace GetTaoBaoGoodsService
{
    public partial class GetTaoBaoGoodsService : ServiceBase
    {

        private readonly Thread getgoodsthread;
        private readonly string getgoodshour = System.Configuration.ConfigurationManager.AppSettings["getgoodshour"];

        public GetTaoBaoGoodsService()
        {
            InitializeComponent();
            try
            {
                getgoodsthread = new Thread(GetGoodsInfo) { Priority = ThreadPriority.Lowest, IsBackground = true };
            }
            catch (Exception ex)
            {
                LogHelper.ServiceLog.RecodeLog("启动线程出错" + ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            getgoodsthread.Start();
        }

        protected override void OnStop()
        {
            getgoodsthread.Abort();
        }

        void GetGoodsInfo()
        {
            while (true)
            {
                //每多少个小时搜集一次订单
                int hour = 2;
                try
                {
                    hour = int.Parse(getgoodshour);
                }
                catch (Exception ex)
                {
                    LogHelper.ServiceLog.RecodeLog("配置获取商品间隔时间错误" + ex.Message);
                }

                TaoBaoGoods taogoods = new TaoBaoGoods();
                LogHelper.ServiceLog.RecodeLog("正在执行获取订单...");
                taogoods.GetTaoBaoGoods();
                LogHelper.ServiceLog.RecodeLog("执行获取订单结束.");
                Thread.Sleep(hour * 3600 * 1000);
            }
        }

    }
}
