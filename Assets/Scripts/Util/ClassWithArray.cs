using System;
using System.Collections;
using System.Collections.Generic;

namespace Spaceships.Util {
    public class ClassWithArray<T> : IEnumerable<T> {
        public T[] Array;

        public T this[IConvertible i] {
            get { return Array[(int) i]; }
            set { Array[(int) i] = value; }
        }

        public static implicit operator T[](ClassWithArray<T> p) {
            return p.Array;
        }

        public static implicit operator ClassWithArray<T>(T[] array) {
            return new ClassWithArray<T> {Array = array};
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>) Array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}