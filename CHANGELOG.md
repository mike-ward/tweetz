# Changelog
All notable changes to this project will be documented in this file.

## 2022.1 - 2022-01-01

### Added
- Add options to hide retweets and tweets with no media #100

### Changed
- Update runtime to .NET 6.0

## 2021.4 - 2021-09-21

### Added
- GDI font metric mode option

### Fixed
- hide minimized window when system tray setting is true #41
- Fix images not showing at times

### Changed
- Change http timeout from 100 seconds (default) to 20 seconds
- Cache flow nodes in twitter status
- Change how link hit-testing works

## 2021.3 - 2021-09-15

### Added
- Alternate tweet layout<br>
  ![Imgur](https://i.imgur.com/QjuJJMr.png)
- Option to hide specific images (shift-click image)
- Images as links option
- Links as \[link\] option
- Translate API key (ping me on how use)
- Private setting to change font (no UI, in settings file only)

### Fixed
- Better image layout
- Handle images with embedded DPI correctly
- Observable Collection fires fewer events

### Changed
- New user profile view
- New public translation API
- Shrink profile images by 5 dips
- Cache images using browser cache settings
- Dotnet 6, RC1
- Jab Dependency injection library

## 2021.2 - 2021-04-10

### Added
- new user profile view (click on profile image or screen name)

### Fixed
- Restore Translate feature
- Restore Image copy from viewer

### Changed
- Using HttpClient vs. WebRequest

## 2021.1 - 2021-02-21

### Added
- Mouse wheel controls volume when playing videos

### Changed
- Version numbers are year and revision based
- Image viewer uses media element for videos only. Other images use image element
- Update packages and SDK to latest releases
- Removed cache hinting option from profile images

## 2.9.5 - 2021-01-31

### Changed
- Change link colors
- Extend timeout to 5 seconds for reverse lookup of short urls

## 2.9.4 - 2021-01-12

### Changed
- UI tweaks

## 2.9.3 - 2021-01-05

### Fixed
- [Menu takes up 2 rows since 2.9.1 #64](https://github.com/mike-ward/tweetz/issues/64) 

## 2.9.2 - 2021-01-05

### Added
- "Hide translate link" option

## 2.9.1 - 2021-01-04

### Fixed
- Use new service provider for translations #61

## 2.9.0 - 2020-12-05

### Added
- Translate tweets on demand

### Changed
- Improved image layout for multiple images

## 2.8.6 - 2020-11-11

### Changed
- Update to .Net 5.0

## 2.8.5 - 2020-09-27

### Changed
- Smooth scrolling [688ab45](https://github.com/mike-ward/tweetz/commit/688ab45e49f5aeb5d7d14fa02e4e1c738c5e2020)

## 2.8.4 - 2020-09-13

### Fixed
- Move ColorConverter reference to theme files to avoid crash [a245e42](https://github.com/mike-ward/tweetz/commit/a245e424da8f2bc0aa1fc3ac636a2a8e9b544142)

## 2.8.3 - 2020-09-13

### Fixed
- Follow/Followed By statuses didn't sync sometimes [759c07f](https://github.com/mike-ward/tweetz/commit/759c07f845e379f348ab6479b522d50c8960cf09)
- Custom color not working in Nord and Dark themes [55449d0](https://github.com/mike-ward/tweetz/commit/55449d08acf1980ea6bf021efac2db4aa4661cc5) 

## 2.8.2 - 2020-09-09

### Fixed
- Crash when image meta data color palette is corrupt [89c845](https://github.com/mike-ward/tweetz/commit/89c84536f2fe40b4bac5ba428eb05d8c23db6dad)

## 2.8.1 - 2020-09-06

### Added
- Add retry logic to image downloads [#38](https://github.com/mike-ward/tweetz/issues/38)

### Changed
- Adapt to changes in `Follwing`, `FollowedBy` are reported in twitter api [380bd7d](https://github.com/mike-ward/tweetz/commit/380bd7df17968b07512c584e8a69ca89bde4831f)
- Use tuples to reduce heap allocations [40c4aad](https://github.com/mike-ward/tweetz/commit/40c4aadde93a4b2fe7a41d10a07c41a531f47465)

### Fixed
- Check for null on opening links [380bd7d](https://github.com/mike-ward/tweetz/commit/380bd7df17968b07512c584e8a69ca89bde4831f)

## 2.8.0 - 2020-08-03

### Added
- Add setting to change color of logged in user tweets [#31](https://github.com/mike-ward/tweetz/issues/31)
- Add grayscale option #32
- \@username autocompletion, work in progress, suggestions welcomed [#21](https://github.com/mike-ward/tweetz/issues/21)

### Changed
- Change layout when tweet has 3 images
- Improve trace messages for diagnosis and debugging

### Fixed
- Fix line height issue [#35](https://github.com/mike-ward/tweetz/issues/35)

## 2.7.1 - 2020-06-30

### Fixed
- Fix issue with cross threading on UI thread

## 2.7.0 - 2020-06-28

### Changed
- Replace fade-in with scroll-in animation when tweet arrives [abcb2aa](abcb2aa152a5bff118658a5de68950244ea9200d) 
- Reduce size of verified badge [36476a7](36476a7522a20b0200a556592e7af88c8652064a)

### Added
- Automatically freeze scrolling of new tweets when scrolled and indicate new tweets arrived with green dot in tab [a2e1853](a2e1853a248eb176335adca00db585e31f84bb3e)
- Click on active tab to scroll to top (right-click still works as well) [a2e1853](a2e1853a248eb176335adca00db585e31f84bb3e)
- Check Desktop Window Manager enabled, notify and exit instead of crash [837e3b4](837e3b4ce2e6f648ac45ef171c612bd72e9fa228) 

### Removed
- Remove freeze timeline when scrolled (no longer needed with indicator feature) [8ee9f9a](8ee9f9abf064487963bc0a9b2252d0a5d63c5d4c)

## 2.6.2 - 2020-06-20

### Fixed
- Hyperlinks in related links required focus [84a0cc1](https://github.com/mike-ward/tweetz/commit/84a0cc1924d8c51b3c6be384149125ca69e63a76)_

### Changed

- Reduce text line height slightly [fcd9e0d](https://github.com/mike-ward/tweetz/commit/fcd9e0d372aa7f42c7ca06c7ecbda313c95355d0)
## 2.6.1 - 2020-06-08

### Fixed
- Fix hyperlinks not working after scrolling [#17](https://github.com/mike-ward/tweetz/issues/17)

## 2.6.0 - 2020-06-05

### Added
- Optional block tweets marked as sensitive
- Change borders and layout of quoted and related content

## 2.5.0 - 2020-05-25

### Added
- Add screen name to title bar with option to turn off [a2c78a1](https://github.com/mike-ward/tweetz/commit/a2c78a19fa890d5a3e4bc6bbe9cb78c44b2122c3)
- Add rewind 10 seconds feature in media player [84f8a54](https://github.com/mike-ward/tweetz/commit/84f8a54fdaec058546764e6aa7605427897c56d8)

### Fixed
- Fix media upload error [4870dfb](https://github.com/mike-ward/tweetz/commit/4870dfb511c2402a30b9a13329b93595d9170071)

### Changed
- Spruce up settings dialog [ec87367](https://github.com/mike-ward/tweetz/commit/ec87367b736315c6364e54012d0ea805bdd9451b)

## 2.4.0 - 2020-05-02

### Added
- Profile switch: on the command line or in a shortcut add a profile name (e.g. `tweetz.core.exe aname`). This instructs Tweetz to read and save settings to a file called `aname.settings.json` allowing Tweetz to access multiple accounts in multiple instances. [#11](https://github.com/mike-ward/tweetz/issues/11)

## 2.3.0 - 2020-04-26

### Fixed
- Handle file in use exception when saving settings [c945ef5](https://github.com/mike-ward/tweetz/commit/c945ef5a090b8b5fc59dfdce744d2510bb6ec9ae)
- Fix copy image to clipboard by closing drawing context [897a3b9](https://github.com/mike-ward/tweetz/commit/897a3b96d8cf5c5ebcdbeb990653f486e388b7f6)
- [#13](https://github.com/mike-ward/tweetz/issues/13) Media player always appears on primary monitor [bdce3f2](https://github.com/mike-ward/tweetz/commit/bdce3f28074b6aadbcb3ab294268b7eddc0b8dbf)

### Added
- [#12](https://github.com/mike-ward/tweetz/issues/12) Always on top [2681aee](https://github.com/mike-ward/tweetz/commit/2681aeecec6131261ea27cf31ebc62968934f2e9)
- [#12](https://github.com/mike-ward/tweetz/issues/12) Add minimize to title bar [496764f](https://github.com/mike-ward/tweetz/commit/496764fb8c39720bcec7681aee30d08422f96dc4)

### Changed
- Use static resource for user profile tooltip [22d4f6e](https://github.com/mike-ward/tweetz/commit/22d4f6e99a42a4176cde6e218b0851d27c42dd27)

## 2.2.3 - 2020-04-16

### Fixed
- Handle DragMove() being weird sometimes [afa8b49](https://github.com/mike-ward/tweetz/commit/afa8b497cafeaa8a532c7657b7ee54bd3d388532)
- CPU usage constantly 2-5% after posting an update [2199964](https://github.com/mike-ward/tweetz/commit/21999640960c5075d134107eb1e7fccd474da299)

### Changed
- Increase min width of profile control [7e84031](https://github.com/mike-ward/tweetz/commit/7e84031c20dbd11b3e9a116baf1f22aede41d6eb)
- Suppress crash reports from debug sessions [1a46798](https://github.com/mike-ward/tweetz/commit/1a467982f6d41aa1ca8e04c606a5db09f4bdec86)

## 2.2.1 - 2020-04-05
### Fixed

- Use fully qualified path name to get app icon [05f0627](https://github.com/mike-ward/tweetz/commit/05f0627d523db374311314ea2199b28941dc03ff)

## 2.2.0 - 2020-04-04
### Changed
- Media player enhancements: Progress bar, play/pause/skip-to-start buttons and separate copy to clipboard buttons for URL and Image
- Show tweet image instead of related when available [d99c1e9](https://github.com/mike-ward/tweetz/commit/d99c1e9aa4893e7e97f3d209417aad8a66d576c1)
- Remove border on extended content [3962cc7](https://github.com/mike-ward/tweetz/commit/3962cc7883dd323c3a4de8ed66dab9c511fcbcfe)
- Stretch smaller images to fill [3e60e19](https://github.com/mike-ward/tweetz/commit/3e60e1996023357e3a2223ab9c244e25a6291751)
- Add divider between tweets when profile image hidden [#10](https://github.com/mike-ward/tweetz/issues/10) [9a238e5](https://github.com/mike-ward/tweetz/commit/9a238e5b653a8b31fc3f173f3de41c0edbc393be)

### Fixed
- Use originating status to check if tweet has media [d7d7d5d](https://github.com/mike-ward/tweetz/commit/d7d7d5d3b1efe2b63747ec62de3426b7cec1294b)

### Added
- Show warning icon when image load fails [e6cec98](https://github.com/mike-ward/tweetz/commit/e6cec98b56ed74493d7d49409db2c363ce4b5bab)
- Add Nord theme [58a51b2](https://github.com/mike-ward/tweetz/commit/58a51b2f8baddca3d36a6d295119a7e1bc4b296b)
- Add AppCenter Support [3ef8690](https://github.com/mike-ward/tweetz/commit/3ef869035469c91b79782069de48a10c185db19a)
- Add option for display in System Tray [#10](https://github.com/mike-ward/tweetz/issues/10) [b173df1](https://github.com/mike-ward/tweetz/commit/b173df1ed108a7814c6fb2434855e9cfe071afaa)
- Add play icon to video media [9959016](https://github.com/mike-ward/tweetz/commit/99590162d3bf98329b3cff66ed97d38ee5194299)

## 2.1.1 - 2020-03-09

### Changed
- Installer always on top [8402782](<https://github.com/mike-ward/tweetz/commit/8402782c68c25de936f56351ee6550a113a9cd31>)

### Fixed
- Windows sizing using borders restored - [#8](<https://github.com/mike-ward/tweetz/issues/8>)

## 2.1.0 - 2020-03-07

### Added
- Short to long URL lookup for tooltips [1d4d723](<https://github.com/mike-ward/tweetz/commit/1d4d7235db9d92419ed2190a8d91a58d45b2f6ac>)
- Splash screen [52a2fc7](<https://github.com/mike-ward/tweetz/commit/52a2fc738f999b088d12ac670531b2f7eb7f672e>)
- Fixed height images with rounded corners [8f55db3](<https://github.com/mike-ward/tweetz/commit/8f55db3cf68fbc39bce49c487e21c65dc03634da>)
- Hourglass symbol to indicate image loading [cc025b1](<https://github.com/mike-ward/tweetz/commit/cc025b1fdba56a2fc3a27ac582ab7ffda9fa1a1a>)
- Add app.manifest with DPI permonitor awareness. [a139a53](<https://github.com/mike-ward/tweetz/commit/a139a53b38a1e909c6a9e69b1c22f2a39c59b5c5>)
- Add usage tips to settings screen [/6b6b14a](<https://github.com/mike-ward/tweetz/commit/6b6b14a4870b47c3b814e303f5c3b8bd6b79cc66>)
- Add font-size key bindings and commands [23ea4e3](<https://github.com/mike-ward/tweetz/commit/23ea4e3a0300460893016d3c96fb45e344d395c7>)
- Brief fade-in animation of new tweets [a9085cc](<https://github.com/mike-ward/tweetz/commit/a9085ccfd5e50d64e70649679c582d38ceb77069>)

### Changed
- Change font selector to slider. [0190b64](<https://github.com/mike-ward/tweetz/commit/0190b64504916dbf721e032a377b2aff6791ff2e>)
- Title close button more like standard windows style [465d366](<https://github.com/mike-ward/tweetz/commit/465d366386d7185abbcf70bf55a11ff1b0f0b83c>)
- Limit length of related link text to 300 characters [e9373ed](<https://github.com/mike-ward/tweetz/commit/e9373edbc28d870f092da7dd8b001532fca77a44>)
- Move user control resources to application resources and other performance improvements.

## 2.0.2 - 2020-02-01

### Changed
- Reduce memory load of related links by only scanning `<head>` section. [bffb90](https://github.com/mike-ward/tweetz/commit/bffb90988f0cff09157efd6becb2fc0b48360b2f)
- Refactorings suggested by SonarLink and FxCop analyzers

## 2.0.1 - 2020-01-26

### Changed
- Show font size in settings dialog. [67d6cd3](https://github.com/mike-ward/tweetz/commit/67d6cd326dfa4e5b626078ce9a3871a6b4d089a3)
- Load only HEAD section of related documents with HtmlAgilityPack. Reduces memory and GC pressure. [bb628ad](https://github.com/mike-ward/tweetz/commit/bb628adb8a79d41c2686f8331dc5fac51bf017c7)

## 2.0.0 - 2020-01-18

### Changed
- Show reply/retweet/like for user's tweets but disable commands. [f3c0e35](https://github.com/mike-ward/tweetz/commit/f3c0e350fa444519dbc171f6de52cd6b0935ee40)

## 2.0 Beta 3 - 2020-01-12

### Fixed
- Restore mouse down handler to enable moving window.
- Trim version info returned from server before comparing.

## 2.0 Beta 2 - 2020-01-11

### Added
- Option to hide images - [138d59b](<https://github.com/mike-ward/tweetz/commit/138d59b17340df3c38b82bfd6dfeceb6fec56ec6>)
- Option to hide extended content - [8fc1ac0](<https://github.com/mike-ward/tweetz/commit/8fc1ac021106ee1bc5d5081e5ae6376d6da03f2b>)

### Changed
- Custom title bar reduces hieght. Stays same color when inactive - [22a524292](https://github.com/mike-ward/tweetz/commit/22a5242921072537b86c931a2ded416658ae81ff)
- Sizing can only be done with gripper tool in lower corner (concession to adding custom title bar) - [22a524292](https://github.com/mike-ward/tweetz/commit/22a5242921072537b86c931a2ded416658ae81ff)

### Fixed
- Reduce UI Jank [e835693fc3](<https://github.com/mike-ward/tweetz/commit/e835693fc349c6652aaf2a1d74f555ad47fb1ba5>)
- Not clearing all buffers on signout

### Removed
- Removed option in installer to pick a directory - [0184051](<https://github.com/mike-ward/tweetz/commit/0184051f4f96f757230bc407603d01f54aee76d2>)

## [2.0-Beta] - 2020-01-04
### Added
- Incorporate mentions into home timeline. Twitter limits the number of requests to this API for all Tweetz clients so the update interval is long (once per hour). - [16112b5](https://github.com/mike-ward/tweetz/commit/16112b58b6e89209e57969922039ebc358c3414d)

### Changed
- Change **Like** color to Firebrick - [22085c3](https://github.com/mike-ward/tweetz/commit/c283e2e439ac529a96e53644c3b9d9623d074c8f)
- Add `extended mode` flag to UpdateStatus call - [22085c3](https://github.com/mike-ward/tweetz/commit/c283e2e439ac529a96e53644c3b9d9623d074c8f) 
- Add `extended mode` flag to Search call
- Add button to copy to clipboard in image viewer - [cab9fe5](https://github.com/mike-ward/tweetz/commit/cab9fe5a75797be968a1c89459a038338805ad6f)

### Removed
- Remove automatic copy to clipboard in image viewer. - [cab9fe5](https://github.com/mike-ward/tweetz/commit/cab9fe5a75797be968a1c89459a038338805ad6f)

### Fixed
- Remove option to install for All Users. Rename installer to `tweetz.setup.exe` - [#1](https://github.com/mike-ward/tweetz/issues/1) 
- Clear search timeline before getting mentions - [888790b](https://github.com/mike-ward/tweetz/commit/888790b0169b22f975bd5118ae352ba72b8aacea)
- Fix `@screen-name` appearing twice in reply. - [21d7ec3](https://github.com/mike-ward/tweetz/commit/21d7ec3f832959f57443caf08e155b1407665a20)
- Clear search textbox when getting mentions - [f21e260](https://github.com/mike-ward/tweetz/commit/f21e260216f6b5996b39eef7edfe2d25f659702b)

### Security

## [2.0-Alpha] - 2019-12-28
- First public release. Complete rewrite in .NET Core 3.1
