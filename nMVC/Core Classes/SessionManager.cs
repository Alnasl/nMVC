using System;
using System.IO;
using System.Net;
using BCrypt.Net;
using System.Security.Cryptography;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using de.netcrave.nMVC.Models;

namespace de.netcrave.nMVC.Session
{
	public sealed class SessionManager : Manager
	{

		private static volatile SessionManager instance;

		private SessionManager ()
		{
		}

		public static new SessionManager Instance 
		{
			get 
			{
				if (instance == null)
				{
					instance = new SessionManager ();
				}
				return instance;
			}
		}					

		/// <summary>
		/// Creates the authenticated user session identity.
		/// </summary>
		/// <returns>The authenticated user session identity.</returns>
		/// <param name="zlu">Zlu.</param>
		internal SessionIdentity CreateAuthenticatedUserSessionIdentity(UserAccount zlu, SessionIdentity CurrentIdentity)
		{
			var valid = CurrentIdentity.Revalidate();

			if(!valid)
			{
				return null;
			}

			CurrentIdentity.UserId = zlu.ZID;
			CurrentIdentity.Guest = false;
			CurrentIdentity.UserName = zlu.UserName;

			var updated = CurrentIdentity.Update();

			if(updated != BackendQueryStatus.ReturnCode.Success)
			{
				return null;
			}
			else
			{
				return CurrentIdentity;
			}
		}

		/// <summary>
		/// Gets the session identity (websocket cookies)
		/// </summary>
		/// <returns>The session identity.</returns>
		/// <param name="coll">Coll.</param>
		/*
		public SessionIdentity GetSessionIdentity(WebSocketSharp.Net.CookieCollection coll)
		{
			if(coll[RESTKeys.SessionCookieId] == null 
				|| string.IsNullOrEmpty(coll[RESTKeys.SessionCookieId].Value))
			{
				//needs a new identity
				return ZombieletObjectRepository<SessionIdentity>.Create();
			}
			else if(coll[RESTKeys.SessionCookieId].Expired)
			{
				//needs a new identity
				return ZombieletObjectRepository<SessionIdentity>.Create();
			}
			else
			{
				//needs an existing identity
				return SessionIdentity.Retrieve(coll[RESTKeys.SessionCookieId].Value, 
					coll[RESTKeys.SessionCookieToken].Value);
			}
		}			
		*/
		/// <summary>
		/// Gets the session identity.(HTTP/s cookies)
		/// </summary>
		/// <returns>The session identity.</returns>
		/// <param name="coll">Coll.</param>
		public SessionIdentity GetSessionIdentity(System.Net.HttpListenerContext ctx)
		{
			SessionIdentity ret;
			System.Net.CookieCollection coll = ctx.Request.Cookies;

			if(coll[RESTKeys.SessionCookieId] == null 
				|| string.IsNullOrEmpty(coll[RESTKeys.SessionCookieId].Value))
			{
				//needs a new identity
				ret = DomainObjectRepository<SessionIdentity>.Create();
			}
			else if(coll[RESTKeys.SessionCookieId].Expired)
			{
				//needs a new identity
				ret = DomainObjectRepository<SessionIdentity>.Create();
			}
			else
			{
				//needs an existing identity
				ret = SessionIdentity.Retrieve(coll[RESTKeys.SessionCookieId].Value, 
					coll[RESTKeys.SessionCookieToken].Value);
			}
			foreach(System.Net.Cookie c in ret.GetCookies())
			{
				ctx.Response.Cookies.Add(c);
			}
			ret.ctx = ctx;
			return ret;
		}
	}
}

