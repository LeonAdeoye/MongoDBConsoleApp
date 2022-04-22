using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBConsoleApp
{
    public class Person
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public Address PrimaryAddress { get; set; }
        [BsonElement("DOB")]
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, LastName: {LastName}, FirstName: {FirstName}, DOB:{DateOfBirth}\nAddress: {PrimaryAddress}";
        }
    }
}
