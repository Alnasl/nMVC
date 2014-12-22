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
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class RESTExpectsParam : System.Attribute
	{
		public enum ParamType
		{
			GET,
			POST,
			HEADER,
		}
		public readonly string ExpectKey; 
		public readonly RESTExpectsParam.ParamType typeOfParam;

		public RESTExpectsParam (string key, RESTExpectsParam.ParamType p)
		{
			ExpectKey = key;
			typeOfParam = p;
		}
	}
}

