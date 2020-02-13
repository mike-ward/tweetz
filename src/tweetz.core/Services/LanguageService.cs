using System.Collections.Generic;

namespace tweetz.core.Services
{
    public class LanguageService
    {
        public static readonly LanguageService Instance = new LanguageService();

        private readonly Dictionary<string, string> _dictionary;

        public LanguageService()
        {
            _dictionary = English();
        }

        public string Lookup(string key)
        {
            return _dictionary.TryGetValue(key, out var text) ? text : "*** not found ***";
        }

        private Dictionary<string, string> English() => new Dictionary<string, string>
        {
            // Get pin
            { "pin-instructions-1",            "To get started, click the \"Get PIN\" button. This opens a Web page where you'll authorize access." },
            { "pin-instructions-2",            "Copy the PIN from the Web page to here and click \"Sign In\"" },
            { "pin-get-pin",                   "Get PIN" },
            { "pin-sign-in",                   "Sign In" },
            { "pin-about",                     "PINs can only be used once so there's no need to save them."},

            // Timeline
            { "retweeted",                     "Retweeted"},
            { "follow",                        "Follow" },
            { "retweet-with-comment",          "Retweet with comment" },
            { "paused-due-to-scroll-pos",      "Paused due to scroll position" },

            // Settings
            { "settings-title",                "Settings" },
            { "hide-profile-images",           "Hide profile images"},
            { "hide-images",                   "Hide images"},
            { "hide-extended-content",        "Hide extended content"},
            { "run-on-startup",                "Run on startup"},
            { "font-size-title",               "Font size"},
            { "theme-title",                   "Theme" },
            { "theme-light",                   "Light" },
            { "theme-dark",                    "Dark" },
            { "donate",                        "Donate" },
            { "sign-out",                      "Sign Out" },
            { "open-settings-file",            "Open Settings..." },
            { "pause-when-scrolled",           "Pause timeline updates when scrolled" },
            { "donated",                       "Yes, I donated!" },
            { "get-the-update",                "Get the Update" },

            // Profile
            { "profile-joined",                "Joined" },
            { "profile-following",             "Following" },
            { "profile-followers",             "Followers" },
            { "profile-not-following",         "Not Following" },
            { "profile-follows-you",           "Follows You" },

            // Compose
            { "whats-happening",               "What's Happening?" },
            { "add-a-comment",                 "Add a comment" },
            { "spell-checker",                 "Spell checker" },
            { "in-reply-to",                   "In reply to {0}" }, // {0} = screen name of user

            // Tooltips
            { "reply-tooltip",                 "Reply" },
            { "retweet-tooltip",               "Retweet / Unretweet" },
            { "like-tooltip",                  "Like / Unlike" },
            { "retweet-with-comment-tooltip",  "Retweet with comment" },
            { "follow-tooltip",                "Follow / Unfollow" },
            { "remove-tooltip",                "Remove" },
            { "add-image-tooltip",             "Add 1 gif (*.gif) or 1 video (*.mp4) or up to 4 images (*.jpg, *.png, *.webp)" },
            { "mentions-tooltip",              "Get mentions" },
            { "new-version-available",         "Update available!\nClick here to get it." },
            { "copy-to-clipboard",             "Copy to clipboard" },
            { "close-tooltip",                 "Close" },

            // Image viewer
            { "copy",                          "copy" },

            // Tips (in settings dialog)
            { "tips",                          "Tips" },
            { "tips-ctrl-n",                   "New tweet (Ctrl+N)" },
            { "tips-scroll",                   "Right-click scrolls to top" },
            { "tips-open",                     "Click timestamp to open in twitter" },
            { "tips-font-size",                "Font size (Alt+Plus/Minus)" },
        };
    }
}