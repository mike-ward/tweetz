namespace tweetz.core.Services
{
    public class LanguageServiceBinder
    {
        private readonly string _key;

        public LanguageServiceBinder(string key) => _key = key;

        public object Value => LanguageService.Instance.Lookup(_key);
    }
}