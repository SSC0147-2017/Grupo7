using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Spaceships.Resources;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {
    public class GameMode : SingletonComponent<GameMode> {

        // @TODO OMG WE'RE DOING THIS THE DUMBEST WAY
        public CinemachineTargetGroup TemporaryGroup;

        public CinemachineVirtualCamera MainCamera;
        public Level[] LevelSequence;
        public int CurrentLevelIndex;
        public int NextLevelIndex = -1;

        // Players
        public List<PlayerController> PlayersAlive;

        [HideInInspector] public List<PlayerController> PlayersDead;

        public PlayerSelection PlayerSelection;

        [Serializable]
        public class TeamArray : ClassWithArray<TeamSettings> { }

        [ArrayBackedByEnum(typeof(Team))] public TeamArray Teams;

        public TeamSettings WinningTeam { get; private set; }

        public Level CurrentLevel {
            get { return LevelSequence[CurrentLevelIndex]; }
        }

        private Level NextLevel {
            get {
                if (NextLevelIndex >= 0 && NextLevelIndex < LevelSequence.Length) {
                    return LevelSequence[NextLevelIndex];
                }
                return null;
            }
        }

        protected override void Awake() {
            base.Awake();

            var levels = FindObjectsOfType<Level>();
            if (LevelSequence.Length == 0) {
                Debug.LogError("The gamemode has no Level Sequence", gameObject);
                if (levels.Length > 0) {
                    LevelSequence = new Level[1];
                    LevelSequence[0] = levels[0];
                    CurrentLevelIndex = 0;
                } else {
                    Debug.LogError("There are no levels in the scene");
                }
            }

            if (CurrentLevelIndex != 0) {
#if !UNITY_EDITOR
                Debug.LogError("Starting game on non 0 level index is for debug only");
#else
                Debug.LogWarning("Starting game on non 0 level index is for debug only");
#endif
            } else {
                // everybody is attacking on level 0
                foreach (var team in Teams) {
                    team.Role = Role.Attacker;
                }
            }

            // @TODO for active teams
            foreach (var spawner in CurrentLevel.Spawners) {
                spawner.Spawn();
            }

            for (int i = 0; i < levels.Length; i++) {
                levels[i].gameObject.SetActive(false);
            }

            CurrentLevel.gameObject.SetActive(true);
        }

        void Update() {
            bool revive = false;
            foreach (var player in PlayersAlive.Concat(PlayersDead)) {
                if (player.Control != null) {
                    if (player.Control.GetButtonDown("Start")) {
                        revive = true;
                        break;
                    }
                }
            }

            if (revive) {
                // player stuff
                foreach (var player in PlayersDead) {
                    player.gameObject.DestroyTemporaries();
                    player.GetComponent<Health>().Revive();
                }
                PlayersAlive.AddRange(PlayersDead);
                PlayersDead.Clear();

                // level stuff
                foreach (var gate in CurrentLevel.Gates) {
                    if (gate != null) {
                        gate.Close();
                    }
                }
                if (NextLevel) {
                    foreach (var gate in NextLevel.Gates) {
                        if (gate != null) {
                            gate.Close();
                        }
                    }
                    NextLevel.gameObject.SetActive(false);
                    NextLevelIndex = -1;
                }

                WinningTeam = null;
                GameGUI.Instance.Hide();
            }

            // @TODO I'M ASHAMED OF MYSELF
            // I DON'T EVEN WANNA SAY WHAT'S WRONG HERE
            Instance.TemporaryGroup.m_Targets = new CinemachineTargetGroup.Target[PlayersAlive.Count];
            for (int i = 0; i < PlayersAlive.Count; i++) {
                Instance.TemporaryGroup.m_Targets[i].target = PlayersAlive[i].transform;
                Instance.TemporaryGroup.m_Targets[i].weight = PlayersAlive[i].TimeAliveClampedTemporaryAndUgly;
                Instance.TemporaryGroup.m_Targets[i].radius = 4;
            }
        }

        public void PlayerDied(GameObject dead) {
            var deadPlayer = dead.GetComponent<PlayerController>();
            PlayersAlive.Remove(deadPlayer);
            PlayersDead.Add(deadPlayer);

            if (PlayersAlive.Count == 0) {
                Debug.LogError("Draw not yet supporter");
                return;
            }

            // check teams remaining
            bool oneTeamRemaining = true;
            var winningTeamValue = PlayersAlive[0].Settings.Team;
            for (int i = 1; i < PlayersAlive.Count; i++) {
                if (winningTeamValue != PlayersAlive[i].Settings.Team) {
                    oneTeamRemaining = false;
                    break;
                }
            }

            // if more than one team, battle is still on
            if (!oneTeamRemaining) {
                return;
            }

            WinningTeam = Teams[winningTeamValue];

            // if the attacker team won, we're moving forward in the sequence 
            bool forwardInSequence = WinningTeam.Role == Role.Attacker;
            NextLevelIndex = CurrentLevelIndex + (forwardInSequence ? +1 : -1);

            // if there's a level to go, we get it
            if (NextLevelIndex >= 0 && NextLevelIndex < LevelSequence.Length) {
                Level nextLevel = NextLevel;

                // open level gates
                var currentLevelGate = CurrentLevel.Gates[WinningTeam.Side.GetOpposite()];
                var nextLevelGate = nextLevel.Gates[WinningTeam.Side];
                currentLevelGate.OpenForLeaving();
                nextLevelGate.OpenForEntering();

                // position next level on the appropriate side
                // @TODO USE BOUNDS, NOT SIZE
                // @TODO USE BOUNDS, NOT SIZE
                // @TODO USE BOUNDS, NOT SIZE
                var currentLevelBounds = CurrentLevel.GetComponent<BoxCollider>();
                var nextLevelBounds = nextLevel.GetComponent<BoxCollider>();

                Vector3 nextLevelside = WinningTeam.Side.GetOpposite().GetVector();
                nextLevel.transform.position = CurrentLevel.transform.position + nextLevelside * .5f *
                                               (currentLevelBounds.size.x * currentLevelBounds.transform.lossyScale.x +
                                                nextLevelBounds.size.x * nextLevelBounds.transform.lossyScale.x);

                // go on
                GameGUI.Instance.Go(WinningTeam.Side.GetOpposite());
                nextLevel.gameObject.SetActive(true);
            } else {
                GameGUI.Instance.Winner(WinningTeam.Color);
                Debug.Log(WinningTeam.name + " Won!");
            }
        }

        public bool EndLevelTransition() {
            var leavingLevel = CurrentLevel;
            var enteringLevel = NextLevel;
            var leavingLevelIndex = CurrentLevelIndex;
            var enteringLevelIndex = NextLevelIndex;
            if (enteringLevel) {
                foreach (var player in PlayersDead) {
                    Destroy(player.gameObject);
                }
                PlayersDead.Clear();

                leavingLevel.gameObject.DestroyTemporaries();
                leavingLevel.gameObject.SetActive(false);

                // close all gates
                foreach (var gate in leavingLevel.Gates) {
                    if (gate)
                        gate.Close();
                }

                if (leavingLevelIndex == 0) {
                    foreach (var team in Teams) {
                        team.Role = Role.Defender;
                    }
                    WinningTeam.Role = Role.Attacker;
                } else if (enteringLevelIndex == 0) {
                    foreach (var team in Teams) {
                        team.Role = Role.Attacker;
                    }
                }

                GameGUI.Instance.Hide();

                CurrentLevelIndex = enteringLevelIndex;
                NextLevelIndex = -1;

                return true;
            }
            return false;
        }

        public Bounds GetCameraBounds() {
            Bounds result = CurrentLevel.Bounds;
            if (NextLevel) {
                Vector3 min = Vector3.Min(CurrentLevel.Bounds.min, NextLevel.Bounds.min);
                Vector3 max = Vector3.Max(CurrentLevel.Bounds.max, NextLevel.Bounds.max);
                result.SetMinMax(min, max);
            }

            return result;
        }
    }
}