using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using miiCard.Consumers.Service.v1.Claims;

namespace miiCard.Providers.ASPNet
{
    /// <summary>
    /// Provides data for the event that is raised as a miiCard member is signing into a web application 
    /// using their miiCard details.
    /// </summary>
    public class MiiCardSigningInEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the set of additional data to be recorded against the user.
        /// </summary>
        public Dictionary<string, string> ExtraData { get; private set; }

        /// <summary>
        /// Gets the MiiUserProfile object that represents the data that the miiCard member
        /// elected to share with your application.
        /// </summary>
        public MiiUserProfile MiiCardUserProfile { get; private set; }

        /// <summary>
        /// Gets or sets whether the sign-in attempt should be cancelled. If set to true, the sign-in
        /// process is aborted.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Initialises a new instance of MiiCardSigningInEventArgs with an 'extra-data' dictionary and the
        /// MiiUserProfile of the miiCard member who is signing in.
        /// </summary>
        /// <param name="userDictionary">The dictionary of additional information to be recorded against the user.</param>
        /// <param name="userProfile">The MiiUserProfile object representing the set of data that the miiCard member
        /// has elected to share with your application.</param>
        internal MiiCardSigningInEventArgs(Dictionary<string, string> userDictionary, MiiUserProfile userProfile)
        {
            this.ExtraData = userDictionary;
            this.MiiCardUserProfile = userProfile;
        }
    }
}
