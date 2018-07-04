using DataBrowser.Service.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DataBrowser.Api
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var userDetails = new UserService();
            var isUser = userDetails.FindUser(context.UserName, context.Password);
            if (!isUser)
            {
                context.SetError("invalid_grant",
                   "The user name or password is incorrect.");
                return;
            }

            identity.AddClaim(new Claim("UserName", context.UserName));
            identity.AddClaim(new Claim("Password", context.Password));
            identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
            context.Validated(identity);


        }

    }
}