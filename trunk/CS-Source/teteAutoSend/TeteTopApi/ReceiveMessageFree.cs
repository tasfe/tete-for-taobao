using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.Logic;

namespace TeteTopApi
{
    public class ReceiveMessageFree
    {
        /// <summary>
        /// 构造函数，获取消息正文
        /// </summary>
        /// <param name="msg"></param>
        public ReceiveMessageFree(string msg)
        {
            this.Msg = msg;
        }

        /// <summary>
        /// 根据消息内容做出相应的逻辑处理
        /// </summary>
        public void ActData()
        {
            string typ = utils.GetMsgType(this.Msg);

            switch (typ)
            {
                case "notify_trade":
                    ActOrderInfo();
                    break;
                case "notify_item":
                    ActOrderInfo();
                    break;
            }
        }

        /// <summary>
        /// 处理订单类型的数据
        /// </summary>
        private void ActOrderInfo()
        {
            string status = utils.GetValueByProperty(this.Msg, "status");
            string nick = utils.GetValueByProperty(this.Msg, "nick");
            Item item = utils.GetItem(this.Msg);

            WeiboSend act = new WeiboSend(item, status, nick, this.Msg);
            act.Start();
        }

        /// <summary>
        /// JSON字符串
        /// </summary>
        private string Msg { get; set; }
    }
}
