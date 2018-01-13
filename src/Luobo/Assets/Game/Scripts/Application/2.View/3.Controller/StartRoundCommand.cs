using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


class StartRoundCommand : Controller
{
    public override void Execute(object data)
    {
        StartRoundArgs e = data as StartRoundArgs;
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().OnRoundStart(e);
    }
}
