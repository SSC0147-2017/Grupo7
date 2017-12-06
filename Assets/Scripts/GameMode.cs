using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Rewired;
using Spaceships.Resources;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {

    public class GameMode : SingletonComponent<GameMode> {

        // Levels
        public CinemachineVirtualCamera MainCamera;

        public Level[] LevelSequence;
        public int CurrentLevelIndex;
        public int NextLevelIndex = -1;

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

        // Players
        public PlayersRuntimeSet AllPlayersModular;

        public PlayersRuntimeSet DeadPlayersModular;
        public PlayersRuntimeSet ActivePlayersModular;


        public List<PlayerController> PlayersAlive;
        public List<PlayerController> PlayersDead;
        public PlayerSelection PlayerSelection;

        // Magic
        [Serializable]
        public class TeamArray : ClassWithArray<TeamSettings> {

        }

        [ArrayBackedByEnum(typeof(Team))] public TeamArray Teams;

        public TeamSettings WinningTeam { get; private set; }

        override protected void Awake() {
            base.Awake(); // needed (singleton)
            var levels = FindObjectsOfType<Level>();

            if (LevelSequence.Length == 0) {
                Debug.LogError("The gamemode has no Level Sequence", gameObject);

                if (levels.Length > 0) {
                    // what the fuck?
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
            }

            // everybody attacks on level zero
            foreach (var team in Teams) {
                team.Role = Role.Attacker;
            }

            // spawn every active team possible
            foreach (var spawner in CurrentLevel.Spawners) {
                spawner.Spawn();
            }

            // deactivate all other levels
            for (int i = 0; i < levels.Length; i++) {
                levels[i].gameObject.SetActive(false);
            }
            CurrentLevel.gameObject.SetActive(true);
        }

        void Update() {
            bool revive = false;
            foreach (var player in AllPlayersModular.Items) {
                if (player.Control != null) {
                    if (player.Control.GetButtonDown("Start")) {
                        revive = true;
                        break;
                    }
                }
            }

            if (revive) {
                // Player stuff
                foreach (var player in DeadPlayersModular.Items.ToList()) {
                    //player.gameObject.DestroyTemporaries();  // this doesn't really do anything yet, right?
                    player.GetComponent<Health>().Revive();
                }

                // Level stuff
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
        }

        public void PlayerDied() {
            var deadPlayer = DeadPlayersModular.Items[DeadPlayersModular.Items.Count - 1];

            if (ActivePlayersModular.Items.Count == 0) {
                Debug.LogError("Everyone's dead!");
                return;
            }

            // Is there only one team?
            bool oneTeamRemaining = true;
            var winningTeamValue = ActivePlayersModular.Items[0].Settings.Team;
            for (int i = 1; i < ActivePlayersModular.Items.Count; i++) {
                if (winningTeamValue != ActivePlayersModular.Items[i].Settings.Team) {
                    oneTeamRemaining = false;
                    break;
                }
            }

            // If more than one team, battle is still on
            if (!oneTeamRemaining) {
                return;
            }

            WinningTeam = Teams[winningTeamValue];

            // If the attacker team won, we're moving forward in the sequence 
            bool forwardInSequence = WinningTeam.Role == Role.Attacker;
            NextLevelIndex = CurrentLevelIndex + (forwardInSequence ? +1 : -1);

            // If there's a level to go, we get it
            if (NextLevelIndex >= 0 && NextLevelIndex < LevelSequence.Length) {
                Level nextLevel = NextLevel;

                // Open level gates
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

                // Procceed
                GameGUI.Instance.Go(WinningTeam.Side.GetOpposite());
                nextLevel.gameObject.SetActive(true);
            } else {
                GameGUI.Instance.Winner(WinningTeam.Color);
                Debug.Log(WinningTeam.name + " Won!");
            }
        }

        public bool EndLevelTransition() {
            var enteringLevel = NextLevel;
            var leavingLevel = CurrentLevel;
            var enteringLevelIndex = NextLevelIndex;
            var leavingLevelIndex = CurrentLevelIndex;
            if (enteringLevel) {
                // Kill old players
                foreach (var player in DeadPlayersModular.Items.ToList()) {
                    Destroy(player.gameObject);
                }

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