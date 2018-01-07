using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UIInfo : View
{
    #region 常量
    #endregion

    #region 事件
    public Button btnClose;
    #endregion

    #region 字段
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Info; }
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
    }


    public void OnCloseClick()
    {
        Debug.Log("UIInfo:鼠标点击!返回！");
        this.Hide();
    }
    #endregion

    #region 帮助方法
    #endregion
}
