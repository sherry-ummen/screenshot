using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Screenshot
{
    internal class ScreenCapture
    {
        public Image CaptureActiveWindowScreen()
        {
            return CaptureWindow(User32.GetForegroundWindow());
        }


        public Image CaptureWindow(IntPtr handle)
        {
            var rect = new User32.RECT();
            User32.GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }
            return result;
        }


        private readonly static string _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Screenshots");

        public static void FullScreenshotPrimaryMonitor(string filename, ImageFormat format)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                string fullpath = Path.Combine(_folder, filename);

                bitmap.Save(fullpath, format);
            }
        }

        public static void ScreenshotOfActiveWindow(string filename, ImageFormat format)
        {
            ScreenCapture sc = new ScreenCapture();
            Image img = sc.CaptureActiveWindowScreen();

            string fullpath = Path.Combine(_folder, filename);

            img.Save(fullpath, format);
            img.Dispose();
        }
    }
}
