﻿using ELittoral.Models;
using ELittoral.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace ELittoral.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class AnalysisResultPage : Page
    {
        public AnalysisResultViewModel ViewModel { get; } = new AnalysisResultViewModel();

        public AnalysisResultPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadData(e.Parameter as AnalysisResultModel);
            
        }

        private void FlipViewImageItemTemplateGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var im = ((Grid)sender).DataContext as ImageModel;
            if (im != null)
            {
                ViewModel.OnImageTapped(im);
            }
        }
    }
}
