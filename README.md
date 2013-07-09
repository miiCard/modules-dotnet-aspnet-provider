modules-dotnet-aspnet-provider
=
The miiCard.Providers.ASPNet library allows quick miiCard integration into ASP.NET MVC4 websites that use the OAuthWebSecurity class.

##What is miiCard
miiCard lets anybody prove their identity to the same level of traceability as using a passport, driver's licence or photo ID. We then allow external web applications to ask miiCard users to share a portion of their identity information with the application through a web-accessible API.

##What is the provider for?
The miiCard ASP.NET provider allows easy integration with the new [ASP.NET OAuthWebSecurity](http://www.asp.net/mvc/tutorials/security/using-oauth-providers-with-mvc) features of MVC4.

With as little as a single line of code you can start accepting miiCard identities in your web application alongside other providers like Facebook and Google, bringing strong identity assurance to your site in minutes.

You can obtain a consumer key and secret from miiCard by contacting us via our [miiCard for Developers](http://www.miicard.com/for/developers) site.

##Usage
The following assumes you have used the standard ASP.NET MVC4 Internet Application template.

First, add a reference to the miiCard.Providers.ASPNet NuGet package or associated source code:

    PM> install-package miiCard.Providers.ASPNet

Then add your miiCard consumer key and secret to your web.config file:

    <appSettings>
       <add key="MiiCardConsumerKey" value="key" />
       <add key="MiiCardConsumerSecret" value="secret" />
    </appSettings>

Finally, register the miiCard provider with OAuthWebSecurity by editing App_Code\AuthConfig.cs:

    public static void RegisterAuth()
    {
       OAuthWebSecurity.RegisterClient(new MiiCardClient(), "miiCard", new Dictionary<string, object>());
    }

Fire up your application and hit the Login page - you should now find the miiCard sign-in option available.

###Customisation
By default, the miiCard provider will return extra information about the miiCard member who signs into your application via the standard ExtraData dictionary. You can access and persist this information yourself as required, from the AccountController.ExternalLoginCallback method.

For your convenience, the keys that index the default data returned by the provider are available via the ExtraDataKeys static class.

<table>
<tr>
<th>Key</th>
<th>ExtraDataKeys equiv</th>
<th>Data</th>
</tr>
<tr>
<th>accesstoken</th>
<td>ExtraDataKeys.ACCESS_TOKEN</td>
<td>OAuth access token for use in subsequent API calls, if you wish to make use of the [Claims API](http://www.miicard.com/developers/claims-api)</td>
</tr>
<tr>
<th>accesstokensecret</th>
<td>ExtraDataKeys.ACCESS_TOKEN_SECRET</td>
<td>OAuth access token secret corresponding to the access token above</td>
</tr>
<tr>
<th>name</th>
<td>ExtraDataKeys.NAME</td>
<td>The miiCard member's full name, if supplied, or their miiCard username</td>
</tr>
<tr>
<th>identityassured</th>
<td>ExtraDataKeys.IDENTITY_ASSURED</td>
<td>Identity assurance status of the miiCard member - 'true' or 'false'</td>
</tr>
<tr>
<th>miicardusername</th>
<td>ExtraDataKeys.MIICARD_USERNAME</td>
<td>The miiCard member's username</td>
</tr>
<tr>
<th>lastverifieddate</th>
<td>ExtraDataKeys.LAST_VERIFIED_DATE</td>
<td>ISO 8601 formatted date and time at which the miiCard member's identity was last assured</td>
</tr>
</table>

Your application can hook the MiiCardClient.SigningIn event to customise the ExtraData dictionary as required - it's passed into the event as an argument:

    public static void RegisterAuth()
    {
       var miiCardClient = new MiiCardClient("key", "secret");
       miiCardClient.SigningIn += miiCardClient_SigningIn;
    
       OAuthWebSecurity.RegisterClient(miiCardClient, "miiCard", new Dictionary<string, object>());
    }
    
    static void miiCardClient_SigningIn(object sender, MiiCardSigningInEventArgs e)
    {
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

If you want to cancel the sign-in process for any reason, such as the miiCard member not having shared information you require (or any other validation you wish to perform), you can set the event argument object's Cancel property to true.

##Dependencies
The library takes dependencies on the DotNetOpenAuth.AspNet package, and the miiCard.Consumers package. Both are available via NuGet and will be resolved automatically.

##Licence
Copyright (c) 2013, miiCard Limited All rights reserved.

http://opensource.org/licenses/BSD-3-Clause

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

- Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

- Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

- Neither the name of miiCard Limited nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.