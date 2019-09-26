using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;

namespace Framework
{
    public class ZipUtil
    {
        public static byte[] compressString(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return compress(buffer);
        }

        public static string decompressString(byte[] data)
        {
            byte[] bytes = decompress(data);
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                DeflaterOutputStream gz = new DeflaterOutputStream(ms);
                //DeflateStream gz = new DeflateStream(ms, CompressionMode.Compress, true);
                //GZipStream gz = new GZipStream(ms, CompressionMode.Compress, true);
                gz.Write(data, 0, data.Length);
                gz.Close();
                byte[] buffer = ms.ToArray();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static byte[] decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                InflaterInputStream gz = new InflaterInputStream(ms);
                //DeflateStream gz = new DeflateStream(ms, CompressionMode.Decompress, true);
                //GZipStream gz = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                int count = 0;
                byte[] buffer = new byte[0x1000];
                while ((count = gz.Read(buffer, 0, buffer.Length)) != 0)
                {
                    msreader.Write(buffer, 0, count);
                }
                gz.Close();
                ms.Close();
                msreader.Position = 0;
                byte[] depress = msreader.ToArray();
                msreader.Close();
                return depress;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 压缩文件夹 
        /// </summary>
        /// <param name="zipOutput">输出zip对象</param>
        /// <param name="source">压缩的文件夹路径</param>
        /// <param name="relativePath">要去除的相对文件路径</param>
        public static void CompressDir(ZipOutputStream zipOutput, string source, string relativePath = "")
        {
            string dir = source.Replace("\\", "/");
            string newDir = dir.Substring(relativePath.Length);
            ZipEntry dirEntry = new ZipEntry(newDir + "/");
            zipOutput.PutNextEntry(dirEntry);
            zipOutput.Flush();

            string[] filenames = Directory.GetFileSystemEntries(source);
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                    CompressDir(zipOutput, file, relativePath);  //递归压缩子文件夹
                else
                    CompressFile(zipOutput, file, relativePath);
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="zipOutput">输出zip对象</param>
        /// <param name="file">压缩的文件路径</param>
        /// <param name="relativePath">要去除的相对文件路径</param>
        public static void CompressFile(ZipOutputStream zipOutput, string file, string relativePath = "")
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] buffer = new byte[4 * 1024];
                file = file.Replace("\\", "/");
                string newFile = file.Substring(relativePath.Length);
                ZipEntry entry = new ZipEntry(newFile);
                entry.IsUnicodeText = true;
                entry.DateTime = DateTime.Now;
                zipOutput.PutNextEntry(entry);

                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zipOutput.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }
    }
}
