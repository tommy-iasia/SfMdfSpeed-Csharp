using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public static class TestData
    {
        public static T[] Repeat<T>(T value, int size)
        {
            var array = new T[size];

            System.Array.Fill(array, value);

            return array;
        }

        public static string Repeat(string text, int count)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < count; i++)
            {
                builder.Append(text);
            }

            return builder.ToString();
        }
    }
}
