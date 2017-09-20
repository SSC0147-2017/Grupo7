using UnityEngine;

public class SpaceshipsUtils {
    public static GameObject FindPlayerInParents(GameObject gameObject) {
        GameObject result = null;
        for (var transform = gameObject.transform; transform.parent; transform = transform.parent) {
            if (transform.tag == "Player") {
                result = transform.gameObject;
                break;
            }
        }
        return result;
    }
}
