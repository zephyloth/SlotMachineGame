using System.Linq;
using UnityEngine;
 
//Auto referenced and initialized singleton behaviour class
public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static SingletonBehaviour<T> instance;
    public static SingletonBehaviour<T> Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectsOfType<SingletonBehaviour<T>>().FirstOrDefault();

            if (instance == null) 
                throw new System.Exception("Singleton base couldn't found.");
            
            instance.Init();

            return instance;
        }
    }

    private bool Initialized = false;
 
    protected virtual bool Init()
    {
        if (!Initialized)
        {
            Initialized = true;
            return false;
        }
 
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return true;
        }
 
        return true;
    }

    protected virtual void Awake()
    {
        Init();
    }
}
