/****************************************************
    文件：PlayerCtrlWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 15:32:56
	功能：玩家战斗界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrlWind : WindowRoot
{
    public Image imgTouch; //操作杆可显示位置
    public Image imgDirBg; //操作杆背景
    public Image imgDirPoint; //摇杆中心点

    private float m_pointDis; //根据屏幕分辨率设置的摇杆移动距离
    private bool m_menuState = true; //菜单开关动画
    private Vector2 m_startPos = Vector2.zero; //记录开始点击位置
    private Vector2 m_defaultPos = Vector2.zero; //记录轮盘默认位置

    public Vector2 CurrentDir { get; private set; }

    public Text txtLevel; //等级UI
    public Text txtName; //名称UI
    public Text txtExpPrg; //经验值UI
    public Transform expPrgTrans; //经验值条对象

    public Button btnNormalAttack;
    public Button btnReleaseSkill_1;
    public Button btnReleaseSkill_2;
    public Button btnReleaseSkill_3;
    
    protected override void InitWind()
    {
        base.InitWind();
        btnNormalAttack.onClick.AddListener(OnClicReleasNormalAttackBtn);
        btnReleaseSkill_1.onClick.AddListener(OnClickReleasSkill_1_Btn);
        btnReleaseSkill_2.onClick.AddListener(OnClickReleasSkill_2_Btn);
        btnReleaseSkill_3.onClick.AddListener(OnClickReleasSkill_3_Btn);
        
        RefreshUI();
        RegisterTouchEvts();
    }

    protected override void ClearWind()
    {
        base.ClearWind();
        btnNormalAttack.onClick.RemoveListener(OnClicReleasNormalAttackBtn);
        btnReleaseSkill_1.onClick.RemoveListener(OnClickReleasSkill_1_Btn);
        btnReleaseSkill_2.onClick.RemoveListener(OnClickReleasSkill_2_Btn);
        btnReleaseSkill_3.onClick.RemoveListener(OnClickReleasSkill_3_Btn);
    }

    public void RefreshUI()
    {
        if (GetWindowState() == false)
            return;

        PlayerData playerData = GameRoot.instance.playerData;

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
    }


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
            CurrentDir = Vector2.zero;
            BattleSys.instance.SetSelfPlayerMoveDir(Vector2.zero);
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
            CurrentDir = dir.normalized;
            BattleSys.instance.SetSelfPlayerMoveDir(dir.normalized);
        });
    }
    
    
    public void OnClicReleasNormalAttackBtn()
    {
        BattleSys.instance.ReqReleaseSkill(0);
    }
    
    public void OnClickReleasSkill_1_Btn()
    {
        BattleSys.instance.ReqReleaseSkill(1);
    }

    public void OnClickReleasSkill_2_Btn()
    {
        BattleSys.instance.ReqReleaseSkill(2);
    }

    public void OnClickReleasSkill_3_Btn()
    {
        BattleSys.instance.ReqReleaseSkill(3);
    }
}