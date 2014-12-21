using System;
using Newtonsoft.Json;
using de.netcrave.nMVC.Session;
//using System.Xml.Serialization;

namespace de.netcrave.nMVC.Models
{
	public abstract class DomainObjectBase// : IXmlSerializable
	{
		private Guid _ID = System.Guid.NewGuid();

		public DateTime Created;

		public DateTime Modified;

		[JsonIgnore]
		public SessionIdentity si;

		public Guid ZID 
		{
			get
			{
				return _ID;
			}
		    set
			{
				_ID = value;
			}
		}

		// this would come in handy for serialize/deserialize when the parameter object is cast down to an ITag.
		public string ObjectType
		{
			get
			{
				return this.GetType().Name;
			}		
		}			

		protected DomainObjectBase ()
		{
			DateTime nnow = DateTime.Now;
			Created = nnow;
			Modified = nnow;
		}
			
		/// <summary>
		/// Creates this instance
		/// </summary>
		abstract public DomainObjectBase Create();

		/// <summary>
		/// Update this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Update();

		/// <summary>
		/// Delete this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Delete();

		/// <summary>
		/// Save this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Save();

		/// <summary>
		/// Rollback this instance.
		/// </summary>
		abstract public BackendQueryStatus.ReturnCode Rollback();

	}
}

