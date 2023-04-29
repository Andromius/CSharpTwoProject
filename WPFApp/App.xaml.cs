using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace C_Projekt
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{

			if (!File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\HairdresserDB.db"))
			{
				DB.CreateTables();
			}

			base.OnStartup(e);
		}	
	}
}
