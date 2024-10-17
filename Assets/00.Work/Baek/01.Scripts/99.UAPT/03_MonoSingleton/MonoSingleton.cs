using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object locker = new object();
    private static T instance = null;
    public static T Instance
    {
        get
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = (T)FindAnyObjectByType(typeof(T));
                    if (instance == null)
                    {
                        GameObject temp = new GameObject(name: $"@{typeof(T).ToString()}");
                        instance = temp.AddComponent<T>();
                    }
                    DontDestroyOnLoad(instance);
                }
            }
            return instance;
        }
    }
}