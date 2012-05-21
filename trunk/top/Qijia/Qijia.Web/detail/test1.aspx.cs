using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;

public partial class Web_detail_test1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Parameter> list = new List<Parameter>();
        Parameter paramter = new Parameter("image", Server.MapPath("1.gif"));
        list.Add(paramter);
        try
        {
            string realUrl = UploadFile.HttpPostWithFile("http://mall.jia.com/site/upload_describe_image", "", list);
            Response.Write(realUrl);
        }
        catch (Exception ex)
        {
            LogHelper.LogInfo.Add("用户上传图片错误", ex.Message);
        }
    }

    public void getUp(string fileToUpload)
    {
        String uploadUrl = "http://mall.jia.com/site/upload_describe_image";
        String fileFormName = "file";
        String contenttype = "gif,jpg,jpeg,png,bmp";
        string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

        HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
        webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
        webrequest.Method = "POST";
        StringBuilder sb = new StringBuilder();
        sb.Append("--");
        sb.Append(boundary);
        sb.Append("\r\n");
        sb.Append("Content-Disposition: form-data; name=\"");
        sb.Append(fileFormName);
        sb.Append("\"; filename=\"");
        sb.Append(Path.GetFileName(fileToUpload));
        sb.Append("\"");
        sb.Append("\r\n");
        sb.Append("Content-Type: ");
        sb.Append(contenttype);
        sb.Append("\r\n");
        sb.Append("\r\n");

        string postHeader = sb.ToString();
        byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
        byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
        FileStream fileStream = new FileStream(fileToUpload, FileMode.Open, FileAccess.Read);
        long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
        webrequest.ContentLength = length;
        Stream requestStream = webrequest.GetRequestStream();
        requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

        byte[] buffer = new Byte[(int)fileStream.Length];
        int bytesRead = 0;
        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            requestStream.Write(buffer, 0, bytesRead);
        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
        requestStream.Close();

        StreamReader responseReader = new StreamReader(webrequest.GetResponse().GetResponseStream());
        string responseData = responseReader.ReadToEnd();

        Response.Write(responseData);
    }

}