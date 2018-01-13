using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UpgradeTowerCommand : Controller
{
    public override void Execute(object data)
    {
        UpgradeTowerArgs e = data as UpgradeTowerArgs;
        Tower tower = e.tower;
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().Gold -=
            tower.BasePrice * tower.Level;
        tower.Level++;
    }
}