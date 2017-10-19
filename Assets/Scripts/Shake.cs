using System;
using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour {
    public bool PlayOnStart = true;
    public Transform Target;
    public ShakePreset Preset;
    public Vector3 AmplitudeModulation = Vector3.one;

    protected void Start() {
        if (!Target) {
            Target = transform;
        }
        if (PlayOnStart && Preset) {
            Play(Preset);
        }
    }

    public void Play(ShakePreset preset = null) {
        if (preset) {
            Preset = preset;
        }

        if (Preset) {
            StartCoroutine(Shaking());
        }
    }

    public void Play(Vector3 amplitude, Vector3 frequency, float duration) {
        var preset = ScriptableObject.CreateInstance<ShakePreset>();
        preset.Amplitude = amplitude;
        preset.Frequency = frequency;
        preset.Duration = duration;
        Play(preset);
    }

    private IEnumerator Shaking() {
        float timeLeft = Preset.Duration;
        while (timeLeft > 0f) {
            float dt = Time.deltaTime;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < 3; i++) {
                pos[i] = AmplitudeModulation[i] * Preset.Amplitude[i] * Mathf.Sin(Time.time * Preset.Frequency[i]);
            }
            Target.localPosition = pos;
            yield return null;
            timeLeft -= dt;
        }
    }
}