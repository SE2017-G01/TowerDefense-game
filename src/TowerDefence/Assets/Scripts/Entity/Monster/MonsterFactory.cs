using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MonsterFactory : Singleton<MonsterFactory>
{
    public void LoadMonsters(Level level)
    {
        var rounds = level.Rounds;
        foreach (var round in rounds)
        {
            if (!ObjectPool.Instance.ContainSubPool(round.Key))
                ObjectPool.Instance.RegisterPool(round.Key, MResources.PointTypeMonster, round.Value);
            else
            {
                ObjectPool.Instance.AddObject(round.Key, round.Value);
            }
        }
    }

    public GameObject Spawn(string id)
    {
        var obj = ObjectPool.Instance.Spawn(id, MResources.PointTypeMonster);
        obj.GetComponent<Transform>().SetPositionAndRotation(Point.Point2Vector3(Game.Instance.StartPoint), new Quaternion(0, 0, 0, 0));
        obj.AddComponent<Silly>();
        return obj;
    }

    public void Unspawn(GameObject obj)
    {
        ObjectPool.Instance.Unspawn(obj);
    }
}
