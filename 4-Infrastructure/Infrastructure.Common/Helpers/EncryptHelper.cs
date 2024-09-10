using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace Infrastructure.Common.Helpers
{
    /// <summary>
    /// 通用工具操作类[加密，解密],对字符串的操作
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>
        /// 8位字符的密钥字符串
        /// </summary>
        public static string StrKey = "ET$yuBI0"; //必须为8位
        /// <summary>
        /// 8位字符的初始化向量字符串
        /// </summary>
        public static string StrIv = "242B8E9C"; //加密矢量：只有在CBC解密模式下才适用

        #region -------------------------MD5加密解密-------------------------  
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>值</returns>
        public static string Md5Encrypt(this string input)
        {
            MD5 md5 = MD5.Create();
            var data = Encoding.UTF8.GetBytes(input);
            var encs = md5.ComputeHash(data);
            return BitConverter.ToString(encs).Replace("-", "");
        }

        /// <summary>
        /// MD5加密(为了兼容2.0版系统)获取32位长度的Md5摘要
        /// </summary>
        /// <param name="inputStr">加密字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>值</returns>
        public static string GetMD5_32(this string inputStr, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(inputStr)) return "待加密的字符串不能为空！";
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            byte[] data;
            using (MD5 md5Hash = MD5.Create())
            {
                data = md5Hash.ComputeHash(encoding.GetBytes(inputStr));
            }

            StringBuilder tmp = new StringBuilder();
            foreach (var t in data)
            {
                tmp.Append(t.ToString("x2"));
            }

            return tmp.ToString();
        }

        #endregion


        #region -------------------------DES加密解密-------------------------

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="sourceString">加密数据</param> 
        /// <returns>值</returns>
        public static string DesEncrypt(this string sourceString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceString)) return "";
                byte[] btKey = Encoding.UTF8.GetBytes(StrKey);

                byte[] btIv = Encoding.UTF8.GetBytes(StrIv);

                DES des = DES.Create();
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch (Exception)
                    {
                        return sourceString;
                    }
                }
            }
            catch
            {
                return "DES加密出错";
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptedString">解密数据</param>
        /// <returns>值</returns>
        public static string DesDecrypt(this string encryptedString)
        {
            var btKey = Encoding.UTF8.GetBytes(StrKey);

            var btIv = Encoding.UTF8.GetBytes(StrIv);

            DES des = DES.Create();

            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    var inData = Convert.FromBase64String(encryptedString);
                    using (var cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return "解密错误";
                }
            }
        }
        #endregion


        #region -------------------------JAVA DES加密解密-------------------------

        /// <summary>
        /// JAVA DES加密
        /// </summary>
        /// <param name="value">加密数据</param> 
        /// <returns>值</returns>
        public static string DesEncryptJava(this string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return "";
                value = WebUtility.UrlEncode(value);
                DES des = DES.Create();
                byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(value ?? "");


                des.Key = Encoding.ASCII.GetBytes(StrKey);
                des.IV = Encoding.ASCII.GetBytes(StrKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }

                var s = ret.ToString();
                return s;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// JAVA DES解密
        /// </summary>
        /// <param name="pToDecrypt">解密数据</param> 
        /// <returns>值</returns>
        public static string DesDecryptJava(this string pToDecrypt)
        {
            DES des = DES.Create();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(StrKey);
            des.IV = Encoding.ASCII.GetBytes(StrKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return WebUtility.UrlDecode(Encoding.Default.GetString(ms.ToArray()));
        }
        #endregion


    }
}
