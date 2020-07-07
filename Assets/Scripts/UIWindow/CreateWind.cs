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
	public InputField iptName;	//角色名称输入
	public Button rdBtn;		//随机按钮
	public Button enterBtn;		//进入游戏按钮
	protected override void InitWind()
	{
		base.InitWind();
		
		rdBtn.onClick.AddListener(OnClickRandBtn);
		enterBtn.onClick.AddListener(OnClickEnterBtn);
		//显示一个随机名字
		iptName.text = resSvc.GetRDNameData(false);
	}
	
	//随机名字按钮
	public void OnClickRandBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		string rdName = resSvc.GetRDNameData(false);
		iptName.text = rdName;

	}

	//进入游戏按钮
	public void OnClickEnterBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		//发送消息给服务器
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