using MongoDB.Bson;

namespace HAN.ADB.RDT.DataMigrationTool.Core
{
	public class PostRef
	{
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Score { get; set; }
    }
}

