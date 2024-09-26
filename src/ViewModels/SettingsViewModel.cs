using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.Globalization;

namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class SettingsViewModel : ViewModel
    {
        private CultureInfo _selectedLanguage = new(ApplicationLanguages.PrimaryLanguageOverride);

        public CultureInfo SelectedLanguage
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

        public ObservableCollection<CultureInfo> AvailableLanguages { get; } = new(ApplicationLanguages.ManifestLanguages.Select(l => new CultureInfo(l)));

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
    }
}