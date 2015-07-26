using Claw.Blasts;
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

        /// <summary>
        /// Adds an icon to a profile.
        /// </summary>
        /// <param name="profile">The profile to add the icon to.</param>
        /// <param name="pngData">The data of the PNG image file.</param>
        void AddIcon(MadCatzProfile profile, byte[] pngData);

        /// <summary>
        /// Checks whether the given profile has been edited.
        /// </summary>
        /// <param name="profile">The profile to check.</param>
        /// <returns>Whether the profile has been edited.</returns>
        bool HasBeenEdited(MadCatzProfile profile);

        /// <summary>
        /// Closes the given profile (meaning releasing it for garbage collection).
        /// </summary>
        /// <param name="profile">The profile to close.</param>
        void CloseProfile(MadCatzProfile profile);

        /// <summary>
        /// Saves the given profile. This throws an IOException if saving fails.
        /// </summary>
        /// <param name="profile">The profile to save.</param>
        void SaveProfile(MadCatzProfile profile);

        /// <summary>
        /// Saves the given profile at the given location and updates the save path.
        /// </summary>
        /// <param name="profile">The profile to save.</param>
        /// <param name="saveFile">The file to save it to.</param>
        void SaveProfile(MadCatzProfile profile, FileInfo saveFile);

        /// <summary>
        /// Chcks whether the model holds a save file for the given profile.
        /// </summary>
        /// <param name="profile">The profile to check for.</param>
        /// <returns>Whether there is a save file.</returns>
        bool HasSaveFile(MadCatzProfile profile);

        /// <summary>
        /// Gets the file info describing where to save the profile to.
        /// </summary>
        /// <param name="profile">The profile thats save file shall be queried.</param>
        /// <returns>The save file.</returns>
        FileInfo GetSaveFile(MadCatzProfile profile);

        /// <summary>
        /// Creates a new profile using the given name.
        /// </summary>
        /// <param name="name">The name of the profile.</param>
        /// <returns>The created profile.</returns>
        MadCatzProfile CreateNewProfile(string name);

        /// <summary>
        /// Removes the given icon from the given profile.
        /// </summary>
        /// <param name="profile">The profile to remove from.</param>
        /// <param name="blast">The icon to remove.</param>
        void RemoveIcon(MadCatzProfile profile, Blast blast);
    }
}
