/****************************************************
    文件：MainCityWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/13 10:40:46
	功能：主城UI
*****************************************************/

using System.Text;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class MainCityWind : WindowRoot
{
    public Text txtFight; //战斗力UI   
    public Text txtPower; //体力值UI
    public Image imgPowerPrg; //体力值进度条UI
    public Text txtLevel; //等级UI
    public Text txtName; //名称UI
    public Text txtExpPrg; //经验值UI
    public Transform expPrgTrans; //经验值条对象
    public Button menuBtn; //菜单按钮
    public Button pageBtn; //信息面板按钮
    public Animator rootAnim; //菜单动画

    public Image imgTouch; //操作杆可显示位置
    public Image imgDirBg; //操作杆背景
    public Image imgDirPoint; //摇杆中心点

    private float m_pointDis; //根据屏幕分辨率设置的摇杆移动距离
    private bool m_menuState = true; //菜单开关动画
    private Vector2 m_startPos = Vector2.zero; //记录开始点击位置
    private Vector2 m_defaultPos = Vector2.zero; //记录轮盘默认位置

    private AutoGuideCfg m_curTaskData; //引导任务数据

    public Button btnGuide; //引导任务按钮
    public Button btnStrong; //强化界面按钮
    public Button btnChat; //聊天按钮
    public Button btnBuyPower; //购买体力按钮
    public Button btnMkCoin;
    public Button btnTask;
    public Button btnFuben;

    protected override void InitWind()
    {
        base.InitWind();
        //注册点击按钮回调
        menuBtn.onClick.AddListener(OnClickMenuBtn);
        pageBtn.onClick.AddListener(OnClickOpenPage);
        btnGuide.onClick.AddListener(OnClickGuideBtn);
        btnStrong.onClick.AddListener(OnClickStrongBtn);
        btnChat.onClick.AddListener(OnClickChatBtn);
        btnBuyPower.onClick.AddListener(OnClickBuyPowerBtn);
        btnMkCoin.onClick.AddListener(OnClickMkCoinBtn);
        btnTask.onClick.AddListener(OnClickTaskBtn);
        btnFuben.onClick.AddListener(OnClickFubenBtn);


        //初始化动画
        rootAnim.SetBool("menuState", m_menuState);
        //设置轮盘移动距离自适应
        m_pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        SetActive(imgDirPoint, false);
        m_defaultPos = imgDirBg.transform.position;
        //刷新UI数据
        RefreshUI();
        //注册点击事件
        RegisterTouchEvts();
    }

    protected override void ClearWind()
    {
        base.ClearWind();
        menuBtn.onClick.RemoveListener(OnClickMenuBtn);
        pageBtn.onClick.RemoveListener(OnClickOpenPage);
        btnGuide.onClick.RemoveListener(OnClickGuideBtn);
        btnStrong.onClick.RemoveListener(OnClickStrongBtn);
        btnChat.onClick.RemoveListener(OnClickChatBtn);
        btnTask.onClick.RemoveListener(OnClickTaskBtn);
        btnFuben.onClick.RemoveListener(OnClickFubenBtn);
    }

    //刷新UI信息
    public void RefreshUI()
    {
        if (GetWindowState() == false)
            return;

        PlayerData playerData = GameRoot.instance.playerData;
        int fight = PECommon.GetFightProps(playerData);
        int power = PECommon.GetPowerLimit(playerData.lv);

        //为UI赋值
        SetText(txtFight, fight);
        SetText(txtPower, new StringBuilder("体力:").Append(playerData.power).Append("/").Append(power).ToString());
        imgPowerPrg.fillAmount = playerData.power * 1.0f / power;
        SetText(txtLevel, playerData.lv);
        SetText(txtName, playerData.name);

        #region ExpPrg

        //经验值条自适应
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalScale = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalScale;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        //计算经验值百分比
        int expProVal = (int) (playerData.exp * 1.0f / PECommon.GetExpUpValByLv(playerData.lv) * 100);
        SetText(txtExpPrg, expProVal + "%");

        //填充经验值进度条
        int index = expProVal / 10;
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expProVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }

        #endregion

        m_curTaskData = resSvc.GetGuideCfgData(playerData.guideId);
        if (m_curTaskData != null)
        {
            SetGuideBtnIcon(m_curTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
    }


    //根据NpcID显示Npc头像
    private void SetGuideBtnIcon(int npcId)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcId)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            case -1:
                spPath = PathDefine.TaskHead;
                break;
        }

        SetSprite(img, spPath);
    }

    #region BtnEvts

    //展开菜单按钮
    private void OnClickMenuBtn()
    {
        m_menuState = !m_menuState;
        rootAnim.SetBool("menuState", m_menuState);
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);
    }

    //打开角色信息按钮
    private void OnClickOpenPage()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.instance.OpenInfoWind();
    }

    //点击引导任务按钮
    private void OnClickGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (m_curTaskData != null)
        {
            MainCitySys.instance.RunTask(m_curTaskData);
        }
        else
        {
            GameRoot.AddTips("更多的引导任务开发中,敬请期待...");
        }
    }

    //点击锻造金币按钮
    private void OnClickMkCoinBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.instance.OpenBuyWind(1);
    }

    //点击购买体力按钮
    private void OnClickBuyPowerBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.instance.OpenBuyWind(0);
    }

    //点击打开副本按钮
    private void OnClickFubenBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.instance.EnterFuben();
    }

    #endregion


    //注册摇杆触摸事件
    public void RegisterTouchEvts()
    {
        //设置轮盘移动距离自适应
        m_pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        SetActive(imgDirPoint, false);
        m_defaultPos = imgDirBg.transform.position;
        
        //按下
        OnClickDown(imgTouch.gameObject, (evt) =>
        {
            m_startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });

        //抬起
        OnClickUp(imgTouch.gameObject, (evt) =>
        {
            imgDirBg.transform.position = m_defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            MainCitySys.instance.SetMoveDir(Vector2.zero);
        });

        //拖拽
        OnDrag(imgTouch.gameObject, (evt) =>
        {
            Vector2 dir = evt.position - m_startPos;
            float len = dir.magnitude;
            if (len > m_pointDis)
            {
                dir = Vector2.ClampMagnitude(dir, m_pointDis);
            }

            imgDirPoint.transform.localPosition = dir;
            MainCitySys.instance.SetMoveDir(dir.normalized);
        });
    }

    //点击打开强化窗口按钮
    private void OnClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.instance.OpenStrongWind();
    }

    //点击打开聊天窗口按钮
    private void OnClickChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.instance.OpenChatWind();
    }

    //点击打开任务窗口按钮
    private void OnClickTaskBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.instance.OpenTaskWind();
    }
}