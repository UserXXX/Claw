using Claw.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claw.Logic
{
    /// <summary>
    /// Presenter implementation for the icons display.
    /// </summary>
    public class IconsPresenter : IIconsPresenter
    {
        private IIconsView view;

        public void SetView(IIconsView newView)
        {
            view = newView;
        }

        public void ActiveProfileChanged(MadCatzProfile profile)
        {
            view.ActiveProfileChanged(profile);
        }
    }
}
