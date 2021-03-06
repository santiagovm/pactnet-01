using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace PactNet01.ProviderApi.Test.Contract.Plumbing
{
    public class XUnitOutput : IOutput
    {
        public XUnitOutput(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        public void WriteLine(string line)
        {
            _outputHelper.WriteLine(line);
        }
        
        private readonly ITestOutputHelper _outputHelper;
    }
}
