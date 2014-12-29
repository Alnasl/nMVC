using System;
using System.IO;
using System.Net;
using BCrypt.Net;
using System.Security.Cryptography;
using de.netcrave.nMVC.Models;
using de.netcrave.nMVC.Accounts;

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
	public class SessionManager : Manager
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
		public SessionIdentity CreateAuthenticatedUserSessionIdentity(UserAccount zlu, SessionIdentity CurrentIdentity)
		{
			var valid = CurrentIdentity.Revalidate();

			if(!valid)
			{
				return null;
			}

			CurrentIdentity.UserId = zlu.ObjectID;
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
	}
}

