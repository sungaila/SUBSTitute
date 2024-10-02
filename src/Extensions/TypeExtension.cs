using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;

namespace Sungaila.SUBSTitute.Extensions
{
    [MarkupExtensionReturnType(ReturnType = typeof(Type))]
    public partial class TypeExtension : MarkupExtension
    {
        public Type? Type { get; set; }

        protected override object? ProvideValue() => Type;

        protected override object? ProvideValue(IXamlServiceProvider serviceProvider) => Type;
    }
}