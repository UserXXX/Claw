using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Claw.Interfaces
{
    /// <summary>
    /// View component of the MVP pattern for the icons display.
    /// </summary>
    public interface IIconsView
    {
        /// <summary>
        /// Sets the presenter for this view.
        /// </summary>
        /// <param name="newPresenter">The presenter.</param>
        void SetPresenter(IIconsPresenter newPresenter);

        /// <summary>
        /// Called when the active profile changed.
        /// </summary>
        /// <param name="activeProfile">The new active profile. If null is passed, the UI is cleared.</param>
        void ActiveProfileChanged(MadCatzProfile activeProfile);

        /// <summary>
        /// Shows an open file dialog to select image files.
        /// </summary>
        /// <returns>The file infos pointing to the selected image files or null if the user cancelled the dialog.</returns>
        FileInfo[] SelectImageFiles();
    }
}
