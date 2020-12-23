using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class Program
    {
        public async static Task Main()
        {
            var guid = Guid.NewGuid();

            Console.WriteLine($"Start at {DateTime.Now:yyMMdd HH:mm:ss}");
            await AppendAsync($"/* start {guid} at {DateTime.Now:yyMMdd HH:mm:ss} */");

            foreach (var test in GetTests())
            {
                Console.WriteLine($"Run {test}");

                await foreach (var result in test.RunAsync())
                {
                    Console.WriteLine($"{result.Title}: {Math.Round((double)(result.Size / result.Time / 1024 / 1024)):#,###.##}MB/s: {result.Size:#,###}B/{result.Time}s");

                    var resultJson = JsonSerializer.Serialize(result);
                    await AppendAsync($"results.push({resultJson})");
                }
            }

            Console.WriteLine($"End at {DateTime.Now:yyMMdd HH:mm:ss}");
            await AppendAsync($"/* end {guid} at {DateTime.Now:yyMMdd HH:mm:ss} */");
        }

        private static IEnumerable<ISpeedTest> GetTests()
        {
            yield return new LoopTest();
            yield return new CopyTest();
            yield return new RegexTest();
            yield return new SocketTest();
            yield return new FileTest();
            yield return new Utf8Test();
            yield return new Base64Test();
            yield return new MapTest();
            yield return new LifeCycleTest();
        }


        private static async Task AppendAsync(string line)
        {
            const string resultFile = "results.js";
            await File.AppendAllLinesAsync(resultFile, new[] { line });
        }
    }
}
