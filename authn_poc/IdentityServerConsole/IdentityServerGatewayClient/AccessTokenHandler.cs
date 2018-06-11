using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.Client;
using log4net;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;

namespace OIDCGatewayClient
{
    public class AccessTokenHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task GetAccessToken(AuthorizationCodeReceivedNotification notification)
        {
            Logger.Debug("*** AuthorizationCodeReceived");
            var configuration =
                await notification.Options.ConfigurationManager.GetConfigurationAsync(notification.Request.CallCancelled);

            var accessToken = await GetAccessToken(notification, configuration);
            var userInfoClaims = await GetUserInfoClaims(configuration, accessToken);
            BuildClaims(notification, userInfoClaims);
            StoreTokenInCookie(accessToken);
           
        }

        private static async Task<string> GetAccessToken(AuthorizationCodeReceivedNotification notification,
                                                        OpenIdConnectConfiguration configuration)
        {
            // use the code to get the access and refresh token
            var tokenClient = new TokenClient(configuration.TokenEndpoint, 
                                                notification.Options.ClientId,
                                                notification.Options.ClientSecret);
            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(notification.Code, notification.RedirectUri);
            if (tokenResponse.IsError) throw new Exception(tokenResponse.Error);
            Logger.Debug("*** Access Token Received");
            return tokenResponse.AccessToken;
        }

        private static async Task<IEnumerable<Claim>> GetUserInfoClaims(OpenIdConnectConfiguration configuration, string accessToken)
        {
            // use the access token to retrieve claims from userinfo
            var userInfoClient = new UserInfoClient(configuration.UserInfoEndpoint);
            var userInfoResponse = await userInfoClient.GetAsync(accessToken);
            if(userInfoResponse.IsError) throw new Exception(userInfoResponse.Error);
            Logger.Debug("*** UserInfo Received");
            return userInfoResponse.Claims;
        }

        private static void StoreTokenInCookie(string accessToken)
        {
            // this is perhaps not OIDC gateway client - maybe move????
            var cookie = new HttpCookie("access_token")
            {
                Value = accessToken,
                HttpOnly = true,
                Secure = true
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            Logger.Debug("*** Access Token Added to Cookies");
        }

        private static void BuildClaims(AuthorizationCodeReceivedNotification notification, 
                                        IEnumerable<Claim> userInfoClaims)
        {
            var authTicket = notification.AuthenticationTicket;
            var identity = authTicket.Identity;
            var nid = new ClaimsIdentity(identity.AuthenticationType);
            nid.AddClaims(identity.Claims);
            //nid.AddClaim(new Claim("refresh_token", tokenResponse.RefreshToken));
            nid.AddClaims(userInfoClaims);
            nid.AddClaim(new Claim("userInfor claims count", userInfoClaims.Count().ToString()));
            notification.AuthenticationTicket = new AuthenticationTicket(nid, authTicket.Properties);
        }
    }
}
