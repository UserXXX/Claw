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
        private const string ERROR_MSG_INVALID_PROFILE = "InvalidPr0File";
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
            FileInfo file = view.SelectProfileFile();
            if (file == null)
            {
                return;
            }

            if (file.Extension.ToUpperInvariant() != PR0_FILE_EXTENSION)
            {
                view.ShowErrorMessage((string)App.Current.FindResource(ERROR_MSG_INVALID_PROFILE));
                return;
            }

            var report = new ClawValidationReport();
            bool success = model.LoadProfile(file, report);

            if (report.SomethingHappened)
            {
                view.ShowMessage((string)App.Current.FindResource(VALIDATION_MSG_STH_HAPPENED) + "\n" + report.Message);
            }

            if (!success)
            {
                return;
            }

            MadCatzProfile profile = model.Profiles.Last.Value;
            view.AddProfile(profile);
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
