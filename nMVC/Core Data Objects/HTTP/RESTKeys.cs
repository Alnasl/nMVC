using System;

namespace de.netcrave.nMVC
{
	/// <summary>
	/// REST keys. This should help to maintain some consistency and accuracy in coding the REST API.
	/// All REST related header parameters should be defined here. 
	/// </summary>
	public class RESTKeys
	{
		/// <summary>
		/// The name of the key attributed to the REST handler request header
		/// </summary>
		public const string RESTHandlerRequest = "NMVC_API_V1_REQUEST";

		/// <summary>
		/// The name of the key attributed to the ID cookie name 
		/// </summary>
		public const string SessionCookieId = "NMVCSESSIONID";

		/// <summary>
		/// The name of the key attributed to the token cookie name
		/// </summary>
		public const string SessionCookieToken = "NMVCSESSIONTOKEN";

		/// <summary>
		/// the name of the key attributed to a userId question in a REST request
		/// </summary>
		public const string ZombieletUserId = "NMVC_API_V1_USERID";

		public RESTKeys ()
		{

		}
	}
}

