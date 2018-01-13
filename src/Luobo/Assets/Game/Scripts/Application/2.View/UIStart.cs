using UnityEngine;
using System.Collections;

public class UIStart : View
{
    public override string Name
    {
        get { return Consts.V_Start; }
    }

    public void OnBtnSelect()
    {
        Game.Instance.LoadScene(2);
    }

    public void OnBtnAbout()
    {
        MVC.GetView<UIInfo>().Show();
    }

    public void OnBtnQuit()
    {
        Application.Quit();
    }

    public override void HandleEvent(string eventName, object data)
    {
        
    }
}