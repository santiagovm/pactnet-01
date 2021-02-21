using System;

namespace provider
{
    public class Foo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Foo(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        [Obsolete("use constructor with parameters", error: true)]
        public Foo()
        {
            // parameter-less constructor for serialization
        }
    }
}
