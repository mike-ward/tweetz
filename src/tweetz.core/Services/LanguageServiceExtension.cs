using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace tweetz.core.Services
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LanguageServiceExtension : MarkupExtension
    {
        private readonly string _key;

        public LanguageServiceExtension(string key)
        {
            _key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
            {
                Source = new LanguageServiceBinder(_key),
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}