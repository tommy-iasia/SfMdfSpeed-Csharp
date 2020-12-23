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
            var parts = ParallelUtility.Split(count, 4);
            Parallel.ForEach(parts, part =>
            {
                for (int i = part.from; i < part.length; i++)
                {
                    var _ = i + 1;
                }
            });
        }

        private static void ForEach(byte[] bytes)
        {
            var parts = ParallelUtility.Split(bytes.Length, 4);
            Parallel.ForEach(parts, part =>
            {
                var span = new Span<byte>(bytes, part.from, part.length);
                foreach (var b in span)
                {
                    var _ = b + 1;
                }
            });
        }
    }
}
