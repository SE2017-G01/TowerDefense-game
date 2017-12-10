using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    readonly Dictionary<string, SubPool> _mPools = new Dictionary<string, SubPool>();


    //取对象
    public GameObject Spawn(string name)
    {
        if (!_mPools.ContainsKey(name))
            RegisterPool(name, 1);
        SubPool pool = _mPools[name];
        return pool.Spawn();
    }

    //回收对象
    public void Unspawn(GameObject go)
    {
        SubPool pool = null;

        foreach (SubPool p in _mPools.Values)
        {
            if (p.Contains(go))
            {
                pool = p;
                break;
            }
        }

        pool.Unspawn(go);
    }

    //回收所有对象
    public void UnspawnAll()
    {
        foreach (SubPool p in _mPools.Values)
            p.UnspawnAll();
    }

    //创建新子池
    public void RegisterPool(string objname, int number)
    {
        if(_mPools.ContainsKey(objname))   return;
        //加载预设
        var prefab = Resources.Load<GameObject>(MResources.ResourceDir + objname);

        //创建子对象池
        var pool = new SubPool(prefab);
        pool.AddObject(number);
        _mPools.Add(pool.Name, pool);
    }
}