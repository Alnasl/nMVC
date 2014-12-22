using System;
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
			return base.Create ();
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
	}
}

