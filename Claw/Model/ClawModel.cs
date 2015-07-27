using Claw.Blasts;
using Claw.Interfaces;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Claw.Model
{
    /// <summary>
    /// Model implementation.
    /// </summary>
    public class ClawModel : IClawModel
    {
        private LinkedList<MadCatzProfile> profiles = new LinkedList<MadCatzProfile>();
        private Dictionary<MadCatzProfile, ProfileInfo> profileInfos = new Dictionary<MadCatzProfile, ProfileInfo>();

        public LinkedList<MadCatzProfile> Profiles
        {
            get { return profiles; }
        }

        public bool LoadProfile(FileInfo file, ValidationReport report)
        {
            var errorCatcher = new ErrorCatcherReport();

            MadCatzProfile profile = ProfileFactory.Load(file, new BranchReport(new ValidationReport[] {report, errorCatcher}));

            if (!errorCatcher.ErrorOccured)
            {
                profiles.AddLast(profile);
                profileInfos.Add(profile, new ProfileInfo(file));
            }

            return !errorCatcher.ErrorOccured;
        }

        public void AddIcon(MadCatzProfile profile, byte[] pngData)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            var blast = new Blast(pngData);
            profile.Blasts.Add(blast);
            profileInfos[profile].Edited = true;
        }

        public bool HasBeenEdited(MadCatzProfile profile)
        {
            return profileInfos[profile].Edited;
        }

        public void CloseProfile(MadCatzProfile profile)
        {
            profiles.Remove(profile);
            profileInfos.Remove(profile);
        }

        public void SaveProfile(MadCatzProfile profile)
        {
            if (!HasSaveFile(profile))
            {
                throw new InvalidOperationException("Can't save the profile: there is no save location available.");
            }

            ProfileFactory.Save(profileInfos[profile].ProfileFile, profile);
            profileInfos[profile].Edited = false;
        }

        public void SaveProfile(MadCatzProfile profile, FileInfo saveFile)
        {
            ProfileFactory.Save(saveFile, profile);
            profileInfos[profile].Edited = false;
            profileInfos[profile].ProfileFile = saveFile;
        }

        public bool HasSaveFile(MadCatzProfile profile)
        {
            return profileInfos[profile].ProfileFile != null;
        }
        
        public FileInfo GetSaveFile(MadCatzProfile profile)
        {
            return profileInfos[profile].ProfileFile;
        }

        public MadCatzProfile CreateNewProfile(string name)
        {
            var profile = new MadCatzProfile(name);
            profiles.AddLast(profile);
            profileInfos.Add(profile, new ProfileInfo());
            return profile;
        }

        public void RemoveIcon(MadCatzProfile profile, Blast blast)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            profile.Blasts.Remove(blast);
            profileInfos[profile].Edited = true;
        }
    }
}
