/****************************************************
    文件：BuyWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/5 21:45:44
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class BuyWind : WindowRoot
{
	private int buyType; //0 体力  1金币
	public Text txtInfo;
	public Button btnSure;
	public Button btnClose;
	protected override void InitWind()
	{
		base.InitWind();
		SetInteractable(btnSure,true);
		SetInteractable(btnClose,true);
		btnSure.onClick.AddListener(OnClickSureBtn);
		btnClose.onClick.AddListener(OnClickCloseBtn);
		RefreshUI();
	}


	
	public void SetBuyType(int type)
	{
		this.buyType = type;
	}
	private void RefreshUI()
	{
		switch (this.buyType)
		{
			case 0:
				//购买体力
				txtInfo.text = string.Format("是否花费<color=red>{0}钻石</color>购买<color=yellow>{1}体力</color>吗？", 10, 100);
				break;
			
			case 1:
				//购买金币
				txtInfo.text = string.Format("是否花费<color=red>{0}钻石</color>购买<color=yellow>{1}金币</color>吗？", 10, 1000);
				break;
		}
	}

	public void OnClickSureBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		//发送网络消息
		GameMsg msg = new GameMsg
		{
			cmd = (int) CMD.ReqBuy,
			reqBuy = new ReqBuy()
			{
				diamond = 10,
				type = buyType,
			}
		};
		netSvc.SendMsg(msg);
		SetInteractable(btnSure,false);
		SetInteractable(btnClose,false);
	}

	public void OnClickCloseBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetWindowState(false);
	}
	
	protected override void ClearWind()
	{
		base.ClearWind();
		btnSure.onClick.RemoveListener(OnClickSureBtn);
		btnClose.onClick.RemoveListener(OnClickCloseBtn);
	}
}