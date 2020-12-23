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
            var fromText = TestData.Repeat("I am very long very long very long very long...", 1_000_000);

            for (int i = 0; i < 10; i++)
            {
                var (encodeTime, bytes) = Run(() => Encoding.UTF8.GetBytes(fromText));
                yield return new SpeedResult("UTF8 Encodes Text", bytes.Length, encodeTime);

                var (decodeTime, text) = Run(() => Encoding.UTF8.GetString(bytes));
                yield return new SpeedResult("UTF8 Decodes Text", bytes.Length, decodeTime);
            }

            await Task.CompletedTask;
        }
    }
}
