using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MDictionary
{
    private static Dictionary<string, CommonEvent> _events = new Dictionary<string, CommonEvent>();
    private static readonly Dictionary<string, CommonSurrounding> Surroundings = new Dictionary<string, CommonSurrounding>();

    public static void RegisterSurrounding(string name, CommonSurrounding obj)
    {
        Surroundings.Add(name, obj);
    }
    public static CommonSurrounding GetSurrounding(string name)
    {
        return !Surroundings.ContainsKey(name) ? null : Surroundings[name];
    }
    public static bool ContainSurrounding(string name)
    {
        return Surroundings.ContainsKey(name);
    }
}
