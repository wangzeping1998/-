/****************************************************
    文件：InfoWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/20 12:14:8
	功能：角色信息UI
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class InfoWind : WindowRoot 
{
	#region UI Define
	//角色信息窗口UI
	public RawImage imgChar;		//角色渲染Image
	public Text txtInfo;			//角色信息
	public Text txtExp;				//经验值文本
	public Image imgExpPrg;			//经验进度条
	public Text txtPower;			//体力值文本
	public Image imgPowerPrg;		//体力进度条

	public Text txtJob;				//职业
	public Text txtFight;			//战斗力
	public Text txtHP;				//血量
	public Text txtHurt;			//伤害
	public Text txtDefense;			//防御
	public Button btnClose;			//关闭按钮
	
	//详细信息窗口UI
	public Button btnDetil;			//打开详细信息按钮
	public Button btnCloseDetail;	//关闭详细信息按钮
	public Transform transDetail;	//详细信息面板
	public Text txtDtHP;			//血量
	public Text txtDtAd;			//物理攻击
	public Text txtDtAp;			//法术攻击
	public Text txtDtAdDef;			//物理防御
	public Text txtDtApDef;			//法术抗性
	public Text txtDtDodge;			//闪避
	public Text txtDtPierce;		//穿甲
	public Text txtDtCritical;		//暴击
	
	#endregion

	private Vector3 m_startPos;		//开始滑动触摸位置
	protected override void InitWind()
	{
		base.InitWind();
		btnDetil.onClick.AddListener(OnClickDetailBtn);
		btnClose.onClick.AddListener(OnClickCloseBtn);
		btnCloseDetail.onClick.AddListener(OnClickCloseDetailBtn);
		SetActive(transDetail,false);
		RegTouchEvts();
		RefreshUI();
	}
	
	//刷新（更新）UI信息
	private void RefreshUI()
	{
		PlayerData playerData = GameRoot.instance.playerData;
		SetText(txtInfo,playerData.name + "LV." + playerData.lv);
		SetText(txtExp,playerData.exp + "/" + PECommon.GetExpUpValByLv(playerData.lv));
		imgExpPrg.fillAmount = playerData.exp * 1.0f / PECommon.GetExpUpValByLv(playerData.lv);
		SetText(txtPower,playerData.power + "/" +PECommon.GetPowerLimit(playerData.lv));
		imgPowerPrg.fillAmount = playerData.power * 1.0f / PECommon.GetPowerLimit(playerData.lv);
		SetText(txtJob,"职业      暗夜刺客");
		SetText(txtFight,"战力    " + PECommon.GetFightProps(playerData));
		SetText(txtHP,"血量    " + playerData.hp);
		SetText(txtHurt,"伤害    " + playerData.ad + playerData.ap);
		SetText(txtDefense,"防御    " + playerData.addef + playerData.apdef);
		
		//详细信息面板
		SetText(txtDtHP,playerData.hp);
		SetText(txtDtAd,playerData.ad);
		SetText(txtDtAp,playerData.ap);
		SetText(txtDtAdDef,playerData.addef);
		SetText(txtDtApDef,playerData.apdef);
		SetText(txtDtDodge,playerData.dodge+ "%");
		SetText(txtDtPierce,playerData.pierce+ "%");
		SetText(txtDtCritical,playerData.critical + "%");

	}

	//注册滑动事件，旋转角色信息窗口模型
	private void RegTouchEvts()
	{
		OnClickDown(imgChar.gameObject, (evt) =>
		{
			m_startPos = evt.position;
			MainCitySys.instance.SetStartRote();
		});
		
		OnDrag(imgChar.gameObject,(evt) =>
		{
			float rote = evt.position.x - m_startPos.x;
			MainCitySys.instance.SetPlayerRote(-rote);
		});
	}
	
	//关闭角色信息窗口
	private void OnClickCloseBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		MainCitySys.instance.CloseInfoWind();
	}
	
	//打开详细信息窗口
	private void OnClickDetailBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetActive(transDetail);
	}
	
	//关闭详细信息窗口
	private void OnClickCloseDetailBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetActive(transDetail,false);
	}

	protected override void ClearWind()
	{
		base.ClearWind();
		btnDetil.onClick.RemoveListener(OnClickDetailBtn);
		btnClose.onClick.RemoveListener(OnClickCloseBtn);
		btnCloseDetail.onClick.RemoveListener(OnClickCloseDetailBtn);
	}
}