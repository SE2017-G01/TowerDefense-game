using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//鼠标点击参数类
public class TileClickEventArgs : EventArgs
{
    public int MouseButton; //0左键，1右键
    public Tile Tile;

    public TileClickEventArgs(int mouseButton, Tile tile)
    {
        this.MouseButton = mouseButton;
        this.Tile = tile;
    }
}

public class BoardClickEventArgs : EventArgs
{
    public int MouseButton; //0左键，1右键
    public Vector3 WorldPos;

    public BoardClickEventArgs(int mouseButton, Vector3 worldPos)
    {
        this.MouseButton = mouseButton;
        this.WorldPos = worldPos;
    }
}

//用于描述一个关卡地图的状态
public class Map : MonoBehaviour
{
    #region 常量
    public const int MAXX = 11;
    public const int MAXY = 6;
    private int[,] dir = new int[,]
    {
        {-1, 0}, {0, 1}, {1, 0}, {0, -1}
    };
    #endregion

    #region 事件
    public event EventHandler<TileClickEventArgs> OnTileClick;
    public event EventHandler<BoardClickEventArgs> OnBoardClick;
    #endregion

    #region 字段
    float MapWidth;//地图宽
    float MapHeight;//地图高
    public int lastx, lasty, startx, starty;
    public Tile last = new Tile(0, 0);
    public Tile start = new Tile(0, 0);
    float TileWidth;//格子宽
    float TileHeight;//格子高

    Level m_level; //关卡数据
    public static List<Tile> m_grid = new List<Tile>(); //格子集合
    List<Tile> m_road = new List<Tile>(); //路径集合
    public bool DrawGizmos = true; //是否绘制网格
    #endregion

    #region 属性

    public Level Level
    {
        get { return m_level; }
    }

