using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using de.netcrave.nMVC.Settings;
using de.netcrave.nMVC.Models;

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
namespace de.netcrave.nMVC.RouterManager
{
	public class RouterManager : Manager
	{
		private Dictionary<string, string> routes = new Dictionary<string, string>();
		private static RouterManager instance; 

		public static new RouterManager Instance 
		{
			get 
			{
				if(instance == null)
				{
					instance = new RouterManager();
				}
				return instance;
			}
		}

		private RouterManager ()
		{
		}
			
		/// <summary>
		/// Match a URL request against the routes collection. This function assumes
		/// for all intents and purposes that there could be more than one route to return
		/// but I can't think of any reason why I'd personally ever want that; still it should 
		/// work if it needs to.
		/// </summary>
		/// <returns>The route.</returns>
		/// <param name="addr">Address.</param>
		public string[] FindRoute(Uri addr)
		{
			List<string> ret = new List<string>();

			routes.Keys
				.Select(s => new Regex(s))
				.Where(w => w.Match(addr.AbsolutePath).Success)
				.Select(s => new KeyValuePair<string, Match>(routes[s.ToString()],
					s.Match(addr.AbsolutePath)))
				.ToList()
				.ForEach(f => 
					{
						if(f.Value.Value.Count() > 1)
						{
							if(f.Key.Contains("/"))
							{
								ret.Add(
									SettingsManager.Instance.settings.HtdocRoot 
									+ f.Key + f.Value.Value.Replace('/', ' ').Trim());
							}
							else
							{
								ret.Add(f.Key);
							}
						}
						else
						{
							ret.Add(SettingsManager.Instance.settings.HtdocRoot 
								+ f.Key 
								+ "");
						}
					});

			return ret.ToArray();
		}

		/// <summary>
		/// Add a route
		/// </summary>
		/// <param name="regex">Route RegEx</param>
		/// <param name="destination">Destination</param>
		public void Add(string regex, string destination)
		{
			routes.Add(regex, destination);
		}
	}
}

