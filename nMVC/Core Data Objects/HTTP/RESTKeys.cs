using System;

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

