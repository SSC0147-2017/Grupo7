﻿using Spaceships.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Spaceships.Util {

    public class GameEventListener : MonoBehaviour {

        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable() {
            Event.RegisterListener(this);
        }

        private void OnDisable() {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised() {
            Response.Invoke();
        }

    }

}