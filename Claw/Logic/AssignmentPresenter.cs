using Claw.Commands;
using Claw.Controllers;
using Claw.Controllers.Controls;
using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Logic
{
    /// <summary>
    /// Presenter implementation for the assginments panel.
    /// </summary>
    public class AssignmentPresenter : IAssignmentPresenter
    {
        private IAssignmentView view;

        private IMainPresenter mainPresenter;

        /// <summary>
        /// Creates a new AssignmentPresenter.
        /// </summary>
        /// <param name="presenter">The applications main presenter.</param>
        public AssignmentPresenter(IMainPresenter presenter)
        {
            mainPresenter = presenter;
        }

        public void SetView(IAssignmentView assignView)
        {
            view = assignView;
        }
        
        public void ActiveProfileChanged(MadCatzProfile profile)
        {
            view.ActiveProfileChanged(profile);
        }

        public Controller RequestInsertController(MadCatzProfile profile, Guid requestedUuid)
        {
            return mainPresenter.Model.TryInsertController(profile, requestedUuid);
        }

        public void CommandNameChanged(Command command)
        {
            view.CommandNameChanged(command);
        }

        public void OnAssociateCommandRequested(Shift shift, Control control, Command command)
        {
            mainPresenter.Model.AssociateCommand(mainPresenter.ActiveProfile, shift, control, command);
            view.ControlAssociationChanged(shift, control, command);
        }
    }
}
