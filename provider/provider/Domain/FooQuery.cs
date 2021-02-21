namespace PactNet01.Provider.Domain
{
    public class FooQuery
    {
        public string Name { get; }

        public FooQuery(string name)
        {
            Name = name;
        }
    }
}
