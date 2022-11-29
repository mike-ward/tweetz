using System;
using System.Media;
using System.Windows;

namespace tweetz.core.Services
{
    public static class SoundService
    {
        public static void PlayNotifySound()
        {
            using var notifySound = Application.GetResourceStream(new Uri("pack://application:,,,/notify.wav"))!.Stream;
            using var player      = new SoundPlayer(notifySound);
            player.PlaySync();
            TraceService.Message("Played default sound");
        }

        public static void PlayFromSoundSource(string filename)
        {
            using var player2 = new SoundPlayer(filename);
            player2.PlaySync();
            TraceService.Message("Played from sound source");
        }
    }
}