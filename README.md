# SUBSTitute
A GUI for mapping directories to virtual drives (see *SUBST* command). It is built on top of **.NET Core 3.0** and **Windows Presentation Foundation (WPF)**.

![Screenshot from version 0.9](https://raw.githubusercontent.com/sungaila/SUBSTitute/master/Content/0.9_Screenshot.png)

While its primary goal is to offer a convenient GUI for the *SUBST* command, I use it as a playground to test C# 8.0, .NET Core and WPF stuff. That's why the code is bloated and/or overengineered.
## Version history
### 0.9.1 (2019-03-16)
* Enabled awareness to DPI changes (PerMonitorV2 and PerMotitor)
* Added a hyperlink to this GitHub repo in the *About dialog*
### 0.9 (2019-03-16)
* Map and unmap directories to virtual drives
* Unmap all virtual drives at once
* A simple browser to easily select sibling directories
* Some settings (like last selected drive letter) are persisted
