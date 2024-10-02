using System.Globalization;
using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class LanguageViewModel(string ietfLanguageTag, string nativeName) : ViewModel
    {
        public string IetfLanguageTag { get; } = ietfLanguageTag;

        public string NativeName { get; } = nativeName;

        public override bool Equals(object? obj) => obj is LanguageViewModel viewModel && viewModel.IetfLanguageTag == IetfLanguageTag;

        public override int GetHashCode() => IetfLanguageTag.GetHashCode();

        public static implicit operator LanguageViewModel(CultureInfo c) => new(c.IetfLanguageTag, c.NativeName);
    }
}