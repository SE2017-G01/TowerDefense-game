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
    public static List<XmlDocument> GetLevelFiles()
    {
        List<XmlDocument> list = new List<XmlDocument>();
        int i = 0;
        TextAsset text = Resources.Load<TextAsset>(Consts.LevelDir + "level" + i++);
        while (text!=null)
        {
            XmlDocument doc = new XmlDocument();
            if (text == null)
                Debug.LogError("关卡文件读取失败！！" + "level" + i);
            else
            {
                using (MemoryStream stream = new MemoryStream(text.bytes))
                {
                    doc.Load(stream);
                }
                list.Add(doc);
            }

            text = Resources.Load<TextAsset>(Consts.LevelDir + "level" + i++);
        }
  
        return list;
    }

    //填充Level类数据
    public static Level FillLevel(XmlDocument doc)
    {
        var level = new Level();

        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
        level.Background = doc.SelectSingleNode("/Level/Background").InnerText;
        level.CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText;
        level.InitGold = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

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
        int x = 0, y = 5;
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
                        level.StartPoint.Add(new Point(x, y));
                        break;
                    case Consts.End:
                        level.EndPoint.Add(new Point(x, y));
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
                y--;
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

        Debug.Log("FillLevel:" + level.Name);
        return level;
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
