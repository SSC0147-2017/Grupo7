using UnityEngine;

namespace Spaceships {
    [RequireComponent(typeof(Camera))]
    public class OrthographicLimits : MonoBehaviour {
        public float MinimumSize = 10;
        private Camera _camera;

        private void Awake() {
            _camera = GetComponent<Camera>();
        }

        void LateUpdate() {
            // Clamp zoom
            var cameraBounds = GameMode.Instance.GetCameraBounds();
            var orthoSize = Mathf.Max(_camera.orthographicSize, MinimumSize);
            orthoSize = Mathf.Min(orthoSize, cameraBounds.extents.y);
            orthoSize = Mathf.Min(orthoSize, cameraBounds.extents.x / _camera.aspect);
            _camera.orthographicSize = orthoSize;

            // Clamp position
            Bounds screen = ScreenBounds;
            Vector3 beforeMin = screen.min - cameraBounds.min;
            Vector3 afterMax = screen.max - cameraBounds.max;
            Vector3 pos = transform.position;
            pos.x -= Mathf.Min(beforeMin.x, 0f);
            pos.y -= Mathf.Min(beforeMin.y, 0f);
            pos.x -= Mathf.Max(afterMax.x, 0f);
            pos.y -= Mathf.Max(afterMax.y, 0f);
            transform.position = pos;
        }

        private Bounds ScreenBounds {
            get {
                Vector3 min = _camera.ScreenToWorldPoint(new Vector3(0, 0));
                Vector3 max = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight));
                Bounds result = new Bounds();
                result.SetMinMax(min, max);
                return result;
            }
        }
    }
}