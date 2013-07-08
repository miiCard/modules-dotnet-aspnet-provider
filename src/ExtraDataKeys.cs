using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miiCard.Providers.ASPNet
{
    /// <summary>
    /// Houses the keys used in the ExtraData dictionary for users who have signed in with a miiCard.
    /// </summary>
    public static class ExtraDataKeys
    {
        /// <summary>
        /// The ExtraData dictionary key that corresponds to the miiCard member's OAuth access token.
        /// </summary>
        public static readonly string ACCESS_TOKEN = "accesstoken";
        /// <summary>
        /// The ExtraData dictionary key that corresponds to the miiCard member's OAuth access token secret.
        /// </summary>
        public static readonly string ACCESS_TOKEN_SECRET = "accesstokensecret";
        /// <summary>
        /// The ExtraData dictionary key that corresponds to the miiCard member's full name, or their miiCard
        /// username if not supplied.
        /// </summary>
        public static readonly string NAME = "name";
        /// <summary>
        /// The ExtraData dictionary key that corresponds to the miiCard member's identity assurance status,
        /// a string of either 'true' or 'false'.
        /// </summary>
        public static readonly string IDENTITY_ASSURED = "identityassured";
        /// <summary>
        /// The ExtraData dictionary key that corresponds to the miiCard member's username.
        /// </summary>
        public static readonly string MIICARD_USERNAME = "miicardusername";
        /// <summary>
        /// The ExtraData dictionary key that corresponds to the date and time (in UTC) that the miiCard
        /// member's identity was last verified. This is rendered in round-trippable ISO 8601 format.
        /// </summary>
        public static readonly string LAST_VERIFIED_DATE = "lastverifieddate";
    }
}
