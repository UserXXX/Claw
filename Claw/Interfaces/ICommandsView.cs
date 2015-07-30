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
    }
}
