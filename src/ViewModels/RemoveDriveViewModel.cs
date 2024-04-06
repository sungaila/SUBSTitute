namespace Sungaila.SUBSTitute.ViewModels
{
    public class RemoveDriveViewModel : ViewModel
    {
        public required MappingViewModel ParentViewModel { get; init; }

        private bool _isPermanent;

        public bool IsPermanent
        {
            get => _isPermanent;
            set => SetProperty(ref _isPermanent, value);
        }
    }
}