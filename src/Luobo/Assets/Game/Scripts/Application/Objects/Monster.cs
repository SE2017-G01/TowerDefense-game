using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Monster : Role
{
    #region 常量
    public const float CLOSED_DISTANCE = 0.1f;
    public const int RowCount = 8;  //行数
    public const int ColumnCount = 12; //列数

    public const int MAXX = 10;
    public const int MAXY = 5;
    private int[,] dir = new int[,]
    {
       {0, 1},{0, -1}, {-1,0},{1,0}
    };
    #endregion

    #region 事件
    public event Action<Monster> Reached;
    #endregion

    #region 字段
    public MonsterType MonsterType = MonsterType.Monster0;//怪物类型
    float m_MoveSpeed;//移动速度（米/秒）
    Vector3[] m_Path = null; //路径拐点
    int m_PointIndex = -1; //当前拐点索引
    bool m_IsReached = false;//是否到达终点
    private Vector3 Next ;
    
    #endregion

    #region 属性
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
        set { m_MoveSpeed = value; }
    }
    /*public int Price
    {
        get { return m_price; }
        set { m_price = value; }
    }*/
    #endregion

    #region 方法

    public void Load(Vector3[] path)
    {
        m_Path = path;
        MoveNext();
    }

    bool HasNext()
    {
        return (m_PointIndex + 1) < (m_Path.Length - 1);
    }

    void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    void MoveNext()
    {
        if (!HasNext())
            return;

        if (m_PointIndex == -1)
        {
            //刚刚出来，那就放置到起点位置
            m_PointIndex = 0;
            MoveTo(Spawner.m_Map.GetPosition(Spawner.m_Map.start));
        }
        else
        {
            //不然就指定下一个目标位置
            m_PointIndex++;
        }
    }
    #endregion

    #region Unity回调

    Tile Getbest(Tile t)
    {
        int x = t.X;
        int y = t.Y;
        Tile MX = new Tile(0,0);
        MX.distance = 999999;
        for (int k = 0; k < 4; k++)
        {
            int xx = x + dir[k, 0];
            int yy = y + dir[k, 1];
            if ((xx >= 0) && (xx <= MAXX) && (yy >= 0) && (yy <= MAXY))
            {
                //Tile e = GetTile(xx, yy);
                Tile e =Spawner.m_Map.GetTile(xx,yy);
                if (e.distance < MX.distance)
                    MX = e;
            }
        }
        //if (MX.distance>99) throw new IndexOutOfRangeException("没有合法格子");
        return MX;
    }
    void Update()
    {
        //到达了终点
        if (m_IsReached)
            return;

        //当前位置
        Vector3 pos = transform.position;
        //Tile nowpos = GetTile(pos);
        //目标位置
        //Vector3 dest = m_Path[m_PointIndex + 1];
        Map nmap=new Map();
        Tile now = nmap.GetTile(pos);

        if (m_PointIndex==0)
            Next= nmap.GetPosition(Getbest(now));
        if ((now.X == Spawner.m_Map.lastx) && (now.Y == Spawner.m_Map.lasty))
            //到达终点
        {
            m_IsReached = true;

            //触发到达终点事件
            if (Reached != null)
                Reached(this);
            return;
        }
        //计算距离
        float dis = Vector3.Distance(pos, Next);
        if (dis <= CLOSED_DISTANCE)
        {
            //到达拐点
            MoveTo(Next);
            Next = nmap.GetPosition(Getbest(now));
            if (nmap.GetTile(Next) == now) throw new IndexOutOfRangeException("???");
           
    }
        else
        {
            //移动的单位方向
            Vector3 direction = (Next - pos).normalized;

            //帧移动(米/帧 =  米/秒  * Time.deltaTime)
            transform.Translate(direction * m_MoveSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        base.OnSpawn();

        MonsterInfo info = Game.Instance.StaticData.GetMonsterInfo((int)MonsterType);
        this.MaxHp = info.Hp;
        this.Hp = info.Hp;
        this.MoveSpeed = info.MoveSpeed;
        this.Price = info.Price;
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();

        this.m_Path = null;
        this.m_PointIndex = -1;
        this.m_IsReached = false;
        this.m_MoveSpeed = 0;
        this.Reached = null;
    }
    #endregion

    #region 帮助方法
   
    #endregion
}       