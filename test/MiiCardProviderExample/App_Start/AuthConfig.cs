using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using miiCard.Consumers.Service.v1.Claims;
using miiCard.Providers.ASPNet;
using MiiCardProviderExample.Models;

namespace MiiCardProviderExample
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To pull consumer key and secret from appSettings keys MiiCardConsumerKey and MiiCardConsumerSecret:
            // OAuthWebSecurity.RegisterClient(new MiiCardClient());

            // To specify them explicitly
            // OAuthWebSecurity.RegisterClient(new MiiCardClient("key", "secret"));

            var miiCardClient = new MiiCardClient("key", "secret");
            // This event handler is totally optional - it lets us customise how miiCard identity information is
            // recorded against the new user record.
            miiCardClient.SigningIn += miiCardClient_SigningIn;

            OAuthWebSecurity.RegisterClient(miiCardClient, "miiCard", new Dictionary<string, object>());

            // We'll also allow sign-in via Facebook for this example
        }

        static void miiCardClient_SigningIn(object sender, MiiCardSigningInEventArgs e)
        {
            // If we wish, we can pull in data from the miiCard profile into the ExtraData
            // dictionary to be recorded against the user. Some are added by default:
            // var accessToken = e.ExtraData[ExtraDataKeys.ACCESS_TOKEN];
            // var accessTokenSecret = e.ExtraData[ExtraDataKeys.ACCESS_TOKEN_SECRET];

            // You can replace the existing keys entirely if you wish, or alter their
            // representation. Here we'll extract the user's city and country from their
            // verified postal address, if we can
            PostalAddress verifiedAddress = (e.MiiCardUserProfile.PostalAddresses ?? Enumerable.Empty<PostalAddress>()).FirstOrDefault(x => x.Verified);
            if (verifiedAddress != null)
            {
                e.ExtraData["region"] = string.Format("{0}, {1}", verifiedAddress.City, verifiedAddress.Country);
            }

            // We can also completely halt the login process and reject this miiCard member
            // if we need to. For example - let's reject miiCard members who don't have a
            // public profile enabled.
            // e.Cancel = !e.MiiCardUserProfile.HasPublicProfile;
        }
    }
}
