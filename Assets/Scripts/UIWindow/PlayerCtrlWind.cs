/****************************************************
    文件：PlayerCtrlWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 15:32:56
	功能：玩家战斗界面
*****************************************************/

using System;
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

        sk1CDTime = resSvc.GetSkillCfg(101).cdTime /1000.0f;
        sk2CDTime = resSvc.GetSkillCfg(102).cdTime /1000.0f;
        sk3CDTime = resSvc.GetSkillCfg(103).cdTime /1000.0f;
        
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
    
    
    
    //普通攻击按钮
    public void OnClicReleasNormalAttackBtn()
    {
        BattleSys.instance.ReqReleaseSkill(0);
    }

    #region 技能处理
    
    private void Update()
    {
        float delta = Time.deltaTime;
        Sk1CDHandle(delta);
        Sk2CDHandle(delta);
        Sk3CDHandle(delta);

        if (Input.GetKeyDown(KeyCode.J))
        {
            OnClicReleasNormalAttackBtn();
        }
        
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            OnClickReleasSkill_1_Btn();
        }
        
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnClickReleasSkill_2_Btn();
        }
    }

    public void Sk1CDHandle(float delta)
    {
        if (isSk1CD)
        {
            sk1CDFill += delta;
            if (sk1CDFill >= sk1CDTime)
            {
                isSk1CD = false;
                SetActive(imgSk1CDFill,false);
                SetActive(txtSk1CD,false);
                sk1CDFill = 0;
            }
            else
            {
                imgSk1CDFill.fillAmount = 1 - sk1CDFill / sk1CDTime;
            }

            sk1Count += delta;
            if (sk1Count >= 1)
            {
                sk1Count -= 1;
                sk1Num--;
                SetText(txtSk1CD,sk1Num);
            }
        }
    }
    public void Sk2CDHandle(float delta)
    {
        if (isSk2CD)
        {
            sk2CDFill += delta;
            if (sk2CDFill >= sk2CDTime)
            {
                isSk2CD = false;
                SetActive(imgSk2CDFill,false);
                SetActive(txtSk2CD,false);
                sk2CDFill = 0;
            }
            else
            {
                imgSk2CDFill.fillAmount = 1 - sk2CDFill / sk2CDTime;
            }

            sk2Count += delta;
            if (sk2Count >= 1)
            {
                sk2Count -= 1;
                sk2Num--;
                SetText(txtSk2CD,sk2Num);
            }
        }
    }
    public void Sk3CDHandle(float delta)
    {
        if (isSk3CD)
        {
            sk3CDFill += delta;
            if (sk3CDFill >= sk3CDTime)
            {
                isSk3CD = false;
                SetActive(imgSk3CDFill,false);
                SetActive(txtSk3CD,false);
                sk3CDFill = 0;
            }
            else
            {
                imgSk3CDFill.fillAmount = 1 - sk3CDFill / sk3CDTime;
            }

            sk3Count += delta;
            if (sk3Count >= 1)
            {
                sk3Count -= 1;
                sk3Num--;
                SetText(txtSk3CD,sk3Num);
            }
        }
    }

    public Image imgSk1CDFill;
    public Text txtSk1CD;
    
    private bool isSk1CD = false;
    private float sk1CDTime;
    private float sk1CDFill;
    private int sk1Num;
    private float sk1Count;
    
    public void OnClickReleasSkill_1_Btn()
    {
        AniState curtState = BattleSys.instance.GetPlayerCurrentState();
        if (curtState == AniState.Attack)
        {
            return;
        }
        if (isSk1CD == false)
        {
            BattleSys.instance.ReqReleaseSkill(1);
            isSk1CD = true;
            SetActive(imgSk1CDFill);
            SetActive(txtSk1CD);
            imgSk1CDFill.fillAmount = 1;
            sk1Num = (int) sk1CDTime;
            SetText(txtSk1CD,sk1Num);
        }

    }

    
    public Image imgSk2CDFill;
    public Text txtSk2CD;
    
    private bool isSk2CD = false;
    private float sk2CDTime;
    private float sk2CDFill;
    private int sk2Num;
    private float sk2Count;

    public void OnClickReleasSkill_2_Btn()
    {
        AniState curtState = BattleSys.instance.GetPlayerCurrentState();
        if (curtState == AniState.Attack)
        {
            return;
        }
        
        if (isSk2CD == false)
        {
            BattleSys.instance.ReqReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2CDFill);
            SetActive(txtSk2CD);
            imgSk2CDFill.fillAmount = 1;
            sk2Num = (int) sk2CDTime;
            SetText(txtSk2CD,sk2Num);
        }
    }

    public Image imgSk3CDFill;
    public Text txtSk3CD;
    
    private bool isSk3CD = false;
    private float sk3CDTime;
    private float sk3CDFill;
    private int sk3Num;
    private float sk3Count;
    public void OnClickReleasSkill_3_Btn()
    {
        AniState curtState = BattleSys.instance.GetPlayerCurrentState();
        if (curtState == AniState.Attack)
        {
            return;
        }
        
        if (isSk3CD == false)
        {
            BattleSys.instance.ReqReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3CDFill);
            SetActive(txtSk3CD);
            imgSk3CDFill.fillAmount = 1;
            sk3Num = (int) sk3CDTime;
            SetText(txtSk3CD,sk3Num);
        }
    }
    
    #endregion
}