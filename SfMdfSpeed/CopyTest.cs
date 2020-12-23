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
                yield return Run("For-loop Copies Byte Array", () => LoopCopy(bytes), bytes.Length);

                yield return Run("Array.Copy Copies Byte Array", () => ArrayCopy(bytes), bytes.Length);

                yield return Run("Buffer.BlockCopy Copies Byte Array", () => BlockCopy(bytes), bytes.Length);

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

            for (int i = 0; i < rawBytes.Length; i++)
            {
                newBytes[i] = rawBytes[i];
            }
        }

        private static void ArrayCopy(byte[] rawBytes)
        {
            byte[] newBytes = new byte[rawBytes.Length];

            Array.Copy(rawBytes, newBytes, rawBytes.Length);
        }

        private static void BlockCopy(byte[] rawBytes)
        {
            byte[] newBytes = new byte[rawBytes.Length];

            Buffer.BlockCopy(rawBytes, 0, newBytes, 0, rawBytes.Length);
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
