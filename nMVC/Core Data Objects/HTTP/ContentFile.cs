using System;
using System.IO;
using System.Threading;
using System.Net.Mime;
using System.Net;
using de.netcrave.nMVC.Logger;

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

