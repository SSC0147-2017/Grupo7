﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInput))]
public class LaserGunController : MonoBehaviour {
    
    public float TimeBetweenBursts = 0.3f;
    public int BurstSize = 3;
    public float TimeBetweenShots = 0.1f;
    public List<GameObject> Guns;

    public GameObject Shot;
    public List<AudioClip> Sounds;
    public Color PlayerColor = Color.white;

    private bool _canFire = true;
    private AudioSource _audioSource;
    private PlayerInput _input;
    private CustomColor _color;
    

    public void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _input = GetComponent<PlayerInput>();
        _color = GetComponent<CustomColor>();
    }

    public void Update() {
        bool fire = _input.Player.GetButton("Fire");
        if (fire && _canFire) {
            StartCoroutine(FireBurst());
        }
    }

    private IEnumerator FireBurst() {
        _canFire = false;
        for (int i = 0; i < BurstSize; i++) {
            // Choose a gun
            var gunIndex = i % Guns.Count;
            var gun = Guns[gunIndex];

            if (gun) {
                // Shoot, man
                var shotInstance = Instantiate(Shot, gun.transform.position, gun.transform.rotation);
//                shotInstance.GetComponent<ProjectileMovement>().Speed +=
//                    Vector3.Dot(_body.velocity, _body.rotation * Vector3.forward);
                shotInstance.GetComponent<DamageOnImpact>().ObjectIgnored = gameObject;
                shotInstance.GetComponent<CustomColor>().Color = _color.Color;

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