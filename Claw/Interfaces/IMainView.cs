using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// View component interface of the MVP pattern for the main window.
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// The icons view associated with this view.
        /// </summary>
        IIconsView IconsView
        {
            get;
        }

        /// <summary>
        /// Sets the presenter for this view.
        /// </summary>
        /// <param name="mainPresenter">The presenter.</param>
        void SetPresenter(IMainPresenter mainPresenter);

        /// <summary>
        /// Displays a file select dialog or something equal to open *.pr0 profile files.
        /// </summary>
        /// <returns>The infos for the selected files or null if the selection was cancelled.</returns>
        FileInfo[] SelectProfileFiles();

        /// <summary>
        /// Displays an error message.
        /// </summary>
        /// <param name="message">Message to display.</param>
        void ShowErrorMessage(string message);

        /// <summary>
        /// Displays a message.
        /// </summary>
        /// <param name="message">Message to display.</param>
        void ShowMessage(string message);

        /// <summary>
        /// Notifies the UI that a new profile has been loaded. This one gets automatically selected.
        /// </summary>
        /// <param name="profile">The new profile.</param>
        void AddProfile(MadCatzProfile profile);

        /// <summary>
        /// Sets the currently active profile.
        /// </summary>
        /// <param name="activeProfile">The new active profile.</param>
        void SetActiveProfile(MadCatzProfile activeProfile);

        /// <summary>
        /// Notifies the view that a profile was closed. This will remove the profile from the available profiles.
        /// </summary>
        /// <param name="profile">The closed profile.</param>
        void ProfileClosed(MadCatzProfile profile);
    }
}
