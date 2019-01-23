using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Media
{
	public static class Helper
	{
		public static byte[] ConvertToMP3(byte[] wavData, uint bitRate)
		{
			using (System.IO.MemoryStream waveMem = new System.IO.MemoryStream(wavData))
			{
				using (WaveStream waveStream = new WaveStream(waveMem))
				{
					Console.WriteLine(string.Format("Sample Rate: {0}", waveStream.Format.nSamplesPerSec));
					Console.WriteLine(string.Format("Bits Per Sample: {0}", waveStream.Format.wBitsPerSample));
					Console.WriteLine(string.Format("Channels: {0}", waveStream.Format.nChannels));
					Console.WriteLine(string.Format("Block Align: {0}", waveStream.Format.nBlockAlign));
					Console.WriteLine(string.Format("Format Tag: {0}", waveStream.Format.wFormatTag));
					Console.WriteLine(string.Format("Avg Bytes per Sec: {0}", waveStream.Format.nAvgBytesPerSec));
										

					StringBuilder str = new StringBuilder();
					List<String> strList = new List<string>();
					

					byte[] wavByteData;
					using (System.IO.MemoryStream tempMem = new System.IO.MemoryStream())
					{
						byte[] buff = new byte[1024];
						int read = 0;
						while ((read = waveStream.Read(buff, 0, buff.Length)) > 0)
						{							
							for (int i = 0; i < read; i++ )
							{
								if (waveStream.Format.wBitsPerSample == 8)
								{									
									//short shortVal = (short)(((buff[i] ^ 0x80) << 8));
									//short notShort = (short)(~shortVal);
									byte upper = (byte)(0);
									byte lower = (byte)((buff[i] ^ 0x80));
									//short newShort = (short)(upper << 8 + lower);
									

									//Console.WriteLine("{0} -> {3} -> invert = {1} -> 2's = {2} -> upper {4} - > lower {5} -> {6}", buff[i], notShort, short2s, newShort, upper, lower, shortVal);
									//Console.ReadLine();
									tempMem.WriteByte(upper); // MSB
									tempMem.WriteByte(lower);
								}
								else
								{									
									tempMem.WriteByte(buff[i]);
								}
							}
						}						
						wavByteData = tempMem.ToArray();
						//WriteBuffer("Output.txt",wavByteData,16);
					}
					if (waveStream.Format.wBitsPerSample == 8)
					{
						waveStream.Format.wBitsPerSample = 16;
						waveStream.Format.nBlockAlign = 2;
						waveStream.Format.nAvgBytesPerSec = waveStream.Format.nSamplesPerSec * waveStream.Format.nBlockAlign;
					}

					Console.WriteLine(string.Format("Sample Rate: {0}", waveStream.Format.nSamplesPerSec));
					Console.WriteLine(string.Format("Bits Per Sample: {0}", waveStream.Format.wBitsPerSample));
					Console.WriteLine(string.Format("Channels: {0}", waveStream.Format.nChannels));
					Console.WriteLine(string.Format("Block Align: {0}", waveStream.Format.nBlockAlign));
					Console.WriteLine(string.Format("Format Tag: {0}", waveStream.Format.wFormatTag));
					Console.WriteLine(string.Format("Avg Bytes per Sec: {0}", waveStream.Format.nAvgBytesPerSec));

					using (System.IO.MemoryStream mp3Mem = new System.IO.MemoryStream(1024))
					{
						using (Mp3.Mp3Writer writer = new Inaugura.Media.Mp3.Mp3Writer(mp3Mem, waveStream.Format, new Lame.BE_CONFIG(waveStream.Format, bitRate)))
						{
							writer.Write(wavByteData);
							return mp3Mem.ToArray();
						}
					}
				}
			}
		}

		public static void WriteBuffer(string fileName, byte[] buffer, int mode)
		{
			List<String> strList = new List<string>();
			if (mode == 8)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					strList.Add(String.Format("[{0}] {1}", i, buffer[i]));
				}
			}
			else if (mode == 16)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					if (i % 2 == 0)
						strList.Add(String.Format("[{0}] {1}:{2}", i, buffer[i], buffer[i + 1]));
				}
			}
			else
				throw new NotSupportedException("The mode was not supported");

			System.IO.File.WriteAllLines(fileName, strList.ToArray());
		}
	}
}
