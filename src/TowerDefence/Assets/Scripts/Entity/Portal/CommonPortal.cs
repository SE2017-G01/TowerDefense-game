using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CommonPortal : CommonEntity
{
    #region 字段
    float monsterGap; //怪兽间距
    private CommonMonster lastMonster;//上一只怪兽
    #endregion

    #region Unity回调函数

    // Use this for initialization
    void Start()
    {
        monsterGap = Map.Instance.CurrentLevel.MonsterGap;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.GetComponent<Transform>().position, lastMonster.GetComponent<Transform>().position) <
            monsterGap)   return;

        //MonsterFactory.Instance.Spawn(Map.Instance.CurrentLevel);
    }

    #endregion
}
