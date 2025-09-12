using UnityEngine;

namespace Platformer2D
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get { return instance; } }
        private static T instance;

        protected virtual void Awake()
        {
            if (instance != null && instance != this) Destroy(this.gameObject);
            else instance = (T)this;
            
            if (!gameObject.transform.parent) DontDestroyOnLoad(this.gameObject);
        }
    }
}
