using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using de.netcrave.nMVC.Settings;
using de.netcrave.nMVC.Models;

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
				.Select(s => new KeyValuePair<string, Match>(
					SettingsManager.Instance.settings.HtdocRoot + routes[s.ToString()],
					s.Match(addr.AbsolutePath)))
				.ToList()
				.ForEach(f => ret.Add(f.Key + ((f.Value.Value.Count() > 1) ? f.Value.Value.Replace('/', ' ').Trim() : "")));
		
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

