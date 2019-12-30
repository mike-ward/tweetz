using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class SearchControlViewModel : TwitterStatusControlViewModel
    {
        private bool showProgress;

        public ITwitterService TwitterService { get; }
        public Action<string>? SetSearchText { get; set; }

        public SearchControlViewModel(ISettings settings, ITwitterService twitterService)
            : base(settings)
        {
            TwitterService = twitterService;
        }

        public bool ShowProgress { get => showProgress; set => SetProperty(ref showProgress, value); }

        public async Task Search(string query)
        {
            try
            {
                SetSearchText?.Invoke(query);

                if (string.IsNullOrEmpty(query)) return;
                StatusCollection.Clear();

                ShowProgress = true;
                var tweets = await TwitterService.Search(query);
                ShowProgress = false;
                UpdateTimeline(tweets.Statuses);
                ExceptionMessage = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
            finally
            {
                ShowProgress = false;
            }
        }

        public async Task Mentions()
        {
            try
            {
                ShowProgress = true;
                var statuses = await TwitterService.GetMentionsTimeline();
                ShowProgress = false;
                UpdateTimeline(statuses);
                ExceptionMessage = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
            finally
            {
                ShowProgress = false;
            }
        }

        private void UpdateTimeline(IEnumerable<TwitterStatus>? statuses)
        {
            if (statuses == null) return;

            foreach (var status in statuses)
            {
                status.IsMyTweet = Settings.ScreenName == status.OriginatingStatus.User.ScreenName;
                StatusCollection.Add(status);
            }

            ExceptionMessage = null;
        }
    }
}