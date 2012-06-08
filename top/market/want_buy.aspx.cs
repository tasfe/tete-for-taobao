using System;
using System.IO;

public partial class want_buy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    RecodeLog(Request.ServerVariables["REMOTE_ADDR"]);
        //}
    }

    private static void RecodeLog(string result)
    {
        string LogAddress = "D:\\点击生成手机应用记录";
        FileStream fstream;
        string dateStr = DateTime.Now.ToLongDateString().Replace("年", "").Replace("月", "").Replace("日", "");
        if (!Directory.Exists(LogAddress))
            Directory.CreateDirectory(LogAddress);
        string filepath = LogAddress + "\\" + dateStr + ".txt";

        if (File.Exists(filepath))
            fstream = new FileStream(filepath, FileMode.Append, FileAccess.Write);
        else fstream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);

        lock (fstream)
        {
            StreamWriter swriter = new StreamWriter(fstream);
            swriter.WriteLine(result + "\t\t" + DateTime.Now);

            swriter.Close();
            fstream.Close();
        }
    }
}
