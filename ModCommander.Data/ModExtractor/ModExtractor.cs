using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ModCommander.Utils;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;
//using DevILNet = DevIL;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Diagnostics;
using ModCommander.Utils.Helper;

namespace ModCommander.Data
{
    public class ModExtractor
    {
        #region Const

        const string MOD_DESC_FILENAME = "modDesc.xml";

        #endregion Const

        #region Static

        static object locker = new object();
        static Logger log = LogManager.GetCurrentClassLogger();

        #endregion Static

        //#region Fields

        //int count;
        //int current;

        //#endregion Fields

        //#region Events

        //public delegate void StatusUpdateHandler(ModExtractorServiceStatusUpdateEventArgs status);
        //public event StatusUpdateHandler OnUpdateStatus = delegate { };

        //void RaiseUpdateStatus(int count, int currentIndex, string fileName)
        //{
        //    OnUpdateStatus(new ModExtractorServiceStatusUpdateEventArgs(count, currentIndex, fileName));
        //}

        //#endregion

        public List<ModData> LoadModsFromDirectoryAsync(string pathToDirectory)
        {
            List<ModData> result = new List<ModData>();

            try
            {
                var paths = new DirectoryInfo(pathToDirectory).GetFiles().Where(x => x.Extension.ToLowerInvariant() == ".zip" || x.Extension.ToLowerInvariant() == ".disabled");

                //count = paths.Count();

                //var moddetails = paths.AsParallel().Select(x => LoadModFromZip(x.FullName));

                Stopwatch watch = Stopwatch.StartNew();


                //foreach (var path in paths.Select(x => x.FullName))
                //{


                Parallel.ForEach<string>(paths.Select(x => x.FullName), (path) =>
                    {
                        try
                        {
                            result.Add(LoadModFromZip(path));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error loading '{0}'".Args(path), ex);
                        }
                    }
                );
                //}

                //foreach (string path in paths.Select(x => x.FullName))
                //{
                //    try
                //    {
                //        result.Add(LoadModFromZip(path));
                //    }
                //    catch (Exception ex)
                //    {
                //        log.Error("Error loading '{0}'".Args(path), ex);
                //    }
                //}


                watch.Stop();

                log.Info("Mods loaded in {0} sec", watch.ElapsedMilliseconds / 1000.0);

                //result.AddRange(moddetails.ToArray());
            }
            catch (Exception ex)
            {
                log.Error("Error while loading mods.", ex);
            }

            return result;
        }

