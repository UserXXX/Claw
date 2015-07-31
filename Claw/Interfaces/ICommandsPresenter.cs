﻿using Claw.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// Presenter component of the MVP pattern for the commands display.
    /// </summary>
    public interface ICommandsPresenter
    {
        /// <summary>
        /// The currently active profile.
        /// </summary>
        MadCatzProfile ActiveProfile { get; }

        /// <summary>
        /// Sets the view associated with this presenter.
        /// </summary>
        /// <param name="newView">The view to manage.</param>
        void SetView(ICommandsView newView);

        /// <summary>
        /// Called when the active profile changed.
        /// </summary>
        /// <param name="profile">The new active profile.</param>
        void ActiveProfileChanged(MadCatzProfile profile);

        /// <summary>
        /// Notifies the presenter that the user wishes to change the name of the given command.
        /// </summary>
        /// <param name="command">The command thats name shall be changed.</param>
        /// <param name="newName">The new name for the command.</param>
        void OnNameChangeRequested(Command command, string newName);
    }
}
