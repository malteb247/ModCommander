namespace ModCommander.Utils
{
    using System;
    using System.IO;
    using System.Text;

    public static class IOExtensions
    {
        public static string CalcMD5(this FileInfo fileInfo)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            using (var input = File.OpenRead(fileInfo.FullName))
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    md5.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                }

                //byte[] filenameBytes = Encoding.GetEncoding(28591).GetBytes(fileInfo.Name);

                byte[] filenameBytes = Encoding.GetEncoding(28591).GetBytes(Path.GetFileNameWithoutExtension(fileInfo.Name));

                md5.TransformBlock(filenameBytes, 0, filenameBytes.Length, buffer, 0);

                // We have to call TransformFinalBlock, but we don't have any
                // more data - just provide 0 bytes.
                md5.TransformFinalBlock(buffer, 0, 0);

                byte[] md5Hash = md5.Hash;

                return BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            }
        }

    }
}
