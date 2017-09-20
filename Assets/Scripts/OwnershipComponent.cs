using UnityEngine;

public class OwnershipComponent : MonoBehaviour {
    private GameObject _owner;

    public GameObject Owner {
        get { return _owner; }
        set {
            _owner = value;
            UpdateColor();
        }
    }

    public Color Color = Color.white;
    public Renderer Renderer;
    public int MaterialIndex = 0;

    private void UpdateColor() {
        if (Owner) {
            var ownership = Owner.GetComponent<OwnershipComponent>();
            if (ownership) {
                Color = ownership.Color;
            }
        }
        if (Renderer) {
            if (MaterialIndex >= 0 && MaterialIndex < Renderer.materials.Length) {
                Renderer.materials[MaterialIndex].color = Color;
            }
        }
    }

    public void Start() {
        UpdateColor();
    }
}