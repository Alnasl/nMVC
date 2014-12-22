using System;
using de.netcrave.nMVC;
using de.netcrave.nMVC.Accounts;

namespace SampleProject
{
	public sealed class CustomUserAccount : UserAccount
	{
		public CustomUserAccount ()
		{
		}

		/// <summary>
		/// Creates this instance
		/// </summary>
		public override de.netcrave.nMVC.Models.DomainObjectBase Create ()
		{
			return base.Create ();
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Delete ()
		{
			return base.Delete ();
		}

		/// <summary>
		/// Rollback this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Rollback ()
		{
			return base.Rollback ();
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Save ()
		{
			return base.Save ();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public override BackendQueryStatus.ReturnCode Update ()
		{
			return base.Update ();
		}

		/// <summary>
		/// Retrieve the specified username and si.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="si">Si.</param>
		public static CustomUserAccount Retrieve(string username, CustomSessionIdentity si) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Users the name exists.
		/// </summary>
		/// <returns>The name exists.</returns>
		/// <param name="username">Username.</param>
		public static BackendQueryStatus.ReturnCode UserNameExists(string username) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieve the specified si.
		/// </summary>
		/// <param name="si">Si.</param>
		public static CustomUserAccount Retrieve(CustomSessionIdentity si) 
		{
			throw new NotImplementedException();
		}
	}
}

