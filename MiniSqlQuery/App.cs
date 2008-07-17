﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MiniSqlQuery.Core;
using MiniSqlQuery.PlugIns;
using System.Threading;
using Castle.Core;
using MiniSqlQuery.Properties;

namespace MiniSqlQuery
{
	static class App
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
#if !DEBUG
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
#endif

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// this DI is 'hard coded'
			ApplicationServices.Instance.Container.AddComponent("AboutForm", typeof(AboutForm));

			ApplicationServices.Instance.LoadPlugIn(new CoreApplicationPlugIn());

			IPlugIn[] plugins = PlugInUtility.GetInstances<IPlugIn>(Environment.CurrentDirectory, Settings.Default.PlugInFileFilter);
			Array.Sort(plugins, new PlugInComparer());
			foreach (IPlugIn plugin in plugins)
			{
				ApplicationServices.Instance.LoadPlugIn(plugin);
			}

			ApplicationServices.Instance.HostWindow.SetArguements(args);
			Application.Run(ApplicationServices.Instance.HostWindow.Instance);
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			if (!(e.Exception is ThreadAbortException))
			{
				HandleException(e.Exception);
			}
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (!(e.ExceptionObject is ThreadAbortException))
			{
				HandleException((Exception)e.ExceptionObject);
			}
		}

		private static void HandleException(Exception e)
		{
			ErrorForm errorForm = new ErrorForm();
			errorForm.SetException(e);
			errorForm.ShowDialog();
			errorForm.Dispose();
		}

    }
}