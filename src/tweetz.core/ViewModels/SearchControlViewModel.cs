﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class SearchControlViewModel : TwitterTimeline
    {
        private       bool showProgress;
        private const int  InfiniteElapsed = int.MaxValue / 60000;

        private ITwitterService TwitterService { get; }
        public  Action<string>? SetSearchText  { get; set; }

        public SearchControlViewModel(ISettings settings, ISystemState systemState, ITwitterService twitterService)
            : base(settings, systemState, InfiniteElapsed)
        {
            TwitterService = twitterService;
            timelineName   = App.GetString("search-timeline");
        }

        public bool ShowProgress
        {
            get => showProgress;
            set => SetProperty(ref showProgress, value);
        }

        public async ValueTask SearchAsync(string query)
        {
            try
            {
                TraceService.Message($"Search => {query}");
                SetSearchText?.Invoke(query);

                if (string.IsNullOrEmpty(query)) return;
                StatusCollection.Clear();

                ShowProgress = true;
                var tweets = await TwitterService.TwitterApi.Search(query).ConfigureAwait(true);
                ShowProgress = false;
                UpdateTimeline(tweets.Statuses);
                ExceptionMessage = null;
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
                ExceptionMessage = ex.Message;
            }
            finally
            {
                ShowProgress = false;
            }
        }

        public async ValueTask MentionsAsync()
        {
            try
            {
                StatusCollection.Clear();
                SetSearchText?.Invoke(string.Empty);

                ShowProgress = true;
                var statuses = await TwitterService.TwitterApi.MentionsTimeline(150).ConfigureAwait(true);
                ShowProgress = false;

                UpdateTimeline(statuses);
                ExceptionMessage = null;
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
                ExceptionMessage = ex.Message;
            }
            finally
            {
                ShowProgress = false;
            }
        }

        private void UpdateTimeline(IEnumerable<TwitterStatus>? statuses)
        {
            if (statuses is null) return;

            foreach (var status in statuses)
            {
                status.UpdateAboutMeProperties(Settings.ScreenName);
                StatusCollection.Add(status);
            }

            ExceptionMessage = null;
        }
    }
}