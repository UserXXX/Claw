using Claw.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// View component of the MVP pattern for the commands display.
    /// </summary>
    public interface ICommandsView
    {
        /// <summary>
        /// Sets the presenter for this view.
        /// </summary>
        /// <param name="newPresenter">The presenter.</param>
        void SetPresenter(ICommandsPresenter newPresenter);

        /// <summary>
        /// Called when the active profile changed.
        /// </summary>
        /// <param name="activeProfile">The new active profile. If null is passed, the UI is cleared.</param>
        void ActiveProfileChanged(MadCatzProfile activeProfile);

        /// <summary>
        /// Notifies the view that the command name changed and the display for the user needs to be adapted.
        /// </summary>
        /// <param name="command">The changed command.</param>
        void CommandNameChanged(Command command);

        /// <summary>
        /// Notifies the view that the command icon changed and the display needs to be adapted.
        /// </summary>
        /// <param name="command">The command thats icon changed.</param>
        void CommandIconChanged(Command command);
    }
}
