using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace File_TransferConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string _ftpURL = "ftp://192.168.0.40:212/";         //Host URL or address of the FTP server
            string _UserName = "ftpuser";             //User Name of the FTP server
            string _Password = "exalca@123";          //Password of the FTP server
            string _ftpDirectory = "ApprovedFiles";      //The directory in FTP server where the file will be uploaded
            string _FileName = "220020180307125814_0_NPO.tiff";         //File name, which one will be uploaded
            string _ftpDirectoryProcessed = "OUTBOX"; //The directory in FTP server where the file will be moved
            //MoveFile(_ftpURL, _UserName, _Password, _ftpDirectory, _ftpDirectoryProcessed, _FileName);



            //string Checksum = GetChecksum(@"C:\Users\80056\Desktop\FileWatcher3\UpdateJsonCode.txt");
            //AppendSmthng(@"C:\Users\80056\Desktop\FileWatcher3\UpdateJsonCode.txt");
            //string Checksum2 = GetChecksum(@"C:\Users\80056\Desktop\FileWatcher3\UpdateJsonCode.txt");
            //if (Checksum == Checksum2)
            //{
            //    Console.WriteLine("Same");
            //}
            //else
            //    Console.WriteLine("NotSame");
        }

        private static string GetChecksum(string m_fileinput)
        {
            try
            {
                string m_checksum;
                using (FileStream stream = File.OpenRead(m_fileinput))
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(stream);
                    m_checksum =
                      BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
                return m_checksum;
            }
            catch (Exception ex)
            {
                //using (StreamWriter swerr = File.AppendText(m_errors))
                //{
                //    //Logger.LogMessage(ex.Message.ToString(), swerr);
                //    swerr.Close();
                //}
                return "unable to retrieve checksum";
            }
        }

        public static void AppendSmthng(string file)
        {
            try
            {
                string GetAllText = System.IO.File.ReadAllText(file);
                System.IO.File.WriteAllText(file, GetAllText + "sumanth");
            }
            catch (Exception ex)
            {

            }
        }

        private static void FileFromFTP() //Download from FTP
        {
            try
            {
                int bytesRead = 0;
                byte[] buffer = new byte[2048];
                FtpWebRequest request = CreateFtpWebRequest("ftp://192.168.0.40:212/ApprovedFiles/", "ftpuser", "exalca@123", true);
                //request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                Stream reader = request.GetResponse().GetResponseStream();
                FileStream fileStream = new FileStream(@"C:\Users\80056\Desktop\FileWatcher\sample.tif", FileMode.Create);
                while (true)
                {
                    bytesRead = reader.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private static FtpWebRequest CreateFtpWebRequest(string ftpDirectoryPath, string userName, string password, bool keepAlive = false)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpDirectoryPath));
            //Set proxy to null. Under current configuration if this option is not set then the proxy that is used will get an html response from the web content gateway (firewall monitoring system)
            request.Proxy = null;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = keepAlive;
            request.Credentials = new NetworkCredential(userName, password);
            return request;
        }

        private static void FileFromSFTP() //Download from SFTP
        {
            try
            {
                string host = @"ftphost";
                string username = "user";
                string password = "********";
                string localFileName = System.IO.Path.GetFileName(@"localfilename");
                string remoteDirectory = "/export/";
                using (var sftp = new SftpClient(host, username, password))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(remoteDirectory);
                    foreach (var file in files)
                    {
                        using (Stream fileStream = File.Create(file.ToString()))
                        {
                            sftp.DownloadFile("/source/remote/path/file.zip", fileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void FileFromShareFolder() //Download from Share Folder
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private static void FileToFTP() //Uplaod to FTP
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private static void FileToSFTP() //Uplaod to SFTP 
        {
            try
            {
                var host = "whateverthehostis.com";
                var port = 22;
                var username = "username";
                var password = "passw0rd";

                // path for file you want to upload
                var uploadFile = @"c:yourfilegoeshere.txt";

                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        using (var fileStream = new FileStream(uploadFile, FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(fileStream, Path.GetFileName(uploadFile));
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void FileToShareFolder() //Uplaod Share Folder 
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        //private static void MoveFile(string ftpURL, string UserName, string Password, string ftpDirectory, string ftpDirectoryProcessed, string FileName)
        //{
        //    try
        //    {
        //        //FTP Server URL.
        //        string ftp = "ftp://192.168.0.40:212/";

        //        //FTP Folder name. Leave blank if you want to list files from root folder.
        //        string ftpFolder = "ApprovedFiles/";

        //        try
        //        {
        //            //Create FTP Request.
        //            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder);
        //            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

        //            //Enter FTP Server credentials.
        //            request.Credentials = new NetworkCredential("ftpuser", "exalca@123");
        //            request.UsePassive = true;
        //            request.UseBinary = true;
        //            request.EnableSsl = false;

        //            //Fetch the Response and read it using StreamReader.
        //            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //            List<string> AllFiles = new List<string>();
        //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                //Read the Response as String and split using New Line character.
        //                //entries = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //                AllFiles = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //            }
        //            response.Close();

        //            foreach (string entry in AllFiles)
        //            {
        //                int bytesRead = 0;
        //                byte[] buffer = new byte[2048];
        //                string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
        //                if(splits[3].Contains('.'))
        //                {
        //                    FileStream fileStream = new FileStream(@"C:\Users\80056\Desktop\FileWatcher\sample.tif", FileMode.Create);
        //                    while (true)
        //                    {
        //                        bytesRead = reader.Read(buffer, 0, buffer.Length);
        //                        if (bytesRead == 0)
        //                            break;
        //                        fileStream.Write(buffer, 0, bytesRead);
        //                    }
        //                    fileStream.Close();
        //                }
        //            }

        //            ////Create a DataTable.
        //            //DataTable dtFiles = new DataTable();
        //            //dtFiles.Columns.AddRange(new DataColumn[3] { new DataColumn("Name", typeof(string)),
        //            //                                new DataColumn("Size", typeof(decimal)),
        //            //                                new DataColumn("Date", typeof(string))});

        //            ////Loop and add details of each File to the DataTable.
        //            //foreach (string entry in entries)
        //            //{
        //            //    string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
        //            //    //Determine whether entry is for File or Directory.
        //            //    bool isFile = splits[0].Substring(0, 1) != "d";
        //            //    bool isDirectory = splits[0].Substring(0, 1) == "d";

        //            //    //If entry is for File, add details to DataTable.
        //            //    if (isFile)
        //            //    {
        //            //        dtFiles.Rows.Add();
        //            //        dtFiles.Rows[dtFiles.Rows.Count - 1]["Size"] = decimal.Parse(splits[2]) / 1024;
        //            //        dtFiles.Rows[dtFiles.Rows.Count - 1]["Date"] = string.Join(" ", splits[0], splits[1]);
        //            //        string name = string.Empty;
        //            //        //for (int i = 8; i < splits.Length; i++)
        //            //        //{
        //            //        name = string.Join(" ", name, splits[3]);
        //            //        //}
        //            //        dtFiles.Rows[dtFiles.Rows.Count - 1]["Name"] = name.Trim();
        //            //    }
        //            //}

        //            ////Bind the GridView.
        //            ////gvFiles.DataSource = dtFiles;
        //            ////gvFiles.DataBind();
        //        }
        //        catch (WebException ex)
        //        {
        //            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}
    }
}
