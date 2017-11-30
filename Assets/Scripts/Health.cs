using UnityEngine;

namespace Spaceships {
    public class Health : MonoBehaviour {
        public int HP;
        public int MaxHP = 3;

        public GameObject DieEffect;

        public void Start() {
            HP = MaxHP;
        }

        public bool IsAlive {
            get { return HP > 0; }
        }

        public void TakeDamage(GameObject instigator, int damage = 1) {
            HP = Mathf.Max(HP - damage, 0);
            if (HP == 0) {
                Die();
            }
        }

        public void Die() {
            HP = 0;
            if (DieEffect) {
                var fx = Instantiate(DieEffect);
                fx.transform.position = transform.position;
            }
            gameObject.SetActive(false);
            GameMode.Instance.PlayerDied(gameObject);
        }

        public void Revive(uint hp = 0) {
            if (!IsAlive) {
                HP = (hp == 0) ? MaxHP : (int) hp;
                gameObject.SetActive(true);
            }
        }
    }
}