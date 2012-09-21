using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JdSoft.Apple.Apns.Notifications;

namespace teteiPhone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [STAThread]
        private void button1_Click(object sender, EventArgs e)
        {
            //True if you are using sandbox certificate, or false if using production
            bool sandbox = false;

            //Put your device token in here
            //hu-2cab0faa9e6e47663ffe79dcc0b979cfd481cd8aa9b65a8dac236d178de51f87
            //lv-fb77f2d0962ccea6061ece2873d29d252397a31dc36a301d7f7e3761fe5b460e
            string testDeviceToken = "2cab0faa9e6e47663ffe79dcc0b979cfd481cd8aa9b65a8dac236d178de51f87";

            //Put your PKCS12 .p12 or .pfx filename here.
            // Assumes it is in the same directory as your app
            string p12File = "apn_developer_identity.p12";

            //This is the password that you protected your p12File 
            //  If you did not use a password, set it as null or an empty string
            string p12FilePassword = "3561402fox";

            //Number of notifications to send
            int count = 1;

            //Number of milliseconds to wait in between sending notifications in the loop
            // This is just to demonstrate that the APNS connection stays alive between messages
            int sleepBetweenNotifications = 5000;


            //Actual Code starts below:
            //--------------------------------

            string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);

            NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

            service.SendRetries = 5; //5 retries before generating notificationfailed event
            service.ReconnectDelay = 5000; //5 seconds

            service.Error += new NotificationService.OnError(service_Error);
            service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

            service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
            service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
            service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
            service.Connecting += new NotificationService.OnConnecting(service_Connecting);
            service.Connected += new NotificationService.OnConnected(service_Connected);
            service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);

            //The notifications will be sent like this:
            //		Testing: 1...
            //		Testing: 2...
            //		Testing: 3...
            // etc...
            for (int i = 1; i <= count; i++)
            {
                //Create a new notification to send
                Notification alertNotification = new Notification(testDeviceToken);

                alertNotification.Payload.Alert.Body = string.Format("尊重的胡先生，您的100元抵扣券即将过期，请您马上使用", i);
                alertNotification.Payload.Sound = "default";
                alertNotification.Payload.Badge = i;

                //Queue the notification to be sent
                if (service.QueueNotification(alertNotification))
                {
                    textBox1.AppendText("Notification Queued!");
                    textBox1.AppendText(service.Host.ToString());
                }
                else
                    textBox1.AppendText("Notification Failed to be Queued!");

                //Sleep in between each message
                //if (i < count)
                //{
                    textBox1.AppendText("Sleeping " + sleepBetweenNotifications + " milliseconds before next Notification...");
                    System.Threading.Thread.Sleep(sleepBetweenNotifications);
                //}
            }

            textBox1.AppendText("Cleaning Up...");

            //First, close the service.  
            //This ensures any queued notifications get sent befor the connections are closed
            service.Close();

            //Clean up
            service.Dispose();
        }


        private void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            textBox1.AppendText(string.Format("Bad Device Token: {0}", ex.Message));
        }

        private void service_Disconnected(object sender)
        {
            textBox1.AppendText("Disconnected...");
        }

        private void service_Connected(object sender)
        {
            textBox1.AppendText("Connected...");
        }

        private void service_Connecting(object sender)
        {
            textBox1.AppendText("Connecting...");
        }

        private void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            textBox1.AppendText(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }

        private void service_NotificationSuccess(object sender, Notification notification)
        {
            textBox1.AppendText(string.Format("Notification Success: {0}", notification.ToString()));
        }

        private void service_NotificationFailed(object sender, Notification notification)
        {
            textBox1.AppendText(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        private void service_Error(object sender, Exception ex)
        {
            textBox1.AppendText(string.Format("Error: {0}", ex.Message));
        }
    }
}
