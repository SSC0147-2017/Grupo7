using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Level : MonoBehaviour {
    public Camera Camera;
    public List<GameObject> Players;

    public GameObject AttackerPrefab;
    public GameObject DefenderPrefab;

    public Level Previous;
    public Level Next;

    public void Update() {
        if (ReInput.players.GetPlayer(0).GetButtonDown("Start")) {
            Players[0].GetComponent<Health>().Start();
            Players[0].SetActive(true);
            Players[1].GetComponent<Health>().Start();
            Players[1].SetActive(true);
        }

        if (ReInput.players.GetPlayer(0).GetButtonDown("NextLevel")) {
            MoveToLevel(Next);
        } else if (ReInput.players.GetPlayer(0).GetButtonDown("PreviousLevel")) {
            MoveToLevel(Previous);
        }
    }

    private void MoveToLevel(Level level) {
        if (!level) {
            Debug.LogError("Trying to move to an invalid level");
            return;
        }

        // Delete all temporary on this level
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Temporary")) {
                Destroy(child);
            }
        }

        // Delete players
        foreach (GameObject player in Players) {
            Destroy(player);
        }

        // Disable this level
        gameObject.SetActive(false);

        // Enable next one
        level.gameObject.SetActive(true);
    }
}