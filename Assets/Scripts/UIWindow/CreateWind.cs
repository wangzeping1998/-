/****************************************************
    文件：CreateWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 19:32:38
	功能：角色创建界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWind : WindowRoot
{
	public InputField iptName;
	public Button rdBtn;
	public Button enterBtn;
	protected override void InitWind()
	{
		base.InitWind();
		
		rdBtn.onClick.AddListener(ClickRandBtn);
		enterBtn.onClick.AddListener(ClickEnterBtn);
		//显示一个随机名字
		iptName.text = resSvc.GetRDNameData(false);
	}
	
	
	public void ClickRandBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		string rdName = resSvc.GetRDNameData(false);
		iptName.text = rdName;

	}

	public void ClickEnterBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		if (!string.IsNullOrEmpty(iptName.text))
		{
			GameMsg msg = new GameMsg()
			{
				cmd = (int) CMD.ReqRename,
				reqRename = new ReqRename()
				{
					name = iptName.text
				}
			};
			
			netSvc.SendMsg(msg);
		}
		else
		{
			GameRoot.AddTips("当前名字不符合规范");
		}
	}
}