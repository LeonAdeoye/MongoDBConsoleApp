namespace MongoDBConsoleApp
{
    public class Address
    {
        public string PropertyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }

        public override string ToString()
        {
            return $"Property Name: {PropertyName} Street: {Street} City: {City}, Postcode: {Postcode}";
        }
    }
}
