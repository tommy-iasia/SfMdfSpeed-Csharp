using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public abstract class SpeedTest : ISpeedTest
    {
        public abstract IAsyncEnumerable<SpeedResult> RunAsync();

        protected static double Run(Action action)
        {
            var stopwatch = Stopwatch.StartNew();

            action();

            return stopwatch.Elapsed.TotalSeconds;
        }
        protected static (double time, T result) Run<T>(Func<T> func)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = func();

            return (stopwatch.Elapsed.TotalSeconds, result);
        }
        protected static async Task<double> RunAsync(Func<Task> func)
        {
            var stopwatch = Stopwatch.StartNew();
            
            await func();

            return stopwatch.Elapsed.TotalSeconds;
        }
        protected static async Task<(double time, T result)> RunAsync<T>(Func<Task<T>> func)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await func();

            return (stopwatch.Elapsed.TotalSeconds, result);
        }

        protected static SpeedResult Run(string title, Action action, long size)
        {
            var time = Run(action);

            return new SpeedResult(title, size, time);
        }
        protected static async Task<SpeedResult> RunAsync(string title, Func<Task> func, long size)
        {
            var time = await RunAsync(func);

            return new SpeedResult(title, size, time);
        }
    }
}
