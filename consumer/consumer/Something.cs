using System;

namespace PactNet01.Consumer
{
    public class Something
    {
        // public setters required for serialization
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Something(string id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        [Obsolete("use constructor with parameters", error: true)]
        public Something()
        {
            // parameter-less constructor required for serialization
        }
    }
}
