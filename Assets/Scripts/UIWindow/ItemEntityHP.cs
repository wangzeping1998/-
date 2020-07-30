/****************************************************
    文件：ItemEntityHP.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/21 20:17:27
	功能：实体血量UI
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHP : MonoBehaviour 
{
    public Image imgHpBg;
    public Image imgHpRed;

    public Text txtCritical;
    public Animator criticalAnim;

    public Text txtDodge;
    public Animator dodgeAnim;

    public Text txtHurt;
    public Animator hurtAnim;

    private int m_hpVal;
    private Transform m_itemRoot;
    private RectTransform m_recTras;
    private float scale = Constants.ScreenStandardHeight * 1.0f/ Screen.height ;
    
    
    public void ShowCriticalAnim(int hurt)
    {
        txtCritical.text = "暴击" + hurt;
        criticalAnim.SetTrigger("show");
    }
    
    
    public void ShowDodgeAnim()
    {
        txtDodge.text = "闪避";
        dodgeAnim.SetTrigger("show");
    }

    
    public void ShowhurtHPlAnim(int hurt)
    {
        txtHurt.text = "-"+hurt;
        hurtAnim.SetTrigger("show");
    }

    public void SetHP(Transform itemRoot,int hp)
    {
        m_recTras = this.GetComponent<RectTransform>();
        m_itemRoot = itemRoot;
        m_itemRoot = itemRoot;
        m_hpVal = hp;
        imgHpRed.fillAmount = 1;
        imgHpBg.fillAmount = 1;
    }

    private float m_curtPrg;
    private float m_targetPrg;
    
    public void SetHP(int oldHp,int curtHp)
    {
        m_targetPrg = curtHp * 1.0f / m_hpVal;
        m_curtPrg = oldHp * 1.0f / m_hpVal;
        imgHpRed.fillAmount = m_targetPrg;
    }

    public void UpdateMixBlend()
    {
        if (Math.Abs(m_curtPrg-m_targetPrg)<Constants.AccelerHPBar * Time.deltaTime)
        {
            //阈值小，不使用渐变
            m_curtPrg = m_targetPrg;
        }
        else if (m_targetPrg < m_curtPrg)
        {
            m_curtPrg -= Constants.AccelerHPBar * Time.deltaTime;
        }else
        {
            m_curtPrg += Constants.AccelerHPBar * Time.deltaTime;
        }

        imgHpBg.fillAmount = m_curtPrg;
    }



    private void Update()
    {
        UpdateMixBlend();
        m_recTras.anchoredPosition = Camera.main.WorldToScreenPoint(m_itemRoot.position) * scale;
    }
}