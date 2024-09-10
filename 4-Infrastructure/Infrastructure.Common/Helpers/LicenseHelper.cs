using Hardware.Info;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Common.Helpers
{
    public class LicenseHelper
    {
        private static string FilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "Config", "License.conf");
        static readonly IHardwareInfo hardwareInfo = new HardwareInfo();


        /// <summary>
        /// 检查是否已经取得授权的级别
        /// </summary>
        /// <returns></returns>
        public static bool CheckLicense()
        {
            var licenseStr = ReadLicenseFile();

            if (!string.IsNullOrWhiteSpace(licenseStr)) { return true; } //TODO:临时设置为只要有值就行

            string encrypt = GetMachineCode();
            var des = EncryptHelper.DesEncrypt(GetHardwareInfo());
            return licenseStr == des;

        }

        /// <summary>
        /// 取得硬件信息
        /// </summary>
        /// <returns></returns>
        private static string GetHardwareInfo()
        {
            hardwareInfo.RefreshCPUList();
            var ret = "";
            foreach (var item in hardwareInfo.CpuList)
            {
                ret += item.ToString(); //cpu信息
            }
            ret = ret.Replace(Environment.NewLine, ""); //去掉换行符
            return ret;
        }

        /// <summary>
        /// 取得用户设备的机器码
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            MD5 md5 = MD5.Create();
            string cpuInfo = GetHardwareInfo();
            string guid = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(cpuInfo)));
            return guid.Replace("-", "");
        }
        public static string GetLicense()
        {
            string cpuInfo = GetHardwareInfo();
            var des = EncryptHelper.DesEncrypt(cpuInfo);
            return des;
        }
        /// <summary>
        /// 读取用户保存的License文件
        /// </summary>
        /// <returns></returns>
        public static string ReadLicenseFile()
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }
            using (StreamReader sr = new StreamReader(FilePath))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 存盘用户输入的License
        /// </summary>
        /// <param name="confStr"></param>
        /// <returns></returns>
        public static bool SaveLicenseFile(string confStr)
        {
            if (File.Exists(FilePath) == false)
            {
                var DirName = Path.GetDirectoryName(FilePath);
                if (string.IsNullOrWhiteSpace(DirName)) return false;
                Directory.CreateDirectory(DirName);
            }
            using (StreamWriter sw = new StreamWriter(FilePath, false))
            {
                sw.Write(confStr);
            }
            return true;
        }

        /// <summary>
        /// 检查当前输入的授权码是否正确
        /// </summary>
        /// <param name="licenseStr"></param>
        /// <returns></returns>
        public static bool CheckCode(string licenseStr)
        {
            if (licenseStr == "" || string.IsNullOrEmpty(licenseStr))
            {
                return false;
            }
            string encrypt = GetMachineCode();
            if (licenseStr == EncryptHelper.DesEncrypt(encrypt))
            {
                return true;
            }
            else return false;
        }
    }
}
