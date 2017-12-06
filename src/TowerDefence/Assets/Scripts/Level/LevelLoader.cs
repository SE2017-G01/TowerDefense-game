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
        FileInfo file = new FileInfo(Directory.GetFiles(MResources.LevelDir, fileName+".xml")[0]);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);

        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
        level.InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

        //关卡字典
        var dicNodes = doc.SelectNodes("/Level/Dictionary/Item");
        var dictionary = new Dictionary<string, string>();
        for (var i = 0; i < dicNodes.Count; i++)
        {
            var item = dicNodes[i];
            if (item.Attributes == null) continue;
            dictionary.Add(item.Attributes["name"].Value, item.Attributes["entity"].Value);
        }

        //读取障碍物
        string[] surrounding = doc.SelectSingleNode("/Level/Holder").InnerText.Split(',');
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
                        break;
                    case MResources.End:
                        break;
                    case "\r": case "\n": case "\n\r":
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

        //var nodes = doc.SelectNodes("/Level/Holder/Point");
        //for (int i = 0; i < nodes.Count; i++)
        //{
        //    XmlNode node = nodes[i];
        //    Point p = new Point(
        //        int.Parse(node.Attributes["X"].Value),
        //        int.Parse(node.Attributes["Y"].Value));

        //    level.Holders.Add(p);
        //}

        //nodes = doc.SelectNodes("/Level/Path/Point");
        //for (int i = 0; i < nodes.Count; i++)
        //{
        //    XmlNode node = nodes[i];

        //    Point p = new Point(
        //        int.Parse(node.Attributes["X"].Value),
        //        int.Parse(node.Attributes["Y"].Value));

        //    level.Path.Add(p);
        //}

        //nodes = doc.SelectNodes("/Level/Rounds/Round");
        //for (int i = 0; i < nodes.Count; i++)
        //{
        //    XmlNode node = nodes[i];

        //    Round r = new Round(
        //            int.Parse(node.Attributes["Monster"].Value),
        //            int.Parse(node.Attributes["Count"].Value)
        //        );

        //    level.Rounds.Add(r);
        //}

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
    //        sb.AppendLine(string.Format("<Round Monster=\"{0}\" Count=\"{1}\"/>", level.Rounds[i].Monster, level.Rounds[i].Count));
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
