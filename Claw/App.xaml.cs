using Claw.Interfaces;
using Claw.Logic;
using Claw.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Claw
{
    /// <summary>
    /// Logic for "App.xaml".
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Called on startup from the first instanciated view, which is the main view component.
        /// </summary>
        /// <param name="view">The main view.</param>
        public static void OnStartup(IMainView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            IMainPresenter presenter = new MainPresenter();
            view.SetPresenter(presenter);
            presenter.SetView(view);

            IClawModel model = new ClawModel();
            presenter.SetModel(model);
        }
    }
}
