using System;

namespace PactNet01.ConsumerApp
{
    public class Something
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Something(string id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        [Obsolete("use constructor with parameters", error: true)]
        public Something()
        {
            // parameter-less constructor required for serialization
        }
    }
}
