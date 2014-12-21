using System;

namespace de.netcrave.nMVC.Logger
{
	/// <summary>
	/// Contains the values of the logging level.
	/// </summary>
	public enum nMVCLogLevel
	{
		// totally didn't rip off PHP right here
		/// <summary>
		/// The trace.
		/// </summary>
		Trace = 1,
		/// <summary>
		/// The debug.
		/// </summary>
		Debug = 2,
		/// <summary>
		/// The info.
		/// </summary>
		Info = 4,
		/// <summary>
		/// The warn.
		/// </summary>
		Warn = 8,
		/// <summary>
		/// The error.
		/// </summary>
		Error = 16,
		/// <summary>
		/// The fatal.
		/// </summary>
		Fatal = 32
	}
}

