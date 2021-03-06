﻿using Claw.Blasts;
using Claw.Commands;
using Claw.Controllers;
using Claw.Controllers.Controls;
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

        /// <summary>
        /// Changes the name of the given command to the new name.
        /// </summary>
        /// <param name="profile">The profile associated with the command.</param>
        /// <param name="command">The command to edit.</param>
        /// <param name="newName">The new name.</param>
        void ChangeCommandName(MadCatzProfile profile, Command command, string newName);

        /// <summary>
        /// Changes the icon for the given command within the given profile to the given icon.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="command">The command.</param>
        /// <param name="blast">The icon.</param>
        void ChangeCommandIcon(MadCatzProfile profile, Command command, Blast blast);

        /// <summary>
        /// Creates a new command for the given profile.
        /// </summary>
        /// <param name="profile">The profile to create a command for.</param>
        /// <param name="defaultName">The default name for a new command.</param>
        /// <returns>The created command.</returns>
        Command CreateNewCommand(MadCatzProfile profile, string defaultName);

        /// <summary>
        /// Removes the given command from the given profile.
        /// </summary>
        /// <param name="profile">The profile to remove from.</param>
        /// <param name="command">The command to remove.</param>
        void RemoveCommand(MadCatzProfile profile, Command command);

        /// <summary>
        /// Tries to insert the default controller for the given UUID into the profile. Throws an ArgumentException if the
        /// given UUID is not one of the default UUIDs.
        /// </summary>
        /// <param name="profile">The profile to insert into.</param>
        /// <param name="requestedUuid">The UUID identifying the controller to insert.</param>
        /// <returns>The generated controller.</returns>
        Controller TryInsertController(MadCatzProfile profile, Guid requestedUuid);

        /// <summary>
        /// Associates the given control with the given command.
        /// </summary>
        /// <param name="profile">The profile that contains the control.</param>
        /// <param name="shift">The shift that shall be programmed.</param>
        /// <param name="control">The control.</param>
        /// <param name="command">The command to associate.</param>
        void AssociateCommand(MadCatzProfile profile, Shift shift, Control control, Command command);
    }
}
