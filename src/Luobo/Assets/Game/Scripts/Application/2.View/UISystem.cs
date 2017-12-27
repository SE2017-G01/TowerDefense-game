using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UISystem : View
{
    #region 常量
    #endregion

    #region 事件
    public Button btnResume;
    public Button btnRestart;
    public Button btnSelect;
    #endregion

    #region 字段
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Sytem; }
    }
    #endregion

    #region 方法
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data)
    {

    }

    public void OnUISystemClick(object sender, BoardClickEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            Debug.Log("UISystem:鼠标点击!currentSelectedGameObject空指针！！");
        else
        {
            Debug.Log("UISystem:鼠标点击!" + EventSystem.current.currentSelectedGameObject.name);
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "btnResume":
                    OnResumeClick();
                    break;
                case "btnRestart":
                    OnRestartClick();
                    break;
                case "btnSelect":
                    OnSelectClick();
                    break;
            }
        }
    }

    public void OnResumeClick()
    {
        GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>().UnSystemClick();
    }

    public void OnRestartClick()
    {
        //停止出怪
        GetModel<RoundModel>().StopRound();
        //停止游戏
        GetModel<GameModel>().EndLevel(false);

        StartLevelArgs e = new StartLevelArgs();
        e.LevelIndex = GetModel<GameModel>().PlayLevelIndex;
        SendEvent(Consts.E_StartLevel, e);
    }

    public void OnSelectClick()
    {
        //停止出怪
        GetModel<RoundModel>().StopRound();
        //停止游戏
        GetModel<GameModel>().EndLevel(false);

        Game.Instance.LoadScene(2);
    }
    #endregion

    #region 帮助方法
    #endregion
}
