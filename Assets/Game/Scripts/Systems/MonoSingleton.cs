using UnityEngine;

namespace CosmosDefender
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T instance;

        protected abstract bool dontDestroyOnLoad { get; }
        public static T Instance
        {
            get
            {
                if (IsInstanceNull())
                {
                    var existingInstance = FindObjectOfType<T>();
                    if (existingInstance != null)
                    {
                        existingInstance.Initialize();
                    }
                    else
                    {
                        var go = new GameObject(nameof(T), typeof(T));
                        go.GetComponent<T>().Initialize();
                    }
                }
                return instance;
            }
        }

        protected void Initialize()
        {
            if (IsInstanceNull())
            {
                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(this);

                instance = this as T;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private static bool IsInstanceNull() => instance == null || instance.Equals(null);
    }
}