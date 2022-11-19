﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "MA0051:Method is too long", Scope = "member", Target = "~M:tweetz.core.BootStrapper.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.BooleanToParameterConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.CountConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.ViewModelLocatorConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.DoubleFormatConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.FileNameConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.IsNotNullConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.IsNullConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.MediaIsVideoToVisibilityConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.NotBooleanConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.NotBooleanToVisibilityConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.NotNullToVisibilityConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.NullToVisibilityConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.TextTrimConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.ProfileLinkConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0025:TODO Implement the functionality", Scope = "member", Target = "~M:tweetz.core.Converters.TimeAgoConverter.ConvertBack(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: SuppressMessage("Design", "MA0049:Type name should not match namespace", Scope = "type", Target = "~T:tweetz.core.Interfaces.DesktopWindowManager.DesktopWindowManager")]
[assembly: SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage", Scope = "member", Target = "~M:tweetz.core.Services.MessageBoxService.ShowMessageBoxAsync(System.String)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage("Design", "MA0049:Type name should not match namespace", Scope = "type", Target = "~T:tweetz.core.Views.ComposeBlock.ComposeBlock")]
[assembly: SuppressMessage("Design", "MA0049:Type name should not match namespace", Scope = "type", Target = "~T:tweetz.core.Views.MediaViewerBlock.MediaViewerBlock")]
[assembly: SuppressMessage("Design", "MA0049:Type name should not match namespace", Scope = "type", Target = "~T:tweetz.core.Views.SettingsBlock.SettingsBlock")]
[assembly: SuppressMessage("Performance", "MA0028:Optimize StringBuilder usage", Scope = "member", Target = "~M:tweetz.core.Models.SystemState.ComputeMD5(System.String)~System.String")]
[assembly: SuppressMessage("Performance", "RCS1096:Convert 'HasFlag' call to bitwise operation (or vice versa).", Scope = "member", Target = "~M:tweetz.core.Views.ComposeBlock.ComposeBlockInputControl.TextBoxOnPreviewKeyDown(System.Object,System.Windows.Input.KeyEventArgs)")]
[assembly: SuppressMessage("Critical Code Smell", "S1215:\"GC.Collect\" should not be called", Justification = "<Pending>", Scope = "member", Target = "~M:tweetz.core.Services.CollectAllTask.Execute~System.Threading.Tasks.ValueTask")]