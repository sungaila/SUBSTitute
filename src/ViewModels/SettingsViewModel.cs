using Microsoft.UI.Xaml;
using Sungaila.SUBSTitute.Commands;
using Sungaila.SUBSTitute.Enums;
using Sungaila.SUBSTitute.Helper;
using Sungaila.SUBSTitute.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Windows.Globalization;

namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class SettingsViewModel : ViewModel
    {
        public static readonly bool IsManagedByAppInstaller = AppInstallerHelper.IsManagedByAppInstaller;

        public Frequency AutoUpdateFrequency
        {
            get => AppInstallerHelper.AutoUpdateFrequency;
            set
            {
                AppInstallerHelper.AutoUpdateFrequency = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAutoUpdateWeekly));
                OnPropertyChanged(nameof(IsAutoUpdateDaily));
                OnPropertyChanged(nameof(IsAutoUpdateHourly));
                OnPropertyChanged(nameof(IsAutoUpdateOnLaunch));
                OnPropertyChanged(nameof(IsAutoUpdateOff));
            }
        }

        public bool IsAutoUpdateWeekly
        {
            get => AutoUpdateFrequency == Frequency.Weekly;
            set
            {
                if (!value)
                    return;

                AutoUpdateFrequency = Frequency.Weekly;
                OnPropertyChanged();
            }
        }

        public bool IsAutoUpdateDaily
        {
            get => AutoUpdateFrequency == Frequency.Daily;
            set
            {
                if (!value)
                    return;

                AutoUpdateFrequency = Frequency.Daily;
                OnPropertyChanged();
            }
        }

        public bool IsAutoUpdateHourly
        {
            get => AutoUpdateFrequency == Frequency.Hourly;
            set
            {
                if (!value)
                    return;

                AutoUpdateFrequency = Frequency.Hourly;
                OnPropertyChanged();
            }
        }

        public bool IsAutoUpdateOnLaunch
        {
            get => AutoUpdateFrequency == Frequency.OnLaunch;
            set
            {
                if (!value)
                    return;

                AutoUpdateFrequency = Frequency.OnLaunch;
                OnPropertyChanged();
            }
        }

        public bool IsAutoUpdateOff
        {
            get => AutoUpdateFrequency == Frequency.Off;
            set
            {
                if (!value)
                    return;

                AutoUpdateFrequency = Frequency.Off;
                OnPropertyChanged();
            }
        }

        public ICommand CheckForUpdatesCommand { get; } = AutoUpdateCommands.CheckForUpdates;

        public static ObservableCollection<LanguageViewModel> AvailableLanguages
        {
            get
            {
                var result = new ObservableCollection<LanguageViewModel>
                {
                    // this represents the "automatic" option
                    // where the ResourceLoader will resolve resources based on the system language
                    new(string.Empty, App.ResourceLoader.GetString("PrimaryLanguageOverrideDisabled"))
                };

                foreach (var l in ApplicationLanguages.ManifestLanguages
                    .OrderBy(l => l)
                    .Select(l => new CultureInfo(l)))
                {
                    result.Add(l);
                }

                foreach (var l in result)
                {
                    if (string.IsNullOrEmpty(l.ParentIetfLanguageTag))
                        continue;

                    l.HasSiblingCultures = result.Any(c => c != l && c.ParentIetfLanguageTag == l.ParentIetfLanguageTag);
                }

                return result;
            }
        }

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

        public ElementTheme SelectedTheme
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
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