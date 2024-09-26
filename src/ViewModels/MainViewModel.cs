namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class MainViewModel : ViewModel
    {
        public MappingViewModel Mapping { get; } = new();

        public SettingsViewModel Settings { get; } = new();
    }
}