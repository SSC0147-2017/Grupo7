using UnityEngine;

namespace Spaceships {
    public class DamageOnImpact : MonoBehaviour {
        public int Damage = 1;
        public bool DestroyOnImpact = true;
        public GameObject DamageEffect;

        public ShakeData Shake;

        public GameObject Instigator;
        public bool DontDamageInstigator = true;

        public void OnCollisionEnter(Collision collision) {
            DoDamage(collision.collider.gameObject, collision.contacts[0].normal);
        }

        public void OnTriggerEnter(Collider other) {
            // @TODO we need a better way to find a valid normal
            DoDamage(other.gameObject, transform.forward);
        }

        private void DoDamage(GameObject obj, Vector3 normal) {
            if (obj == Instigator && DontDamageInstigator) {
                obj = null;
            }

            if (obj) {
                var health = obj.GetComponent<Health>();
                if (health) {
                    // Deal damage
                    health.TakeDamage(Instigator, Damage);

                    // Destroy
                    if (DamageEffect) {
                        Instantiate(DamageEffect, transform.position + transform.forward,
                            Quaternion.Inverse(transform.rotation), obj.transform);
                    }

                    var shaker = GameMode.Instance.MainCamera.GetComponent<Shaker>();
                    if (shaker) {
                        shaker.Play(Shake.Noise, Shake.Duration, Shake.Gain, normal);
                    }
                }

                if (DestroyOnImpact) {
                    Destroy(gameObject);
                }
            }
        }
    }
}