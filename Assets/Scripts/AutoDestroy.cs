using UnityEngine;

public class AutoDestroy : MonoBehaviour {
    public float SecondsAlive = 20.0f;
    private float _startTime;
    public void Start () {
        _startTime = Time.time;
    }

    public void Update() {
        if (Time.time - _startTime >= SecondsAlive) {
            Destroy(gameObject);
        }
    }
}
