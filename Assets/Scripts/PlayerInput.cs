using Rewired;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    private int _playerID;

    public int PlayerID {
        get { return _playerID; }
        set {
            _playerID = value;
            Player = ReInput.players.GetPlayer(_playerID);
        }
    }

#if UNITY_EDITOR
    private bool _playerIsDirty;
    private Player _player;
    public Player Player {
        get {
            if (!ReInput.isReady) {
                _playerIsDirty = true;
                return null;
            }

            if (_playerIsDirty) {
                PlayerID = _playerID;
                _playerIsDirty = (_player == null);
            }

            return _player;
        }
        private set { _player = value; }
    }
#else
    public Player Player { get; private set; }
#endif
}