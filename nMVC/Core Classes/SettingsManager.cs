using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace de.netcrave.nMVC.Settings
{
    public sealed class SettingsManager
    {
		private Settings _settings;
        public Settings settings
        {
            private set
			{
				_settings = value;
			}
            get
			{
				return _settings;
			}
        }

        private static volatile SettingsManager instance;
        private static object syncRoot = new object ();
        

        private SettingsManager ()
        {

            LoadSettings();
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
			// ...
			//Log.WriteLine("loading settings from XML configuration file");
			lock(syncRoot)
			{
	            bool newConfig = false;

	            if (!File.Exists ("settings.xml"))
	            {
	                newConfig = true;
	            }

				/*
				using (FileStream file = File.Open("mime.xml", FileMode.OpenOrCreate))
				{
					var serializer = new XmlSerializer (typeof(object));
					using (XmlReader xr = XmlReader.Create(file))
					{
						var crap = (object)serializer.Deserialize (xr);
						Console.WriteLine("omg");
					}
				}*/

	            using (FileStream file = File.Open("settings.xml", FileMode.OpenOrCreate))
	            {
	                if (newConfig)
	                {
						settings = new Settings();
	                    var serializer = new XmlSerializer (typeof(Settings));
	                    using (XmlWriter writer = XmlWriter.Create(file, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true}))
	                    {
	                        serializer.Serialize (writer, settings);
	                    }
	                }

	                else
	                {
	                    var serializer = new XmlSerializer (typeof(Settings));
	                    using (XmlReader xr = XmlReader.Create(file))
	                    {
	                        this.settings = (Settings)serializer.Deserialize (xr);
	                    }
	                }
	            }
			}
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings ()
        {
			
            throw new NotImplementedException();
        }

        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsManager ();
                }

                return instance;            
            }
        }
    }
}

