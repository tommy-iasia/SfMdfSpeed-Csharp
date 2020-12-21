using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class FileTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            var writeBytes = TestData.Repeat((byte)51, 1_000_000_000);

            var fileNames = Enumerable.Range(0, 10).Select(t => $"{nameof(FileTest)}-{Guid.NewGuid()}.bin").ToArray();
            try
            {
                foreach (var fileName in fileNames)
                {
                    yield return await RunAsync(
                        "Writes File Bytes",
                        async () => await File.WriteAllBytesAsync(fileName, writeBytes),
                        writeBytes.Length);
                }

                foreach (var fileName in fileNames)
                {
                    var (readTime, readBytes) = await RunAsync(async () => await File.ReadAllBytesAsync(fileName));

                    if (readBytes.LongLength < writeBytes.LongLength)
                    {
                        throw new TestException();
                    }

                    yield return new SpeedResult("Reads File Bytes", writeBytes.Length, readTime);
                }
            }
            finally
            {
                foreach (var fileName in fileNames)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
            }
        }
    }
}
