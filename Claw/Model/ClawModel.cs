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
        private Dictionary<MadCatzProfile, FileInfo> profileFiles = new Dictionary<MadCatzProfile, FileInfo>();

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
                profileFiles.Add(profile, file);
            }

            return !errorCatcher.ErrorOccured;
        }
    }
}
