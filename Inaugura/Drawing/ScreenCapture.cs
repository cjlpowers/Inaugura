using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Inaugura.Drawing
{
	/// <summary>
	/// Summary description for ScreenCapture.
	/// </summary>
	public class ScreenCapture
	{
		#region Internal Classes
		private class GDI32
		{
			[DllImport("GDI32.dll")]
			public static extern bool BitBlt(int hdcDest,int nXDest,int nYDest, int nWidth,int nHeight,int hdcSrc, int nXSrc,int nYSrc,int dwRop);
			[DllImport("GDI32.dll")]
			public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);
			[DllImport("GDI32.dll")]
			public static extern int CreateCompatibleDC(int hdc);
			[DllImport("GDI32.dll")]
			public static extern bool DeleteDC(int hdc);
			[DllImport("GDI32.dll")]
			public static extern bool DeleteObject(int hObject);
			[DllImport("GDI32.dll")]
			public static extern int GetDeviceCaps(int hdc,int nIndex);
			[DllImport("GDI32.dll")]
			public static extern int SelectObject(int hdc,int hgdiobj);
				
		}

		private class User32
		{
			[DllImport("User32.dll")]
			public static extern int GetDesktopWindow();
			[DllImport("User32.dll")]
			public static extern int GetWindowDC(int hWnd);
			[DllImport("User32.dll")]
			public static extern int ReleaseDC(int hWnd, int hDC);
		}
		#endregion


		public static Bitmap CaptureScreen()
		{
			int hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow()), hdcDest = GDI32.CreateCompatibleDC(hdcSrc), hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, GDI32.GetDeviceCaps(hdcSrc, 8), GDI32.GetDeviceCaps(hdcSrc, 10));

			GDI32.SelectObject(hdcDest, hBitmap);
			GDI32.BitBlt(hdcDest, 0, 0, GDI32.GetDeviceCaps(hdcSrc, 8), GDI32.GetDeviceCaps(hdcSrc, 10), hdcSrc, 0, 0, 0x00CC0020);

			Bitmap image = new Bitmap(Image.FromHbitmap(new IntPtr(hBitmap)), Image.FromHbitmap(new IntPtr(hBitmap)).Width, Image.FromHbitmap(new IntPtr(hBitmap)).Height);
			
			User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			GDI32.DeleteDC(hdcDest);
			GDI32.DeleteObject(hBitmap);

			return image;
		}

		public static void CaptureScreenToFile(string fileName, ImageFormat imageFormat)
		{
			Bitmap image = ScreenCapture.CaptureScreen();
			image.Save(fileName, imageFormat);
		}
	}
}