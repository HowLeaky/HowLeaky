
using System;

namespace HowLeaky.SyncModels
{
    public abstract class CustomSyncModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }       //Going to use strings here - as User Account could be deleted, and we would like to preserve who did this.

        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }      //Going to use strings here - as User Account could be deleted, and we would like to preserve who did this.

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Comments { get; set; }
        public bool Published { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomSyncModel() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="userName"></param>
        /// <param name="dt"></param>
        public CustomSyncModel(Guid guid, string userName, DateTime dt)
        {
            Id = guid;
            ModifiedBy = userName;
            ModifiedDate = dt;
        }
    }
}