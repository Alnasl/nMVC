using System;
using System.IO;
using System.Threading;
using System.Net.Mime;
using System.Net;
using de.netcrave.nMVC.Logger;

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
	public class ContentFile
	{
		private object syncroot = new object();
		private bool _disabled = false;
		private ContentType _ContentType;
		private string _path;
		private byte[] _content;

		public bool Disabled 
		{
			get
			{
				return _disabled;
			}
			set 
			{
				_disabled =true;
			}
		}

		public ContentType ContentType
		{
			get 
			{
				if(beginLock)
				{
					lock(syncroot)
					{
						return _ContentType;
					}
				}
				return _ContentType;
			}
			set
			{
				if(beginLock)
				{
					lock(syncroot)
					{
						_ContentType = value;
						return;
					}
				}
				_ContentType = value;
			}
		}
		public string path
		{
			get
			{
				if(beginLock)
				{
					lock(syncroot)
					{
						return _path;
					}
				}
				return _path;

			}
			set 
			{
				if(beginLock)
				{
					lock(syncroot)
					{
						_path = value;
						return;
					}
				}
				_path = value;
			}
		}
		public byte[] content
		{
			get
			{
				if(Disabled)
				{
					return null;
				}
				if(beginLock)
				{
					lock(syncroot)
					{
						return _content;
					}
				}
				return _content;
			}
			set
			{
				if(beginLock)
				{
					lock(syncroot)
					{
						_content = value;
						return;
					}
				}
				_content = value;
			}
		}

		public bool beginLock;

		public ContentFile (string fullpathandfile, string contenttype)
		{
			this.path = fullpathandfile;
			nMVCLogger.Instance.Info("loading a properly formatted data file " + fullpathandfile 
				+ " content-type " 
				+ contenttype);
			LoadFile();
			this.ContentType = new ContentType(contenttype);		
		}

		public void BeginUpdate()
		{
			beginLock = true;
			lock(syncroot)
			{
				Thread.Sleep(1);

				try
				{
					LoadFile();
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
					beginLock = false;
				}
			}
		}

		private void LoadFile()
		{
			beginLock = true;

			try
			{
				if(!File.Exists(this.path))
				{
					Disabled = true;
					nMVCLogger.Instance.Error("file doesn't exist, disabling " + this.path);
					return;
				}
				this.content = File.ReadAllBytes(this.path);
			}
			catch(Exception ex)
			{
				nMVCLogger.Instance.Error("BUG way to go emacs for accomplishing the impossible: " 
					+ this.path
					+ " : " 
					+ ex.Message);
				//throw ex;
			}
			finally
			{
				beginLock = false;
			}
		}

		public void DisableMe()
		{
			Disabled = true;
		}
	}
}

