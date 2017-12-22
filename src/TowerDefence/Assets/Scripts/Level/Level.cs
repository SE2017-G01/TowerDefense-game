using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
    #region 字段
    public string Name;//名字
    public string Road;//路径
    public int InitScore;//金币
    public float MonsterGap;//怪兽间距 

    //障碍物的位置
    public List<Point> SurroundingPoint = new List<Point>();
    public Dictionary<Point, string> Surroundings = new Dictionary<Point, string>();

    //炮塔可放置的位置
    public List<Point> Holders = new List<Point>();

    //怪物行走的路径
    public List<Point> Path = new List<Point>();

    //出怪回合信息
    //public List<KeyValuePair<string, int>> Rounds = new List<KeyValuePair<string, int>>();
    public List<Round> Rounds = new List<Round>();
    #endregion

}