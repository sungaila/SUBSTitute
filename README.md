# SUBSTitute
A GUI for mapping directories to virtual drives (see *SUBST* command). It is built on top of **.NET Core 3.0** and **Windows Presentation Foundation (WPF)**.

<img src="https://raw.githubusercontent.com/sungaila/SUBSTitute/master/Content/1.0.0_Screenshot.png" width="390" alt="Screenshot from version 1.0.0"><img src="https://raw.githubusercontent.com/sungaila/SUBSTitute/master/Content/1.0.0_About_Screenshot.png" width="300" alt="Screenshot of the about dialog from version 1.0.0">

While its primary goal is to offer a convenient GUI for the *SUBST* command, I use it as a playground to test C# 8.0, .NET Core and WPF stuff.

At this point I am satisfied with SUBSTitute: There are no major features/changes planned. I still keep an eye on .NET Core updates, though. And I might expand on the easter egg (about dialog) just for fun.
## Version history
### 1.0.0 (2019-09-29)
* Target framework updated to [.NET Core 3.0.0](https://github.com/dotnet/core/blob/master/release-notes/3.0/3.0.0/3.0.0.md) (stable version)
* Changed UI render settings so it looks less blurry on certain displays and DPI settings
* Code and project file cleanup (now that the framework is stable and not a preview anymore)
### 0.9.5 (2019-08-22)
* Target framework updated to [.NET Core 3.0.0 Preview 8](https://github.com/dotnet/core/blob/master/release-notes/3.0/preview/3.0.0-preview8.md)
* Fixed issues with C# 8 nullables (due to updating to preview 7 last time)
* Standalone downloads are now single file executables (file size could shrink once assembly trimming works for WPF)
### 0.9.4 (2019-08-13)
* Target framework updated to [.NET Core 3.0.0 Preview 7](https://github.com/dotnet/core/blob/master/release-notes/3.0/preview/3.0.0-preview7.md)
* Improved start-up time for standalone downloads (using ReadyToRun [not working for the portable download yet])
### 0.9.3 (2019-04-19)
* Mapped browser directories are bold now (but only for the selected drive)
* Pressing the Enter key will map the selected directory
* The last mapped directory is preselected on startup
* Fixed an issue where the *(Re)map* button was not updated properly
* Added icons to the *(Re)map*, *Unmap* and *Unmap all* buttons
* The standalone releases are updated to [.NET Core 3.0.0 Preview 4](https://github.com/dotnet/core/blob/master/release-notes/3.0/preview/3.0.0-preview4.md)
### 0.9.2 (2019-03-16)
* Mapped drive letters are bold now (this is for cases where the icon is missing)
* Fixed the modal dialog behavior of the *About dialog*
### 0.9.1 (2019-03-16)
* Enabled awareness to DPI changes (PerMonitorV2 and PerMonitor)
* Added a hyperlink to this GitHub repo in the *About dialog*
### 0.9 (2019-03-16)
* Map and unmap directories to virtual drives
* Unmap all virtual drives at once
* A simple browser to easily select sibling directories
* Some settings (like last selected drive letter) are persisted
