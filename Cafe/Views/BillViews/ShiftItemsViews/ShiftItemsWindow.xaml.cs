﻿using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.BillViews.ShiftItemsViews
{
    public partial class ShiftItemsWindow : MetroWindow
    {
        public ShiftItemsWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("ShiftItems");
        }
    }
}
