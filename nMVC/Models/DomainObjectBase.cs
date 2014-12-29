using System;
using Newtonsoft.Json;
using de.netcrave.nMVC.Session;
//using System.Xml.Serialization;

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
namespace de.netcrave.nMVC.Models
{
	public abstract class DomainObjectBase// : IXmlSerializable
	{
		private Guid _ID = System.Guid.NewGuid();

		public DateTime Created;

		public DateTime Modified;

		[JsonIgnore]
		public SessionIdentity si;

		public Guid ObjectID 
		{
			get
			{
				return _ID;
			}
		    set
			{
				_ID = value;
			}
		}

		// this would come in handy for serialize/deserialize when the parameter object is cast down to an ITag.
		public string ObjectType
		{
			get
			{
				return this.GetType().Name;
			}		
		}			

		protected DomainObjectBase ()
		{
			DateTime nnow = DateTime.Now;
			Created = nnow;
			Modified = nnow;
		}
			
		/// <summary>
		/// Creates this instance
		/// </summary>
		abstract public DomainObjectBase Create();

		/// <summary>
		/// Update this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Update();

		/// <summary>
		/// Delete this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Delete();

		/// <summary>
		/// Save this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Save();

		/// <summary>
		/// Rollback this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Rollback();

	}
}

