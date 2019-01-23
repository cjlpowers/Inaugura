using System;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace Inaugura
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class Helper
	{
		[DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private static extern bool EnableWindow(IntPtr hWnd, bool enable);

		// get the file contents
		public static string ReadFileContent(string file)
		{			
			if(!System.IO.File.Exists(file))
				return "";

			System.IO.StreamReader reader = System.IO.File.OpenText(file);
			string str = reader.ReadToEnd();
			reader.Close();
			return str;
		}

		// save the file contents
		public static void SaveFileContent(string file, string contents)
		{
			SaveFileContent(file, StringToBytes(contents));
		}

		// save the file contents
		public static void SaveFileContent(string file, byte[] contents)
		{
			System.IO.FileStream writer = System.IO.File.Create(file);
			writer.Write(contents,0,contents.Length);
			writer.Close();						
		}

		public static bool IsNumeric(string str)
		{
			return true;
		}		

		public static void EnableControl(System.Windows.Forms.Control control, bool enable)
		{
			Helper.EnableWindow((IntPtr)control.Handle, enable);
		}


		// save the file contents
		public static byte[] ZipData(byte[] contents)
		{
			using (System.IO.MemoryStream result = new System.IO.MemoryStream())
			{
				using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(result, System.IO.Compression.CompressionMode.Compress))
				{
					gzip.Write(contents, 0, contents.Length);					
				}
				result.Close();
				return result.ToArray();
			}
		}

		// save the file contents
		public static byte[] UnZipData(byte[] zippedData)
		{		
			using (System.IO.MemoryStream zippedDataStream = new System.IO.MemoryStream(zippedData))	
			{
				using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(zippedDataStream, System.IO.Compression.CompressionMode.Decompress))
				{
					using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
					{
						byte[] buffer = new byte[32768];
						int bytesRead = 0;

						// Loop until we have nothing more to read
						// from the source stream.
						while ((bytesRead = gzip.Read(buffer, 0, buffer.Length)) > 0)
						{
							mem.Write(buffer, 0, bytesRead);
						}
						return mem.ToArray();
					}
					
				}
			}
		}

		public static string BytesToString(byte[] data)
		{
			return System.Text.UnicodeEncoding.Unicode.GetString(data);
		}

		public static byte[] StringToBytes(string data)
		{
			return System.Text.UnicodeEncoding.Unicode.GetBytes(data);			
		}

		/// <summary>
		/// Compresses a string using GZIP
		/// </summary>
		/// <param name="str">The string to compress</param>
		/// <returns>The ziped string byte array</returns>
		public static byte[] CompressString(string str)
		{
			return ZipData(StringToBytes(str));
		}

		/// <summary>
		/// Uncompresses a string using GZIP
		/// </summary>
		/// <param name="zippedString">The binary array of the compressed string</param>
		/// <returns>The uncompressed string</returns>
		public static string UnCompressString(byte[] zippedString)
		{
			return BytesToString(UnZipData(zippedString));			
		}
	}
}
