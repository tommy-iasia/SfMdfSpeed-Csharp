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
    }
}
