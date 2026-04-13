using Sungaila.SUBSTitute.Enums;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace Sungaila.SUBSTitute.Helper
{
    public static class AppInstallerHelper
    {
        public static bool IsManagedByAppInstaller
        {
            get
            {
                try
                {
                    return Package.Current.GetAppInstallerInfo()?.Uri != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static Frequency AutoUpdateFrequency
        {
            get
            {
                try
                {
                    if (Package.Current.GetAppInstallerInfo() is not AppInstallerInfo appInstallerInfo || !appInstallerInfo.OnLaunch)
                        return Frequency.Off;

                    return Package.Current.GetAppInstallerInfo()?.HoursBetweenUpdateChecks switch
                    {
                        0 => Frequency.OnLaunch,
                        < 24 => Frequency.Hourly,
                        < 168 => Frequency.Daily,
                        _ => Frequency.Weekly
                    };
                }
                catch
                {
                    return Frequency.Off;
                }
            }
            set
            {
                if (Package.Current.GetAppInstallerInfo() is not AppInstallerInfo appInstallerInfo)
                    return;

                try
                {
                    var options = new AutoUpdateSettingsOptions
                    {
                        AppInstallerUri = appInstallerInfo.Uri,
                        AutomaticBackgroundTask = appInstallerInfo.AutomaticBackgroundTask,
                        ForceUpdateFromAnyVersion = appInstallerInfo.ForceUpdateFromAnyVersion,
                        IsAutoRepairEnabled = appInstallerInfo.IsAutoRepairEnabled,
                        ShowPrompt = appInstallerInfo.ShowPrompt,
                        UpdateBlocksActivation = appInstallerInfo.UpdateBlocksActivation,
                        Version = appInstallerInfo.Version,
                    };

                    foreach (var item in appInstallerInfo.DependencyPackageUris)
                    {
                        options.DependencyPackageUris.Add(item);
                    }

                    foreach (var item in appInstallerInfo.OptionalPackageUris)
                    {
                        options.OptionalPackageUris.Add(item);
                    }

                    foreach (var item in appInstallerInfo.RepairUris)
                    {
                        options.RepairUris.Add(item);
                    }

                    foreach (var item in appInstallerInfo.UpdateUris)
                    {
                        options.UpdateUris.Add(item);
                    }

                    var manager = AppInstallerManager.GetDefault();

                    if (value == Frequency.Off)
                    {
                        options.OnLaunch = false;
                    }
                    else
                    {
                        options.OnLaunch = true;
                        options.HoursBetweenUpdateChecks = value switch
                        {
                            Frequency.OnLaunch => 0,
                            Frequency.Hourly => 1,
                            Frequency.Daily => 24,
                            Frequency.Weekly => 168,
                            _ => 0
                        };
                    }

                    manager.SetAutoUpdateSettings(Package.Current.Id.FamilyName, options);
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static async ValueTask<PackageUpdateAvailability> GetUpdateAvailabilityAsync()
        {
            try
            {
                var result = await Package.Current.CheckUpdateAvailabilityAsync();
                return result.Availability;
            }
            catch
            {
                return PackageUpdateAvailability.Unknown;
            }
        }
    }
}