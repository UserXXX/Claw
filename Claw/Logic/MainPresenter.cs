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
        private const string VALIDATION_MSG_STH_HAPPENED = "ValidationSthHappened";

        private IMainView view;
        private IClawModel model;

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
