using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OrbisSwitch
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);

			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			Application.Run(new OrbisSwitchDlg());
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			ExceptionDlg dlg = new ExceptionDlg(e.Exception);
			dlg.ShowDialog();
		}

		static void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ExceptionDlg dlg = new ExceptionDlg((System.Exception)e.ExceptionObject);
			dlg.ShowDialog();
		}
	}
}