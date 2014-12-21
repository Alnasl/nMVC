using System;
using de.netcrave.nMVC.Models;
using System.Reflection;

/// <summary>
/// JobFinderBootstrap
/// Author : Paige Thompson (paigeadele@gmail.com)
/// Copyright 2014
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

namespace de.netcrave.nMVC.Settings
{
    public class Settings
    {
		public string[] HTTPListenerAddress = new string[] { "http://localhost:8000/" };

		public int HTTPWorkerThreads = 15;
		public int HTTPCompletionPortThreads = 15;
			
		public string HtdocRoot = "HTML/";

		public string HTTPServerHeader = "nMVC " + "0.1";//AssemblyName.GetAssemblyName(.Version.Major.ToString();

		public DebuggingSettings debugging = new DebuggingSettings();

		// TODO add your setttings here

        public Settings ()
        {

        }
    }
}

