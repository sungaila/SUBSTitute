namespace Sungaila.SUBSTitute.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MappingViewModel Mapping { get; } = new();

        public SettingsViewModel Settings { get; } = new();
    }
}