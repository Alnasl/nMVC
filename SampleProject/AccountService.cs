using System;
using System.Collections.Generic;
using System.Linq;
using de.netcrave.nMVC;
using de.netcrave.nMVC.Models;
using de.netcrave.nMVC.Session;

namespace SampleProject
{
	[RESTService()]
	public sealed class AccountService : Manager
	{
		private static AccountService instance;


		private AccountService ()
		{

		}

		public static new AccountService Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AccountService ();
				}

				return instance;
			}
		}

		[RESTExpectsParam("USERNAME", RESTExpectsParam.ParamType.POST)]
		[RESTExpectsParam("PASSWORD", RESTExpectsParam.ParamType.POST)]
		[RESTContract(RESTContractAttribute.RequestType.POST, RESTKeys.RESTHandlerRequest, "AUTHENTICATELOCAL")]
		/// <summary>
		/// Authenticates from local.
		/// </summary>
		/// <returns>The from local.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		/// <param name="postData">Post data.</param>
		public HttpResponse AuthenticateFromLocal(Dictionary<string, string> headers, CustomSessionIdentity si, Dictionary<string,string> postData)
		{
			if(si.Guest)
			{
				string username = postData["USERNAME"];
				string password = postData["PASSWORD"];

				CustomUserAccount zlu = CustomUserAccount.Retrieve(username, si);
				if(zlu == null)
				{
					return HttpResponse.NewResponse().AddPOSTDataError(
						new POSTDataError
						{
							FieldName = "USERNAME",
							Error = "is not valid"
						});
				}

				if(BCrypt.Net.BCrypt.Verify(password, zlu.Password))
				{
					var newSessionIdentity = SessionManager.Instance.CreateAuthenticatedUserSessionIdentity(zlu, si);
					if(newSessionIdentity == null)
					{
						return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.BackendSessionQueryError);
					}

					return HttpResponse.NewResponse().AddData(zlu.GetClientSideSafeUserObject());
				}
				else
				{
					return HttpResponse.NewResponse().AddPOSTDataError(
						new POSTDataError
						{
							FieldName = "PASSWORD",
							Error = "did not match"
						});
				}
			}
			else
			{
				return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.UserAlreadyAuthenticated);
			}
		}
			
		[RESTContract(RESTContractAttribute.RequestType.POST, RESTKeys.RESTHandlerRequest, "AUTHENTICATEFACEBOOK")]
		/// <summary>
		/// Authenticates from facebook.
		/// </summary>
		/// <returns>The from facebook.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		/// <param name="postData">Post data.</param>
		public HttpResponse AuthenticateFromFacebook(Dictionary<string, string> headers, CustomSessionIdentity si, Dictionary<string,string> postData)
		{
			return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.NotImplemented);
		}
			
		[RESTContract(RESTContractAttribute.RequestType.POST, RESTKeys.RESTHandlerRequest, "AUTHENTICATETWITTER")]
		/// <summary>
		/// Authenticates from twitter.
		/// </summary>
		/// <returns>The from twitter.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		/// <param name="postData">Post data.</param>
		public HttpResponse AuthenticateFromTwitter(Dictionary<string, string> headers, CustomSessionIdentity si, Dictionary<string,string> postData)
		{
			return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.NotImplemented);
		}


		[RESTExpectsParam("USERNAME", RESTExpectsParam.ParamType.POST)]
		[RESTExpectsParam("EMAIL", RESTExpectsParam.ParamType.POST)]
		[RESTExpectsParam("PASSWORD", RESTExpectsParam.ParamType.POST)]
		[RESTContract(RESTContractAttribute.RequestType.POST, RESTKeys.RESTHandlerRequest, "CREATEACCOUNT")]
		/// <summary>
		/// Creates the account.
		/// </summary>
		/// <returns>The account.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		/// <param name="postData">Post data.</param>
		public HttpResponse CreateAccount(Dictionary<string, string> headers, CustomSessionIdentity si, Dictionary<string,string> postData)
		{
			if(si.Guest)
			{
				var result = CustomUserAccount.UserNameExists(postData["USERNAME"]);

				if(result == BackendQueryStatus.ReturnCode.Exists)
				{
					return HttpResponse.NewResponse().AddPOSTDataError(
						new POSTDataError
						{
							FieldName = "USERNAME",
							Error = "User already exists"
						});
				}
				else if(result != BackendQueryStatus.ReturnCode.DoesntExist)
				{
					return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.BackendUserQueryError);
				}

				DomainObjectRepository<DomainObjectBase> repo = new DomainObjectRepository<DomainObjectBase>(si);

				CustomUserAccount zlu = new CustomUserAccount 
				{
					Email = postData["EMAIL"],
					//FirstName = postData["FIRSTNAME"],
					//LastName = postData["LASTNAME"],
					LinkCategories = new string[] { "default" },
					UserName = postData["USERNAME"],
					Password = BCrypt.Net.BCrypt.HashPassword(postData["PASSWORD"]),
					Following = new string[] {},
				};

				repo.Add(zlu);

				if(repo.Save().Any(q => q != BackendQueryStatus.ReturnCode.Success))
				{
					//TODO Rollback
					return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.BackendUserQueryError);
				}

				return HttpResponse.NewResponse().AddData(zlu.GetClientSideSafeUserObject());
			}
			else
			{
				return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.UserAlreadyAuthenticated);
			}
		}

		[RESTContract(RESTContractAttribute.RequestType.POST, RESTKeys.RESTHandlerRequest, "MODIFYACCOUNTINFO")]
		/// <summary>
		/// Modifies the account info.
		/// </summary>
		/// <returns>The account info.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		/// <param name="postData">Post data.</param>
		public HttpResponse ModifyAccountInfo(Dictionary<string, string> headers, CustomSessionIdentity si, Dictionary<string,string> postData)
		{
			return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.NotImplemented);
		}

		[RESTContract(RESTContractAttribute.RequestType.POST, RESTKeys.RESTHandlerRequest, "DELETEACCOUNT")]
		/// <summary>
		/// Deletes the account.
		/// </summary>
		/// <returns>The account.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		/// <param name="postData">Post data.</param>
		public HttpResponse DeleteAccount(Dictionary<string, string> headers, CustomSessionIdentity si, Dictionary<string,string> postData)
		{
			return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.NotImplemented);
		}

		[RESTContract(RESTContractAttribute.RequestType.GET, RESTKeys.RESTHandlerRequest, "GETSESSIONSTATE")]
		/// <summary>
		/// Gets the state of the session.
		/// </summary>
		/// <returns>The session state.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		public HttpResponse GetSessionState(Dictionary<string, string> headers, CustomSessionIdentity si)
		{
			if(si.Guest)
			{
				CustomUserAccount zlu = new CustomUserAccount();
				zlu.UserName = "guest";
				zlu.ObjectID = System.Guid.Empty;
				return HttpResponse.NewResponse().AddData(zlu);
			}
			else
			{
				CustomUserAccount zlu = CustomUserAccount.Retrieve(si);
				if(zlu !=null)
				{
					return HttpResponse.NewResponse().AddData(zlu.GetClientSideSafeUserObject());
				}
				else
				{
					return  HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.BackendSessionQueryError);
				}
			}
		}

		[RESTContract(RESTContractAttribute.RequestType.GET, RESTKeys.RESTHandlerRequest, "LOGOUTUSER")]
		/// <summary>
		/// Logouts the user.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="headers">Headers.</param>
		/// <param name="si">Si.</param>
		public HttpResponse LogoutUser(Dictionary<string, string> headers,CustomSessionIdentity si)
		{
			return HttpResponse.NewResponse().AddErrorCode(GuruMeditation.ErrorCode.NotImplemented);
		}
	}
}

