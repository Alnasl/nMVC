using System;

namespace de.netcrave.nMVC
{
	public class GuruMeditation 
	{
		public enum ErrorCode 
		{
			NotAuthenticated = 0x10,
			RESTHandlerRequestNotFound = 0x11,
			UserNameExists = 0x12,
			PasswordMismatch = 0x13,
			UserNonexistant = 0x14, 

			SessionExpired = 0x20,
			GuestDestroySessionAttempt = 0x21,
			SessionIDNotFound = 0x22,
			SessionCookieIDandSessionTokenCookieMismatch = 0x23,

			NotImplemented = 0x420,
			UserAlreadyAuthenticated = 0x666,

			BackendUserQueryError = 0x900,
			BackendSessionQueryError = 0x901,
			BackendStreamQueryError = 0x902,
		}
	}
}

