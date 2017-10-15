using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [HideInInspector] public List<Transform> Targets;
    public float MinimumSize = 10.0f;
    public float SafeRatio = 0.9f;
    public float ZoomTime = 2.0f;
    public float Smoothness = 10;
    private Vector3 _screenSizeInWorld;
    private Camera _camera;
    private Vector3 _targetPosition = Vector3.zero, _lastTargetPosition;

    void Start() {
        _camera = GetComponent<Camera>();
        UpdateScreenSizeInWorld();
    }

    void LateUpdate() {
        Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);
        Vector2 mid = Vector2.zero;
        int numActive = 0;
        for (int i = 0; i < Targets.Count; i++) {
            if (Targets[i].gameObject.activeInHierarchy) {
                Vector3 pos = Targets[i].position;

                min = Vector2.Min(pos, min);
                max = Vector2.Max(pos, max);

                mid.x += pos.x;
                mid.y += pos.y;
                numActive++;
            }
        }

        if (numActive > 0) {
            mid.x /= numActive;
            mid.y /= numActive;
        } else {
            mid = Vector2.zero;
        }

        _lastTargetPosition = _targetPosition;
        _targetPosition = new Vector3(mid.x, mid.y, transform.position.z);
        Vector3 currentPosition;
        currentPosition.x = MovementPD(transform.position.x, _targetPosition.x, _lastTargetPosition.x, 1 / Smoothness,
            Time.deltaTime);
        currentPosition.y = MovementPD(transform.position.y, _targetPosition.y, _lastTargetPosition.y, 1 / Smoothness,
            Time.deltaTime);
        currentPosition.z = _targetPosition.z;
        transform.position = currentPosition;

        Vector2 maxDistance = new Vector2(max.x - min.x, max.y - min.y);
        float targetOrthoSize;
        if (maxDistance.x / _screenSizeInWorld.x > maxDistance.y / _screenSizeInWorld.y) {
            targetOrthoSize = (.5f * maxDistance.x / _camera.aspect) / SafeRatio;
        } else {
            targetOrthoSize = (.5f * maxDistance.y) / SafeRatio;
        }

        float zoomVel = 0.0f;
        targetOrthoSize = Mathf.Max(targetOrthoSize, MinimumSize);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthoSize, ref zoomVel, ZoomTime,
            float.MaxValue, Time.deltaTime);

        UpdateScreenSizeInWorld();
    }

    void UpdateScreenSizeInWorld() {
        _screenSizeInWorld.y = 2 * _camera.orthographicSize;
        _screenSizeInWorld.x = _screenSizeInWorld.y * Screen.width / Screen.height;
    }

    float MovementPD(float current, float target, float lastTarget, float speed, float dt) {
        float t = dt * speed;
        float v = (target - lastTarget) / t;
        float f = current - lastTarget + v;
        return target - v + f * Mathf.Exp(-t);
    }
}