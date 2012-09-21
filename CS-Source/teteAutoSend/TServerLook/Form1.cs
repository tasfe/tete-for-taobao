using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;

namespace TServerLook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ShowServerData();
        }

        /// <summary>
        /// 显示服务运行情况
        /// </summary>
        private void ShowServerData()
        {
            //显示短信剩余条数
            Thread newThread1 = new Thread(ShowMsgCount);
            newThread1.Start();

            //显示礼品短信发送情况
            Thread newThread2 = new Thread(ShowGiftMsg);
            newThread2.Start();

            //显示活动短信发送情况
            Thread newThread3 = new Thread(ShowActMsg);
            newThread3.Start();

            //显示支付宝红包发送情况
            Thread newThread4 = new Thread(ShowAlipayMsg);
            newThread4.Start();

            //显示催单短信发送情况
            Thread newThread5 = new Thread(ShowCuiMsg);
            newThread5.Start();

            //显示发货短信发送情况
            Thread newThread6 = new Thread(ShowFahuoMsg);
            newThread6.Start();

            //显示催评短信发送情况
            Thread newThread7 = new Thread(ShowReviewMsg);
            newThread7.Start();

            //显示催评短信发送情况
            Thread newThread8 = new Thread(ShowShippingMsg);
            newThread8.Start();

            //显示催评短信发送情况
            Thread newThread9 = new Thread(ShowTestMsg);
            newThread9.Start();

            //显示催评短信发送情况
            Thread newThread10 = new Thread(ShowTaobaoMsg);
            newThread10.Start();

            //显示催评短信发送情况
            Thread newThread11 = new Thread(ShowFreeCardMsg);
            newThread11.Start();

            //显示催评短信发送情况
            Thread newThread12 = new Thread(ShowFreeCardUse);
            newThread12.Start();
        }

        /// <summary>
        /// 显示短信
        /// </summary>
        private void ShowMsgCount()
        {
            string count = CommonPost("http://sms.pica.com:8082/zqhdServer/getbalance.jsp?regcode=ZXHD-SDK-0107-XNYFLX&pwd=127fb1b4b056c3b979fa1eddac154d30&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            count = count.Replace("<response><result>", "");
            count = count.Replace("</result></response>", "");

            label2.Text = count;

            Thread.Sleep(2000);
            ShowMsgCount();
        }

        /// <summary>
        /// 显示礼品短信发送情况
        /// </summary>
        private void ShowGiftMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=giftmsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label4.Text = ary[0];
            label6.Text = ary[1];
            label8.Text = ary[2];

            Thread.Sleep(20000);
            ShowGiftMsg();
        }


        private void ShowActMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=actmsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label13.Text = ary[0];
            label11.Text = ary[1];
            label9.Text = ary[2];

            Thread.Sleep(20000);
            ShowActMsg();
        }

        private void ShowAlipayMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=alipaymsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label19.Text = ary[0];
            label17.Text = ary[1];
            label15.Text = ary[2];

            Thread.Sleep(20000);
            ShowAlipayMsg();
        }

        private void ShowCuiMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=cuimsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label25.Text = ary[0];
            label23.Text = ary[1];
            label21.Text = ary[2];

            Thread.Sleep(20000);
            ShowCuiMsg();
        }

        private void ShowFahuoMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=fahuomsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label31.Text = ary[0];
            label29.Text = ary[1];
            label27.Text = ary[2];

            Thread.Sleep(20000);
            ShowFahuoMsg();
        }

        private void ShowReviewMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=reviewmsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label37.Text = ary[0];
            label35.Text = ary[1];
            label33.Text = ary[2];

            Thread.Sleep(20000);
            ShowReviewMsg();
        }

        private void ShowShippingMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=shippingmsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label43.Text = ary[0];
            label41.Text = ary[1];
            label39.Text = ary[2];

            Thread.Sleep(20000);
            ShowShippingMsg();
        }

        private void ShowTestMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=testmsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label49.Text = ary[0];
            label47.Text = ary[1];
            label45.Text = ary[2];

            Thread.Sleep(20000);
            ShowTestMsg();
        }

        private void ShowFreeCardMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=freecardmsg";
            string count = CommonPost(url);

            string[] ary = count.Split('|');

            label60.Text = ary[0];
            label62.Text = ary[1];
            label64.Text = ary[2];

            Thread.Sleep(20000);
            ShowFreeCardMsg();
        }

        private void ShowFreeCardUse()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=freecarduse";
            string count = CommonPost(url);

            string[] ary = count.Split(',');

            label66.Text = ary[0];
            label68.Text = ary[1];
            label71.Text = ary[2];

            Thread.Sleep(20000);
            ShowFreeCardUse();
        }

        private void ShowTaobaoMsg()
        {
            string url = "http://haoping.7fshop.com/top/reviewnew/tsapi.aspx?act=taobaomsg";
            string count = CommonPost(url);

            string[] ary = count.Split(',');

            label52.Text = ary[0];
            label53.Text = ary[1];
            label55.Text = ary[2];
            label57.Text = ary[3];

            Thread.Sleep(10000);
            ShowTaobaoMsg();
        }

        #region
        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CommonPost(string url)
        {
            try
            {
                string result = string.Empty;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                StreamReader reader = null;
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                result = reader.ReadToEnd();
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
                return result;
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }
#endregion

    }
}
