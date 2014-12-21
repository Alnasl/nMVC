using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using de.netcrave.nMVC;
using de.netcrave.nMVC.Settings;
using de.netcrave.nMVC.Logger;
using de.netcrave.nMVC.Utilities;

namespace de.netcrave.nMVC.WebServer
{
	/// <summary>
	/// TODO I would like to re-implement this as an F# actor
	/// </summary>
	public class WebServer
	{
		private readonly HttpListener _listener = new HttpListener ();
		private readonly Func<HttpListenerContext, object> _responderMethod;

		public WebServer (string[] prefixes, Func<HttpListenerContext, object> method)
		{
			if (!HttpListener.IsSupported) {
				throw new NotSupportedException ("Needs Windows XP SP2, Server 2003 or later... or Mono 3.2");
			}

			// URI prefixes are required, for example
			// "http://localhost:8080/index/".
			if (prefixes == null || prefixes.Length == 0) {
				throw new ArgumentException ("prefixes");
			}

			// A responder method is required
			if (method == null) {
				throw new ArgumentException ("method");
			}

			foreach (string s in prefixes) {
				_listener.Prefixes.Add (s);
			}

			_responderMethod = method;

			_listener.Start ();
		}

		public WebServer (Func<HttpListenerContext, object> method, params string[] prefixes)
			: this (prefixes, method)
		{
		}

		public void Run ()
		{
			ThreadPool.SetMinThreads (SettingsManager.Instance.settings.HTTPWorkerThreads,
				SettingsManager.Instance.settings.HTTPCompletionPortThreads);

			ThreadPool.QueueUserWorkItem ((o) => {
				nMVCLogger.Instance.Info("Webserver running...");

				_listener.IgnoreWriteExceptions = true;
				while (_listener.IsListening) {
					ThreadPool.QueueUserWorkItem ((c) => {
						var ctx = c as HttpListenerContext;

						try {       	

							object result = _responderMethod (ctx);
					
							ctx.Response.Headers["Server"] = SettingsManager.Instance.settings.HTTPServerHeader;
                            
							byte[] buf = null;

							if (result.GetType () == typeof(byte[])) 
							{
								buf = ((byte[])result).ToArray ();
							}
							else if(result.GetType() == typeof(ContentFile))
							{
								buf = ((ContentFile)result).content;
							}
							else if (result.GetType () == typeof(string)) 
							{
								buf = Encoding.UTF8.GetBytes ((string)result);
							}
							
							//TODO figure out how to set the Content-Type correctly still... 
							// Its being done in main but I'm not sure if its the "right" way
							// to do it... 
							//ctx.Response.AddHeader("", "utf-8");
							//ctx.Response.ContentEncoding = UTF8Encoding.UTF8;
							ctx.Response.ContentLength64 = buf.Length;
							ctx.Response.OutputStream.Write (buf, 0, buf.Length);
						} catch (Exception ex) {
							nMVCLogger.Instance.Error(UtilitiesManager.Instance.PrettyRenderException(ex));

						} finally {
							// always close the stream
							ctx.Response.OutputStream.Close ();
						}
					}, _listener.GetContext ());
				}
			});
		}

		public void Stop ()
		{
			_listener.Stop ();
			_listener.Close ();
		}
	}
}
