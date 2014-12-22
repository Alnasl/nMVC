using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using de.netcrave.nMVC.Logger;
using de.netcrave.nMVC.Settings;
using de.netcrave.nMVC.Utilities;

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
    public class HttpResponse
    {
        public List<string> Messages = new List<string> ();
        public object[] data;
        public List<HttpResponse> responses = new List<HttpResponse> ();
		public POSTDataError[] PostDataErrors;

		private List<GuruMeditation.ErrorCode> _ErrorCodes = new List<GuruMeditation.ErrorCode>();

		public int[] ErrorCode 
		{
			get
			{
				return _ErrorCodes.Cast<int>().ToArray();
			}
		}

        public HttpResponse ()
        {
        }

        /// <summary>
        /// Factory method for JobFinderHttpResponse, optional param for nesting a response, returns new instance with nested response
        /// </summary>
        /// <param name='response2'>
        /// Response2.
        /// </param>
        public HttpResponse Response (HttpResponse response2)
        {
			if(SettingsManager.Instance.settings.debugging.AddTracingInfoInHTTPResponse)
			{
				return new HttpResponse ()
					.AddMessage ("created in " + new StackTrace ()
						.GetFrame (1)
						.GetMethod ()
						.Name);
			}
			return this.AddResponse (response2);
        }

        /// <summary>
        /// News the response.
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        /// <param name='response2'>
        /// Response2.
        /// </param>
        public static HttpResponse NewResponse (HttpResponse response2)
        {
			if(SettingsManager.Instance.settings.debugging.AddTracingInfoInHTTPResponse)
			{
	            return new HttpResponse ()
	                   .AddResponse (response2)
	                   .AddMessage ("created in " + new StackTrace ()
	                                .GetFrame (1)
	                                .GetMethod ()
	                                .Name);
			}
			return new HttpResponse ()
				.AddResponse (response2);
        }

        /// <summary>
        /// Default factory method
        /// </summary>
        public static HttpResponse NewResponse ()
        {
			if(SettingsManager.Instance.settings.debugging.AddTracingInfoInHTTPResponse)
			{
				return new HttpResponse ()
                   .AddMessage ("created in " + new StackTrace ()
                                .GetFrame (1)
                                .GetMethod ()
                                .Name);
			}
			return new HttpResponse();
        }

        /// <summary>
        /// Adds the message to this object, this returns this objec
        /// </summary>
        /// <returns>
        /// The message.
        /// </returns>
        /// <param name='message'>
        /// Message.
        /// </param>
        public HttpResponse AddMessage (string message)
        {
            Messages.Add (message);
            return this;
        }

        /// <summary>
        /// Adds the response to this object, then returns this object
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        /// <param name='response'>
        /// Response.
        /// </param>
        private HttpResponse AddResponse (HttpResponse response)
        {
            if (response != null)
            {
                responses.Add (response);
            }

            return this;
        }

		/// <summary>
		/// Adds the web form error.
		/// </summary>
		/// <returns>The web form error.</returns>
		/// <param name="wfm">Wfm.</param>
		public HttpResponse AddPOSTDataError(POSTDataError wfm)
		{
			if(PostDataErrors == null)
			{
				PostDataErrors = new POSTDataError[] {};
			}
			PostDataErrors = PostDataErrors.Union(new POSTDataError[] { wfm }).ToArray();
			return this;
		}

		public HttpResponse AddErrorCode(GuruMeditation.ErrorCode error)
		{
			string loggerMessage = "error ";
			if(SettingsManager.Instance.settings.debugging.AddTracingInfoInHTTPResponse)
			{
				loggerMessage = " " + UtilitiesManager.Instance.PrettyRenderStackTrace(new StackTrace(true));

			}
			nMVCLogger.Instance.Info(loggerMessage + "\nError Code: " + error.ToString());

			_ErrorCodes.Add(error);
			return this;
		}

        /// <summary>
        /// Adds or unions data returns itself
        /// </summary>
        /// <returns>
        /// The data.
        /// </returns>
        /// <param name='data2'>
        /// Data2.
        /// </param>
        public HttpResponse AddData (object[] data2)
        {
            if (data != null)
            {
                data = data.Union ((data2 == null) 
					? new object []
                {
                } 
					: data2).ToArray ();
            }

            else
            {
                data = data2;
            }

            return this;
        }

        /// <summary>
        /// Adds the data.
        /// </summary>
        /// <returns>
        /// The data.
        /// </returns>
        /// <param name='data2'>
        /// Data2.
        /// </param>
        public HttpResponse AddData (object data2)
        {
            if (data != null)
            {
                data = data.Union (new object[]
                {
                    data2
                }).ToArray ();
            }

            else
            {
                data = new object[]
                {
                    data2
                };
            }

            return this;
        }

		/// <summary>
		/// Serialize response to JSON
		/// </summary>
		/// <returns>The JSO.</returns>
		public string ToJSON()
		{
			JsonSerializerSettings jss = new JsonSerializerSettings ();
			if(SettingsManager.Instance.settings.debugging.PrettyPrintJSONResponse)
			{
				jss.Formatting = Newtonsoft.Json.Formatting.Indented;
			}
			return JsonConvert.SerializeObject (this, jss);
		}

		/// <summary>
		/// Tos the XM.
		/// </summary>
		/// <returns>The XM.</returns>
		public string ToXML()
		{
			/*
			var serializer = new XmlSerializer (typeof(HttpResponse));
			var settings = new XmlWriterSettings();

			if(SettingsManager.Instance.settings.debugging.PrettyPrintJSONResponse)
			{
				settings.Indent = true;
				settings.NewLineOnAttributes = true;
			}


			using (XmlTextWriter xtw = new XmlTextWriter()
			{
				serializer.Serialize (xtw, settings);
				return xtw.ToString();
			}
			*/
			throw new NotImplementedException();
		}
    }
}

