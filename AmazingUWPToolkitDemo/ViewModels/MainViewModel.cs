using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazingUWPToolkit.ApplicatonView;
using Caliburn.Micro;

namespace AmazingUWPToolkitDemo.ViewModels
{
    public class MainViewModel : Conductor<IScreen>.Collection.OneActive
    {
        #region Fields

        private readonly IApplicationViewHelper applicationViewHelper;

        #endregion

        #region Contructor

        public MainViewModel(IApplicationViewHelper applicationViewHelper)
        {
            this.applicationViewHelper = applicationViewHelper;
        }

        #endregion

        #region Properties

        public IApplicationViewHelper ApplicationViewHelper => applicationViewHelper;

        #endregion

        #region Overrides of IScreen

        protected async override void OnViewAttached(object view, object context)
        {
            await applicationViewHelper.SetAsync();

            base.OnViewAttached(view, context);
        }

        #endregion
    }
}