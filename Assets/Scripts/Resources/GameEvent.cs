using System.Collections.Generic;
using Spaceships.Util;
using UnityEngine;

namespace Spaceships.Resources {

    [CreateAssetMenu(fileName = "Event", menuName = "Spaceships/Event")]
    public class GameEvent : ScriptableObject {

        private readonly List<GameEventListener> Listeners = new List<GameEventListener>();

        public void Raise() {
            foreach (var listener in Listeners) {
                listener.OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener) {
            if (!Listeners.Contains(listener)) {
                Listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener) {
            if (Listeners.Contains(listener)) {
                Listeners.Remove(listener);
            }
        }

    }

}