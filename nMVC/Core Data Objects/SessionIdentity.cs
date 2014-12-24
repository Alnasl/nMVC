using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
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
namespace de.netcrave.nMVC.Session 
{
	public class SessionIdentity : DomainObjectBase
	{

		/* TODO alot of properties here couldn't be set internal due to the riak client not being able 
		 * to deserialize them due to not being in this same namespace, need to figure out if deserializing 
		 * in SessionManager manually will be an option but I fail to see how it could be since the deserializer
		 * ...maybe when it gets casted locally to SessionIdentity from object (which is what the deserializer yields) in SessionManager?
		 */

		// guest should be the only privilege related property in this class. 
		// The entire footprint of this class should be as minimal as possible. Guest is necesarry because it is used to by the Session
		// Manager to determine whether a user can authenticate, and whether or not a user may destroy a session.
		private bool _guest = true; // good default
		private Guid _UserId; 
		private string _cookie;
		private string _userName;
		private DateTime _LastUpdated = DateTime.Now;
		private DateTime _Expires = DateTime.Now.AddMinutes(5.0);

		// TODO for now this is needed for login
		[JsonIgnore]
		private HttpListenerContext _ctx; 

		public bool Guest 
		{ 
			get 
			{ 
				return _guest; 
			}  
			internal set 
			{ 
				_guest = value; 
			} 
		}

		public Guid UserId 
		{ 
			get 
			{ 
				return _UserId; 
			} 
			internal set 
			{ 
				_UserId = value; 
			} 
		} 

		public string Cookie 
		{ 
			get 
			{ 
				return _cookie; 
			} 
			internal set 
			{ 
				_cookie = value; 
			} 
		}

		public string UserName 
		{ 
			get 
			{ 
				return _userName; 
			} 
			internal set 
			{ 
				_userName = value; 
			} 
		}

		/// <summary>
		/// Gets the user. TODO needs to be refactored/ reconsidered
		/// </summary>
		/// <value>The user.</value>


		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		[JsonIgnore]
		public HttpListenerContext ctx 
		{
			internal get 
			{
				return _ctx;
			}
			set 
			{
				if(this._ctx == null)
				{
					this._ctx = value;
				}
				else
				{
					nMVCLogger.Instance.Error(
						"can't assign HTTPListener context to identity after it's already been assigned.");
				}
			}
		}
			
		/// <summary>
		/// Gets or sets the last updated.
		/// </summary>
		/// <value>The last updated.</value>
		public DateTime LastUpdated 
		{ 
			get 
			{ 
				return _LastUpdated; 
			} 
			internal set 
			{ 
				if(_LastUpdated == new DateTime()) 
					_LastUpdated = value; 
			}
		}

		/// <summary>
		/// Gets or sets the expires.
		/// </summary>
		/// <value>The expires.</value>
		public DateTime Expires 
		{ 
			get 
			{ 
				return _Expires; 
			} 
			internal set 
			{ 
				if(_Expires == new DateTime()) _Expires = value; 
			} 
		}

		public SessionIdentity ()
		{

		}

		/// <summary>
		/// updates the expiration and modified times of the session identity object
		/// </summary>
		private void Tick()
		{
			_LastUpdated = DateTime.Now;
			// TODO this needs to not add minutes if the distance between expires and last updated is greater than a certain amount
			// also not even bother if never expires (stay logged in) is ticked 
			if(_LastUpdated.AddMinutes(30) >_Expires)
			{
				_Expires = DateTime.Now.AddMinutes(30);
			}
		}

		/// <summary>
		/// Revalidate this instance.
		/// </summary>
		internal bool Revalidate()
		{
			if(_LastUpdated >= _Expires)
			{
				return false;
			}
			Tick();
			return true;
		}

		/// <summary>
		/// Gets cookies representing this session identity object
		/// </summary>
		/// <returns>The cookie.</returns>
		public IEnumerable<Cookie> GetCookies()
		{
			List<Cookie> cookies = new List<Cookie>();

			Cookie a = new Cookie(RESTKeys.SessionCookieId, this.ZID.ToString());
			Cookie b = new Cookie(RESTKeys.SessionCookieToken, this.Cookie);

			a.Expires = this._Expires;
			b.Expires = this._Expires;

			//TODO currently mod_proxy forwarding from an SSL terminating load balancer, but I want all connections 
			// from load balancers to application servers to be SSL as well.
			//a.Secure = true;
			//b.Secure = true;

			cookies.Add(a);
			cookies.Add(b);

			return cookies.AsEnumerable();
		}

		#region implemented abstract members of DomainObjectBase

		public override DomainObjectBase Create ()
		{		
			throw new NotImplementedException();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Update ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Delete ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Save ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Rollback this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Rollback()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}

