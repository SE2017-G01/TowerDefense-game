using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//用于描述一个关卡地图的状态
public class Map : MonoBehaviour
{
    #region 字段
    public int RowCount = 5;   //行数
    public int ColumnCount = 10; //列数

    float MapWidth;//地图宽
    float MapHeight;//地图高

    float TileWidth;//格子宽
    float TileHeight;//格子高

    List<Point> m_grid = new List<Point>(); //格子集合
    List<Point> m_road = new List<Point>(); //路径集合

    //Level m_level; //关卡数据

    public bool DrawGizmos = true; //是否绘制网格
    #endregion


    #region Unity回调

    //只在运行期起作用
    void Awake()
    {
        //计算地图和格子大小
        CalculateSize();

        //创建所有的格子
        for (int i = 0; i < RowCount; i++)
            for (int j = 0; j < ColumnCount; j++)
                m_grid.Add(new Point(j, i));

        Debug.Log("hello,world!");
        //GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        //监听鼠标点击事件
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
            Vector2 from = new Vector2(-MapWidth / 2, -MapHeight / 2 + row * TileHeight);
            Vector2 to = new Vector2(-MapWidth / 2 + MapWidth, -MapHeight / 2 + row * TileHeight);
            Gizmos.DrawLine(from, to);
        }

        //绘制列
        for (int col = 0; col <= ColumnCount; col++)
        {
            Vector2 from = new Vector2(-MapWidth / 2 + col * TileWidth, MapHeight / 2);
            Vector2 to = new Vector2(-MapWidth / 2 + col * TileWidth, -MapHeight / 2);
            Gizmos.DrawLine(from, to);
        }
    }

    #endregion


    #region 帮助方法
    //计算地图大小，格子大小
    void CalculateSize()
    {
        //Vector3 leftDown = new Vector3(0, 0);
        //Vector3 rightUp = new Vector3(1, 1);
        //Vector3 p1 = Camera.main.ViewportToWorldPoint(leftDown);
        //Vector3 p2 = Camera.main.ViewportToWorldPoint(rightUp);
        //MapWidth = (p2.x - p1.x);
        //MapHeight = (p2.y - p1.y);

        MapHeight = GetComponent<Renderer>().bounds.size.y;//高度。
        MapWidth = GetComponent<Renderer>().bounds.size.x;//宽度。

        TileWidth = MapWidth / ColumnCount;
        TileHeight = MapHeight / RowCount;
    }

    #endregion
}