using UnityEngine;

namespace Spaceships {
    public class HarmonicTween : MonoBehaviour {
        public Vector3 MinScale = Vector3.one;
        public Vector3 MaxScale = Vector3.one * 1.5f;
        public float Frequency = 10f;
        public float Exponential = 1.2f;

        void Update() {
            transform.localScale =
                Vector3.Lerp(MinScale, MaxScale, Mathf.Pow(.5f + .5f * Mathf.Sin(Frequency * Time.time), Exponential));
        }
    }
}