using System;
using UnityEngine;

namespace Spaceships.Util {
    [Serializable]
    public class PIDControllerData {
        public float P = 1f;
        public float I = .5f;
        public float D = .1f;
    }

    [Serializable]
    public class PIDController {
        public PIDControllerData Data;

        private float _lastError = 0f;
        private float _integralError = 0f;

        public PIDController(PIDControllerData data) {
            Data = data;
        }

        public float Update(float error, float dt) {
            var d = (error - _lastError) / dt;
            _integralError += error * dt;
            _lastError = error;

            return Data.P * error + Data.I * _integralError + Data.D * d;
        }

        public float Update(float error) {
            return Update(error, Time.deltaTime);
        }
    }

    [Serializable]
    public class PIDController3D {
        public PIDControllerData Data;

        private Vector3 _lastError;
        private Vector3 _integralError;

        public PIDController3D(PIDControllerData data) {
            Data = data;
        }

        public Vector3 Update(Vector3 error, float dt) {
            var d = (error - _lastError) / dt;
            _integralError += error * dt;
            _lastError = error;

            return Data.P * error + Data.I * _integralError + Data.D * d;
        }
    }
}