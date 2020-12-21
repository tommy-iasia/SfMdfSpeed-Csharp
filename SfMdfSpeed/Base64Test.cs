using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class Base64Test : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            var fromBytes = TestData.Repeat((byte)51, 60_000_000);

            for (int i = 0; i < 10; i++)
            {
                const int parts = 10;
                var (encodeTime, texts) = Run(() =>
                {
                    var texts = new List<string>();

                    Parallel.For(1, parts, _ => texts.Add(Convert.ToBase64String(fromBytes)));

                    return texts;
                });
                yield return new SpeedResult("Base64 Encodes Text", fromBytes.Length * parts, encodeTime);

                var text = texts.First();
                var textBytes = Encoding.UTF8.GetBytes(text);

                yield return Run("Base64 Decodes Text",
                    () => Parallel.ForEach(texts, text => Convert.FromBase64String(text)),
                    textBytes.Length * parts);
            }

            await Task.CompletedTask;
        }
    }
}
