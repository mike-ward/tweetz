using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace tweetz.core.Services
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LanguageServiceExtension : MarkupExtension
    {
        private readonly string key;

        public LanguageServiceExtension(string key)
        {
            this.key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
            {
                Source = new LanguageServiceBinder(key),
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}