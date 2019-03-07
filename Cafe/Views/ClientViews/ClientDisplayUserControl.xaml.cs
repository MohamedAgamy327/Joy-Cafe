﻿using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.ClientViews
{

    public partial class ClientDisplayUserControl : UserControl
    {
        public ClientDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientDisplay");
        }
    }
}
