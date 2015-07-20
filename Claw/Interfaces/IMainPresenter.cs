using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// Presenter component of the MVP pattern for the main view.
    /// </summary>
    public interface IMainPresenter
    {
        /// <summary>
        /// The active profile that is currently edited.
        /// </summary>
        MadCatzProfile ActiveProfile { get; }

        /// <summary>
        /// The underlying data model.
        /// </summary>
        IClawModel Model { get; }

        /// <summary>
        /// Sets the view component for this presenter.
        /// </summary>
        /// <param name="mainView">The view.</param>
        void SetView(IMainView mainView);

        /// <summary>
        /// Sets the underlying data model.
        /// </summary>
        /// <param name="clawModel">The model.</param>
        void SetModel(IClawModel clawModel);

        /// <summary>
        /// Notification that the user requests to open a file.
        /// </summary>
        void OpenFileRequested();

        /// <summary>
        /// Notifies the presenter, that the user wants to close the given profile.
        /// </summary>
        /// <param name="profile">The profile to close.</param>
        void CloseProfileRequested(MadCatzProfile profile);

        /// <summary>
        /// Called when the currently active profile changes.
        /// </summary>
        /// <param name="profile">The new active profile.</param>
        void ActiveProfileChanged(MadCatzProfile profile);

        /// <summary>
        /// Forwards an error message to the main UI.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        void ForwardError(string errorMessage);
    }
}
