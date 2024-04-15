using UnityEngine;

public class SingletonClass<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance != null && _instance != this as T) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            _instance = this as T; 
            DontDestroyOnLoad(gameObject);
        }  
    }
}
