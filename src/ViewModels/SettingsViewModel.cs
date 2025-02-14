using Microsoft.UI.Xaml;
using Sungaila.SUBSTitute.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.Globalization;

namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class SettingsViewModel : ViewModel
    {
        private LanguageViewModel _selectedLanguage = new CultureInfo(ApplicationLanguages.PrimaryLanguageOverride);

        public LanguageViewModel SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    ApplicationLanguages.PrimaryLanguageOverride = value.IetfLanguageTag;
                }
            }
        }

        public ObservableCollection<LanguageViewModel> AvailableLanguages { get; } = [.. ApplicationLanguages.ManifestLanguages.Select(l => (LanguageViewModel)new CultureInfo(l))];

        private ElementTheme _selectedTheme;

        public ElementTheme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    App.RequestedAppTheme = value;
                }
            }
        }

        public ObservableCollection<ElementTheme> AvailableThemes { get; } =
        [
            ElementTheme.Light,
            ElementTheme.Dark,
            ElementTheme.Default
        ];

        private bool _patternCanvasVisible = MainWindow.GetPatternCanvasVisible();

        public bool PatternCanvasVisible
        {
            get => _patternCanvasVisible;
            set
            {
                if (SetProperty(ref _patternCanvasVisible, value))
                {
                    MainWindow.SetPatternCanvasVisible(value);
                }
            }
        }
    }
}