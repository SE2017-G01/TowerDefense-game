using UnityEngine;
using System.Collections;

//格子坐标
public class Point
{
    public int X;
    public int Y;
    public int type; //是否可以放置塔

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
        this.type = MResources.POINT_TYPE_PLATE;
    }

    public override string ToString()
    {
        string typestr = type == MResources.POINT_TYPE_PLATE
            ? "Plate"
            : type == MResources.POINT_TYPE_SURROUNDING
                ? "Surroundings"
                : type == MResources.POINT_TYPE_TOWER
                    ? "Tower"
                    : "Unknown";
        return string.Format("[X:{0},Y:{1},Type:{2}]",
            this.X,
            this.Y,
            typestr
        );
    }
}