    public string BackgroundImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Background").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, render));
        }
    }

    public string RoadImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Road").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, render));
        }
    }

    public Rect MapRect
    {
        get { return new Rect(-MapWidth / 2, -MapHeight / 2, MapWidth, MapHeight); }
    }

    public List<Tile> Grid
    {
        get { return m_grid; }
    }

    public List<Tile> Road
    {
        get { return m_road; }
    }

    //怪物的寻路路径
    public Vector3[] Path
    {
        get
        {
            List<Vector3> m_path = new List<Vector3>();
            for (int i = 0; i < m_road.Count; i++)
            {
                Tile t = m_road[i];
                Vector3 point = GetPosition(t);
                m_path.Add(point);
            }
            return m_path.ToArray();
        }
    }

    #endregion

    #region 方法
    public void LoadLevel(Level level)
    {
        //清除当前状态
        Clear();
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold += level.InitGold;
        //保存
        this.m_level = level;
        //加载图片
        this.BackgroundImage = "file://" + Consts.MapDir + "/" + level.Background;
        this.RoadImage = "file://" + Consts.MapDir + "/" + level.Road;

        //寻路点
        for (int i = 0; i < level.Path.Count; i++)
        {
            Point p = level.Path[i];
            Tile t = GetTile(p.X, p.Y);
            m_road.Add(t);
        }

        //起点和终点的加载
        if (level == null)
        {
            lastx = 10;
            lasty = 3;
            startx = 1;
            starty = 4;
            Debug.LogWarning("Map.cs：Level为空！！");
        }
        else if (level.StartPoint == null)
        {
            lastx = 10;
            lasty = 3;
            startx = 1;
            starty = 4;
            Debug.LogWarning("Map.cs：StartPoint为空！！" + level.Holder.Count + " " + level.Rounds.Count);
        }
        else
        {
            lastx = level.EndPoint.X;
            lasty = level.EndPoint.Y;
            startx = level.StartPoint.X;
            starty = level.StartPoint.Y;
        }
        start.X = startx;
        start.Y = starty;
        last.X = lastx;
        last.Y = lasty;
        //炮塔点
        for (int x = 0; x < MAXX; x++)
            for (int y = 0; y < MAXY ; y++)
            {
                Tile t = GetTile(x, y);
                t.CanHold = true;
                t.distance = 999;
                t.Data = null;
            }
        GetTile(lastx, lasty).CanHold = false;
        GetTile(startx, starty).CanHold = false;


        CalcShortPath();
    }

    //清除塔位信息
    public void ClearHolder()
    {
        foreach (Tile t in m_grid)
        {
            t.Data = null;
            t.CanHold = false;
        }
    }

    //清除寻路格子集合
    public void ClearRoad()
    {
        m_road.Clear();
    }

    //清除所有信息
    public void Clear()
    {
        m_level = null;
        ClearHolder();
        ClearRoad();
    }

    #endregion

    #region Unity回调
    //只在运行期起作用
    void Awake()
    {
        Debug.Log("hello,world!");
        //计算地图和格子大小
        CalculateSize();

        //创建所有的格子
        for (int i = 0; i < MAXY; i++)
            for (int j = 0; j < MAXX; j++)
                m_grid.Add(new Tile(j, i));

        //监听鼠标点击事件
        OnTileClick += Map_OnTileClick;
        OnBoardClick += Map_OnBoardClick;
    }

    void Update()
    {
        //鼠标左键检测
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("当前触摸在UI上");

                BoardClickEventArgs e = new BoardClickEventArgs(0, GetWorldPosition());
                if (OnBoardClick != null)
                    OnBoardClick(this, e);

            }
            else
            {
                Debug.Log("当前没有触摸在UI上");
                try
                {
                    Tile t = GetTileUnderMouse();
                    if (t != null)
                    {
                        //触发鼠标左键点击事件
                        TileClickEventArgs e = new TileClickEventArgs(0, t);
                        if (OnTileClick != null)
                            OnTileClick(this, e);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Debug.Log("格子越界！！！");
                }
            }

        }

        //鼠标右键检测
        if (Input.GetMouseButtonDown(1))
        {
            Tile t = GetTileUnderMouse();
            if (t != null)
            {
                //触发鼠标右键点击事件
                TileClickEventArgs e = new TileClickEventArgs(1, t);
                if (OnTileClick != null)
                    OnTileClick(this, e);
            }
        }
    }

    //只在编辑器里起作用
    void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        //计算地图和格子大小
        CalculateSize();
        Debug.Log("hello,world!");
        //绘制格子
        Gizmos.color = Color.green;
        //绘制行
        for (int row = 0; row <= MAXY; row++)
        {
            Vector2 from = new Vector2(-MapWidth / 2, -MapHeight / 2 + row * TileHeight);
            Vector2 to = new Vector2(-MapWidth / 2 + MapWidth, -MapHeight / 2 + row * TileHeight);
            Gizmos.DrawLine(from, to);
        }
        //绘制列
        for (int col = 0; col <= MAXX; col++)
        {
            Vector2 from = new Vector2(-MapWidth / 2 + col * TileWidth, MapHeight / 2);
            Vector2 to = new Vector2(-MapWidth / 2 + col * TileWidth, -MapHeight / 2);
            Gizmos.DrawLine(from, to);
        }

        //绘制怪兽前进路线
    }

    #endregion

    #region 事件回调

    void Map_OnTileClick(object sender, TileClickEventArgs e)
    {
        ////当前场景不是LevelBuilder不能编辑
        //if (gameObject.scene.name != "LevelBuilder")
        //    return;

        //if (Level == null)
        //    return;

        ////处理放塔操作
        //if (e.MouseButton == 0 && !m_road.Contains(e.Tile))
        //{
        //    e.Tile.CanHold = !e.Tile.CanHold;
        //}

        ////处理寻路点操作
        //if (e.MouseButton == 1 && !e.Tile.CanHold)
        //{
        //    if (m_road.Contains(e.Tile))
        //        m_road.Remove(e.Tile);
        //    else
        //        m_road.Add(e.Tile);
        //}
        //CalcShortPath();
    }

    void Map_OnBoardClick(object sender, BoardClickEventArgs e)
    {
        Vector3 pos = e.WorldPos;
        //Debug.Log("Map:鼠标点击!" + pos.x + "," + pos.y + " " + EventSystem.current.currentSelectedGameObject.name);
        GameObject.Find("UIBoard").GetComponent<UIBoard>().OnBoardClick(sender, e);
    }

    #endregion

    #region 帮助方法

    public Boolean CalcShortPath()
    {
        //Queue<Tile> g;
        for (int x = 0; x < MAXX; x++)
        for (int y = 0; y < MAXY; y++)
        {
            GetTile(x, y).distance = 100;
        }
        List<Tile> queue = new List<Tile>();
        Tile[] g = new Tile[100];
        int l = 0;
        int r = 1;
        g[1] = GetTile(lastx, lasty);
        g[1].distance = 0;
        while (l < r)
        {
            Tile t = g[++l];
            int x = t.X;
            int y = t.Y;
            for (int k = 0; k < 4; k++)
            {
                int xx = x + dir[k, 0];
                int yy = y + dir[k, 1];
                if ((xx >= 0) && (xx < MAXX) && (yy >= 0) && (yy < MAXY))
                {
                    Tile e = GetTile(xx, yy);
                    if (e.Data == null)
                        if (t.distance + 1 < e.distance)
                        {
                            e.distance = t.distance + 1;
                            g[++r] = e;
                        }
                }
            }
        }
        if (GetTile(startx, starty).distance >= 100) return false;
        for (int x = 0; x < MAXX; x++)
        for (int y = 0; y < MAXY; y++)
            if (GetTile(x, y).Data==null)
                if (GetTile(x, y).distance == 100)
                    return false;
        return true;
    }

    //计算地图大小，格子大小
    void CalculateSize()
    {
        GameObject plate = GameObject.Find("Road");
        if (plate != null)
        {
            MapHeight = plate.GetComponent<Renderer>().bounds.size.y;//高度。
            MapWidth = plate.GetComponent<Renderer>().bounds.size.x;//宽度。

            TileWidth = MapWidth / MAXX;
            TileHeight = MapHeight / MAXY;

            Game.Instance.TileWidth = TileWidth;
            Game.Instance.TileHeight = TileHeight;

        }
        else
        {
            Debug.LogError("游戏场地加载失败！");
        }
    }

    public Vector3 GetPosition(Tile t)
    {
        var tileWidth = Game.Instance.TileWidth;
        var tileHeight = Game.Instance.TileHeight;
        var result = new Vector3(-(MAXX / 2.0f - 0.5f) * tileWidth + t.X * tileWidth,
            -(MAXY / 2.0f - 0.5f) * tileHeight + t.Y * tileHeight, 0);
        return result;
    }

    //根据格子索引号获得格子
    public Tile GetTile(int tileX, int tileY)
    {
        int index = tileX + tileY * MAXX;
        if (index < 0 || index >= m_grid.Count)
            throw new IndexOutOfRangeException("格子索引越界!  x:" + tileX + " y:" + tileY + " index:" + index);
        return m_grid[index];
    }

    //获取所在位置获得格子
    public Tile GetTile(Vector3 position)
    {
        CalculateSize();
        int tileX = (int)((MapWidth / 2 + position.x) / TileWidth);
        int tileY = (int)((MapHeight / 2 + position.y) / TileHeight);
        return GetTile(tileX, tileY);
    }

    //获取鼠标下面的格子
    Tile GetTileUnderMouse()
    {
        Vector2 wordPos = GetWorldPosition();
        return GetTile(wordPos);
    }

    //获取鼠标所在位置的世界坐标
    Vector3 GetWorldPosition()
    {
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }
    #endregion
}