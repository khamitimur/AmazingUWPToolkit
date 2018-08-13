# AmazingUWPToolkit

A number of small helpers, behaviors, etc. to help you create UWP application with ease. From most known like `BooleanToVisibilityConverter` to something more specific like `ApplicationViewHelper`.

It's designed to be a set of small tools brought together under one solution. So you can get only what you actually need.

Currently it's under kinda active development. I will update README after each new feature will be implemented. 

## [ApplicationViewHelper](https://github.com/khamitimur/AmazingUWPToolkit/tree/master/AmazingUWPToolkit.ApplicatonView)

Will help you to take control of a [`CoreApplicationViewTitleBar`](https://docs.microsoft.com/en-us/uwp/api/windows.applicationmodel.core.coreapplicationviewtitlebar) and a [`ApplicationViewTitleBar`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.viewmanagement.applicationviewtitlebar) in a scenario when you have your collection of brushes for a `TitleBar` defined as `ThemeResource`'s and you want them to change along UI theme color.

### How to use

```cs
// Your implementation of IApplicationViewData.
var applicationViewData = new ApplicationViewData();
var applicationViewHelper = new ApplicationViewHelper(applicationViewData);

await applicationViewHelper.SetAsync();
```

## Libraries used

- [PropertyChanged.Fody](https://github.com/Fody/PropertyChanged)
- [Microsoft.Toolkit.Uwp.UI.Animations](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Animations)

## License

This project is licensed under the [WTFPL License](LICENSE).
