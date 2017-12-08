 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SurroundingFactory : Singleton<SurroundingFactory>
{
    public void LoadSurroundings(Level level)
    {

        foreach (var point in level.Surroundings.Keys)
        {
            var id = level.Surroundings[point];
            ObjectPool.Instance.RegisterPool(id, MResources.PointTypeSurrounding, 1);
            var obj = ObjectPool.Instance.Spawn(id, MResources.PointTypeSurrounding);
            obj.GetComponent<Transform>().SetPositionAndRotation(Point.Point2Vector3(point), new Quaternion(0, 0, 0, 0));
        }
    }

}
