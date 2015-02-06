using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ModCommander.Utils.Helper;
using System.Drawing;
using System.Drawing.Imaging;

namespace ModCommander.Utils.Test
{
    [TestClass]
    public class DdsImageTest
    {
        [TestMethod]
        public void LoadDDS()
        {
            byte[] b = File.ReadAllBytes(@"e:\myfile.dds");

            DDSImage img = new DDSImage(b);


            for (int i = 0; i < img.images.Length; i++)
            {
                img.images[i].Save(@"e:\mipmap-" + i + ".png", ImageFormat.Png);
            }         


            Assert.IsNotNull(img);
        }
    }
}
