namespace provider.Controllers
{
    public class Something
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public Something(string id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
