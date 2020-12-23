using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class MapTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                const int count = 1_000_000;
                yield return Run("Integer Fills Dictionary", () => IntegerFillDictionary(count), count * 4);
                yield return Run("Integer To Dictionary", () => IntegerToDictionary(count), count * 4);
            }

            await Task.CompletedTask;
        }

        private static void IntegerFillDictionary(int count)
        {
            var dictionary = new Dictionary<int, bool>();

            for (int i = 0; i < count; i++)
            {
                dictionary.Add(i, true);
            }
        }
        private static void IntegerToDictionary(int count)
        {
            _ = Enumerable.Range(0, count).ToDictionary(t => t, _ => true);
        }
    }
}
