using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType(typeof(T)) as T;
            if (_instance != null) return _instance;

            var obj = new GameObject {hideFlags = HideFlags.HideAndDontSave};
            obj.AddComponent<T>();
            _instance = obj.GetComponent<T>();
            return _instance;
        }
    }
}