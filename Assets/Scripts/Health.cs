using UnityEngine;

public class Health : MonoBehaviour {
    public int MaxHP = 3;

    [HideInInspector]
    public int HP;

    public GameObject DieEffect;

    public void Start() {
        HP = MaxHP;
    }

    public bool IsAlive => HP == 0;

    public void TakeDamage(int damage = 1) {
        HP = Mathf.Max(HP - damage, 0);
        if (HP == 0) {
            Die();
        }
    }

    public void Die() {
        HP = 0;
        if (DieEffect) {
            Instantiate(DieEffect, transform.position, gameObject.transform.rotation);
        }
        gameObject.SetActive(false);
    }
}