using System;

namespace de.netcrave.nMVC.Logger
{
	public class nMVCLogData
	{
		private nMVCLogLevel _level;
		private object[] _debugging = new object[] {};
		private string _message = "empty message";
		public nMVCLogLevel level
		{
			get
			{
				return _level;
			}
			internal set
			{
				_level = value;
			}
		}
		public object[] debugging
		{
			get
			{
				return _debugging;
			}
			internal set
			{
				_debugging = value;
			}
		}
		public string message
		{
			get
			{
				return _message;
			}
			internal set
			{
				_message = value;
			}
		}
	}}

