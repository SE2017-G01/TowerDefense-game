using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CountDownCompleteCommand : Controller
{
    public override void Execute(object data)
    {
        //开始游戏
        GameModel gModel = GetModel<GameModel>();
        gModel.IsPlaying = true;

        //开始出怪
        RoundModel rModel = GetModel<RoundModel>();
        if(rModel == null)
            Debug.LogAssertion("RoundModel为空！!");
        Debug.Log("CountDownComplete!");
        rModel.StartRound();
    }
}