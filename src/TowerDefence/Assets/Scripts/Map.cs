using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//用于描述一个关卡地图的状态
public class Map : Singleton<Map>
{
    #region 字段
    public int RowCount = 5;   //行数
    public int ColumnCount = 10; //列数

    float _mapWidth;//地图宽
    float _mapHeight;//地图高

    float _tileWidth;//格子宽
    float _tileHeight;//格子高

    List<Point> _mGrid = new List<Point>(); //格子集合
    List<Point> _mRoad = new List<Point>(); //路径集合
    
    public Level CurrentLevel; //当前关卡

    public bool DrawGizmos = true; //是否绘制网格
    #endregion


    #region Unity回调

    //只在运行期起作用
    void Awake()
    {
        //计算地图和格子大小
        CalculateSize();

        //创建所有的格子
        for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
                _mGrid.Add(new Point(j, i));

        Debug.Log("hello,world!");
        CurrentLevel = LevelLoader.LoadLevel("level0");
        SurroundingFactory.Instance.LoadSurroundings(CurrentLevel);
        MonsterFactory.Instance.LoadMonsters(CurrentLevel);
        MonsterFactory.Instance.Spawn("Silly");
    }

    void Update()
    {
    }

    //只在编辑器里起作用
    void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        //计算地图和格子大小
        CalculateSize();

        //绘制格子
        Gizmos.color = Color.green;
        //绘制行
        for (int row = 0; row <= RowCount; row++)
        {
            Vector2 from = new Vector2(-_mapWidth / 2, -_mapHeight / 2 + row * _tileHeight);
            Vector2 to = new Vector2(-_mapWidth / 2 + _mapWidth, -_mapHeight / 2 + row * _tileHeight);
            Gizmos.DrawLine(from, to);
        }
        //绘制列
        for (int col = 0; col <= ColumnCount; col++)
        {
            Vector2 from = new Vector2(-_mapWidth / 2 + col * _tileWidth, _mapHeight / 2);
            Vector2 to = new Vector2(-_mapWidth / 2 + col * _tileWidth, -_mapHeight / 2);
            Gizmos.DrawLine(from, to);
        }

        //绘制怪兽前进路线
    }

    #endregion


    #region 帮助方法
    //计算地图大小，格子大小
    void CalculateSize()
    {
        _mapHeight = GetComponent<Renderer>().bounds.size.y;//高度。
        _mapWidth = GetComponent<Renderer>().bounds.size.x;//宽度。

        _tileWidth = _mapWidth / ColumnCount;
        _tileHeight = _mapHeight / RowCount;

        Game.Instance.MapHeight = _mapHeight;
        Game.Instance.MapWidth = _mapWidth;
        Game.Instance.TileHeight = _tileHeight;
        Game.Instance.TileWidth = _tileWidth;
    }
    #endregion
}