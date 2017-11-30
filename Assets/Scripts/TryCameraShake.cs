using System.Collections;
using Cinemachine;
using Spaceships;
using UnityEngine;

public class TryCameraShake : MonoBehaviour {
    public bool PlayOnAwake = true;
    public ShakeData Shake;

    private void Awake() {
        if (PlayOnAwake) {
            Play();
        }
    }

    public void Play() {
        GameMode.Instance.MainCamera.GetComponent<Shaker>().Play(Shake.Noise, Shake.Duration, Shake.Gain);
    }
}