using Claw.Blasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// Presenter component of the MVP pattern for the icons display.
    /// </summary>
    public interface IIconsPresenter
    {
        /// <summary>
        /// Sets the view associated with this presenter.
        /// </summary>
        /// <param name="newView">The view to manage.</param>
        void SetView(IIconsView newView);

        /// <summary>
        /// Called when the active profile changed.
        /// </summary>
        /// <param name="profile">The new active profile.</param>
        void ActiveProfileChanged(MadCatzProfile profile);

        /// <summary>
        /// Notifies the presenter that a new icon shall be opened and inserted into the profile.
        /// </summary>
        void AddIconsRequested();

        /// <summary>
        /// Notifies the presenter that the given icons shall be removed.
        /// </summary>
        /// <param name="blasts">The icons to remove.</param>
        void RemoveIconsRequested(LinkedList<Blast> blasts);
    }
}
