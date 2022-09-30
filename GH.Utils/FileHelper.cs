using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GH.Utils
{
    using static GH.Utils.AppHelper;
    public static class FileHelper
    {
        public static void SaveTextToFile(string text, string filePath, bool openDir = false)
        {
            string ext = Path.GetExtension(filePath);
            if (ext == null)
            {
                ext = ".txt";
            }

            filePath = Path.GetFileNameWithoutExtension(filePath) + ext;

            filePath = Path.Combine(StartupPath, ExportFolder, filePath);

            CheckDirectory(filePath);

            File.WriteAllText(filePath, text, Encoding.UTF8);

            if (!File.Exists(filePath) || !openDir)
            {
                return;
            }

            OpenDirrectory(filePath);
        }

        public static void OpenDirrectory(string filePath)
        {
            string args = string.Format("/e, /select, \"{0}\"", filePath);

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = args;
            Process.Start(info);
        }

        public static void CheckDirectory(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

        }
        public static void MoveFile(string dirName, string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            fileName = Path.Combine(StartupPath, dirName, Path.GetFileName(fileName));
            CheckDirectory(fileName);
            if (File.Exists(fileName))
                File.Delete(fileName);
            file.MoveTo(fileName);
        }


        private static string pass = "AbCdEfGh";

        public static string DeCrypt(string filePath)
        {
            string result = null;
            if (File.Exists(filePath))
                try
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider())
                        {
                            cryptic.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                            cryptic.IV = ASCIIEncoding.ASCII.GetBytes(pass);
                            using (CryptoStream crStream = new CryptoStream(stream, cryptic.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                using (StreamReader reader = new StreamReader(crStream))
                                {
                                    result = reader.ReadToEnd();
                                    reader.Close();
                                }
                            }
                        }
                        stream.Close();
                    }
                }
                catch { }
            return result;
        }

        public static void EnCrypt(string filePath, string content)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {

                using (DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider())
                {

                    cryptic.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                    cryptic.IV = ASCIIEncoding.ASCII.GetBytes(pass);

                    using (CryptoStream crStream = new CryptoStream(stream, cryptic.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] data = ASCIIEncoding.ASCII.GetBytes(content);

                        crStream.Write(data, 0, data.Length);

                        crStream.Close();
                    }
                }
                stream.Close();
            }
        }

    }
}
