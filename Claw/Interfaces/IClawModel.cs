using Claw.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// Model interface part of the MVP pattern implementation for Claw.
    /// </summary>
    public interface IClawModel
    {
        /// <summary>
        /// The profiles managed by this model.
        /// </summary>
        LinkedList<MadCatzProfile> Profiles
        {
            get;
        }

        /// <summary>
        /// Loads a profile from the given file into the model. If an error occurs during loading the profile is not added to the model.
        /// </summary>
        /// <param name="file">File to load.</param>
        /// <param name="report">Report to store infos, errors and warnings in.</param>
        /// <returns>Whether loading was successful.</returns>
        bool LoadProfile(FileInfo file, ValidationReport report);
    }
}
