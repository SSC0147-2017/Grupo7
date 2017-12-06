using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Cinemachine;
using Spaceships;
using Spaceships.Resources;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public CinemachineTargetGroup TargetGroup;

    public PlayersRuntimeSet AllPlayers;
    public PlayersRuntimeSet DeadPlayers;
    public PlayersRuntimeSet ActivePlayers;

    // Use this for initialization
    void Start() {
        TargetGroup.m_Targets = new CinemachineTargetGroup.Target[AllPlayers.Items.Count];
        for (int i = 0; i < AllPlayers.Items.Count; i++) {
            TargetGroup.m_Targets[i].target = AllPlayers.Items[i].transform;
            TargetGroup.m_Targets[i].radius = 4;
            TargetGroup.m_Targets[i].weight = 1;
        }
    }

    private void Update() {
        for (int i = 0; i < TargetGroup.m_Targets.Length; i++) {
            DebugPanel.Log("Target " + i + " weight", "Camera", TargetGroup.m_Targets[i].weight);
        }

        DebugPanel.Log("Members in AllPlayers", "RuntimeSets", AllPlayers.Items.Count);
        DebugPanel.Log("Members in DeadPlayers", "RuntimeSets", DeadPlayers.Items.Count);
        DebugPanel.Log("Members in ActivvePlayers", "RuntimeSets", ActivePlayers.Items.Count);
    }

    public void OnPlayerKilled() {
        StartCoroutine("RemovePlayerWeight");
    }

    public void OnPlayerRevived() {
        StartCoroutine("RestorePlayerWeight");
    }

    public void OnNewLevelLoaded() {
        // Carry forward old weights
        var oldTargetGroup = TargetGroup.m_Targets;
        var newTargetGroup = new CinemachineTargetGroup.Target[ActivePlayers.Items.Count];
        for (int i = 0; i < ActivePlayers.Items.Count; i++) {
            newTargetGroup[i].target = ActivePlayers.Items[i].transform;
            newTargetGroup[i].radius = 4;
            newTargetGroup[i].weight = 0;

            for (int j = 0; j < oldTargetGroup.Length; j++) {
                if (newTargetGroup[i].target == oldTargetGroup[j].target)
                    newTargetGroup[i].weight = oldTargetGroup[j].weight;
            }
        }

        // Set new values, do smooth increase
        TargetGroup.m_Targets = newTargetGroup;
        for (int i = 0; i < TargetGroup.m_Targets.Length; i++) {
            if (TargetGroup.m_Targets[i].weight == 0)
                StartCoroutine("NewPlayerWeight", i);
        }
    }

    private IEnumerator NewPlayerWeight(int iNewPlayer) {
        DebugPanel.Log("New Level Player Index", "Interactions", iNewPlayer);

        float elapsedTime = 0f;
        while (elapsedTime < 1f) {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, 1f);
            TargetGroup.m_Targets[iNewPlayer].weight = elapsedTime;
            yield return null;
        }
    }

    private IEnumerator RestorePlayerWeight() {
        PlayerController revivedPlayer = ActivePlayers.Items[ActivePlayers.Items.Count - 1];
        int iRevivedPlayer = TargetGroup.m_Targets.ToList().FindIndex(i => i.target == revivedPlayer.transform);
        DebugPanel.Log("Revived Player Index", "Interactions", iRevivedPlayer);

        float elapsedTime = 0f;
        while (elapsedTime < 1f) {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, 1f);
            TargetGroup.m_Targets[iRevivedPlayer].weight = elapsedTime;
            yield return null;
        }
    }

    private IEnumerator RemovePlayerWeight() {
        PlayerController killedPlayer = DeadPlayers.Items[DeadPlayers.Items.Count - 1];
        int iKilledPlayer = TargetGroup.m_Targets.ToList().FindIndex(i => i.target == killedPlayer.transform);
        DebugPanel.Log("Killed Player Index", "Interactions", iKilledPlayer);

        float elapsedTime = 1f;
        while (elapsedTime > 0f) {
            elapsedTime = Mathf.Max(elapsedTime - Time.deltaTime, 0f);
            TargetGroup.m_Targets[iKilledPlayer].weight = elapsedTime;
            yield return null;
        }
    }

}