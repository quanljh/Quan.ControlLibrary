﻿using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using Prism.Regions;

namespace Quan.ControlLibrary.Demo
{
    public class QuanButtonViewModel : BindableBase, INavigationAware
    {
        #region Public Properties



        #endregion

        #region Constructor

        public QuanButtonViewModel()
        {

        }

        #endregion

        /// <inheritdoc />
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
