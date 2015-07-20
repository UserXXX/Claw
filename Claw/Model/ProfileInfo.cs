using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Claw.Model
{
    /// <summary>
    /// Contains various information associted with a MadCatzProfile.
    /// </summary>
    public class ProfileInfo
    {
        private FileInfo profileFile;
        /// <summary>
        /// The file for saving the profile.
        /// </summary>
        public FileInfo ProfileFile
        {
            get { return profileFile; }
            set { profileFile = value; }
        }

        private bool edited = false;
        /// <summary>
        /// Whether the profile has been edited.
        /// </summary>
        public bool Edited
        {
            get { return edited; }
            set { edited = value; }
        }

        /// <summary>
        /// Creates a new ProfileInfo.
        /// </summary>
        /// <param name="profileSaveFile">The location to save the profile to.</param>
        public ProfileInfo(FileInfo profileSaveFile)
        {
            profileFile = profileSaveFile;
        }
    }
}
