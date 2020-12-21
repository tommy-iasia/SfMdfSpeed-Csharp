using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class Utf8Test : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            var fromText = GetText();

            for (int i = 0; i < 10; i++)
            {
                var (encodeTime, bytes) = Run(() => Encoding.UTF8.GetBytes(fromText));
                yield return new SpeedResult("UTF8 Encodes Text", bytes.Length, encodeTime);

                var (decodeTime, text) = Run(() => Encoding.UTF8.GetString(bytes));
                yield return new SpeedResult("UTF8 Decodes Text", bytes.Length, decodeTime);
            }

            await Task.CompletedTask;
        }

        private static string GetText()
        {
            const string @long = "I am very long very long very long very long...";
            const int count = 1_000_000;

            var builder = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                builder.Append(@long);
            }

            return builder.ToString();
        }
    }
}
