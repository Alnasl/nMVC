using System;
using System.Collections.Generic;
using System.Collections;
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
	public class DomainObjectEnumerator<T> : IEnumerator<T> where T : DomainObjectBase
	{
		protected DomainObjectRepository<T> _collection; //enumerated collection
		protected int index; //current index
		protected T _current; //current enumerated object in the collection

		// Default constructor
		[DebuggerStepThrough]
		public DomainObjectEnumerator()
		{
			//nothing
		}

		// Paramaterized constructor which takes
		// the collection which this enumerator will enumerate
		[DebuggerStepThrough]
		public DomainObjectEnumerator(DomainObjectRepository<T> collection)
		{
			_collection = collection;
			index = -1;
			_current = default(T);
		}

		// Current Enumerated object in the inner collection

		public virtual T Current
		{
			get
			{
				return _current;
			}
		}

		// Explicit non-generic interface implementation for IEnumerator
		// (extended and required by IEnumerator<T>)

		object IEnumerator.Current
		{
			get
			{
				return _current;
			}
		}

		// Dispose method
		[DebuggerStepThrough]
		public virtual void Dispose()
		{
			_collection = null;
			_current = default(T);
			index = -1;
		}

		// Move to next element in the inner collection
		[DebuggerStepThrough]
		public virtual bool MoveNext()
		{
			//make sure we are within the bounds of the collection
			if (++index >= _collection.Count)
			{
				//if not return false
				return false;
			}
			else
			{
				//if we are, then set the current element
				//to the next object in the collection
				_current = _collection[index];
			}
			//return true
			return true;
		}

		// Reset the enumerator
		[DebuggerStepThrough]
		public virtual void Reset()
		{
			_current = default(T); //reset current object
			index = -1;
		}
	}
}