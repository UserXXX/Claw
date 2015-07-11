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
        /// Sets the presenter for this view.
        /// </summary>
        /// <param name="mainPresenter">The presenter.</param>
        void SetPresenter(IMainPresenter mainPresenter);

        /// <summary>
        /// Displays a file select dialog or something equal to open a *.pr0 profile file.
        /// </summary>
        /// <returns>The info for the selected file or null if the selection was cancelled.</returns>
        FileInfo SelectProfileFile();

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
    }
}
