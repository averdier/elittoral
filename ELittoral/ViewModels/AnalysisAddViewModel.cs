﻿using ELittoral.Helpers;
using ELittoral.Models;
using ELittoral.Services;
using ELittoral.Services.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ELittoral.ViewModels
{
    public class AnalysisAddViewModel : Observable
    {
        private FlightplanModel _selectedFlightplan;
        public FlightplanModel SelectedFlightplan
        {
            get { return _selectedFlightplan; }
            set { Set(ref _selectedFlightplan, value); }
        }

        private ReconModel _selectedReconA;
        public ReconModel SelectedReconA
        {
            get { return _selectedReconA;  }
            set { Set(ref _selectedReconA, value); }
        }

        private ReconModel _selectedReconB;
        public ReconModel SelectedReconB
        {
            get { return _selectedReconB; }
            set { Set(ref _selectedReconB, value); }
        }

        public ObservableCollection<FlightplanModel> FlightPlanItems { get; private set; } = new ObservableCollection<FlightplanModel>();


        public ICommand CancelClickCommand { get; private set; }

        public ICommand LaunchClickCommand { get; private set; }

        public ICommand FlightplanSelectionChangedCommand { get; private set; }

        public ICommand ReconASelectionChangedCommand { get; private set; }

        public ICommand ReconBSelectionChangedCommand { get; private set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { Set(ref _loadingMessage, value); }
        }

        public bool LaunchBtn_IsEnabled { get { return _selectedFlightplan != null && _selectedReconA != null && _selectedReconB != null; } }

        private RESTFlightplanModelService _flightplanModelService;
        private RESTReconModelService _reconModelService;
        private RESTAnalysisModelService _analysisModelService;

        public AnalysisAddViewModel()
        {
            LaunchClickCommand = new RelayCommand<RoutedEventArgs>(OnLaunchClick);
            CancelClickCommand = new RelayCommand<RoutedEventArgs>(OnCancelClick);
            FlightplanSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnFlightplanSelectionChanged);
            ReconASelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnReconASelectionChanged);
            ReconBSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnReconBSelectionChanged);
            _flightplanModelService = new RESTFlightplanModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _reconModelService = new RESTReconModelService("http://vps361908.ovh.net/dev/elittoral/api/");
            _analysisModelService = new RESTAnalysisModelService("http://vps361908.ovh.net/dev/elittoral/api/");
        }

        public async Task LoadDataAsync()
        {
            FlightPlanItems.Clear();

            try
            {
                IsLoading = true;
                LoadingMessage = "Chargement des plans de vol";

                var data = await _flightplanModelService.GetFlightplansAsync();
                if (data != null)
                {
                    LoadingMessage = "Chargement des reconnaissances";
                    foreach (FlightplanModel fp in data)
                    {
                        var recons = await _reconModelService.GetReconsFromFlightplanIdAsync(fp.Id);
                        if (recons != null)
                        {
                            fp.Recons = recons.ToList<ReconModel>();
                        }
                        FlightPlanItems.Add(fp);
                    }
                }

                IsLoading = false;
                LoadingMessage = "";
            }
            catch
            {

            }
        }

        private async void OnLaunchClick(RoutedEventArgs args)
        {
            if (SelectedReconA != null && SelectedReconB != null)
            {
                try
                {
                    IsLoading = true;
                    LoadingMessage = "Lancement de l'analyse";

                    var analysis = await _analysisModelService.LaunchAnalysis(SelectedReconA, SelectedReconB);

                    if (analysis != null)
                    {
                        NavigationService.Navigate<Views.AnalyzesPage>(analysis);
                    }
                    else
                    {
                        IsLoading = false;
                        LoadingMessage = "";

                        var unknowErrordialog = new Windows.UI.Popups.MessageDialog(
                            "Une erreur est survenue",
                            "Erreur");
                        unknowErrordialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                        unknowErrordialog.DefaultCommandIndex = 0;

                        var resultUnknow = await unknowErrordialog.ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    IsLoading = false;
                    LoadingMessage = "";

                    var dialog = new Windows.UI.Popups.MessageDialog(
                    ex.Message,
                    "Erreur"
                    );
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                    dialog.DefaultCommandIndex = 0;

                    var result = await dialog.ShowAsync();
                }
            }
          
        }

        private void OnReconASelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedRecon = args.AddedItems[0] as ReconModel;
            SelectedReconA = selectedRecon;
            OnPropertyChanged(nameof(LaunchBtn_IsEnabled));
        }

        private void OnReconBSelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedRecon = args.AddedItems[0] as ReconModel;
            SelectedReconB = selectedRecon;
            OnPropertyChanged(nameof(LaunchBtn_IsEnabled));
        }

        private void OnFlightplanSelectionChanged(SelectionChangedEventArgs args)
        {
            var selectedFlightplan = args.AddedItems[0] as FlightplanModel;
            SelectedFlightplan = selectedFlightplan;
            OnPropertyChanged(nameof(LaunchBtn_IsEnabled));
        }

        /// <summary>
        /// On click on cancel button
        /// </summary>
        /// <param name="args"></param>
        private void OnCancelClick(RoutedEventArgs args)
        {
            if (NavigationService.CanGoBack) { NavigationService.GoBack();  }
        }
    }
}
