using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

public class LevelLoader
{
    //读取关卡列表
    public static List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(MResources.LevelDir, "*.xml");

        List<FileInfo> list = new List<FileInfo>();
        foreach (string t in files)
        {
            FileInfo file = new FileInfo(t);
            list.Add(file);
        }
        return list;
    }

    //填充Level类数据
    public static Level LoadLevel(string fileName)
    {
        var level = new Level();
        FileInfo file = new FileInfo(Directory.GetFiles(MResources.LevelDir, fileName + ".xml")[0]);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);
        
        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
        level.InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);
        level.MonsterGap = float.Parse(doc.SelectSingleNode("/Level/MonsterGap").InnerText);

        #region 读取关卡字典
        var dicNodes = doc.SelectNodes("/Level/Dictionary/Item");
        var dictionary = new Dictionary<string, string>();
        for (var i = 0; i < dicNodes.Count; i++)
        {
            var item = dicNodes[i];
            if (item.Attributes == null) continue;
            dictionary.Add(item.Attributes["name"].Value, item.Attributes["entity"].Value);
        }
        #endregion

        #region 读取障碍物

        string surroundingStr = doc.SelectSingleNode("/Level/Holder").InnerText;
        surroundingStr = surroundingStr.Replace("\r\n", "");
        surroundingStr = surroundingStr.Replace(" ", "");
        string[] surrounding = surroundingStr.Split(',');
        int x = 0, y = 0;
        foreach (var s in surrounding)
        {
            if (dictionary.ContainsKey(s))
            {
                switch (dictionary[s])
                {
                    case MResources.Plate:
                        level.Holders.Add(new Point(x, y));
                        break;
                    case MResources.Start:
                        Game.Instance.StartPoint = new Point(x, y);
                        break;
                    case MResources.End:
                        Game.Instance.EndPoint = new Point(x, y);
                        break;
                    case "\r":
                    case "\n":
                    case "\n\r":
                        break;
                    default:
                        var p = new Point(x, y, MResources.PointTypeSurrounding);
                        level.SurroundingPoint.Add(p);
                        level.Surroundings.Add(p, dictionary[s]);
                        break;
                }
            }

            if (x == 9)
            {
                y++;
                x = 0;
            }
            else
            {
                x++;
            }
        }
        #endregion

        #region 读取回合信息
        var roundNodes = doc.SelectNodes("/Level/Rounds/Round");
        if (roundNodes != null)
            for (var i = 0; i < roundNodes.Count; i++)
            {
                var node = roundNodes[i];
                if (node.Attributes == null) continue;
                var monster = node.Attributes["Monster"].Value;
                var number = int.Parse(node.Attributes["Count"].Value);
                var round = new Round(i + 1, monster, number);
                level.Rounds.Add(round);
            }

        #endregion

        sr.Close();
        sr.Dispose();

        return level;
    }

    ////保存关卡
    //public static void SaveLevel(string fileName, Level level)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    //    sb.AppendLine("<Level>");

    //    sb.AppendLine(string.Format("<Name>{0}</Name>", level.Name));
    //    sb.AppendLine(string.Format("<Background>{0}</Background>", level.Background));
    //    sb.AppendLine(string.Format("<Road>{0}</Road>", level.Road));
    //    sb.AppendLine(string.Format("<InitScore>{0}</InitScore>", level.InitScore));

    //    sb.AppendLine("<Holder>");
    //    for (int i = 0; i < level.Holder.Count; i++)
    //    {
    //        sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Holder[i].X, level.Holder[i].Y));
    //    }
    //    sb.AppendLine("</Holder>");

    //    sb.AppendLine("<Path>");
    //    for (int i = 0; i < level.Path.Count; i++)
    //    {
    //        sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Path[i].X, level.Path[i].Y));
    //    }
    //    sb.AppendLine("</Path>");

    //    sb.AppendLine("<Rounds>");
    //    for (int i = 0; i < level.Rounds.Count; i++)
    //    {
    //        sb.AppendLine(string.Format("<MRound Monster=\"{0}\" Count=\"{1}\"/>", level.Rounds[i].Monster, level.Rounds[i].Count));
    //    }
    //    sb.AppendLine("</Rounds>");

    //    sb.AppendLine("</Level>");

    //    string content = sb.ToString();

    //    StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
    //    sw.Write(content);
    //    sw.Flush();
    //    sw.Dispose();
    //}

    ////加载图片
    //public static IEnumerator LoadImage(string url, SpriteRenderer render)
    //{
    //    WWW www = new WWW(url);

    //    while (!www.isDone)
    //        yield return www;

    //    Texture2D texture = www.texture;
    //    Sprite sp = Sprite.Create(
    //        texture,
    //        new Rect(0, 0, texture.width, texture.height),
    //        new Vector2(0.5f, 0.5f));
    //    render.sprite = sp;
    //}
}
