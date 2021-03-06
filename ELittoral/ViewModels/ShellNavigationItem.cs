using System;

using ELittoral.Helpers;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ELittoral.ViewModels
{
    public class ShellNavigationItem : Observable
    {
        private bool _isSelected;

        private Visibility _selectedVis = Visibility.Collapsed;
        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set { Set(ref _selectedVis, value); }
        }

        private SolidColorBrush _selectedForeground = null;
        public SolidColorBrush SelectedForeground
        {
            get
            {
                return _selectedForeground ?? (_selectedForeground = GetStandardTextColorBrush());
            }
            set { Set(ref _selectedForeground, value); }
        }

        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar { get { return (char)Symbol; } }
        public Type PageType { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(ref _isSelected, value);
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                SelectedForeground = value
                    ? Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush
                    : GetStandardTextColorBrush();
            }
        }

        private SolidColorBrush GetStandardTextColorBrush()
        {
            var result = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;

            if (!Services.ThemeSelectorService.IsLightThemeEnabled)
            {
                result = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as SolidColorBrush;
            }

            return result;
        }

        private ShellNavigationItem(string name, Symbol symbol, Type pageType)
        {
            this.Label = name;
            this.Symbol = symbol;
            this.PageType = pageType;

            Services.ThemeSelectorService.OnThemeChanged += (s, e) => { if (!IsSelected) SelectedForeground = GetStandardTextColorBrush(); };
        }

        public static ShellNavigationItem FromType<T>(string name, Symbol symbol) where T : Page
        {
            return new ShellNavigationItem(name, symbol, typeof(T));
        }
    }
}
