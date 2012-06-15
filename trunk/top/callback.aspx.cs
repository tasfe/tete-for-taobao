using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Text.RegularExpressions;

public partial class top_callback : System.Web.UI.Page
{
    public string vistor = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString);

        string result = Base64Decode(top_parameters);

        string vistor = Regex.Match(result, "visitor_nick=([^&]*)").Groups[1].ToString();
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    /// <param name="Message"></param>
    /// <returns></returns>
    public static string Base64Decode(string Message)
    {
        if ((Message.Length % 4) != 0)
        {
            throw new ArgumentException("不是正确的BASE64编码，请检查。", "Message");
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(Message, "^[A-Z0-9/+=]*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            throw new ArgumentException("包含不正确的BASE64编码，请检查。", "Message");
        }
        string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        int page = Message.Length / 4;
        System.Collections.ArrayList outMessage = new System.Collections.ArrayList(page * 3);
        char[] message = Message.ToCharArray();
        for (int i = 0; i < page; i++)
        {
            byte[] instr = new byte[4];
            instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
            instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
            instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
            instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
            byte[] outstr = new byte[3];
            outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
            if (instr[2] != 64)
            {
                outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
            }
            else
            {
                outstr[2] = 0;
            }
            if (instr[3] != 64)
            {
                outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
            }
            else
            {
                outstr[2] = 0;
            }
            outMessage.Add(outstr[0]);
            if (outstr[1] != 0)
                outMessage.Add(outstr[1]);
            if (outstr[2] != 0)
                outMessage.Add(outstr[2]);
        }
        byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
        return System.Text.Encoding.Default.GetString(outbyte);
    }
}