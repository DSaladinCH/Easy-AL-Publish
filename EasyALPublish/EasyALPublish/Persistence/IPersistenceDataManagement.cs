using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Persistence
{
    public interface IPersistenceDataManagement
    {
        PersistentData Load();
        bool Save(PersistentData dataToSave);
    }
}
