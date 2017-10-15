using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Level : MonoBehaviour {
    public Camera Camera;
    public List<GameObject> Players;

    public GameObject AttackerPrefab;
    public GameObject DefenderPrefab;
    
    public void Update() {
        
        if (ReInput.players.GetPlayer(0).GetButton("Start")) {
            Players[0].GetComponent<Health>().Start();
            Players[0].SetActive(true);
            Players[1].GetComponent<Health>().Start();
            Players[1].SetActive(true);
        }
    }
}