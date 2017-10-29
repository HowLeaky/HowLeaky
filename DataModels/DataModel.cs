using HowLeaky.SyncModels;
using System;

namespace HowLeaky.DataModels
{
    public class DataModel : CustomSyncModel
    {
        public string FileName { get; set; }

        public DataModel() { }

        public DataModel(Guid guid, string fileName, string userName, DateTime dt) : base (guid, userName, dt)
        {
            FileName = fileName;
        }
    }
}
