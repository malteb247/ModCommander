namespace ModCommander.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows;

   public static class BitmapExtensions
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource CreateBitmapSource(this Bitmap source)
        {         
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }
    }
}
