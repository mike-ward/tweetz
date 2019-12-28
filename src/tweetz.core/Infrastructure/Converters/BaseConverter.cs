using System;
using System.Windows.Markup;

namespace tweetz.core.Infrastructure.Converters
{
    public class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}