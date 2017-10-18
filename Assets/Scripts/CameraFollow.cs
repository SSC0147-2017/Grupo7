using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    public List<Transform> Targets;
    public float MinimumSize = 10.0f;
    public float SafeRatio = 0.9f;
    public float ZoomTime = 2.0f;
    public float Smoothness = 10;

    private Vector3 _screenSizeInWorld;
    private Camera _camera;
    private Vector3 _targetPosition = Vector3.zero, _lastTargetPosition;
    private BoxCollider _bounds;

    void OnEnable() {
        _camera = GetComponent<Camera>();
        UpdateScreenSizeInWorld();
        Targets.RemoveAll(t => t == null);

        _bounds = SpaceshipsUtils.FindLevel(gameObject).GetComponent<BoxCollider>();
    }

    void LateUpdate() {
        // Aggregate targets positions
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

        // Target zoom
        Vector2 maxDistance = new Vector2(max.x - min.x, max.y - min.y);
        float targetOrthoSize;
        if (maxDistance.x / _screenSizeInWorld.x > maxDistance.y / _screenSizeInWorld.y) {
            targetOrthoSize = (.5f * maxDistance.x / _camera.aspect) / SafeRatio;
        } else {
            targetOrthoSize = (.5f * maxDistance.y) / SafeRatio;
        }

        // Clamp zoom
        targetOrthoSize = Mathf.Max(targetOrthoSize, MinimumSize);
        targetOrthoSize = Mathf.Min(targetOrthoSize, _bounds.size.y);

        // Set zoom
        float zoomVel = 0.0f;
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthoSize, ref zoomVel, ZoomTime,
            float.MaxValue, Time.deltaTime);
        UpdateScreenSizeInWorld();


        // Target position
        _lastTargetPosition = _targetPosition;
        _targetPosition = new Vector3(mid.x, mid.y, transform.position.z);
        Vector3 currentPosition;
        currentPosition.x = MovementPD(transform.position.x, _targetPosition.x, _lastTargetPosition.x, 1 / Smoothness,
            Time.deltaTime);
        currentPosition.y = MovementPD(transform.position.y, _targetPosition.y, _lastTargetPosition.y, 1 / Smoothness,
            Time.deltaTime);
        currentPosition.z = _targetPosition.z;

        // Clamp position
//        var boundsMax = _bounds.center + .5f*_bounds.size;
//        var camMax = currentPosition + _screenSizeInWorld;
//        currentPosition.x -= Mathf.Max(camMax.x - boundsMax.x, 0);
//        currentPosition.y -= Mathf.Max(camMax.y - boundsMax.y, 0);
//        DebugPanel.Log("Camera Max", camMax);
//        DebugPanel.Log("Bounds Max", boundsMax);
//        DebugPanel.Log("Diff Max", camMax - boundsMax);
//        var boundsMin = _bounds.center - .5f*_bounds.size;
//        var camMin = currentPosition - _screenSizeInWorld;
//        currentPosition.x -= Mathf.Min(boundsMin.x - camMin.x, 0);
//        currentPosition.y -= Mathf.Min(boundsMin.y - camMin.y, 0);

        // Set position
        transform.position = currentPosition;
    }

    private void UpdateScreenSizeInWorld() {
        _screenSizeInWorld.y = 2 * _camera.orthographicSize;
        _screenSizeInWorld.x = _screenSizeInWorld.y * Screen.width / Screen.height;
    }

    private static float MovementPD(float current, float target, float lastTarget, float speed, float dt) {
        float t = dt * speed;
        float v = (target - lastTarget) / t;
        float f = current - lastTarget + v;
        return target - v + f * Mathf.Exp(-t);
    }
}