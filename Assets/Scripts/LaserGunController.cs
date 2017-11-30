using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spaceships {
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PlayerController))]
    public class LaserGunController : MonoBehaviour {
        public int BurstSize = 3;
        public float TimeBetweenBursts = 0.3f;
        public float TimeBetweenShots = 0.1f;
        public List<GameObject> Guns;

        public GameObject Shot;
        public List<AudioClip> Sounds;

        private bool _canFire = true;
        private AudioSource _audioSource;
        private PlayerController _player;
        private int _gunIndex;

        private void OnEnable() {
            _canFire = true;
        }

        public void Awake() {
            _audioSource = GetComponent<AudioSource>();
            _player = GetComponent<PlayerController>();
        }

        public void Update() {
            bool fire = false;
            if (_player.Control != null) {
                fire = _player.Control.GetButtonDown("Fire");
            }
            if (fire && _canFire) {
                StartCoroutine(FireBurst());
            }
        }

        private IEnumerator FireBurst() {
            _canFire = false;
            for (int i = 0; i < BurstSize; i++) {
                // Choose a gun
                _gunIndex = (_gunIndex+1) % Guns.Count;
                var gun = Guns[_gunIndex];

                if (gun) {
                    var shot = Instantiate(Shot, gun.transform.position, gun.transform.rotation);
                    shot.GetComponent<DamageOnImpact>().Instigator = gameObject;

                    var color = GameMode.Instance.Teams[_player.Settings.Team].Color;
                    shot.GetComponentInChildren<SgtProminence>().Color = color; // Beam body
                    shot.GetComponentInChildren<TrailRenderer>().startColor = color; // Trail begin
                    shot.GetComponentInChildren<TrailRenderer>().endColor = color; // Trail ending

                    // Play those annoying sounds
                    var selectedSound = Sounds[Random.Range(0, Sounds.Count)];
                    if (selectedSound) {
                        _audioSource.clip = selectedSound;
                        _audioSource.Play();
                    }
                    yield return new WaitForSeconds(TimeBetweenShots);
                }
            }
            yield return new WaitForSeconds(TimeBetweenBursts);
            _canFire = true;
        }
    }
}