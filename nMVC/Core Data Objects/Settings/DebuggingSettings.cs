using System;

namespace de.netcrave.nMVC.Settings
{
	public class DebuggingSettings
	{
		public bool PrettyPrintJSONResponse = true;
		public bool AddTracingInfoInHTTPResponse = true; 
		public bool EnableMessagesInHTTPResponse = true;
		public bool EnableExceptions = true;
		public DebuggingSettings ()
		{
		}
	}
}

