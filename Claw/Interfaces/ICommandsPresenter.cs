﻿using Claw.Blasts;
using Claw.Commands;
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

        /// <summary>
        /// Notifies the presenter that the user wants to change the icon of the given command.
        /// </summary>
        /// <param name="command">The command tahts icon shall be changed.</param>
        /// <param name="blast">The blast to set as icon.</param>
        void OnIconChangeRequested(Command command, Blast blast);

        /// <summary>
        /// Notifies the presenter that the user wants to create a new command.
        /// </summary>
        void OnCreateCommandRequested();

        /// <summary>
        /// Notifies the presenter, that the user wants to delete the given commands.
        /// </summary>
        /// <param name="commands">The commands to delete.</param>
        void OnDeleteCommandsRequested(Command[] commands);

        /// <summary>
        /// Notifies this presenter that the name of the given command changed.
        /// </summary>
        /// <param name="command">The changed command.</param>
        void CommandNameChanged(Command command);
    }
}
