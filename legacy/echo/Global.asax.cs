using System;
using System.IO;
using System.Threading;
using System.Xml;

using Echo.Maths;
using Echo.Objects;

namespace Echo.Web
{
	public class Global : System.Web.HttpApplication
	{
		private static bool isInitialised;
		private static bool isShutdown;
		private static Universe universe;
		private static FileSystemWatcher dataWatcher;
		private static string dataPath;

		public Global()
		{
			BeginRequest += OnBeginRequest;
			Disposed += OnShutdown;
		}

		public static Universe Universe
		{
			get { return universe; }
		}

		private static void OnShutdown(object sender, EventArgs e)
		{
			lock (typeof(Global))
			{
				if ( isShutdown == false )
					Shutdown();
			}
		}

		private void OnBeginRequest(object sender, EventArgs e)
		{
			lock (typeof(Global))
			{
				if ( isInitialised == false )
					Initialise();
			}
		}

		private void Initialise()
		{
			isShutdown = false;

			Rand.Initialise(0);
			universe = new Universe();

			dataPath = Server.MapPath("~/app_data");

			var templateFilename = Path.Combine(dataPath, "templates.xml");
			var stateFilename = Path.Combine(dataPath, "universe.xml");
			var date = DateTime.Now;

			if ( File.Exists(templateFilename) )
			{
				var templates = new XmlDocument();
				templates.Load(templateFilename);

				universe.ObjectFactory.Load(templates.SelectRootElement());
			}

			if ( File.Exists(stateFilename) )
			{
				string backupName = string.Format("{0:yyyyMd_HHmmss}_universe.xml", date);
				File.Copy(stateFilename, Path.Combine(dataPath, backupName));

				var state = new XmlDocument();
				state.Load(stateFilename);

				universe.LoadState(state.SelectRootElement());
			}

			dataWatcher = new FileSystemWatcher(dataPath, "*.xml");
			dataWatcher.Changed += UniverseChanged;
			dataWatcher.EnableRaisingEvents = true;

			isInitialised = true;
		}

		private static void UniverseChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Changed)
				return;

			var name = Path.GetFileNameWithoutExtension(e.FullPath);

			var data = new XmlDocument();
			var attempts = 0;
			while ( attempts < 5 )
			{
				try
				{
					data.Load(e.FullPath);
					switch ( name )
					{
						case "universe":
							universe.LoadState(data.SelectRootElement());
							break;

						case "templates":
							universe.ObjectFactory.Load(data.SelectRootElement());
							break;
					}

					return;
				}
				catch (IOException)
				{
				}

				attempts++;
				Thread.Sleep(200);
			}
		}

		private static void Shutdown()
		{
			try
			{
				isInitialised = false;
				SaveTemplates();
				SaveState();
			}
			finally
			{
				isShutdown = true;
			}
		}

		public static void SaveTemplates()
		{
			var templates = universe.ObjectFactory.Save();
			try
			{
				dataWatcher.EnableRaisingEvents = false;

				var templatesFileName = Path.Combine(dataPath, "templates.xml");

				if ( File.Exists(templatesFileName) )
				{
					string backupName = string.Format("{0:yyyyMd_HHmmss}_templates.xml", DateTime.Now);
					File.Copy(templatesFileName, Path.Combine(dataPath, backupName));
				}

				using ( var writer = File.CreateText(templatesFileName) )
				{
					var xtw = new XmlTextWriter(writer) { Formatting = Formatting.Indented };
					templates.WriteTo(xtw);
				}
			}
			finally
			{
				dataWatcher.EnableRaisingEvents = true;
			}
		}

		public static void SaveState()
		{
			var state = universe.SaveState();

			try
			{
				dataWatcher.EnableRaisingEvents = false;

				using ( var writer = File.CreateText(Path.Combine(dataPath, "universe.xml")) )
				{
					var xtw = new XmlTextWriter(writer) { Formatting = Formatting.Indented };
					state.WriteTo(xtw);
				}
			}
			finally
			{
				dataWatcher.EnableRaisingEvents = true;
			}
		}
	}
}