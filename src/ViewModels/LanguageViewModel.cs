using System.Globalization;
using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class LanguageViewModel(string ietfLanguageTag, string nativeName, string? parentIetfLanguageTag = null, string? parentNativeName = null) : ViewModel
    {
        public string IetfLanguageTag { get; } = ietfLanguageTag;

        public string NativeName { get; } = nativeName;

        public string? ParentIetfLanguageTag { get; } = parentIetfLanguageTag;

        public string? ParentNativeName { get; } = parentNativeName;

        public bool HasSiblingCultures { get; set; }

        public string DisplayName => HasSiblingCultures ? NativeName : (ParentNativeName ?? NativeName);

        public override bool Equals(object? obj) => obj is LanguageViewModel viewModel && viewModel.IetfLanguageTag == IetfLanguageTag;

        public override int GetHashCode() => IetfLanguageTag.GetHashCode();

        public static implicit operator LanguageViewModel(CultureInfo c)
        {
            if (c.Parent != null && c.Parent != CultureInfo.InvariantCulture)
            {
                return new(c.IetfLanguageTag, c.NativeName, c.Parent.IetfLanguageTag, c.Parent.NativeName);
            }

            return new(c.IetfLanguageTag, c.NativeName);
        }
    }
}