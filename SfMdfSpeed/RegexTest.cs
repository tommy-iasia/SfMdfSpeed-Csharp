using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class RegexTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                var text = GetText(i);

                var bytes = Encoding.UTF8.GetBytes(text);

                yield return Run("Regex Matches", () => Regex.Matches(text, @"Message(s)?(\d+)").ToArray(), bytes.Length);
            }

            await Task.CompletedTask;
        }

        private static string GetText(int index)
        {
            const int count = 3_000_000;

            var builder = new StringBuilder($"Messages{count}");
            for (var i = 0; i < count; i++)
            {
                builder.Append($" {index} Message{i}");
            }

            return builder.ToString();
        }
    }
}
