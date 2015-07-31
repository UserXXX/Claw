using Claw.Commands;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Logic
{
    /// <summary>
    /// The implementation of the MVP presenter component for the commands display.
    /// </summary>
    public class CommandsPresenter : ICommandsPresenter
    {
        private ICommandsView view;

        private IMainPresenter mainPresenter;

        public MadCatzProfile ActiveProfile
        {
            get { return mainPresenter.ActiveProfile; }
        }

        /// <summary>
        /// Creates a new CommandsPresenter.
        /// </summary>
        /// <param name="presenter">The main presenter of the application.</param>
        public CommandsPresenter(IMainPresenter presenter)
        {
            mainPresenter = presenter;
        }

        public void SetView(ICommandsView newView)
        {
            view = newView;
        }

        public void ActiveProfileChanged(MadCatzProfile profile)
        {
            view.ActiveProfileChanged(profile);
        }

        public void OnNameChangeRequested(Command command, string newName)
        {
            mainPresenter.Model.ChangeCommandName(mainPresenter.ActiveProfile, command, newName);
            view.CommandNameChanged(command);
        }
    }
}
