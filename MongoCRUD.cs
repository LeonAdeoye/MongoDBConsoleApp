using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBConsoleApp
{
    public class MongoCRUD
    {
        private readonly IMongoDatabase database;
        private readonly static string COLLECTION_NAME = "Persons";

        public MongoCRUD(string databaseName)
        {
            string connectionURI = "mongodb://wo*ch_user:wo*ch_user@leonadeoyemongodbcluster-shard-00-01-gni1u.azure.mongodb.net:27017,leonadeoyemongodbcluster-shard-00-00-gni1u.azure.mongodb.net:27017,leonadeoyemongodbcluster-shard-00-02-gni1u.azure.mongodb.net:27017/admin?serverSelectionTimeoutMS=20000&readPreference=primary&ssl=true";
            var client = new MongoClient(connectionURI);
            database = client.GetDatabase(databaseName);
        }

        public void InsertDocument<T>(string collectionName, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        public List<T> LoadDocuments<T>(string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadDocumentById<T>(string collectionName, Guid id)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return collection.Find(filter).First();
        }

        public void UpsertDocument<T>(string collectionName, Guid id, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.ReplaceOne(
                new BsonDocument("_id", id), 
                document, 
                new ReplaceOptions { IsUpsert = true});
        }

        public void DeleteDocument<T>(string collectionName, Guid id)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }

        public static void Main()
        {
            MongoCRUD db = new("AddressBook");

            Person personWithAddress = new()
            {
                LastName = "Saori",
                FirstName = "Adeoye",
                PrimaryAddress = new()
                {
                    PropertyName = "Coastline",
                    Street =  "Upper Frederick Street",
                    City = "Tokyo",
                    Postcode = "L1 5DF"
                }
            };

            db.InsertDocument(COLLECTION_NAME, personWithAddress);

            // TODO comment out as necessary.
            Person person = db.LoadDocumentById<Person>(COLLECTION_NAME, new Guid("bcf85152-f025-405c-8058-912581e5588a"));
            Console.WriteLine(person);
            person.DateOfBirth = new DateTime(1982, 10, 25, 0, 0, 0, DateTimeKind.Utc);
            db.UpsertDocument<Person>(COLLECTION_NAME, person.Id, person);            
            db.DeleteDocument<Person>(COLLECTION_NAME, person.Id);           
            
            var documents = db.LoadDocuments<Person>(COLLECTION_NAME);
            foreach(var document in documents)
            {
                Console.WriteLine($"{document.Id}: {document.FirstName} {document.LastName}");

                if(document.PrimaryAddress != null)
                {
                    Console.WriteLine($"City: {document.PrimaryAddress.City}");
                }
                Console.WriteLine();
            }
        }
    }
}
