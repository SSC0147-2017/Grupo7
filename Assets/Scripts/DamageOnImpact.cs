using UnityEngine;

public class DamageOnImpact : MonoBehaviour {
    public int Damage = 1;
    public bool DestroyOnImpact = true;
    public GameObject DamageEffect;

    public GameObject ObjectIgnored;

    public void OnTriggerEnter(Collider other) {
        var obj = other.gameObject;
        if (obj == ObjectIgnored) {
            obj = null;
        }

        if (obj) {
            // Deal damage
            var health = obj.GetComponent<Health>();
            if (health) {
                health.TakeDamage(Damage);

                // Destroy
                if (DamageEffect) {
                    var impact = Instantiate(DamageEffect, transform.position + transform.forward,
                        Quaternion.Inverse(transform.rotation), other.transform);
                    var parentScale = other.transform.lossyScale;
                    var scale = Vector3.one;
                    scale.x /= parentScale.x;
                    scale.y /= parentScale.y;
                    scale.x /= parentScale.z;
                    impact.transform.localScale = scale;
                }
            }

            if (DestroyOnImpact) {
                Destroy(gameObject);
            }
        }
    }
}