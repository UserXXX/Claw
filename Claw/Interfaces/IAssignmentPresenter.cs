using Claw.Commands;
using Claw.Controllers;
using Claw.Controllers.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// Presenter component of the MVP pattern for the assignments panel.
    /// </summary>
    public interface IAssignmentPresenter
    {
        /// <summary>
        /// Sets the view associated with this presenter.
        /// </summary>
        /// <param name="assignView">The view.</param>
        void SetView(IAssignmentView assignView);

        /// <summary>
        /// Notifies this presenter that the active profile changed, causing it to load the UI data again.
        /// </summary>
        /// <param name="profile">The new active profile.</param>
        void ActiveProfileChanged(MadCatzProfile profile);

        /// <summary>
        /// Requests the presenter to try to add a default controller instance with the given uuid to the given profile.
        /// If the given uuid is not one of the default ones, an ArgumentException is thrown.
        /// </summary>
        /// <param name="profile">The profile to add the controller to.</param>
        /// <param name="requestedUuid">The unique identifier of the controller to add.</param>
        /// <returns>The inserted controller.</returns>
        Controller RequestInsertController(MadCatzProfile profile, Guid requestedUuid);

        /// <summary>
        /// Notifies this presenter that the name of the given command changed.
        /// </summary>
        /// <param name="command">The changed command.</param>
        void CommandNameChanged(Command command);

        /// <summary>
        /// Notifies the presenter that the user wishes to change the command associated with the given control.
        /// </summary>
        /// <param name="shift">The shift that shall be programmed.</param>
        /// <param name="control">The control.</param>
        /// <param name="command">The command.</param>
        void OnAssociateCommandRequested(Shift shift, Control control, Command command);
    }
}
