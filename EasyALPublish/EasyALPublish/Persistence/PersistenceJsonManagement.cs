using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Reflection;

namespace EasyALPublish.Persistence
{
    public class PersistenceJsonManagement : IPersistenceDataManagement
    {
        private string savePath = "";
        private string saveFileName = "EasyALPublishConfigs.json";
        private string savePathFile
        {
            get
            {
                return savePath + saveFileName;
            }
        }

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        public PersistenceJsonManagement()
        {
            savePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        }

        public PersistentData Load()
        {
            if (!File.Exists(savePathFile))
                return new PersistentData();

            string json = File.ReadAllText(savePathFile);
            try
            {
                return JsonSerializer.Deserialize<PersistentData>(json, options);
            }
            catch
            {
                return new PersistentData();
            }
        }

        public bool Save(PersistentData dataToSave)
        {
            string json = JsonSerializer.Serialize(dataToSave, options);
            try
            {
                File.WriteAllText(savePathFile, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
