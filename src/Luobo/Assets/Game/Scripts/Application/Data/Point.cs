using UnityEngine;
using System.Collections;

//格子坐标
public class Point
{
    public int X;
    public int Y;
    public int Type;

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
        this.Type = Consts.PointTypePlate;
    }
    public Point(int x, int y, int type)
    {
        this.X = x;
        this.Y = y;
        this.Type = type;
    }

    public override string ToString()
    {
        string typestr = Type == Consts.PointTypePlate
            ? "Plate"
            : Type == Consts.PointTypeSurrounding
                ? "Surroundings"
                : Type == Consts.PointTypeTower
                    ? "Tower"
                    : "Unknown";
        return string.Format("[X:{0},Y:{1},Type:{2}]",
            this.X,
            this.Y,
            typestr
        );
    }

}