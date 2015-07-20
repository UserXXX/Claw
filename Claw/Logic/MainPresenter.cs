using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Claw.Logic
{
    /// <summary>
    /// Presenter implementation for the main presenter.
    /// </summary>
    public class MainPresenter : IMainPresenter
    {
        private const string PR0_FILE_EXTENSION = ".PR0";
        private const string ERROR_MSG_INVALID_PROFILES = "InvalidPr0Files";
        private const string ERROR_MSG_UNABLE_TO_SAVE = "UnableToSave";
        private const string VALIDATION_MSG_STH_HAPPENED = "ValidationSthHappened";
        private const string QUESTION_SAVE_BEFORE_CLOSING = "SaveBoforeClosing";

        private IMainView view;
        private IClawModel model;
        public IClawModel Model
        {
            get { return model; }
        }

        private IIconsPresenter iconsPresenter;

        private MadCatzProfile activeProfile;
        public MadCatzProfile ActiveProfile
        {
            get { return activeProfile; }
        }

        public void SetView(IMainView mainView)
        {
            view = mainView;
            iconsPresenter = new IconsPresenter(this);
            IIconsView iconsView = view.IconsView;
            iconsPresenter.SetView(iconsView);
            iconsView.SetPresenter(iconsPresenter);
        }

        public void SetModel(IClawModel clawModel)
        {
            model = clawModel;
        }

        public void OpenFileRequested()
        {
            FileInfo[] files = view.SelectProfileFiles();
            if (files == null)
            {
                return;
            }

            string invalidFiles = null;
            LinkedList<string> validationMessages = new LinkedList<string>();
            foreach (FileInfo file in files)
            {
                if (file.Extension.ToUpperInvariant() != PR0_FILE_EXTENSION)
                {
                    if (invalidFiles == null)
                    {
                        invalidFiles = file.Name;
                    }
                    else
                    {
                        invalidFiles += "\n" + file.Name;
                    }
                    continue;
                }

                var report = new ClawValidationReport();
                bool success = model.LoadProfile(file, report);

                if (report.SomethingHappened)
                {
                    validationMessages.AddLast((string)App.Current.FindResource(VALIDATION_MSG_STH_HAPPENED) + "\n" + report.Message);
                }

                if (!success)
                {
                    continue;
                }

                MadCatzProfile profile = model.Profiles.Last.Value;
                view.AddProfile(profile);
            }

            if (invalidFiles != null)
            {
                view.ShowErrorMessage((string)App.Current.FindResource(ERROR_MSG_INVALID_PROFILES) + "\n" + invalidFiles);
            }

            foreach (string msg in validationMessages)
            {
                view.ShowMessage(msg);
            }
        }

        public void CloseProfileRequested(MadCatzProfile profile)
        {
            if (model.HasBeenEdited(profile))
            {
                bool? doSave = view.ShowYesNoAbortQuestion(string.Format((string)App.Current.FindResource(QUESTION_SAVE_BEFORE_CLOSING), profile.Name));
                if (!doSave.HasValue)
                {
                    return;
                }

                // If saveing is cancelld it is also necessary to cancel closing.
                // Be aware: SaveProfileRequested is only executed when doSave.Value is true because of rapid evaluation.
                if (doSave.Value && !SaveProfileRequested(profile))
                {
                    return;
                }
            }

            model.CloseProfile(profile);

            if (profile == activeProfile)
            {
                if (model.Profiles.Count == 0)
                {
                    activeProfile = null;
                }
                else
                {
                    activeProfile = model.Profiles.First.Value;
                }
                view.SetActiveProfile(activeProfile);
            }

            view.ProfileClosed(profile);
        }

        public void SaveActiveProfileRequested()
        {
            SaveProfileRequested(activeProfile);
        }

        /// <summary>
        /// Asks to save the given profile.
        /// </summary>
        /// <param name="profile">The profile to save.</param>
        /// <returns>Whether the profile was saved.</returns>
        public bool SaveProfileRequested(MadCatzProfile profile)
        {
            if (!model.HasSaveFile(profile))
            {
                return SaveProfileAsRequested(profile);
            }

            try
            {
                model.SaveProfile(profile);
                return true;
            }
            catch (IOException exc)
            {
                view.ShowErrorMessage(App.Current.FindResource(ERROR_MSG_UNABLE_TO_SAVE) + " " + exc.ToString());
                return false;
            }
        }

        public void SaveActiveProfileAsRequested()
        {
            SaveProfileAsRequested(activeProfile);
        }

        /// <summary>
        /// Does a save as for the given profile.
        /// </summary>
        /// <param name="profile">The profile to save.</param>
        /// <returns>Whether the profile was saved.</returns>
        public bool SaveProfileAsRequested(MadCatzProfile profile)
        {
            FileInfo saveFile = view.SelectProfileSaveFile(profile, model.GetSaveFile(profile));

            if (saveFile == null)
            {
                return false;
            }

            model.SaveProfile(profile, saveFile);
            return true;
        }

        public void ActiveProfileChanged(MadCatzProfile profile)
        {
            activeProfile = profile;
            iconsPresenter.ActiveProfileChanged(profile);
        }

        public void ForwardError(string error)
        {
            view.ShowErrorMessage(error);
        }
    }
}
