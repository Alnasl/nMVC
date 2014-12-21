using System;

namespace de.netcrave.nMVC
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RESTContractAttribute : System.Attribute
	{
		public enum RequestType 
		{
			GET,
			POST,
			PUT,
			DELETE
		}
		public readonly RESTContractAttribute.RequestType HTTPMethod;
		public readonly string APILevel = "";
		public readonly string RequestHandlerName = "";
		public RESTContractAttribute (RESTContractAttribute.RequestType rt, string apilvl, string reqtoanswer)
		{
			HTTPMethod = rt;
			APILevel = apilvl;
			RequestHandlerName = reqtoanswer;
		}
	}
}

