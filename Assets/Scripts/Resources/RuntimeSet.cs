using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Spaceships.Resources {

    [Serializable]
    public abstract class RuntimeSet<T> : ScriptableObject {

        public List<T> Items = new List<T>();

        public void Insert(T t) {
            if (!Items.Contains(t))
                Items.Add(t);
        }

        public void Remove(T t) {
            if (Items.Contains(t))
                Items.Remove(t);
        }

    }

}