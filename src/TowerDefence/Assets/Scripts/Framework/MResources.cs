using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class MResources
{
    /// <summary>
    /// Point
    /// </summary>
    //Point
    public const int PointTypePlate = 0;
    public const int PointTypeSurrounding = 1;
    public const int PointTypeTower = 2;
    public const int PointTypeMonster = 3;

    public const string ResourceDir = "Prefabs/";
    public const string LevelDir = "Assets/Resources/Levels/";

    public const string Plate = "Plate";
    public const string Start = "Start";
    public const string End = "End";

    public const int MonsterStateForward = 1;
    public const int MonsterStatePaused = 0;
    public const int MonsterStateDying = -1;
}

