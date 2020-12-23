using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public static class ParallelUtility
    {
        public static IEnumerable<(int from, int length)> Split(int length, int count)
        {
            var smallSize = length / count;
            var sizeAdds = length % count;

            var from = 0;
            for (int i = 0; i < count; i++)
            {
                var size = smallSize + (i < sizeAdds ? 1 : 0);
                yield return (from, size);

                from += size;
            }
        }
    }
}
