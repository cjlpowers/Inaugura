using System;
using System.Drawing;

namespace Inaugura
{
	#region LogEventArgs
	/// <summary>
	/// The log event args
	/// </summary>
	public class LogEventArgs : EventArgs
	{
		private string mLogText; // The 

		/// <summary>
		/// The text
		/// </summary>
		/// <value></value>
		public string Text
		{
			get { return mLogText; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="service">The text to log</param>
		public LogEventArgs(string logText)
		{
			if (logText == null)
				throw new ArgumentNullException("logText", "The logText argument can not be null");
			this.mLogText = logText;
		}
	}
	#endregion

	public delegate void LogHandler(object sender, LogEventArgs e);

	public class Log
	{
		private static int padding = 0;
		private static Font mFont;
		private static Color mColor = Color.Black;
		
		static public event LogHandler LogText;
		static public event LogHandler LogError;


		static public Font Font
		{
			get
			{
				return Log.mFont;
			}
			set
			{
				Log.mFont = value;
			}
		}

		static public Color Color
		{
			get
			{
				return Log.mColor;
			}
			set
			{
				Log.mColor = value;
			}
		}

		public Log()
		{
		}

		static public void AddLog(string text)
		{
			if (LogText != null)
			{
				lock (LogText)
				{

					LogEventArgs args = new LogEventArgs("".PadLeft(padding, ' ') + text);
					Inaugura.Threading.Helper.ThreadSafeInvoke(LogText, true, new object[] { null, args });
				}
			}
		}

		static public void AddError(string text)
		{
			if (LogError != null)
			{
				lock (LogText)
				{
					LogEventArgs args = new LogEventArgs("".PadLeft(padding, ' ') + text);
					Inaugura.Threading.Helper.ThreadSafeInvoke(LogError, true, new object[] { null, args });
				}
			}
		}

        static public void AddException(Exception exception)
        {
            AddError(exception.ToString());
        }

		static public void Left()
		{
			if (LogText != null)
			{
				lock (LogText)
				{
					if (padding > 0)
						padding -= 2;
				}
			}
		}

		static public void Right()
		{
			if (LogText != null)
			{
				lock (LogText)
				{
					padding += 2;
				}
			}
		}
	}
}
