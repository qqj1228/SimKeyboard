using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SimKeyboard {
    public class Config {
        public ConfigFile<HotKeySetting> Setting { get; set; }

        public Config() {
            Setting = new ConfigFile<HotKeySetting>(".\\Configs\\HotKeySetting.xml");
            LoadConfig(Setting);
        }

        public void LoadConfig<T>(ConfigFile<T> config) where T : new() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream reader = new FileStream(config.File_xml, FileMode.Open)) {
                    config.Data = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception ex) {
                MessageBox.Show("Using default " + config.Name + " because of failed to load them, reason: " + ex.Message);
                config.Data = new T();
            }
        }

        public void SaveConfig<T>(ConfigFile<T> config) where T : new() {
            if (config == null || config.Data == null) {
                throw new ArgumentNullException(nameof(config.Data));
            }
            try {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(config.File_xml)) {
                    xmlSerializer.Serialize(writer, config.Data);
                    writer.Close();
                }
            } catch (Exception ex) {
                MessageBox.Show("Save " + config.Name + " error, reason: " + ex.Message);
            }
        }
    }

    public class ConfigFile<T> where T : new() {
        public string File_xml { get; }
        public T Data { get; set; }
        public string Name {
            get { return Path.GetFileName(File_xml).Split('.')[0]; }
        }

        public ConfigFile(string xml) {
            this.File_xml = xml;
        }
    }

    [Serializable]
    public class HotKeySetting {
        public string HotKey { get; set; }
        public List<string> SendTexts { get; set; }

        public HotKeySetting() {
            HotKey = "Ctrl + Alt + Up";
            SendTexts = new List<string>();
        }
    }
}
