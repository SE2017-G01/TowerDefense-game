using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UIBoard : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段

    public Text txtGold;
    public Image imgRoundInfo;
    public Text txtCurrent;
    public Text txtTotal;
    public Image imgPauseInfo;
    public Button btnSpeed1;
    public Button btnSpeed2;
    public Button btnResume;
    public Button btnPause;
    public Button btnSystem;

    public bool onUISystem = false;

    bool m_IsPlaying = false;
    float lastSpeed = 1.0f;
    public int m_Gold = 0;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Board; }
    }

    public int Gold
    {
        get { return m_Gold; }
        set
        {
            m_Gold = value;
            //GameObject.Find("Score").GetComponent<Text>().text = value.ToString();
            txtGold.text = value.ToString();
        }
    }

    public bool IsPlaying
    {
        get { return m_IsPlaying; }
        set
        {
            m_IsPlaying = value;

            imgRoundInfo.gameObject.SetActive(value);
            imgPauseInfo.gameObject.SetActive(!value);
            btnPause.gameObject.SetActive(value);
            btnResume.gameObject.SetActive(!value);
        }
    }

    #endregion

    #region 方法
    public void UpdateRoundInfo(int currentRound, int totalRound)
    {
        txtCurrent.text = currentRound.ToString("D2");//始终保留2位整数
        txtTotal.text = totalRound.ToString();
    }
    #endregion

    #region Unity回调
    void Awake()
    {
        this.Gold = 0;
        this.IsPlaying = true;
<<<<<<< HEAD
        this.Speed = GameSpeed.One;
=======
        btnSpeed1.gameObject.SetActive(true);
        btnSpeed2.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
>>>>>>> dev
        //this.txtTotal.text = MVC.GetModel<RoundModel>().RoundTotal.ToString();
    }
    #endregion

    #region 事件回调

    public void OnBoardClick(object sender, BoardClickEventArgs e)
    {
        Vector3 pos = e.WorldPos;
<<<<<<< HEAD
        if(EventSystem.current.currentSelectedGameObject == null)
=======

        if (onUISystem)
        {
            GameObject.Find("Canvas").transform.Find("UISystem").GetComponent<UISystem>().OnUISystemClick(sender, e);
        }

        if (EventSystem.current.currentSelectedGameObject == null)
>>>>>>> dev
            Debug.Log("UIBoard:鼠标点击!currentSelectedGameObject空指针！！");
        else
        {
            Debug.Log("UIBoard:鼠标点击!" + EventSystem.current.currentSelectedGameObject.name);
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "BtnSpeed1":
                    OnSpeed1Click();
                    break;
                case "BtnSpeed2":
                    OnSpeed2Click();
                    break;
                case "BtnResume":
                    OnResumeClick();
                    break;
                case "BtnPause":
                    OnPauseClick();
                    break;
                case "BtnSystem":
                    OnSystemClick();
                    break;
            }
        }
    }

    public void OnSpeed1Click()
    {
        if (!IsPlaying) return;
        Time.timeScale = 2.0f;
        btnSpeed1.gameObject.SetActive(false);
        btnSpeed2.gameObject.SetActive(true);
    }

    public void OnSpeed2Click()
    {
        if (!IsPlaying) return;
        Time.timeScale = 1.0f;
        btnSpeed1.gameObject.SetActive(true);
        btnSpeed2.gameObject.SetActive(false);
    }

    public void OnPauseClick()
    {
        if (!IsPlaying) return;
        lastSpeed = Time.timeScale;
        IsPlaying = false;
        Time.timeScale = 0;
    }

    public void OnResumeClick()
    {
        if (IsPlaying || onUISystem) return;
        IsPlaying = true;
        Time.timeScale = lastSpeed;
    }

    public void OnRoundStart(StartRoundArgs e)
    {
        this.txtCurrent.text = e.RoundIndex < 10 ? "0" + (e.RoundIndex + 1).ToString() : (e.RoundIndex + 1).ToString();
        this.txtTotal.text = e.RoundTotal < 10 ? "0" + e.RoundTotal.ToString() : e.RoundTotal.ToString();
    }

    public void OnRoundStart(StartRoundArgs e)
    {
        this.txtCurrent.text = e.RoundIndex < 10 ? "0" + e.RoundIndex.ToString() : e.RoundIndex.ToString();
        this.txtTotal.text = e.RoundIndex < 10 ? "0" + e.RoundTotal.ToString() : e.RoundTotal.ToString();
    }

    public void OnSystemClick()
    {
        if (onUISystem) return;
        onUISystem = true;
        OnPauseClick();
        MVC.GetView<UISystem>().Show();
    }

    public void UnSystemClick()
    {
        onUISystem = false;
        OnResumeClick();
        MVC.GetView<UISystem>().Hide();
    }

    public override void RegisterEvents()
    {

    }

    public override void HandleEvent(string eventName, object data)
    {

    }
    #endregion

    #region 帮助方法
    #endregion
}