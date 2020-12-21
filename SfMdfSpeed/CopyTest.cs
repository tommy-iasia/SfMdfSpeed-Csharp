using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class CopyTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            var bytes = TestData.Repeat((byte)51, 600_000_000);

            await Task.CompletedTask;

            for (int i = 0; i < 10; i++)
            {
                yield return Run("10 For-loops Copy Byte Array", () => LoopCopy(bytes), bytes.Length);

                yield return Run("10 Array.Copy Copy Byte Array", () => ArrayCopy(bytes), bytes.Length);

                yield return Run("MemoryStream.CopyTo Copies Byte Array",
                    () => MemeryStreamCopyTo(bytes), bytes.Length);

                yield return await RunAsync("MemoryStream.CopyToAsync Copies Byte Array",
                    async () => await MemeryStreamCopyToAsync(bytes), bytes.Length);

                yield return Run("Span Wraps Byte Array", () => CreateSpan(bytes), bytes.Length);
            }
        }

        private static void LoopCopy(byte[] rawBytes)
        {
            byte[] newBytes = new byte[rawBytes.Length];

            const int parts = 10;
            Parallel.For(1, parts, i =>
            {
                var length = rawBytes.Length / parts;

                var from = (i - 1) * length;
                var to = from + length;

                for (int j = from; j < to; j++)
                {
                    newBytes[j] = rawBytes[j];
                }
            });

            var paralleled = rawBytes.Length / parts * parts;
            for (int i = paralleled; i < rawBytes.Length; i++)
            {
                newBytes[i] = rawBytes[i];
            }
        }

        private static void ArrayCopy(byte[] rawBytes)
        {
            byte[] newBytes = new byte[rawBytes.Length];

            const int parts = 10;
            Parallel.For(0, parts - 1, i =>
            {
                var length = rawBytes.Length / parts;
                var index = i * length;

                Array.Copy(rawBytes, index, newBytes, index, length);
            });

            var paralleled = rawBytes.Length / parts * parts;
            var remaining = rawBytes.Length - paralleled;
            Array.Copy(rawBytes, paralleled, newBytes, paralleled, remaining);
        }

        private static void MemeryStreamCopyTo(byte[] rawBytes)
        {
            var fromStream = new MemoryStream(rawBytes);

            var toStream = new MemoryStream();
            fromStream.CopyTo(toStream);
        }
        private static async Task MemeryStreamCopyToAsync(byte[] rawBytes)
        {
            var fromStream = new MemoryStream(rawBytes);

            var toStream = new MemoryStream();
            await fromStream.CopyToAsync(toStream);
        }

        private static void CreateSpan(byte[] rawBytes) => _ = new Span<byte>(rawBytes);
    }
}
