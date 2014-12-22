using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using de.netcrave.nMVC.Session;
using System.Diagnostics;

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
	public class DomainObjectRepository<T> : ICollection<T> where T : DomainObjectBase
	{
		protected SessionIdentity si;

		/// <summary>
		/// The inner array.
		/// </summary>
		protected ArrayList _innerArray;

		/// <summary>
		/// readonly flag
		/// </summary>
		protected bool _IsReadOnly;

		/// <summary>
		/// Initializes a new instance of the <see cref="de.netcrave.nMVC.Models.DomainObjectRepository`1"/> class.
		/// </summary>
		/// <param name="si">Si.</param>
		public DomainObjectRepository(SessionIdentity si)
		{
			this.si = si;
			_innerArray = new ArrayList();
		}

		/// <summary>
		/// Gets or sets the <see cref="de.netcrave.nMVC.Models.DomainObjectRepository`1"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public virtual T this[int index]
		{
			get
			{
				return (T)_innerArray[index];
			}
			set
			{
				_innerArray[index] = value;
			}
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public virtual int Count
		{
			get
			{
				return _innerArray.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public virtual bool IsReadOnly
		{
			get
			{
				return _IsReadOnly;
			}
		}

		/// <summary>
		/// Add the specified DomainObject.
		/// </summary>
		/// <param name="DomainObject">Domain object.</param>
		public virtual void Add(T DomainObject)
		{
			DomainObject.si = this.si;
			_innerArray.Add(DomainObject);
		}

		/// <summary>
		/// Add the specified DomainObjects.
		/// </summary>
		/// <param name="DomainObjects">Domain objects.</param>
		public virtual void Add(IEnumerable<T> DomainObjects)
		{
			foreach(T obj in DomainObjects)
			{
				obj.si = this.si;
				Add(obj);
			}
		}

		/// <summary>
		/// Remove the specified DomainObject.
		/// </summary>
		/// <param name="DomainObject">Domain object.</param>
		public virtual bool Remove(T DomainObject) 
		{
			bool result = false;

			//loop through the inner array's indices
			for (int i = 0; i < _innerArray.Count; i++)
			{
				//store current index being checked
				T obj = (T)_innerArray[i];

				//compare the BusinessObjectBase UniqueId property
				if (obj.ZID == DomainObject.ZID)
				{
					//remove item from inner ArrayList at index i
					_innerArray.RemoveAt(i);
					result = true;
					break;
				}
			}

			return result;
		}
			
		/// <summary>
		/// Contains the specified DomainObject.
		/// </summary>
		/// <param name="DomainObject">Domain object.</param>
		public bool Contains(T DomainObject)
		{
			//loop through the inner ArrayList
			foreach (T obj in _innerArray)
			{
				//compare the BusinessObjectBase UniqueId property
				if (obj.ZID == DomainObject.ZID)
				{
					//if it matches return true
					return true;
				}
			}
			//no match
			return false;
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="DomainObjectArray">Domain object array.</param>
		/// <param name="index">Index.</param>
		public virtual void CopyTo(T[] DomainObjectArray, int index)
		{
			_innerArray.CopyTo(DomainObjectArray, index);
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public virtual void Clear()
		{
			_innerArray.Clear();
		}
			
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		[DebuggerStepThrough]
		public virtual IEnumerator<T> GetEnumerator()
		{
			return new DomainObjectEnumerator<T>(this);
		}
			
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DomainObjectEnumerator<T>(this);
		}

		/// <summary>
		/// Create this instance.
		/// </summary>
		public static T Create ()
		{
			return(T)((DomainObjectBase)Activator.CreateInstance(typeof(T))).Create();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public IEnumerable<BackendQueryStatus.ReturnCode> Update ()
		{
			List<BackendQueryStatus.ReturnCode> ret = new List<BackendQueryStatus.ReturnCode>();
			foreach(DomainObjectBase zob in this)
			{
				ret.Add(zob.Save());

			}
			return ret;
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public IEnumerable<BackendQueryStatus.ReturnCode> Delete ()
		{
			List<BackendQueryStatus.ReturnCode> ret = new List<BackendQueryStatus.ReturnCode>();
			foreach(DomainObjectBase zob in this)
			{
				ret.Add(zob.Delete());

			}
			return ret;
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public IEnumerable<BackendQueryStatus.ReturnCode> Save ()
		{
			List<BackendQueryStatus.ReturnCode> ret = new List<BackendQueryStatus.ReturnCode>();
			foreach(DomainObjectBase zob in this)
			{
				ret.Add(zob.Save());

			}
			return ret;
		}
	}
}

