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
    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;
    public Button menuBtn;
    public Animator rootAnim;
    public Image imgTouch;    //操作杆可显示位置
    public Image imgDirBg;    //操作杆背景
    public Image imgDirPoint;    //摇杆中心点
    private float m_pointDis;    //根据屏幕分辨率设置的摇杆移动距离
    private bool m_menuState = true;    //菜单开关动画
    private Vector2 m_startPos = Vector2.zero;    //记录开始点击位置
    private Vector2 m_defaultPos = Vector2.zero;    //记录轮盘默认位置
    protected override void InitWind()
    {
        base.InitWind();
        menuBtn.onClick.AddListener(OnMenuBtn);
        rootAnim.SetBool("menuState",m_menuState);
        m_pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        RegisterTouchEvts();
        SetActive(imgDirPoint, false);
        m_defaultPos = imgDirBg.transform.position;
        RefreshUI();
    }

    private void RefreshUI()
    {
        //自适应
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalScale = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalScale;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);


        PlayerData playerData = GameRoot.instance.playerData;
        int fight = PECommon.GetFightProps(playerData);
        int power = PECommon.GetPowerLimit(playerData.lv);
        SetText(txtFight, fight);
        SetText(txtPower, new StringBuilder("体力:").Append(playerData.power).Append("/").Append(power).ToString());
        imgPowerPrg.fillAmount = playerData.power * 1.0f / power;
        SetText(txtLevel, playerData.lv);
        SetText(txtName, playerData.name);

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
    }

    private void OnMenuBtn()
    {
        m_menuState = !m_menuState;
        rootAnim.SetBool("menuState",m_menuState);
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);
    }

    public void RegisterTouchEvts()
    {
;
        OnClickDown(imgTouch.gameObject, (evt) =>
        {
            m_startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });
        
        OnClickUp(imgTouch.gameObject, (evt) =>
        {
            imgDirBg.transform.position = m_defaultPos;
            SetActive(imgDirPoint,false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            MainCitySys.instance.SetMoveDir(Vector2.zero);
        });
        
        OnDrag(imgTouch.gameObject,(evt) =>
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
}