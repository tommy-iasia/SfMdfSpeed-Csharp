using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public record Box<T>
    {
        public T Value { get; set; }
    }

    public static class Box
    {
        public static Box<T> Create<T>(T value) => new Box<T> { Value = value };
    }
}
