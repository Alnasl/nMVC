using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using de.netcrave.nMVC.Session;
using de.netcrave.nMVC.Settings;
using de.netcrave.nMVC.Models;
using de.netcrave.nMVC.Logger;

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
namespace de.netcrave.nMVC.RESTCallbackManager
{
	public class RESTCallbackManager : Manager
	{
		/// <summary>
		/// The REST GET API handler mappings (Maps a string in all caps to a method)
		/// The method must accept the following arguments: 
		/// - a dictionary<string, string> which will contain the HTTP REQUEST Headers
		/// - a SessionIdentity object which contains information about the authenticated/unauthenticated user's session
		/// The method must return an object of type HTTPResponse. These methods are mostly declared in ServiceManager and
		/// AccountManager respectively. 
		/// </summary>

		public Dictionary<string, Func<Dictionary<string, string>, SessionIdentity, HttpResponse>> GETCallbacks =
			new Dictionary<string, Func<Dictionary<string, string>, SessionIdentity, HttpResponse>> ();

		//POST Rest API Callbacks
		public  Dictionary<string, Func<Dictionary<string, string>, SessionIdentity, Dictionary<string, string>, HttpResponse>> POSTCallbacks =
			new Dictionary<string, Func<Dictionary<string, string>, SessionIdentity, Dictionary<string, string>, HttpResponse>> ();

		// TODO REST parameter / FORM Post / GET Vars Data assertions and validators
		public Dictionary<string, Func<Dictionary<string, string[]>, bool>> RequiredDataValidators = 
			new Dictionary<string, Func<Dictionary<string, string[]>, bool>>();

		private static RESTCallbackManager instance;

		private RESTCallbackManager  ()
		{
		}

		public static new RESTCallbackManager Instance 
		{
			get 
			{
				if (instance == null)
				{
					instance = new RESTCallbackManager  ();
				}
				return instance;
			}
		}

		/// <summary>
		/// Registers the global callback handlers.
		/// reflection loader written by Paige =)
		/// This is how it works: 
		/// - First it clooks for classes that have a [RESTService] attribute applied to them
		/// - For the ones that do it looks for a memeber called Instance (currently nothing else supportted)
		/// 
		/// </summary>
		public void RegisterGlobalCallbackHandlers()
		{
			var services = Assembly
				.GetCallingAssembly()
				.GetTypes()
				.Union(
					Assembly
					.GetExecutingAssembly()
					.GetTypes()).ToArray();

			foreach(Type t in services)
			{
				if(t.GetCustomAttributes(true).Any(a => a.GetType() == typeof(RESTService)))
				{
					nMVCLogger.Instance.Debug(new { t }, "Adding REST Service");
					if(t.GetProperty("Instance") != null)
					{
						//TODO maybe also add support for instantiating a non-singleton class marked as being 
						// a REST service or make an abstract class or something... wonder if abstract 
						// enforces attibutes
						var instance = t.GetProperty("Instance").GetValue(null, null);
						var contracts = instance.GetType().GetMethods();
						foreach(MethodInfo mi in contracts)
						{
							//TODO if binding flags private || static WARN
							if(mi.GetCustomAttributes(true).Any(a => a.GetType() == typeof(RESTContractAttribute)))
							{
								//Mark
								RESTContractAttribute data = (RESTContractAttribute) mi.GetCustomAttributes(true)
									.Single(a => a.GetType() == typeof(RESTContractAttribute));

								nMVCLogger.Instance.Debug(new { data, t, mi }, "Adding data contract");

								switch(data.HTTPMethod)
								{

								case RESTContractAttribute.RequestType.GET:
									// best way I could figure out how to do this was to wrap a MethodInfo invoke
									// inside of a lambda, seems to work ok.
									Func<Dictionary<string, string>, SessionIdentity, HttpResponse> funcGet = 
										(Dictionary<string, string> headers, SessionIdentity si) => 
										(HttpResponse) mi.Invoke(instance, new object[] 
											{ 
												headers, 
												si 
											});

									this.GETCallbacks.Add(data.RequestHandlerName, funcGet);					
									break;

								case RESTContractAttribute.RequestType.POST:

									Func<Dictionary<string, string>, SessionIdentity, Dictionary<string, string>, HttpResponse> funcPost = 
										(Dictionary<string, string> headers, SessionIdentity si, Dictionary<string, string> postvars) => 
										(HttpResponse)mi.Invoke(instance, new object[] { headers, si, postvars });

									this.POSTCallbacks.Add(data.RequestHandlerName, funcPost);
									break;

								case RESTContractAttribute.RequestType.PUT:															
								case RESTContractAttribute.RequestType.DELETE:								
								default:
									nMVCLogger.Instance.Debug(new { data, t, mi }, 
										"TODO: unsupported REST contract based on specified request type");
									break;
								}
							}
							if (mi.GetCustomAttributes(true).Any(a => a.GetType() == typeof(RESTExpectsParam)))
							{
								nMVCLogger.Instance.Warn("TODO RESTExpectsParam needs to be tested more and isn't actually used at the moment. "
									+ "You shouldn't rely on it");
	
								RESTExpectsParam[] eparams = mi.GetCustomAttributes(true)
									.Where(a => a.GetType() == typeof(RESTExpectsParam)).Cast<RESTExpectsParam>().ToArray();

								RESTContractAttribute data = (RESTContractAttribute) mi.GetCustomAttributes(true)
									.Single(a => a.GetType() == typeof(RESTContractAttribute));

								foreach(RESTExpectsParam eparam in eparams)
								{
									nMVCLogger.Instance.Debug(new { data, eparam },"adding data input handler");

								}

								Func<Dictionary<string, string[]>, bool> validator = 
									(Dictionary<string, string[]> idata) =>
									(eparams.AsEnumerable().Where(w => !idata[data.RequestHandlerName]
										.ToList()
										.Contains(w.ExpectKey))
										.Count() == 0)
									? false : true;

								this.RequiredDataValidators.Add(data.RequestHandlerName, validator);
							}
						}
					}
				}
			}
		}
	}
}

