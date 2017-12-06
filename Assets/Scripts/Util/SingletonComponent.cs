using UnityEngine;

namespace Spaceships.Util {

    public class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T> {

        public static T Instance { get; protected set; }

        protected virtual void Awake() {
            if (Instance) {
                Debug.LogError("You have multiple instances of " + typeof(T).Name + " Singleton");
            }

            DontDestroyOnLoad(gameObject);

            Instance = (T) this;
        }

    }

}