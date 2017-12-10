using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

public class SubPool
{
    //预设
    private readonly GameObject _mPrefab;

    //集合
    private readonly List<GameObject> _mObjects = new List<GameObject>();


    //名字标识
    public string Name
    {
        get { return _mPrefab.name; }
    }

    //构造
    public SubPool(GameObject prefab)
    {
        this._mPrefab = prefab;
    }

    //加对象
    public void AddObject(int number)
    {
        for (var i = 0; i < number; i++)
        {
            var ob = Object.Instantiate<GameObject>(_mPrefab);
            ob.SetActive(false);
            _mObjects.Add(ob);
        }
    }

    //取对象
    public GameObject Spawn()
    {
        GameObject go = null;

        foreach (var obj in _mObjects)
        {
            if (obj.activeSelf) continue;
            go = obj;
            break;
        }

        if (go == null)
        {
            go = Object.Instantiate<GameObject>(_mPrefab);
            _mObjects.Add(go);
        }

        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    //回收对象
    public void Unspawn(GameObject go)
    {
        if (!Contains(go)) return;
        go.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);
        go.SetActive(false);
    }

    //回收该池子的所有对象
    public void UnspawnAll()
    {
        foreach (var item in _mObjects)
        {
            if (item.activeSelf)
            {
                Unspawn(item);
            }
        }
    }

    //是否包含对象
    public bool Contains(GameObject go)
    {
        return _mObjects.Contains(go);
    }
}