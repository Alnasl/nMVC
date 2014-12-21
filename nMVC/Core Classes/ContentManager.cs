using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using de.netcrave.nMVC.Settings;
using System.Text.RegularExpressions;
using System.Text;

namespace de.netcrave.nMVC
{
	public class ContentManager
	{
		private static volatile ContentManager instance;
		List<FileSystemWatcher> fswl = new List<FileSystemWatcher>();

		// static content pages preloaded
		public Dictionary<string, ContentFile> content = new Dictionary<string, ContentFile>();

		private ContentManager ()
		{
		}

		public static ContentManager Instance 
		{
			get 
			{
				if (instance == null) 
				{
					instance = new ContentManager ();
				}

				return instance;
			}
		}

		private void WalkDirectoryTree(System.IO.DirectoryInfo root, int depth = 1)
		{
			System.IO.FileInfo[] files = null;
			System.IO.DirectoryInfo[] subDirs = null;
			files = root.GetFiles("*.*");
			if (files != null)
			{
				foreach (System.IO.FileInfo fi in files)
				{
					string ext = fi.Extension.ToLower().Replace('.', ' ').Trim();
					if(!UtilitiesManager.Instance.mimeTypes.ContainsKey(ext))
					{
						LoggingFacility.GetConfigurationDefault().WriteLine("unsupported file type " + fi.FullName);
						continue;

					}

					string filekey = ""; 

					fi.DirectoryName
						.Split(Path.DirectorySeparatorChar)
						.Reverse()
						.Take(depth)
						.Reverse()
						.ToList()
						.ForEach(s => filekey += s + Path.DirectorySeparatorChar);

					this.content.Add(filekey + fi.Name, new ContentFile(fi.FullName, 
						UtilitiesManager.Instance.mimeTypes[ext]));
				}

				// Now find all the subdirectories under this directory.
				subDirs = root.GetDirectories();

				foreach (System.IO.DirectoryInfo dirInfo in subDirs)
				{
					FileSystemWatcher fsw = new FileSystemWatcher(dirInfo.FullName);
					fsw.Changed += HandleChanged;
					fsw.Created += HandleCreated;
					fsw.Deleted += HandleDeleted;
					fsw.EnableRaisingEvents = true;
					fsw.BeginInit();
					fswl.Add(fsw);

					// Resursive call for each subdirectory.
					WalkDirectoryTree(dirInfo, depth+1);
				}
			}            
		}

		/// <summary>
		/// File Changed Handler
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleChanged (object sender, FileSystemEventArgs e)
		{
			var match = this.content.Keys
				.Where(s => new Regex(s.Split('/').Last()).Match(e.Name).Success)
				.ToArray();

			if(this.content.Keys.Any(a => a == match.First()))
			{
				LoggingFacility.GetConfigurationDefault().WriteLine(match.First() + " changed.");
				this.content[match.First()].BeginUpdate();
			}
		}

		/// <summary>
		/// File Deleted Handler
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleDeleted (object sender, FileSystemEventArgs e)
		{
			LoggingFacility.GetConfigurationDefault().WriteLine(e.FullPath + " deleted.");
			this.content[e.FullPath].DisableMe();
		}

		/// <summary>
		/// File Created Handler
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleCreated (object sender, FileSystemEventArgs e)
		{
			LoggingFacility.GetConfigurationDefault().WriteLine(e.FullPath + " created.");
			LoggingFacility.GetConfigurationDefault().WriteLine("TODO, handle created for new content file..");
		}

		/// <summary>
		/// Preloads Content into memory
		/// </summary>
		public void PreloadContent()
		{
			System.IO.DirectoryInfo rootDir = new DirectoryInfo(SettingsManager.Instance.settings.HtdocRoot);
			WalkDirectoryTree(rootDir);
		}			
	}
}

