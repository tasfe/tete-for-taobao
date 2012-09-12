using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net;

/// <summary>
/// 上传类
/// </summary>
public class UploadFile
{
    public static string HttpPostWithFile(string url, string queryString, List<Parameter> files)
    {
        Stream requestStream = null;
        StreamReader responseReader = null;
        string responseData = null;
        string boundary = DateTime.Now.Ticks.ToString("x");
        url += '?' + queryString;
        HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
        webRequest.ServicePoint.Expect100Continue = false;
        webRequest.Timeout = 20000;
        webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
        webRequest.Method = "POST";
        webRequest.KeepAlive = true;
        webRequest.Credentials = CredentialCache.DefaultCredentials;

        try
        {
            Stream memStream = new MemoryStream();

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            List<Parameter> listParams = HttpUtil.GetQueryParameters(queryString);

            foreach (Parameter param in listParams)
            {
                string formitem = string.Format(formdataTemplate, param.Name, param.Value);
                byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            memStream.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

            foreach (Parameter param in files)
            {
                string name = param.Name;
                string filePath = param.Value;
                //string file = Path.GetFileName(filePath);
                string file = filePath;
                string contentType = HttpUtil.GetContentType(file);

                string header = string.Format(headerTemplate, name, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[1024];
                int bytesRead = 0;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }

                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                fileStream.Close();
            }

            webRequest.ContentLength = memStream.Length;

            requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            requestStream = null;

            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            responseData = responseReader.ReadToEnd();
        }
        catch
        {
            throw;
        }
        finally
        {
            if (requestStream != null)
            {
                requestStream.Close();
                requestStream = null;
            }

            if (responseReader != null)
            {
                responseReader.Close();
                responseReader = null;
            }

            webRequest.GetResponse().GetResponseStream().Close();
            webRequest = null;
        }

        return responseData;
    }
}