        ModData LoadModFromZip(string zipFileFullName)
        {
            log.Debug("Loading '{0}'", Path.GetFileName(zipFileFullName));

            ModData detail = new ModData();
            string tempDir = Path.Combine(Path.GetTempPath(), "mc", Guid.NewGuid().ToString());

            detail.FullPath = zipFileFullName;
            detail.FileSize = new FileInfo(zipFileFullName).Length;
            detail.Hash = new FileInfo(zipFileFullName).CalcMD5();

            // Fallback title
            detail.Title = Path.GetFileNameWithoutExtension(detail.FileName).ToLowerInvariant();
            detail.Title = detail.Title.ReplaceEx(new string[] { "aaa_", "zzz_", "aaa", "zzz" }, string.Empty);

            // Make any word begin with upercase letter
            detail.Title = Regex.Replace(detail.Title, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");

            // Remove duplicate spaces
            detail.Title.Normalize();

            try
            {
                Directory.CreateDirectory(tempDir);

                if (Unzip(zipFileFullName, tempDir, new string[] { MOD_DESC_FILENAME }, true, true))
                {
                    StringBuilder sb = new StringBuilder();

                    // Clear comments, cause they are often not wellformed
                    foreach (string line in File.ReadAllLines(Path.Combine(tempDir, MOD_DESC_FILENAME)))
                    {
                        string currentLine = new Regex("(?s)<!--.*?-->", RegexOptions.Compiled).Replace(line, "");
                        currentLine = new Regex("[&]+", RegexOptions.Compiled).Replace(line, "");

                        if (string.IsNullOrEmpty(currentLine))
                            continue;

                        sb.AppendLine(currentLine.Trim());
                    }

                    try
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();

                        using (TextReader txtReader = new StringReader(sb.ToString()))
                        using (XmlReader reader = XmlReader.Create(txtReader, settings))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(reader);

                            detail.Title = doc.GetInnerTextNormalized(string.Format("/modDesc/title/{0}", "de"));
                            if (string.IsNullOrEmpty(detail.Title))
                                detail.Title = doc.GetInnerTextNormalized("modDesc/title/en");

                            detail.Description = doc.GetInnerTextNormalized(string.Format("/modDesc/description/{0}", "de"));
                            if (string.IsNullOrEmpty(detail.Description))
                                detail.Description = doc.GetInnerTextNormalized("modDesc/description/en");

                            detail.Author = doc.GetInnerTextNormalized("/modDesc/author");
                            detail.Version = doc.GetInnerTextNormalized("/modDesc/version");
                            detail.IsMultiplayerReady = Convert.ToBoolean(doc.GetInnerTextSave("/modDesc/multiplayer/@supported"));

                            //lock (locker)
                            //{
                            //detail.Image = LoadBitmap(zipFileFullName, tempDir, doc.GetInnerTextSave("/modDesc/iconFilename"));
                            //}

                            foreach (XmlNode node in doc.SelectNodes("/modDesc/storeItems/storeItem"))
                            {
                                StoreItemData shopItem = new StoreItemData();

                                shopItem.Name = node.GetInnerTextSave(string.Format("{0}/name", "de"));
                                if (string.IsNullOrEmpty(shopItem.Name))
                                    shopItem.Name = node.GetInnerTextSave("en/name");

                                shopItem.Price = Convert.ToDouble(node.GetInnerTextSave("price"));
                                shopItem.Upkeep = Convert.ToDouble(node.GetInnerTextSave("dailyUpkeep"));
                                shopItem.MachineType = node.GetInnerTextSave("machineType");
                                shopItem.Brand = node.GetInnerTextSave("brand");

                                shopItem.Description = node.GetInnerTextSave(string.Format("{0}/description", "de"));
                                if (string.IsNullOrEmpty(shopItem.Description))
                                    shopItem.Description = node.GetInnerTextSave("en/description");

                                shopItem.Description = shopItem.Description.Replace("\r\n", " ");

                                //shopItem.IsMultiplayerReady = Convert.ToBoolean(node.GetInnerTextSave("/modDesc/multiplayer/@supported"));
                                shopItem.XmlFileName = node.GetInnerTextSave("xmlFilename");
                                shopItem.ImagePath = node.GetInnerTextSave("image/@active");
                                shopItem.BrandImagePath = node.GetInnerTextSave("image/@brand");

                                lock (locker)
                                {
                                    if (detail.Image == null)
                                    {
                                        detail.Image = LoadBitmap(zipFileFullName, tempDir, shopItem.ImagePath);
                                        shopItem.Image = detail.Image;
                                    }
                                    else
                                    {
                                        shopItem.Image = LoadBitmap(zipFileFullName, tempDir, shopItem.ImagePath);
                                    }

                                    shopItem.BrandImage = LoadBitmap(zipFileFullName, tempDir, shopItem.BrandImagePath);
                                }

                                detail.StoreItems.Add(shopItem);
                            }

                            doc = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while loading {0}".Args(zipFileFullName), ex);
                    }

                    if (detail.Image == null)
                    {
                        detail.Image = null;//Resources.NoPicture;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while loadings mod: " + zipFileFullName, ex);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }

            return detail;
        }

        static bool Unzip(string zipFilename, string outFolder, string[] searchFilenames, bool ignoreInnerPath, bool inRoot = false, string password = null)
        {
            int fileCount = 0;
            bool success = false;
            using (FileStream fs = File.OpenRead(zipFilename))
            {
                using (ZipFile zf = new ZipFile(fs) { IsStreamOwner = true })
                {
                    // AES encrypted entries are handled automatically
                    if (!String.IsNullOrEmpty(password)) { zf.Password = password; }

                    foreach (ZipEntry zipEntry in zf)
                    {
                        // Ignore directories
                        if (!zipEntry.IsFile) { continue; }

                        for (int i = 0; i < searchFilenames.Length; i++)
                        {
                            searchFilenames[i] = searchFilenames[i].ToLowerInvariant();
                        }

                        if (inRoot)
                            ignoreInnerPath = true;


                        String entryFileName = zipEntry.Name.ToLowerInvariant();

                        if (ignoreInnerPath)
                            entryFileName = Path.GetFileName(entryFileName);


                        // filename filter set?
                        if (searchFilenames != null && !searchFilenames.Contains(entryFileName))
                        {
                            continue;
                        }

                        // file has to be in root?
                        if (inRoot && entryFileName.Contains("/")) { continue; }

                        fileCount++;
                        success = true;

                        byte[] buffer = new byte[4096]; // 4K is optimum
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(outFolder, entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                        // of the file, but does not waste memory.
                        // The "using" will close the stream even if an exception occurs.
                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }

                        // found all the files we need?
                        if (searchFilenames != null && searchFilenames.Length == fileCount)
                            break;
                    }
                }
            }

            return success;
        }

        static Bitmap LoadBitmap(string pathToZip, string tempPath, string imageFilename)
        {
            imageFilename = imageFilename.Replace("\r", string.Empty);
            imageFilename = imageFilename.Replace("\n", string.Empty);

            List<string> fileNames = new List<string>();
            Bitmap result = null;
            try
            {

                if (Path.GetExtension(imageFilename) == ".png")
                {
                    fileNames.Add(Path.ChangeExtension(imageFilename, ".dds"));
                }
                else if (Path.GetExtension(imageFilename) == ".dds")
                {
                    fileNames.Add(Path.ChangeExtension(imageFilename, ".png"));
                }

                fileNames.Add(imageFilename);

                Unzip(pathToZip, tempPath, fileNames.ToArray(), false);


                foreach (string file in fileNames)
                {
                    if (File.Exists(Path.Combine(tempPath, file)))
                    {
                        //DDSImage img = new DDSImage(File.ReadAllBytes(Path.Combine(tempPath, file)));
                        //result = img.images[0];

                        result = DevIL.DevIL.LoadBitmap(Path.Combine(tempPath, file));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't load image. Zip:{0} File:{1}".Args(pathToZip, imageFilename), ex);
            }

            return result;
        }
    }
}
