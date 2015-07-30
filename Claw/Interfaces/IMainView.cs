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
        /// The commands view associated with this view.
        /// </summary>
        ICommandsView CommandsView
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

        /// <summary>
        /// Asks the user to select a save file for the given profile.
        /// </summary>
        /// <param name="profile">The profile that shall be saved.</param>
        /// <param name="currentSaveFile">The file the profile is currently saved to.</param>
        /// <returns>The selected file info or null if the action was cancelled.</returns>
        FileInfo SelectProfileSaveFile(MadCatzProfile profile, FileInfo currentSaveFile);

        /// <summary>
        /// Shows a question to the user with the answer possibilities yes, no and abort.
        /// </summary>
        /// <param name="message">The question message.</param>
        /// <returns>True, if the user selected yes, false if no and null if the user aborted.</returns>
        bool? ShowYesNoAbortQuestion(string message);

        /// <summary>
        /// Shows a text input dialog.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="message">The message of the dialog.</param>
        /// <param name="forbiddenChars">The chars that are not allowed in the result.</param>
        /// <returns>The text given by the user or null if the dialog was cancelled.</returns>
        string ShowTextQuestion(string title, string message, char[] forbiddenChars);

        /// <summary>
        /// Shows a question to the user with the answer possibilities yes and no.
        /// </summary>
        /// <param name="question">The question message.</param>
        /// <returns>True, if the user selected yes, false if no.</returns>
        bool ShowYesNoQuestion(string question);
    }
}
