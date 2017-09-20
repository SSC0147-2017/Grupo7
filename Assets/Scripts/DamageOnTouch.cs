using UnityEngine;

public class DamageOnTouch : MonoBehaviour {
    public int Damage = 1;
    public bool IgnoreOwner = true;
    public bool DestroyOnDealingDamage = true;
    public GameObject ImpactEffect;
    public float ImpactScale = 15.0f;

    private OwnershipComponent _ownership;

    public void Start() {
        _ownership = GetComponent<OwnershipComponent>();
    }

    public void OnTriggerEnter(Collider other) {
        var player = SpaceshipsUtils.FindPlayerInParents(other.gameObject);
        if (IgnoreOwner && player == _ownership.Owner) player = null;

        if (player) {
            var healthComp = player.GetComponent<HealthComponent>();
            if (healthComp) {
                healthComp.TakeDamage(Damage);
                if (DestroyOnDealingDamage) {
                    if (ImpactEffect) {
                        var Impact = Instantiate(ImpactEffect, transform.position + transform.forward, Quaternion.Inverse(transform.rotation));
                        Impact.transform.parent = other.transform.parent;
                        Impact.transform.localScale = Vector3.one * ImpactScale;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}