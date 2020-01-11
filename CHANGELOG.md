# Changelog
All notable changes to this project will be documented in this file.

## [2.0-Beta] - 2020-01-11
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
- Incorporate mentions into home timeline. Twitter limits the nummber of requests to this API for all Tweetz clients so the update interval is long (once per hour). - [16112b5](https://github.com/mike-ward/tweetz/commit/16112b58b6e89209e57969922039ebc358c3414d)

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

