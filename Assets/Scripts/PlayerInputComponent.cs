using Rewired;
using UnityEngine;

public class PlayerInputComponent : MonoBehaviour {
    public int PlayerID = 0;

    public Player Player { get; private set; }

    public void Awake() {
        Player = ReInput.players.GetPlayer(PlayerID);
    }
}
