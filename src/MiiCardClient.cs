using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OAuth.Messages;
using miiCard.Consumers;
using miiCard.Consumers.Infrastructure;
using miiCard.Consumers.Service.v1.Claims;

namespace miiCard.Providers.ASPNet
{
    /// <summary>
    /// Allows a miiCard member to sign into your web application.
    /// </summary>
    public class MiiCardClient : OAuthClient
    {
        string _consumerKey;
        string _consumerSecret;

        /// <summary>
        /// Raised after the miiCard member has shared their identity with your application but before the sign-in process
        /// is completed, allowing your code to process the set of shared identity information before the user is logged in.
        /// </summary>
        public event EventHandler<MiiCardSigningInEventArgs> SigningIn = delegate { };

        /// <summary>
        /// Initialises a new MiiCardClient, pulling the miiCard consumer key and consumer secret from two web.config
        /// appSettings keys, 'MiiCardConsumerKey' and 'MiiCardConsumerSecret' respectively.
        /// </summary>
        public MiiCardClient()
            : this(ConfigurationManager.AppSettings["MiiCardConsumerKey"], ConfigurationManager.AppSettings["MiiCardConsumerSecret"])
        {

        }

        public MiiCardClient(string consumerKey, string consumerSecret)
            : base("miiCard", MiiCardConsumer.ServiceDescription, new SessionStateConsumerTokenManager(consumerKey, consumerSecret))
        {
            if (string.IsNullOrWhiteSpace(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }
            else if (string.IsNullOrWhiteSpace(consumerSecret))
            {
                throw new ArgumentNullException("consumerSecret");
            }

            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        protected override DotNetOpenAuth.AspNet.AuthenticationResult VerifyAuthenticationCore(DotNetOpenAuth.OAuth.Messages.AuthorizedTokenResponse response)
        {
            AuthenticationResult toReturn = null;

            try
            {
                var accessToken = response.AccessToken;
                var accessTokenSecret = ((ITokenSecretContainingMessage)response).TokenSecret;

                var miiCardApi = new miiCard.Consumers.Service.v1.MiiCardOAuthClaimsService(_consumerKey, _consumerSecret, accessToken, accessTokenSecret);
                var claimsResponse = miiCardApi.GetClaims();

                bool success = claimsResponse.Status == MiiApiCallStatus.Success;
                if (success && claimsResponse.Data != null)
                {
                    string friendlyName = string.Format("{0} {1}", claimsResponse.Data.FirstName, claimsResponse.Data.LastName);
                    if (string.IsNullOrWhiteSpace(friendlyName))
                    {
                        friendlyName = claimsResponse.Data.Username;
                    }

                    var extraData = new Dictionary<string, string>();
                    extraData.Add(ExtraDataKeys.ACCESS_TOKEN, accessToken);
                    extraData.Add(ExtraDataKeys.ACCESS_TOKEN_SECRET, accessTokenSecret);
                    extraData.Add(ExtraDataKeys.NAME, friendlyName);
                    extraData.Add(ExtraDataKeys.IDENTITY_ASSURED, claimsResponse.Data.IdentityAssured.ToString().ToLower());
                    extraData.Add(ExtraDataKeys.MIICARD_USERNAME, claimsResponse.Data.Username);

                    if (claimsResponse.Data.LastVerified.HasValue)
                    {
                        extraData.Add(ExtraDataKeys.LAST_VERIFIED_DATE, claimsResponse.Data.LastVerified.Value.ToUniversalTime().ToString("o"));
                    }

                    // If the caller's given us a processing event handler then let them have a stab at extracting some
                    // user profile data
                    var eventArgs = new MiiCardSigningInEventArgs(extraData, claimsResponse.Data);
                    this.SigningIn(this, eventArgs);

                    if (!eventArgs.Cancel)
                    {
                        toReturn = new AuthenticationResult(true, this.ProviderName, claimsResponse.Data.Username, friendlyName, extraData);
                    }
                    else
                    {
                        // Calling code's decided against letting this user sign in
                        toReturn = new AuthenticationResult(false);
                    }
                }
                else
                {
                    toReturn = new AuthenticationResult(false);
                }
            }
            catch (Exception ex)
            {
                toReturn = new AuthenticationResult(ex, this.ProviderName);
            }

            return toReturn;
        }
    }
}
