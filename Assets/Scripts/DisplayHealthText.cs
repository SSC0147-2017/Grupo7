using UnityEngine;
using UnityEngine.UI;

public class DisplayHealthText : MonoBehaviour {
    private Text _text;

    public int PlayerNumber = 0;
    public int Health { set { _text.text = "P" + PlayerNumber + " HEALTH: " + value; } }

    public void Awake() {
        _text = GetComponent<Text>();
    }


}
