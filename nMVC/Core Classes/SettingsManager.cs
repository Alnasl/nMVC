using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

/// <summary>
/// nMVC
/// Author : Paige Thompson (paigeadele@gmail.com / erratic@yourstruly.sx)
/// Copyright 2014 Netcrave Communications
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.

/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.

/// You should have received a copy of the GNU General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
/// </summary>
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

