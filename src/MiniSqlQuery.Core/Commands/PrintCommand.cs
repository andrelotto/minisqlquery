﻿#region License

// Copyright 2005-2009 Paul Kohler (http://pksoftware.net/MiniSqlQuery/). All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (Ms-PL)
// http://minisqlquery.codeplex.com/license
#endregion

using System;
using System.Windows.Forms;

namespace MiniSqlQuery.Core.Commands
{
	/// <summary>
	/// 	The print command.
	/// </summary>
	public class PrintCommand
		: CommandBase
	{
		/// <summary>
		/// 	Initializes a new instance of the <see cref = "PrintCommand" /> class.
		/// </summary>
		public PrintCommand()
			: base("Print...")
		{
			SmallImage = ImageResource.printer;
		}

		/// <summary>
		/// 	Gets a value indicating whether Enabled.
		/// </summary>
		/// <value>The enabled state.</value>
		public override bool Enabled
		{
			get
			{
				var printable = HostWindow.ActiveChildForm as IPrintableContent;
				if (printable != null)
				{
					var doc = printable.PrintDocument;

					if (doc != null)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// 	Execute the command.
		/// </summary>
		public override void Execute()
		{
			var printable = HostWindow.ActiveChildForm as IPrintableContent;
			if (printable != null)
			{
				var doc = printable.PrintDocument;

				if (doc != null)
				{
					using (var ppd = new PrintDialog())
					{
						ppd.Document = doc;
						ppd.AllowSomePages = true;
						if (ppd.ShowDialog(HostWindow.Instance) == DialogResult.OK)
						{
							doc.Print();
						}
					}
				}
			}
		}
	}
}