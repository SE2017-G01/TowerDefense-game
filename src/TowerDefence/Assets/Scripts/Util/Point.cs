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
        this.Type = MResources.PointTypePlate;
    }
    public Point(int x, int y, int type)
    {
        this.X = x;
        this.Y = y;
        this.Type = type;
    }

    public override string ToString()
    {
        string typestr = Type == MResources.PointTypePlate
            ? "Plate"
            : Type == MResources.PointTypeSurrounding
                ? "Surroundings"
                : Type == MResources.PointTypeTower
                    ? "Tower"
                    : "Unknown";
        return string.Format("[X:{0},Y:{1},Type:{2}]",
            this.X,
            this.Y,
            typestr
        );
    }

    public static Vector3 Point2Vector3(Point point)
    {
        var tileWidth = Game.Instance.TileWidth;
        var tileHeight = Game.Instance.TileHeight;
        var result = new Vector3(-4.5f * tileWidth + point.X * tileWidth,
            2 * tileHeight - point.Y * tileHeight, 0);
        return result;
    }
}