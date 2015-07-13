﻿using System;
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
        /// Called when the currently active profile changes.
        /// </summary>
        /// <param name="profile">The new active profile.</param>
        void ActiveProfileChanged(MadCatzProfile profile);
    }
}
