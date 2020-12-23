using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class LifeCycleTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();

                var startMemory = GC.GetTotalMemory(forceFullCollection: true);

                var (createTime, box) = Run(() =>
                {
                    var blocks = Enumerable.Range(0, 50_000_000).Select(i => new Block(i, i, i, i)).ToArray();
                    return Box.Create(blocks);
                });
                var beforeMemory = GC.GetTotalMemory(forceFullCollection: true);
                yield return new SpeedResult("Create Objects", beforeMemory - startMemory, createTime);

                var collectTime = Run(() =>
                {
                    box.Value = null;

                    GC.Collect();
                });
                var afterMemory = GC.GetTotalMemory(forceFullCollection: true);
                yield return new SpeedResult("GC Removes Objects", beforeMemory - afterMemory, collectTime);
            }

            await Task.CompletedTask;
        }

        private class Block
        {
            public Block(int a, int b, int c, int d)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
            }
            private readonly int a;
            private readonly int b;
            private readonly int c;
            private readonly int d;

            public override string ToString() => $"{a}, {b}, {c}, {d}";
        }
    }
}
