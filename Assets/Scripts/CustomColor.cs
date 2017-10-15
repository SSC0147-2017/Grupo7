using UnityEngine;

public class CustomColor : MonoBehaviour {
    public Color Color;
    public Renderer Renderer;
    public int MaterialIndex;

    public void Start() {
        if (!Renderer) {
            Renderer = GetComponent<Renderer>();
        }

        if (Renderer) {
            if (MaterialIndex >= 0 && MaterialIndex < Renderer.materials.Length) {
                Renderer.materials[MaterialIndex].color = Color;
            }
        }
    }
}