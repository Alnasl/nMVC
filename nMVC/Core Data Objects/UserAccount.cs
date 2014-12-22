using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using de.netcrave.nMVC.Session;
using de.netcrave.nMVC.Models;
using de.netcrave.nMVC.Settings;

namespace de.netcrave.nMVC.Accounts
{
	public class UserAccount : DomainObjectBase
	{
		public string UserName;
		public string Password;
		public string Email;
		public string[] LinkCategories;

		public string[] Following;
		public string[] FollowedBy; 

		public string FirstName;
		public string LastName;
		public string NickName;
		public bool EmailVisible = false;
		public bool NameVisible = false;

		public UserAccount ()
		{
		}

		/// <summary>
		/// Gets the client side safe user object. TODO there is probably some interface that can be extended
		/// provided by Newtonsoft json that is a Serialize/Sleep/Wakeup method that would make it so that 
		/// this doesn't have to be called manually when we want a safe object intended to be serialized and 
		/// sent to the client.
		/// 
		/// little messy, needs a converter for every single object it seems
		///  http://stackoverflow.com/questions/22354867/how-to-make-json-net-serializer-to-call-tostring-when-serializing-a-particular
		/// 
		/// Also with regards to "a magic method that gets called by the json serializer onSerialize" this is where 
		/// having Session context switching for the SessionManager would be kinda handy, not that even matters considering
		/// that theres.... not a magic method that works the way I want it to. 
		/// 
		/// was also thinking this could have a SessionIdentity member reference to the SessionIdentity object but that seems silly 
		/// and excessive since this object is meant to represent any user. 
		/// 
		/// </summary>
		/// <returns>The client side safe user object.</returns>
		/// <param name="si">Si.</param>
		public UserAccount GetClientSideSafeUserObject()
		{
			UserAccount zlu = new UserAccount();
			zlu.ZID = ZID;
			zlu.UserName = this.UserName;
			zlu.FirstName = this.FirstName;
			zlu.LastName = this.LastName;
			zlu.Email = (ZID == this.si.UserId) ? this.Email : (EmailVisible) ? this.Email : null;
			zlu.FirstName = (ZID == this.si.UserId) ? this.FirstName : (NameVisible) ? this.FirstName : null;
			zlu.LastName =  (ZID == this.si.UserId) ? this.LastName : (NameVisible) ? this.LastName : null;
			zlu.NickName = this.NickName;
		//	zlu.EmailVisible = this.EmailVisible;
		//	zlu.NameVisible = this.NameVisible;
			return zlu;
		}
			
		#region implemented abstract members of DomainObjectBase

		/// <summary>
		/// Creates this instance
		/// </summary>
		public override DomainObjectBase Create ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Update ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Delete ()
		{
			throw new NotImplementedException ();
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
		public override BackendQueryStatus.ReturnCode Rollback ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

