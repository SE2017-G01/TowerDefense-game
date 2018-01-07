using UnityEngine;
using System.Collections;

public class UpgradeIcon : MonoBehaviour
{
    SpriteRenderer m_Render;
    Tower m_Tower;

    void Awake()
    {
        m_Render = GetComponent<SpriteRenderer>();
    }

    public void Load(GameModel gm, Tower tower)
    {
        m_Tower = tower;
        
        //图标
        TowerInfo info = Game.Instance.StaticData.GetTowerInfo(tower.ID);
        /*string path = "Res/Roles/"+ (tower.IsTopLevel ? info.DisabledIcon : info.NormalIcon);
        if(GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold<tower.BasePrice*tower.Level)
        path = "Res/Roles/"+info.DisabledIcon;*/
        string path = "Res/m_picture/upgrade_";
        if (GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold <
            tower.BasePrice * tower.Level)
            path += "-";
        path = path + "180.png";
        //path= "Res/m_picture/upgrade_180.png";
        m_Render.sprite = Resources.Load<Sprite>(path);
    }

    void OnMouseDown()
    {
        if (m_Tower.IsTopLevel)
            return;
        if (GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold <
            m_Tower.BasePrice * m_Tower.Level) return;
        UpgradeTowerArgs e = new UpgradeTowerArgs()
        {
            tower = m_Tower
        };
        SendMessageUpwards("UpgradeTower", e, SendMessageOptions.DontRequireReceiver);
    }
}