using Claw.Blasts;
using Claw.Commands;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Claw.Logic
{
    /// <summary>
    /// The implementation of the MVP presenter component for the commands display.
    /// </summary>
    public class CommandsPresenter : ICommandsPresenter
    {
        private const string DEFAULT_COMMAND_NAME = "DefaultCommandName";
        private const string QUESTION_SURE_TO_DELETE_COMMANDS = "SureToRemoveCommands";

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

        public void OnIconChangeRequested(Command command, Blast blast)
        {
            mainPresenter.Model.ChangeCommandIcon(ActiveProfile, command, blast);
            view.CommandIconChanged(command);
        }

        public void OnCreateCommandRequested()
        {
            Command command = mainPresenter.Model.CreateNewCommand(ActiveProfile, (string)Application.Current.FindResource(DEFAULT_COMMAND_NAME));
            view.ActiveProfileChanged(ActiveProfile);
            view.SetActiveCommand(command);
        }

        public void OnDeleteCommandsRequested(Command[] commands)
        {
            if (!mainPresenter.ForwardYesNoQuestion((string)Application.Current.FindResource(QUESTION_SURE_TO_DELETE_COMMANDS)))
            {
                return;
            }

            foreach (Command command in commands)
            {
                mainPresenter.Model.RemoveCommand(ActiveProfile, command);
            }

            view.ActiveProfileChanged(ActiveProfile);
        }
    }
}
