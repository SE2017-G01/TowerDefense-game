using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;


public class Tools 
{
    //读取关卡列表
    public static List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Consts.LevelDir, "*.xml");

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

    //填充Level类数据
    public static Level FillLevel(string fileName)
    {
        Debug.Log("FillLevel:"+ fileName);
        //    FileInfo file = new FileInfo(fileName);
        //    StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);
        var level = new Level();
        FileInfo file = new FileInfo(fileName);
        //FileInfo file = new FileInfo(Directory.GetFiles(Consts.LevelDir, fileName + ".xml")[0]);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);

        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
<<<<<<< HEAD

        level.InitGold = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

=======
        level.CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText;
        level.InitGold = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

>>>>>>> dev
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
                    case Consts.Plate:
                        level.Holder.Add(new Point(x, y));
                        break;
                    case Consts.Start:
                        level.StartPoint = new Point(x, y);
                        break;
                    case Consts.End:
                        level.EndPoint = new Point(x, y);
                        break;
                    case "\r":
                    case "\n":
                    case "\n\r":
                        break;
                    default:
                        var p = new Point(x, y, Consts.PointTypeSurrounding);
                        break;
                }
            }

            if (x == 10)
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
                var monster = int.Parse(node.Attributes["Monster"].Value);
                var number = int.Parse(node.Attributes["Count"].Value);
                var round = new Round(monster, number);
                level.Rounds.Add(round);
            }

        #endregion

        sr.Close();
        sr.Dispose();

        return level;
    }
    //public static void FillLevel(string fileName, ref Level level)
    //{
    //    FileInfo file = new FileInfo(fileName);
    //    StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

    //    XmlDocument doc = new XmlDocument();
    //    doc.Load(sr);

    //    level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
    //    level.CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText;
    //    level.Background = doc.SelectSingleNode("/Level/Background").InnerText;
    //    level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
    //    level.InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

    //    XmlNodeList nodes;

    //    nodes = doc.SelectNodes("/Level/Holder/Point");
    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        XmlNode node = nodes[i];
    //        Point p = new Point(
    //            int.Parse(node.Attributes["X"].Value),
    //            int.Parse(node.Attributes["Y"].Value));

    //        level.Holder.Add(p);
    //    }

    //    nodes = doc.SelectNodes("/Level/Path/Point");
    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        XmlNode node = nodes[i];

    //        Point p = new Point(
    //            int.Parse(node.Attributes["X"].Value),
    //            int.Parse(node.Attributes["Y"].Value));

    //        level.Path.Add(p);
    //    }

    //    nodes = doc.SelectNodes("/Level/Rounds/Round");
    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        XmlNode node = nodes[i];

    //        Round r = new Round(
    //                int.Parse(node.Attributes["Monster"].Value),
    //                int.Parse(node.Attributes["Count"].Value)
    //            );

    //        level.Rounds.Add(r);
    //    }

    //    sr.Close();
    //    sr.Dispose();
    //}

    //保存关卡

    public static void SaveLevel(string fileName, Level level)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<Level>");

        sb.AppendLine(string.Format("<Name>{0}</Name>", level.Name));
        sb.AppendLine(string.Format("<CardImage>{0}</CardImage>", level.CardImage));
        sb.AppendLine(string.Format("<Background>{0}</Background>", level.Background));
        sb.AppendLine(string.Format("<Road>{0}</Road>", level.Road));
        sb.AppendLine(string.Format("<InitScore>{0}</InitScore>", level.InitGold));

        sb.AppendLine("<Holder>");
        for (int i = 0; i < level.Holder.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Holder[i].X, level.Holder[i].Y));
        }
        sb.AppendLine("</Holder>");

        sb.AppendLine("<Path>");
        for (int i = 0; i < level.Path.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Path[i].X, level.Path[i].Y));
        }
        sb.AppendLine("</Path>");

        sb.AppendLine("<Rounds>");
        for (int i = 0; i < level.Rounds.Count; i++)
        {
            sb.AppendLine(string.Format("<Round Monster=\"{0}\" Count=\"{1}\"/>", level.Rounds[i].Monster, level.Rounds[i].Count));
        }
        sb.AppendLine("</Rounds>");

        sb.AppendLine("</Level>");

        string content = sb.ToString();


        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.ConformanceLevel = ConformanceLevel.Auto;
        settings.IndentChars = "\t";
        settings.OmitXmlDeclaration = false;

        XmlWriter xw =XmlWriter.Create(fileName,settings);
        
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(content);
        doc.WriteTo(xw);

        xw.Flush();
        xw.Close();
    }

    //加载图片
    public static IEnumerator LoadImage(string url, SpriteRenderer render)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return www;

        Texture2D texture = www.texture;
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        render.sprite = sp;
    }

    public static IEnumerator LoadImage(string url, Image image)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return www;

        Texture2D texture = www.texture;
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = sp;
    }
}
