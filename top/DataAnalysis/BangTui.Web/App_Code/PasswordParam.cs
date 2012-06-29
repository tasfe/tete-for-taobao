using System;
using System.Text;
using System.Security.Cryptography;


public class PasswordParam
{
    public PasswordParam()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    private Encoding encoding;

    /// <summary>
    /// 获取密匙
    /// </summary>
    public string Key
    {
        get
        {
            return "bangbang";
        }
    }

    /// <summary>
    /// 获取或设置加密解密的编码
    /// </summary>
    public System.Text.Encoding Encoding
    {
        get
        {
            if (encoding == null)
            {
                encoding = System.Text.Encoding.UTF8;
            }
            return encoding;
        }
        set
        {
            encoding = value;
        }
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="strString"></param>
    /// <returns></returns>
    public string Encrypt3DES(string strString)
    {
        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = Encoding.GetBytes(this.Key);
        DES.Mode = CipherMode.ECB;
        DES.Padding = PaddingMode.Zeros;
        ICryptoTransform DESEncrypt = DES.CreateEncryptor();
        byte[] Buffer = encoding.GetBytes(strString);
        return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="strString"></param>
    /// <returns></returns>
    public string Decrypt3DES(string strString)
    {
        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = Encoding.UTF8.GetBytes(this.Key);
        DES.Mode = CipherMode.ECB;
        DES.Padding = PaddingMode.Zeros;
        ICryptoTransform DESDecrypt = DES.CreateDecryptor();
        byte[] Buffer = Convert.FromBase64String(strString);
        return UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
    }
}
