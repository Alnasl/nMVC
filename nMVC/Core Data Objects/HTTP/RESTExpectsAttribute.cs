using System;

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

