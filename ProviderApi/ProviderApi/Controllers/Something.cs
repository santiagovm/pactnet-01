﻿namespace PactNet01.ProviderApi.Controllers
{
    public class Something
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }

        public Something(string id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }
}
