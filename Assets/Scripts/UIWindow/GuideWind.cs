/****************************************************
    文件：GuideWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/22 11:53:33
	功能：任务对话面板
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWind : WindowRoot
{
	public Text txtName;
	public Text txtTalk;
	public Image imgIcon;
	public Button btnNext;
	
	private AutoGuideCfg m_currTaskData;
	private string[] m_dialogArr;
	private int m_index;
	private PlayerData m_pd;
	protected override void InitWind()
	{
		base.InitWind();
		btnNext.onClick.AddListener(OnClickNextGuide);
		m_currTaskData = MainCitySys.instance.GetTaskData();
		m_pd = GameRoot.instance.playerData;
		m_dialogArr = m_currTaskData.dilogArr.Split('#');
		m_index = 1;
		SetTalk();
	}

	protected override void ClearWind()
	{
		base.ClearWind();
		btnNext.onClick.RemoveListener(OnClickNextGuide);
	}

	//开始对话
	private void SetTalk()
	{
		string[] talkArr = m_dialogArr[m_index].Split('|');
		if (talkArr[0] == "0")
		{
			//是自己
			SetSprite(imgIcon,PathDefine.SelfIcon);
			SetText(txtName,m_pd.name);
		}
		else
		{
			//是NPC
			switch (m_currTaskData.npcID)
			{
				case Constants.NPCWiseMan:
					SetSprite(imgIcon,PathDefine.WiseManIcon);
					SetText(txtName,"智者");
					break;
				
				case Constants.NPCGeneral:
					SetSprite(imgIcon,PathDefine.GeneralIcon);
					SetText(txtName,"将军");
					break;
				
				case Constants.NPCArtisan:
					SetSprite(imgIcon,PathDefine.ArtisanIcon);
					SetText(txtName,"工匠");
					break;
				
				case Constants.NPCTrader:
					SetSprite(imgIcon,PathDefine.TraderIcon);
					SetText(txtName,"商人");
					break;
				
				case Constants.NPCGuide:
					SetSprite(imgIcon,PathDefine.GuideIcon);
					SetText(txtName,"小芸");
					break;
			}
		}
		imgIcon.SetNativeSize();
		SetText(txtTalk,talkArr[1].Replace("$name",m_pd.name));
	}
	
	
	//点击下一个对话按钮
	private void OnClickNextGuide()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		m_index++;
		if (m_index == m_dialogArr.Length)
		{
			//对话完成 发送网络消息
			GameMsg msg = new GameMsg
			{
				cmd = (int)CMD.ReqGuide,
				reqGuide = new ReqGuide
				{
					guideId = m_currTaskData.id
				}
			};
			netSvc.SendMsg(msg);
			SetWindowState(false);
		}
		else
		{
			SetTalk();
		}

	}
}