using UnityEngine;

[CreateAssetMenu(fileName = "Shake", menuName = "Spaceships/Shake Preset")]
public class ShakePreset : ScriptableObject {
    public float Duration = 1f;
    public Vector3 Amplitude = Vector3.one;
    public Vector3 Frequency = 10 * Vector3.one;
}
