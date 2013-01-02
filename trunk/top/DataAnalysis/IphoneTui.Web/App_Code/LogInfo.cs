using System;
using System.IO;
using System.Web;

public class LogInfo
{
    public static string LogFile
    {
        get
        {
            return HttpRuntime.AppDomainAppPath + "log.txt";
        }
    }
    /// <summary>
    /// 添加日志记录
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">内容</param>
    /// <returns></returns>
    public static string Add(string name, string value)
    {
        if (File.Exists(LogFile))
        {
            //写入日志内容
            return WriteLog(name, value);
        }

        else
        {
            //创建日志文件
            CreateLog();
            return WriteLog(name, value);
        }
        return "false";
    }

    /// <summary>
    /// 创建日志文件
    /// </summary>
    private static void CreateLog()
    {
        StreamWriter SW;
        SW = File.CreateText(LogFile);
        SW.WriteLine("Log created at: " +
                             DateTime.Now.ToString());
        SW.Close();
    }


    /// <summary>
    /// 写入日志内容
    /// </summary>
    /// <param name="name">调用服务路径</param>
    /// <param name="value">内容</param>
    /// <returns></returns>
    public static string WriteLog(string name, string value)
    {
        try
        {
            using (StreamWriter w = File.AppendText(LogFile))
            {

                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1} {2}", DateTime.Now.ToString(), name, value);
                w.WriteLine("-------------------------------");
                // Update the underlying file.
                w.Flush();
                // Close the writer and underlying file.
                w.Close();
            }
            return "True";
        }
        catch
        {
            return "False";
        }
    }

}
