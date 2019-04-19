# SUBSTitute
A GUI for mapping directories to virtual drives (see *SUBST* command). It is built on top of **.NET Core 3.0** and **Windows Presentation Foundation (WPF)**.

![Screenshot from version 0.9.1](https://raw.githubusercontent.com/sungaila/SUBSTitute/master/Content/0.9.1_Screenshot.png)

While its primary goal is to offer a convenient GUI for the *SUBST* command, I use it as a playground to test C# 8.0, .NET Core and WPF stuff. That's why the code is bloated and/or overengineered.
## Version history
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
