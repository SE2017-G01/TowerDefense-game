using UnityEngine;

public class Spawner : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public static Map m_Map = null;
    Luobo m_Luobo = null;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Spanwner; }
    }
    #endregion

    #region 方法
    //创建萝卜
    void SpawnLuobo(Vector3 position)
    {
        GameObject go = Game.Instance.ObjectPool.Spawn("Luobo");
        Luobo luobo = go.GetComponent<Luobo>();
        luobo.Position = position;
        luobo.Dead += luobo_Dead;

        m_Luobo = luobo;
    }

    //创建起点
    void SpawnStart(Vector3 position)
    {
        GameObject go = Game.Instance.ObjectPool.Spawn("Start");
        go.transform.position = position;
    }

    //创建怪物
    void SpawnMonster(int MonsterID)
    {
        Debug.Log("Spawner:SpawnMonster");
        string prefabName = "Monster" + MonsterID;
        
        for (int i = 0; i < m_Map.StartPoint.Count; i++)
        {
            GameObject go = Game.Instance.ObjectPool.Spawn(prefabName);
            Monster monster = go.GetComponent<Monster>();
            monster.Reached += monster_Reached;
            monster.HpChanged += monster_HpChanged;
            monster.Dead += monster_Dead;
            Debug.Log("!!!!" + m_Map.StartPoint[i]);
            monster.Load(m_Map.StartPoint[i]);
        }
    }

    void SpawnTower(Vector3 position, int towerID)
    {
        //找到Tile
        Tile tile = m_Map.GetTile(position);
        tile.Data=new Tile(0,0);
        if (!m_Map.CalcShortPath())
        {
            tile.Data = null;
            m_Map.CalcShortPath();
            return; //提示这个位置不能造塔
        }
        tile.Data = null;
        m_Map.CalcShortPath();
        //创建Tower
        TowerInfo info = Game.Instance.StaticData.GetTowerInfo(towerID);
        if (GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold < info.BasePrice)
        {
            return;//提示金钱不足
        }
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold -= info.BasePrice;
        GameObject go = Game.Instance.ObjectPool.Spawn(info.PrefabName);
        Tower tower = go.GetComponent<Tower>();
        tower.transform.position = position;
        tower.Load(towerID, tile, m_Map.MapRect);
        //设置Tile数据
        tile.Data = tower;
        m_Map.CalcShortPath();//计算最短路-yjc
    }

    void monster_HpChanged(int hp, int maxHp)
    {
        
    }

    void monster_Dead(Role monster)
    {
        //怪物回收
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold += monster.Price;
        Game.Instance.ObjectPool.Unspawn(monster.gameObject);
       
        //胜利条件判断
        RoundModel rm = GetModel<RoundModel>();
        GameModel gm = GetModel<GameModel>();
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0        //场景里没有怪物了
            && !m_Luobo.IsDead          //萝卜还活着
            && rm.AllRoundsComplete)    //所有怪物都已出完
        {
            //游戏胜利
            SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelID = gm.PlayLevelIndex, IsSuccess = true });
        }
    }

    void luobo_Dead(Role luobo)
    {
        //萝卜回收
        Game.Instance.ObjectPool.Unspawn(luobo.gameObject);

        //游戏结束
        GameModel gm = GetModel<GameModel>();
        SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelID = gm.PlayLevelIndex, IsSuccess = false });
    }

    void monster_Reached(Monster monster)
    {
        //萝卜掉血
        m_Luobo.Damage(1);
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold -= monster.Price;

        //怪物死亡
        monster.Hp = 0;
    }

    void map_OnTileClick(object sender, TileClickEventArgs e)
    {
        GameModel gm = GetModel<GameModel>();
        

        //游戏还未开始，那么不操作菜单
        if (!gm.IsPlaying)
            return;

        //如果有菜单显示，那么隐藏菜单
        if (TowerPopup.Instance.IsPopShow)
        {
            SendEvent(Consts.E_HidePopup);
            return;
        }

        //非放塔格子，不操作菜单
        if(!e.Tile.CanHold)
        {
            SendEvent(Consts.E_HidePopup);
            return;
        }

        if (e.Tile.Data == null)
        {
            ShowCreateArgs arg = new ShowCreateArgs()
            {
                Position = m_Map.GetPosition(e.Tile),
                UpSide = e.Tile.Y < Map.MAXX / 2
            };
            SendEvent(Consts.E_ShowCreate, arg);
        }
        else
        {
            ShowUpgradeArgs arg = new ShowUpgradeArgs()
            {
                Tower = e.Tile.Data as Tower
            };
            SendEvent(Consts.E_ShowUpgrade, arg);
        }
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_SpawnMonster);
        AttentionEvents.Add(Consts.E_SpawnTower);
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 3)
                {
                    //获取地图组件
                    m_Map = GetComponent<Map>();
                    m_Map.OnTileClick += map_OnTileClick;

                    //加载地图
                    
                    GameModel gModel = GetModel<GameModel>();
                    Debug.Log("Spawner:" + gModel.PlayLevel.Name);
                    m_Map.LoadLevel(gModel.PlayLevel);

                    //加载萝卜
                    for (int i = 0; i < m_Map.EndPoint.Count; i++)
                    {
                        Vector3 luoboPos = m_Map.GetPosition(m_Map.GetTile(m_Map.EndPoint[i]));
                        SpawnLuobo(luoboPos);
                    }
                    for (int i = 0; i < m_Map.StartPoint.Count; i++)
                    {
                        //加载起点
                        Vector3 startPos = m_Map.GetPosition(m_Map.GetTile(m_Map.StartPoint[i]));
                        SpawnStart(startPos);
                    }
                }
                break;
            case Consts.E_SpawnMonster:
                SpawnMonsterArgs e1 = data as SpawnMonsterArgs;
                SpawnMonster(e1.MonsterID);
                break;
            case Consts.E_SpawnTower:
                SpawnTowerArgs e2 = data as SpawnTowerArgs;
                SpawnTower(e2.Position, e2.TowerID);
                break;
            default:
                break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
