using Rewired;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Level : MonoBehaviour {
    public Camera LevelCamera;
    public GameObject[] Players;
    public float SafetyMargin = 20;
    
    [Tooltip("Set a vertical size for the bounding box. The horizontal size is obtained using the aspect ratio.")]
    public float VerticalBounds = 1000f;
    
    private BoxCollider _bounds;

    private void Awake()
    {
        _bounds = GetComponent<BoxCollider>();
    }

    public void Start() {
        // orthographic camera stuff here
        //float height = 2.0f * LevelCamera.orthographicSize + SafetyMargin;
        //float width = 2.0f * LevelCamera.orthographicSize * Screen.width / Screen.height + SafetyMargin;
        //float depth = LevelCamera.farClipPlane - LevelCamera.nearClipPlane + SafetyMargin;
        float height = VerticalBounds + SafetyMargin;
        float width = VerticalBounds * Camera.main.aspect + SafetyMargin;
        float depth = LevelCamera.farClipPlane - LevelCamera.nearClipPlane + SafetyMargin;
        
        _bounds.size = new Vector3(width, height, depth);
    }

    public void Update() {
        if (ReInput.players.Players[0].GetButton("Start") || ReInput.players.Players[1].GetButton("Start")) {
            Players[0].SetActive(true);
            Players[0].GetComponent<HealthComponent>().Awake();
            Players[1].SetActive(true);
            Players[1].GetComponent<HealthComponent>().Awake();
        }
        if (ReInput.players.Players[0].GetButton("Debug")) {
            Players[0].transform.position = Vector3.zero;
            Players[1].transform.position = Vector3.zero;
            GetComponent<AudioSource>().Play();
        }
    }

    public void OnTriggerExit(Collider other) {
        //TODO Spawn a coroutine and only move player after some safe space
        // Also, when teleported, you can just stay out of bounds. Important to fix.
        var player = SpaceshipsUtils.FindPlayerInParents(other.gameObject);
        if (player) {
            Vector3 offset = player.transform.position - _bounds.transform.localPosition;
            player.transform.SetPositionAndRotation(player.transform.position - 2 * offset,
                player.transform.rotation);
        }
        else {
            Destroy(other.gameObject);
        }
    }
}