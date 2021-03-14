namespace PactNet01.ProviderApi.Domain
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
