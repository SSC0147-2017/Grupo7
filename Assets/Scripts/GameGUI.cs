using System;
using UnityEngine;
using UnityEngine.UI;
using Spaceships.Util;

namespace Spaceships {
    public class GameGUI : SingletonComponent<GameGUI> {
        [Serializable]
        public class GoArray : ClassWithArray<Image> { }

        [ArrayBackedByEnum(typeof(Side))] [SerializeField] private GoArray _goImages;

        [SerializeField] private Image _winnerImage;

        [SerializeField] private AudioClip[] _goSounds;

        [SerializeField] private AudioClip[] _winnerSounds;

        private AudioSource _audio;

        private void Start() {
            _audio = GetComponent<AudioSource>();
            // @TODO change material color for team
            //        foreach (var team in GameMode.Instance.Teams) {
            //            if (!team) continue;
            //            var image = GoImages[team.Side];
            //            if (image) {
            //                var mat = new Material(image.material);
            //                Debug.Log(team.Color);
            //                mat.SetColor("_EmissionColor", team.Color);
            //                image.material = mat;
            //            }
            //        }
        }

        public void Go(Side side) {
            var image = _goImages[side];
            if (image) {
                image.gameObject.SetActive(true);
            }
            _audio.clip = _goSounds.RandomItem();
            _audio.Play();
        }

        public void Hide() {
            foreach (var image in _goImages) {
                if (image) {
                    image.gameObject.SetActive(false);
                }
            }

            if (_winnerImage) {
                _winnerImage.gameObject.SetActive(false);
            }
        }

        public void Winner(Color color) {
            if (_winnerImage) {
                _winnerImage.gameObject.SetActive(true);
                _winnerImage.material.color = color;
            }
            _audio.clip = _winnerSounds.RandomItem();
            _audio.Play();
        }
    }
}