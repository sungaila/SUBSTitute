using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sungaila.SUBSTitute
{
    /// <summary>
    /// Contains settings which should persisted when the application was closed.
    /// The settings are (de-)serialized as a XML file in the application root directory.
    /// </summary>
    public sealed class UserSettings
    {
        private const string SETTING_FILENAME = "UserSettings.xml";

        public char? LastSelectedDriveLetter { get; set; }

        public string? LastMappedDirectory { get; set; }

        public string? LastBrowserRootDirectory { get; set; }

        public bool IsExpanded { get; set; } = true;

        public static void Save(UserSettings userSettings)
        {
            var serializer = new XmlSerializer(typeof(UserSettings));
            using var writer = XmlWriter.Create(SETTING_FILENAME);

            serializer.Serialize(writer, userSettings);
        }

        public static UserSettings? Load()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(UserSettings));
                using var reader = XmlReader.Create(SETTING_FILENAME);

                return serializer.Deserialize(reader) as UserSettings;
            }
            catch
            {
                return null;
            }
        }
    }
}
