using tweetz.core.Interfaces;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private User? userProfile;

        public User? UserProfile
        {
            get => userProfile;
            set => SetProperty(ref userProfile, value);
        }
    }
}