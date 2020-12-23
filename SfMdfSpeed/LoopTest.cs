using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class LoopTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            const int count = 600_000_000;
            var bytes = TestData.Repeat((byte)51, count);

            await Task.CompletedTask;

            for (int i = 0; i < 10; i++)
            {
                yield return Run("For-loop Reads Byte Array", () => ForLoop(count), bytes.Length);

                yield return Run("For-each Reads Byte Array", () => ForEach(bytes), bytes.Length);
            }
        }

        private static void ForLoop(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var _ = i + 1;
            }
        }

        private static void ForEach(byte[] bytes)
        {
            foreach (var b in bytes)
            {
                var _ = b + 1;
            }
        }
    }
}
