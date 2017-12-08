using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Lock : CommonSurrounding
{
    public static string Id = "Lock";

    // Use this for initialization
    void Start()
    {
        MDictionary.RegisterSurrounding(Lock.Id, this);
    }
}



  