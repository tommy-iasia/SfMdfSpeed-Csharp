using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class Base64Test : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            var fromBytes = TestData.Repeat((byte)51, 600_000_000);

            for (int i = 0; i < 10; i++)
            {
                var (encodeTime, text) = Run(() => Convert.ToBase64String(fromBytes));
                yield return new SpeedResult("Base64 Encodes Text", fromBytes.Length, encodeTime);

                var textBytes = Encoding.UTF8.GetBytes(text);

                yield return Run("Base64 Decodes Text",
                    () => Convert.FromBase64String(text),
                    textBytes.Length);
            }

            await Task.CompletedTask;
        }
    }
}
