using Amazon.DynamoDBv2.DataModel;
using POC_Hangfire.Repositories;

namespace POC_Hangfire.Models
{
    [DynamoDBTable(RegisterTables.TABLE_NAME)]
    public class Car
    {
        public int Id { get; set; }

        public string Model { get; set; }
        public string Fabricator { get; set; }
        public string Motor { get; set; }
        public string Description { get; set; }
    }
}
