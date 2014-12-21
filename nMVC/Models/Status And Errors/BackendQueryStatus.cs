using System;

namespace de.netcrave.nMVC
{
	public class BackendQueryStatus
	{
		public enum ReturnCode
		{
			Success, 
			Failure, 
			Exists, 
			DoesntExist,
			BackendOtherError,
			BackendSaveError,
			BackendGetError,
			NotImplemented,
			BackendUpdateError,

		}
		public BackendQueryStatus ()
		{

		}
	}
}

