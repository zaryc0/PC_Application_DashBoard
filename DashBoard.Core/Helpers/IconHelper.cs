using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class IconHelper
{
    private const uint SHGFI_ICON = 0x000000100;
    private const uint SHGFI_LARGEICON = 0x000000000; // Large icon (48x48)
    private const uint SHGFI_SMALLICON = 0x000000001; // Small icon (32x32)

    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    private static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    public static ImageSource ExtractIconImageSource(string filePath, bool largeIcon = true)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        uint flags = SHGFI_ICON | (largeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);

        SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

        if (shinfo.hIcon == IntPtr.Zero)
            return null;

        using (Icon icon = Icon.FromHandle(shinfo.hIcon))
        {
            return ConvertIconToImageSource(icon);
        }
    }
    public static ImageSource ExtractIconImageSource(List<string> filePath, bool largeIcon = true)
    {
        List<ImageSource> icons = new List<ImageSource>();
        if (filePath.Count > 0)
        {
            icons.Add(ExtractIconImageSource(filePath[1]));
        }
        if (filePath.Count > 1)
        {
            icons.Add(ExtractIconImageSource(filePath[1]));
        }
        if (filePath.Count > 2)
        {
            icons.Add(ExtractIconImageSource(filePath[2]));
        }
        if (filePath.Count > 3)
        {
            icons.Add(ExtractIconImageSource(filePath[3]));
        }

        return icons.Count > 1 ? CombineImages(icons.ToArray()) : icons[0];
    }

    public static ImageSource CombineImages(ImageSource[] images, int imageSize = 64)
    {
        if (images == null || images.Length == 0)
            return null;

        // Determine grid size (2x2)
        int gridSize = 2;
        int finalSize = imageSize * gridSize;

        // Create a RenderTargetBitmap to hold the final image
        RenderTargetBitmap renderBitmap = new RenderTargetBitmap(finalSize, finalSize, 96, 96, PixelFormats.Pbgra32);
        DrawingVisual visual = new DrawingVisual();

        using (DrawingContext dc = visual.RenderOpen())
        {
            for (int i = 0; i < images.Length && i < 4; i++)
            {
                int x = (i % gridSize) * imageSize;  // Column position
                int y = (i / gridSize) * imageSize;  // Row position

                dc.DrawImage(images[i], new Rect(x, y, imageSize, imageSize));
            }
        }

        renderBitmap.Render(visual);
        return renderBitmap;
    }
    public static ImageSource LoadImageFromFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            return null;

        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        bitmap.Freeze(); // Freezing improves performance for UI binding

        return bitmap;
    }

    private static ImageSource ConvertIconToImageSource(Icon icon)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            icon.ToBitmap().Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}