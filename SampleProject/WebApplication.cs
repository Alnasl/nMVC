using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using de.netcrave.nMVC;
using de.netcrave.nMVC.WebServer;
using de.netcrave.nMVC.ContentManager;
using de.netcrave.nMVC.RouterManager;
using de.netcrave.nMVC.RESTCallbackManager;
using de.netcrave.nMVC.Logger;
using de.netcrave.nMVC.Session;
using de.netcrave.nMVC.Settings;
using de.netcrave.nMVC.Utilities;


namespace SampleProject
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			nMVCLogger.Instance.Output += (logData) => 
			{
				switch(logData.level)
				{
				case nMVCLogLevel.Debug:
					CustomDebugInfoLogging.HandleDebugLog(logData);
					break;
				default:
					if(SettingsManager.Instance.settings.debugging.AddTracingInfoInHTTPResponse)
					{
						StackTrace st = new StackTrace();
						string methodName = st.GetFrame(1).GetMethod().Name;
						Console.WriteLine(logData.level.ToString() + " : " + methodName + " : " + logData.message);
						Console.WriteLine("----");
						return;
					}
					Console.WriteLine(logData.level.ToString() + " : " + logData.message);
					Console.WriteLine("----");
					break;
				}
			};

			nMVCLogger.Instance.AddErrorLevel(nMVCLogLevel.Debug);
			nMVCLogger.Instance.AddErrorLevel(nMVCLogLevel.Error);
			nMVCLogger.Instance.AddErrorLevel(nMVCLogLevel.Info);
			nMVCLogger.Instance.AddErrorLevel(nMVCLogLevel.Warn);
			nMVCLogger.Instance.AddErrorLevel(nMVCLogLevel.Fatal);


			WebServer wserv = new WebServer (
				MainClass.HTTPRequestHandler, 
				SettingsManager.Instance.settings.HTTPListenerAddress);

			RESTCallbackManager.Instance.RegisterGlobalCallbackHandlers();
			ContentManager.Instance.PreloadContent();

			RouterManager.Instance.Add(@"^/$", "index.html");
			RouterManager.Instance.Add(@"^/.*\.(js)$", "js/");
			RouterManager.Instance.Add(@"^/.*\.(css)$", "css/");
			RouterManager.Instance.Add(@"^/.*\.(jpg|png)$", "images/");
			// theres also this
			//router.Add(@"^/follow/(.*?)$", "QUICKFOLLOWUSER");

			wserv.Run();
			// TODO all of this code implies that this service will always be run as a command line program
			// but this is only useful for testing and development. In the future it will need to daemonize. 
			nMVCLogger.Instance.Info("Web application is running, press F10 to exit");

			readKey:
			ConsoleKeyInfo key = Console.ReadKey ();
			if(key.Key != ConsoleKey.F10)
				goto readKey;				
		}

		/// <summary>
		/// HTTPs the request handler.
		/// </summary>
		/// <returns>The request handler.</returns>
		/// <param name="ctx">Context.</param>
		public static object HTTPRequestHandler (HttpListenerContext ctx)
		{
			switch (ctx.Request.HttpMethod)
			{
			case "GET":
				return MainClass.HandleGETRequest (ctx);

			case "POST":
				return MainClass.HandlePOSTRequest (ctx, CustomSessionIdentity.GetSessionIdentity(ctx));

			case "HEAD":
				return "";

			case "OPTIONS":
				ctx.Response.AddHeader("Allow", "GET,HEAD,POST,OPTIONS");
				return "";

			case "PUT":
			case "DELETE":
			case "TRACE":            
			case "CONNECT":
			case "PATCH":
			default:
				ctx.Response.StatusCode = 405; 
				return "NOT ALLOWED";
			}
		}
		/// <summary>
		/// Handles the GET request.
		/// </summary>
		/// <returns>
		/// The GET request.
		/// </returns>
		/// <param name='ctx'>
		/// Context.
		/// </param>
		public static object HandleGETRequest (HttpListenerContext ctx)
		{
			if (!string.IsNullOrEmpty (ctx.Request.Headers [RESTKeys.RESTHandlerRequest]))
			{
				if(!RESTCallbackManager.Instance.GETCallbacks.ContainsKey(ctx.Request.Headers [RESTKeys.RESTHandlerRequest]))
				{
					ctx.Response.StatusCode = 404;
					return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.RESTHandlerRequestNotFound).ToJSON();
				}

				// Is a REST API request
				SessionIdentity si = CustomSessionIdentity.GetSessionIdentity(ctx);
				Dictionary<string, string> Headers = UtilitiesManager.Instance.GetHeaders(ctx);

				// This is where the string specified in the REQUEST header is matched against the 
				// GET REST API mappings
				HttpResponse ret = RESTCallbackManager.Instance.GETCallbacks [ctx.Request.Headers [RESTKeys.RESTHandlerRequest]] (Headers, si);						
				ctx.Response.ContentType = "application/json; charset=utf-8";
				if(ret.ErrorCode.Count() > 0)
				{
					ctx.Response.StatusCode = 500;
				}
				return ret.ToJSON();
			}

			else
			{
				// Not a REST API Request
				string route = RouterManager.Instance.FindRoute(ctx.Request.Url).FirstOrDefault();

				if(!string.IsNullOrEmpty(route) && ContentManager.Instance.content.ContainsKey(route))
				{
					var item = ContentManager.Instance.content[route];
					ctx.Response.ContentType = item.ContentType.MediaType;
					ctx.Response.ContentEncoding = System.Text.Encoding.UTF8;
					return item;
				}
				else if(!string.IsNullOrEmpty(route) && RESTCallbackManager.Instance.GETCallbacks.ContainsKey(route))
				{
					SessionIdentity si = CustomSessionIdentity.GetSessionIdentity(ctx);
					Dictionary<string, string> Headers = UtilitiesManager.Instance.GetHeaders(ctx);

					// This is where the string specified in the REQUEST header is matched against the 
					// GET REST API mappings
					HttpResponse ret = RESTCallbackManager.Instance.GETCallbacks [route] (Headers, si);						
					//ctx.Response.ContentType = "application/json; charset=utf-8";
					if(ret.ErrorCode.Count() > 0)
					{
						ctx.Response.StatusCode = 500;
					}
					ctx.Response.Redirect("/");
					return "";
				}
				else
				{
					ctx.Response.StatusCode = 404;
					return "NOT FOUND";
				}
			}
		}

		/// <summary>
		/// Handles the POST request.
		/// </summary>
		/// <returns>
		/// The POST request.
		/// </returns>
		/// <param name='ctx'>
		/// Context.
		/// </param>
		public static string HandlePOSTRequest (HttpListenerContext ctx, SessionIdentity si)
		{
			if (!string.IsNullOrEmpty (ctx.Request.Headers [RESTKeys.RESTHandlerRequest]))
			{

				// This is where the string specified in the REQUEST header is matched against the 
				// POST REST API mappings
				Dictionary<string, string> Headers = UtilitiesManager.Instance.GetHeaders(ctx);
				Dictionary<string, string> PostData = UtilitiesManager.Instance.GetPOSTData(ctx);

				if(!RESTCallbackManager.Instance.POSTCallbacks.ContainsKey(ctx.Request.Headers [RESTKeys.RESTHandlerRequest]))
				{
					ctx.Response.StatusCode = 404;
					return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.RESTHandlerRequestNotFound).ToJSON();
				}

				HttpResponse ret = RESTCallbackManager.Instance.POSTCallbacks [ctx.Request.Headers [RESTKeys.RESTHandlerRequest]] 
					(Headers, si, PostData);

				ctx.Response.ContentType = "application/json; charset=utf-8";
				return ret.ToJSON();
			}
			else
			{
				nMVCLogger.Instance.Error("POST requests must include the API_V1_REQUEST kv pair to indicate a route");
				ctx.Response.StatusCode = 500;
				return "BAD REQUEST";
			}
		}
	}
}
