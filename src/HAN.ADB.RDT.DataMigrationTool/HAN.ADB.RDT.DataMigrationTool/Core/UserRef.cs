using MongoDB.Bson;

namespace HAN.ADB.RDT.DataMigrationTool.Core
{
	public class UserRef
	{
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}