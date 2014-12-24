using System;
using de.netcrave.nMVC;
using de.netcrave.nMVC.Session;

namespace SampleProject
{
	public sealed class CustomSessionIdentity : SessionIdentity
	{
		public CustomSessionIdentity ()
		{
		}

		/// <summary>
		/// Creates this instance
		/// </summary>
		public override de.netcrave.nMVC.Models.DomainObjectBase Create ()
		{
			return CustomSessionIdentity.CreateNew();
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public override de.netcrave.nMVC.BackendQueryStatus.ReturnCode Delete ()
		{
			return base.Delete ();
		}

		/// <summary>
		/// Rollback this instance.
		/// </summary>
		public override de.netcrave.nMVC.BackendQueryStatus.ReturnCode Rollback ()
		{
			return base.Rollback ();
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public override de.netcrave.nMVC.BackendQueryStatus.ReturnCode Save ()
		{
			return base.Save ();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public override de.netcrave.nMVC.BackendQueryStatus.ReturnCode Update ()
		{
			return base.Update ();
		}

		/// <summary>
		/// Gets the session identity.
		/// </summary>
		/// <returns>The session identity.</returns>
		/// <param name="ctx">Context.</param>
		public static CustomSessionIdentity GetSessionIdentity(System.Net.HttpListenerContext ctx)
		{
			CustomSessionIdentity ret;
			System.Net.CookieCollection coll = ctx.Request.Cookies;

			if(coll[RESTKeys.SessionCookieId] == null 
				|| string.IsNullOrEmpty(coll[RESTKeys.SessionCookieId].Value))
			{
				//needs a new identity
				ret = CustomSessionIdentity.CreateNew();
			}
			else if(coll[RESTKeys.SessionCookieId].Expired)
			{
				//needs a new identity
				ret = CustomSessionIdentity.CreateNew();
			}
			else
			{
				//needs an existing identity
				ret = CustomSessionIdentity.Retrieve(coll[RESTKeys.SessionCookieId].Value, 
					coll[RESTKeys.SessionCookieToken].Value);
			}
			foreach(System.Net.Cookie c in ret.GetCookies())
			{
				ctx.Response.Cookies.Add(c);
			}
			ret.ctx = ctx;
			return ret;
		}

		/// <summary>
		/// Retrieve this instance.
		/// </summary>
		public static CustomSessionIdentity Retrieve(string IDCookieID, string ValueOfTokenCookie) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates the new.
		/// </summary>
		/// <returns>The new.</returns>
		public static CustomSessionIdentity CreateNew() 
		{
			throw new NotImplementedException();
		}
	}
}

