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
    /// The view component of the MVP pattern for the assignments panel.
    /// </summary>
    public interface IAssignmentView
    {
        /// <summary>
        /// Sets the presenter associated with this view.
        /// </summary>
        /// <param name="assignPresenter">The presenter for this view.</param>
        void SetPresenter(IAssignmentPresenter assignPresenter);

        /// <summary>
        /// Notifies this view that the active profile changed. The presented data will be set to the one of the given profile.
        /// </summary>
        /// <param name="profile">The new active profile.</param>
        void ActiveProfileChanged(MadCatzProfile profile);

        /// <summary>
        /// Notifies the view, that the name of the given command changed.
        /// </summary>
        /// <param name="command">The changed command.</param>
        void CommandNameChanged(Command command);

        /// <summary>
        /// Notifies the view, that the associated command for the given control in the given shift changed.
        /// </summary>
        /// <param name="shift">The shift.</param>
        /// <param name="control">The control.</param>
        /// <param name="command">The command.</param>
        void ControlAssociationChanged(Shift shift, Control control, Command command);
    }
}
