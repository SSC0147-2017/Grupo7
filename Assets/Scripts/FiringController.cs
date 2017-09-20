using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(PlayerInputComponent))]
public class FiringController : MonoBehaviour {
    public float FiringRate = 10;
    public List<GameObject> Guns;

    public GameObject Shot;
    public List<AudioClip> Sounds;
//    public float Pitch

    public Color PlayerColor = Color.white;

    private int _lastGunIndex = -1;
    private float _nextFireTime;
    private AudioSource _audioSource;
    private MovementController _movement;
    private PlayerInputComponent _input;

    public void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<MovementController>();
        _input = GetComponent<PlayerInputComponent>();
    }

    public void Update() {
        bool fire = _input.Player.GetButton("Fire");
        if (fire && Time.time > _nextFireTime) {
            // Setup automatic fire
            _nextFireTime = Time.time + 1.0f / FiringRate;

            // Choose a gun
            var gunIndex = (_lastGunIndex + 1) % Guns.Count;
            var gun = Guns[gunIndex];
            _lastGunIndex = gunIndex;

            if (gun) {
                // Shoot, man
                var shotInstance = Instantiate(Shot, gun.transform.position, gun.transform.rotation);
                shotInstance.GetComponent<OwnershipComponent>().Owner = gameObject;
                shotInstance.GetComponent<ProjectileMovement>().Speed += Mathf.Max(0.0f, _movement.Velocity);
                
                // Play those annoying sounds
                var selectedSound = Sounds[Random.Range(0, Sounds.Count)];
                if (selectedSound) {
                    _audioSource.Stop();
                    _audioSource.clip = selectedSound;
                    _audioSource.Play();
                }
            }
        }
    }
}