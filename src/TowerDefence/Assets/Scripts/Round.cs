using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Round
{
    private int id = -1;
    private string Monster; //怪物类型ID
    private int Count;   //怪物数量

    public Round(int id, string monster, int count)
    {
        this.id = id;
        this.Monster = monster;
        this.Count = count;
    }
}