using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;

public static class HttpUtil
{
    //�����ļ�����ȡ�ļ�����
    public static string GetContentType(string fileName)
    {
        string contentType = "application/octetstream";
        string ext = Path.GetExtension(fileName).ToLower();
        RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ext);

        if (registryKey != null && registryKey.GetValue("Content Type") != null)
        {
            contentType = registryKey.GetValue("Content Type").ToString();
        }

        return contentType;
    }

    //����query String��ȡparameter����
    public static List<Parameter> GetQueryParameters(string queryString)
    {
        if (queryString.StartsWith("?"))
        {
            queryString = queryString.Remove(0, 1);
        }

        List<Parameter> result = new List<Parameter>();

        if (!string.IsNullOrEmpty(queryString))
        {
            string[] p = queryString.Split('&');
            foreach (string s in p)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (s.IndexOf('=') > -1)
                    {
                        string[] temp = s.Split('=');
                        result.Add(new Parameter(temp[0], temp[1]));
                    }
                }
            }
        }

        return result;
    }

    ///<summary>
    /// ��������ͼ
    /// </summary>
    /// <param name="originalImagePath">Դͼ·��������·����</param>
    /// <param name="thumbnailPath">����ͼ·��������·����</param>
    /// <param name="width">����ͼ����</param>
    /// <param name="height">����ͼ�߶�</param>
    /// <param name="mode">��������ͼ�ķ�ʽ</param>    
    public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {
        System.Drawing.Image originalImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(originalImagePath));

        int towidth = width;
        int toheight = height;

        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height;

        switch (mode)
        {
            case "HW"://ָ���߿����ţ����ܱ��Σ�                
                break;
            case "W"://ָ�������߰�����                    
                toheight = originalImage.Height * width / originalImage.Width;
                break;
            case "H"://ָ���ߣ���������
                towidth = originalImage.Width * height / originalImage.Height;
                break;
            case "Cut"://ָ���߿��ü��������Σ�  

                //if(originalImage.Width < width && originalImage.Height < height)
                //������߶��������ͼƬ�ⲿ�ӿհ�
                if (originalImage.Width < width && originalImage.Height < height)
                {
                    x = (towidth - ow) / 2;
                    y = (toheight - oh) / 2;
                }
                else
                {
                    //��������¼ӿհ�
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height * towidth / originalImage.Width;
                        ow = towidth;
                        x = 0;
                        y = (toheight - oh) / 2;
                    }
                    //��������Ҽӿհ�
                    else if ((double)originalImage.Width / (double)originalImage.Height < (double)towidth / (double)toheight)
                    {
                        ow = originalImage.Width * toheight / originalImage.Height;
                        oh = toheight;
                        x = (towidth - ow) / 2;
                        y = 0;
                    }
                    //���ֻ������
                    else
                    {
                        x = 0;
                        y = 0;
                        ow = towidth;
                        oh = toheight;
                    }
                }
                break;
            default:
                break;
        }

        //�½�һ��bmpͼƬ
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //�½�һ������
        Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //���ø�������ֵ��
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //���ø�����,���ٶȳ���ƽ���̶�
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //��ջ�������͸������ɫ���
        g.Clear(Color.White);

        //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
        g.DrawImage(originalImage, new Rectangle(x, y, ow, oh), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);

        //Bitmap newmap = ImgSharpChange(bitmap);


        ImageCodecInfo ici;
        System.Drawing.Imaging.Encoder enc;
        EncoderParameter ep;
        EncoderParameters epa;

        //   Initialize   the   necessary   objects   
        ici = GetEncoderInfo("image/jpeg");
        enc = System.Drawing.Imaging.Encoder.Quality;//���ñ�������   
        epa = new EncoderParameters(1);
        //   Set   the   compression   level   
        ep = new EncoderParameter(enc, 100L);//�����ȼ�Ϊ25%   
        epa.Param[0] = ep;

        try
        {
            bitmap.Save(thumbnailPath, ici, epa);
        }
        catch (System.Exception e)
        {
            //ShowErrorPage(e.Message.ToString());
        }
        finally
        {
            originalImage.Dispose();
            bitmap.Dispose();
            g.Dispose();
            GC.Collect();
        }
    }

    private static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
            if (encoders[j].MimeType == mimeType)
                return encoders[j];
        return null;
    }

}