using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UIClear : View
{
    public override string Name
    {
        get { return Consts.V_Info; }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public override void HandleEvent(string eventName, object data)
    {

    }

    public void OnUISystemClick(object sender, BoardClickEventArgs e)
    {
        this.Hide();
    }
    void Update()
    {
        //鼠标左键检测
        if (Input.GetMouseButtonDown(0))
        {
            this.Hide();
        }
    }
}
