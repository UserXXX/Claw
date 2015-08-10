using Claw.Blasts;
using Claw.Commands;
using Claw.Controllers;
using Claw.Controllers.Assignments;
using Claw.Controllers.Controls;
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

        public void ChangeCommandName(MadCatzProfile profile, Command command, string newName)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.Name = newName;
            profileInfos[profile].Edited = true;
        }

        public void ChangeCommandIcon(MadCatzProfile profile, Command command, Blast blast)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.SetIcon(blast);
            profileInfos[profile].Edited = true;
        }
       
        public Command CreateNewCommand(MadCatzProfile profile, string defaultName)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            Command command = null;
            if (profile.Commands.GetCommandByName(defaultName) == null)
            {
                command = new ActionCommand(defaultName);
            }
            else
            {
                int counter = 1;
                while (profile.Commands.GetCommandByName(defaultName + counter) != null)
                {
                    counter++;
                }
                command = new ActionCommand(defaultName + counter);
            }

            profile.Commands.Add(command);
            profileInfos[profile].Edited = true;

            return command;
        }

        public void RemoveCommand(MadCatzProfile profile, Command command)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            profile.Commands.Remove(command);
            profileInfos[profile].Edited = true;
        }

        public Controller TryInsertController(MadCatzProfile profile, Guid requestedUuid)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            Controller controller = Controller.CreateDefaultById(requestedUuid);

            profile.Controllers.Add(controller);
            profileInfos[profile].Edited = true;

            return controller;
        }

        public void AssociateCommand(MadCatzProfile profile, Shift shift, Control control, Command command)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }
            if (shift == null)
            {
                throw new ArgumentNullException("shift");
            }

            if (!(control is ButtonControl))
            {
                throw new InvalidOperationException("Can't associate a command to a non-button control.");
            }

            if (command != null && !profile.Commands.Contains(command))
            {
                throw new ArgumentException("The given command needs to be in the same profile as the given control.");
            }

            profileInfos[profile].Edited = true;

            if (command == null)
            {
                shift.Assignments.RemoveAssignmentForControl(control);
                return;
            }

            var band = new Band(command);

            ButtonAssignment buttonAssignment = GetButtonAssignment(shift, control);
            buttonAssignment.Bands.Clear();
            buttonAssignment.Bands.Add(band);
        }

        /// <summary>
        /// Gets the button assignment for the given control in the given shift, creates a new one if none exists.
        /// </summary>
        /// <param name="shift">The parent shift of the control.</param>
        /// <param name="control">The control to get a button assignment for.</param>
        /// <returns>The new or existing button assignment.</returns>
        private static ButtonAssignment GetButtonAssignment(Shift shift, Control control)
        {
            Assignment assignment = shift.Assignments.GetAssignmentForControl(control);

            if (assignment == null)
            {
                var buttonAssignment = new ButtonAssignment(control);
                shift.Assignments.Add(buttonAssignment);
                return buttonAssignment;
            }

            ButtonAssignment button = assignment as ButtonAssignment;

            if (button == null)
            {
                throw new InvalidOperationException("Profile structure is corrupted! A button control also needs a button assignment!");
            }
            return button;
        }
    }
}
