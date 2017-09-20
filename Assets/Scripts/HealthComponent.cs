using UnityEngine;

public class HealthComponent : MonoBehaviour {
    public int MaxHealth = 3;

    [HideInInspector]
    public int Health;

    public DisplayHealthText Display;

    public GameObject DieEffect;
    public float EffectScale = 50.0f;

    public void Awake() {
        Health = MaxHealth;
        FeedDisplay();
    }

    public bool IsAlive {
        get { return Health == 0; }
    }

    public void TakeDamage(int damage = 1) {
        Health = Mathf.Max(Health - damage, 0);
        FeedDisplay();
        if (Health == 0) {
            Die();
        }
    }

    public void Die() {
        Health = 0;
        if (DieEffect) {
            var impact = Instantiate(DieEffect, transform.position, SpaceshipsUtils.FindPlayerInParents(gameObject).transform.rotation);
            impact.transform.localScale = Vector3.one * EffectScale;
        }
        gameObject.SetActive(false);
    }

    private void FeedDisplay() {
        if (Display) {
            Display.Health = Health;
        }
    }
}