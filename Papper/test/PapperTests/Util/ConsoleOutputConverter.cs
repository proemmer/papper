using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace Papper.Tests.Util
{
    internal class ConsoleOutputConverter : TextWriter
    {
        readonly ITestOutputHelper _output;
        public ConsoleOutputConverter(ITestOutputHelper output)
        {
            _output = output;
        }
        public override Encoding Encoding => Encoding.ASCII;

        public override void WriteLine(string message) => _output.WriteLine(message);

        public override void WriteLine(string format, params object[] args) => _output.WriteLine(format, args);
    }
}