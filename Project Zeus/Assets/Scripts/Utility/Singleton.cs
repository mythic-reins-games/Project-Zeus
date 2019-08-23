using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _appFinished = false;

    public static T Instance
    {
        get
        {
            if (_appFinished)
            {
                Debug.LogErrorFormat("Accessing Singleton ({0}) after app finished!", typeof(T).ToString());
                return null;
            }

            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError("Singleton with the name '" + typeof(T).ToString() + "' was not found is scene!");
                }
            }

            return _instance;
        }
    }

    private void OnEnable()
    {
        _appFinished = false;
    }

    private void OnDestroy()
    {
        _appFinished = true;
    }
}
