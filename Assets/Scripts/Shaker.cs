using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Spaceships {
    public class Shaker : MonoBehaviour {
        private bool _isShaking = false;

        public void Play(NoiseSettings settings, float duration, AnimationCurve gain, Vector3 modulation) {
            if (settings && duration > 0f && !_isShaking && isActiveAndEnabled) {
                StartCoroutine(Shaking(settings, duration, gain, modulation));
            }
        }

        public void Play(NoiseSettings settings, float duration, AnimationCurve gain) {
            Play(settings, duration, gain, Vector3.one);
        }

        private IEnumerator Shaking(NoiseSettings settings, float duration, AnimationCurve gainCurve,
            Vector3 modulation) {
            _isShaking = true;
            var noise = GameMode.Instance.MainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (!noise) {
                noise = GameMode.Instance.MainCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            var originalSettings = noise.m_Definition;
            noise.m_Definition = Instantiate(settings);

            for (int i = 0; i < noise.m_Definition.PositionNoise.Length; i++) {
                noise.m_Definition.PositionNoise[i].X.Amplitude *= modulation.x;
                noise.m_Definition.PositionNoise[i].Y.Amplitude *= modulation.y;
                noise.m_Definition.PositionNoise[i].Z.Amplitude *= modulation.z;
            }

            float timeShaking = 0f;
            float gainSpeed = gainCurve[gainCurve.length - 1].time / duration;
            while (timeShaking < duration) {
                float gain = gainCurve.Evaluate(timeShaking * gainSpeed);
                noise.m_AmplitudeGain = gain;
                // noise.m_FrequencyGain = gain;
                timeShaking += Time.deltaTime;
                yield return null;
            }

            noise.m_Definition = originalSettings;
            _isShaking = false;
        }
    }

    [Serializable]
    public class ShakeData {
        public NoiseSettings Noise;
        public float Duration = 0.25f;
        public AnimationCurve Gain = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    }
}