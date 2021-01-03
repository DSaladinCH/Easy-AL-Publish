using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

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

        JsonSerializerSettings options = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            Formatting = Formatting.Indented
        };

        public PersistenceJsonManagement()
        {
            //savePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            savePath = "C:\\temp\\";
        }

        public PersistentData Load()
        {
            if (!File.Exists(savePathFile))
                return new PersistentData();

            string json = File.ReadAllText(savePathFile);
            try
            {
                return JsonConvert.DeserializeObject<PersistentData>(json, options);
            }
            catch
            {
                return new PersistentData();
            }
        }

        public bool Save(PersistentData dataToSave)
        {
            string json = JsonConvert.SerializeObject(dataToSave, options);
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
