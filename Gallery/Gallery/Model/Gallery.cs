using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace Galleriet.Model
{
    public class Gallery
    {
        private static readonly Regex ApprovedExtensions;
        private static readonly Regex SantizePath;

        private static string PicsFolder;
        private static string ThumbsFolder;
        
        static Gallery()
        {
            ApprovedExtensions = new Regex(@"^.*\.(gif|jpg|png)$", RegexOptions.IgnoreCase);
              
            var invalidChars = new String(Path.GetInvalidFileNameChars());
            SantizePath = new Regex(String.Format("[{0}]", Regex.Escape(invalidChars)));
            
            PicsFolder = AppDomain.CurrentDomain.GetData("APPBASE").ToString();
            ThumbsFolder = String.Format("{0}/Pictures/Thumbs/", PicsFolder);
            PicsFolder = String.Format("{0}/Pictures/", PicsFolder);            
        }

        public IEnumerable<string> GetImageNames()
        {
            var images = Directory.GetFiles(ThumbsFolder);
            var imageList = new List<string>(100);

            FileInfo fi;
            foreach (var img in images)
            {
                fi = new FileInfo(img);
                imageList.Add(fi.Name);
            }

            imageList.TrimExcess();
            imageList.Sort();
            return imageList;
        }

        public bool ImageExists(string name)
        {
            return File.Exists(PicsFolder + name);
        }

        public bool IsValidImage(Image ext) // denna måste ändras senare
        {
            return (ext.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid ||
                ext.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid ||
                ext.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid);
        }

        public string CheckImageName(string fileName)
        {
            int copyNr = 2; // för formatet filnamn(2).jpg
         
            while (ImageExists(fileName)) // tills det finns ett ledigt namn vid dubletter
            {
                if (copyNr > 2)
                {
                    var matcher = new Regex(@"(\(\d\))"); // så det inte blir filnamn(2)(3).jpg
                    fileName = matcher.Replace(fileName, "");
                }

                fileName = String.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(fileName), copyNr, Path.GetExtension(fileName));
                copyNr++;
            }

            return fileName;
        }

        public string SaveImage(Stream stream, string fileName)
        {
            var image = System.Drawing.Image.FromStream(stream);

            if (IsValidImage(image))
            {
                fileName = SantizePath.Replace(fileName, String.Empty);

                fileName = CheckImageName(fileName);

                var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
                image.Save(String.Format("{0}{1}", PicsFolder, fileName));
                thumbnail.Save(String.Format("{0}Small{1}", ThumbsFolder, fileName));

                return fileName;
            }
            else
            {
                throw new ArgumentException("Vi tar endast emot jpg, png och gif bilder. Absolut inget annat!");
            }
        }
    }
}