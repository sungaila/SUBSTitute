using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class MainViewModel : ViewModel
    {
        public MappingViewModel Mapping { get; } = new();

        public SettingsViewModel Settings { get; } = new();
    }
}