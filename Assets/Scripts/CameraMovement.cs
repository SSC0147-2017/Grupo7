using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	// externals
	public GameObject Player1;
	public GameObject Player2;
	public float LerpSpeed = 0.3f;
	[Tooltip("Not only for the ship's sides, but also for the far clip plane depth.")]
	public float SafetyMargin = 20f;
	[Tooltip("Should be bigger than the Safety Margin. About 5~10 units should do.")]
	public float NoClipMargin = 30f;
	public float MinimalDistance = 30f;

	// cacheables
	private Transform _p1Transform;
	private Transform _p2Transform;

	// internal use
	private float _camDistanceTweak;

	void Awake()
	{
	    _p1Transform = Player1.transform;
	    _p2Transform = Player2.transform;
	}

    void Start()
    {
		_camDistanceTweak = Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    void LateUpdate()
    {
        Debug.Log(string.Format("P1 pos: {0}; P2 pos: {1}; Cam pos: {2}; Cam far plane: {3}", _p1Transform.position,
        _p2Transform.position, Camera.main.transform.position, Camera.main.farClipPlane));

		// find player stuff
		Vector3 distanceVector = _p2Transform.position - _p1Transform.position;
	    float distanceHeight = Mathf.Abs(distanceVector.y);
	    float distanceWidth = Mathf.Abs(distanceVector.x);

		// track the middle point:
		// 1. follow player 1 all the way
		// 2. follow halfway through player 2
		Vector3 newCameraCenter = _p1Transform.position + 0.5f * distanceVector;

		// calculate new camera distance given a frustum height:
		// distance = frustumHeight * 0.5 / tan(fieldOfView * 0.5), where frustumHeight = frustumWidth / aspectRatio;
	    float newCameraDistance =
		    Mathf.Max(distanceHeight * 0.5f / _camDistanceTweak, 				 // accounts for frustum height
			    distanceWidth / Camera.main.aspect * 0.5f / _camDistanceTweak);  // accounts for frustum width

	    // restrains so ppl don't throw up
	    newCameraDistance = Mathf.Max(newCameraDistance, MinimalDistance);

		// set final values
		Vector3 upwards = new Vector3(0f, 0f, -1f);
		Vector3 oldPosition = Camera.main.transform.position;
		Vector3 newPosition = newCameraCenter + upwards * (newCameraDistance + SafetyMargin);
	    Camera.main.farClipPlane = Interp(Camera.main.farClipPlane, newCameraDistance + NoClipMargin, LerpSpeed);  // but why <questionmark>
		Camera.main.transform.position = Vector3.Lerp(oldPosition, newPosition, LerpSpeed);
    }
	
	// Mone, yout function has been kibada
    float Interp(float current, float target, float speed) {
	    
        if (speed <= 0.0f) {
            return target;
        }

        float distance = target - current;
        if (distance * distance < 0.01f) {
            return target;
        }
	    
        float delta = distance * Mathf.Clamp01(speed);
        return current + delta;
    }
}