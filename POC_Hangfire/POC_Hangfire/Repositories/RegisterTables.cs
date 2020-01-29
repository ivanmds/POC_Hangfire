using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace POC_Hangfire.Repositories
{
    public class RegisterTables : IRegisterTables
    {
        public const string TABLE_NAME = "HangfireTest";
        private readonly IAmazonDynamoDB _dynamoDB;
        private ListTablesResponse _tables;
        public RegisterTables(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public async Task RegisterAsync()
        {
            _tables = await _dynamoDB.ListTablesAsync();
            await CreateTableCard();
        }

        private async Task CreateTableCard()
        {
            var request = new CreateTableRequest
            {
                TableName = TABLE_NAME,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                     new AttributeDefinition("Id", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement("Id", KeyType.HASH),
                },
                ProvisionedThroughput = new ProvisionedThroughput(10, 5)
            };

            if (!_tables.TableNames.Contains(TABLE_NAME))
                await _dynamoDB.CreateTableAsync(request);
        }
    }
}
