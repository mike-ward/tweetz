﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class TranslateCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private                ISettings     Settings { get; }

        public TranslateCommand(ISettings settings)
        {
            Settings = settings;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            try
            {
                var tweet    = (TwitterStatus)ea.Parameter;
                var fromLang = tweet.Language ?? "und";
                var toLang   = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                tweet.TranslatedText = App.GetString("translate-text-working");
                tweet.TranslatedText = await TranslateService.Translate(tweet.FullText, fromLang, toLang, Settings.TranslateApiKey).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}