using System;
using System.Collections.Generic;
using System.Linq;
using de.netcrave.nMVC.Logger;
using de.netcrave.nMVC.DynamicExtensions;

namespace SampleProject
{
	public static class CustomDebugInfoLogging
	{
		public static void HandleDebugLog(nMVCLogData data) 
		{
			//[0]	{{ data = de.netcrave.nMVC.RESTContractAttribute, t = SampleProject.AccountService, mi = de.netcrave.nMVC.HttpResponse AuthenticateFromLocal(System.Collections.Generic.Dictionary`2[System.String,System.String], SampleProject.CustomSessionIdentity, System.Collections.Generic.Dictionary`2[System.String,System.String]) }}	<>__AnonType1<de.netcrave.nMVC.RESTContractAttribute,System.Type,System.Reflection.MethodInfo>
			foreach(object obj in data.debugging)
			{
				if(obj.GetType().GetProperty("data") != null
					&& obj.GetType().GetProperty("t") != null
					&& obj.GetType().GetProperty("mi") != null)
				{
					var debugData = obj.ToDynamic();
					LogRestCallBackHandler(data, debugData.data, debugData.t, debugData.mi);
				}
			}
		}

		/// <summary>
		/// TODO it'd be pretty cool to generate a cURL command line request for each rest API contract for development
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="c">C.</param>
		private static void LogRestCallBackHandler(nMVCLogData data, object a, object b, object c)
		{
			if(a.GetType() == typeof(de.netcrave.nMVC.RESTContractAttribute))
			{
				var contractAttr = (de.netcrave.nMVC.RESTContractAttribute) a;
				var t = (System.Reflection.TypeInfo) b;
				var mi = (System.Reflection.MethodInfo) c;

				Console.WriteLine(data.level.ToString() 
					+ " : " 
					+ data.message
					+ " : "
					+ contractAttr.APILevel
					+ " : "
					+ t.Name
					+ " : "
					+ mi.Name
				);
			}
		}
	}
}

