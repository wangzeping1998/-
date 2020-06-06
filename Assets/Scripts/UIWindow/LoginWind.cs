/****************************************************
    文件：LoginWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:42:1
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoginWind : WindowRoot
{
	public InputField iptAcct;
	public InputField iptPass;
	public Button btnEnter;
	public Button btnNotice;

	protected override void InitWind()
	{
		base.InitWind();
		btnEnter.onClick.AddListener(ClickEnterBtn);
		btnNotice.onClick.AddListener(ClickNoticeBtn);
		
		//获取本地存储的账号密码
		if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
		{
			iptAcct.text = PlayerPrefs.GetString("Acct");
			iptPass.text = PlayerPrefs.GetString("Pass");
		}
		else
		{
			iptAcct.text = "";
			iptPass.text = "";
		}
	}

	//TODO 更新本地存储的账号密码

	public void ClickEnterBtn()
	{
		audioSvc.PlayUIAudio(Constants.UILoginBtn);
		string acct = iptAcct.text;
		string pass = iptPass.text;
		if (acct != "" && pass != "")
		{
			PlayerPrefs.SetString("Acct",acct);
			PlayerPrefs.SetString("Pass",acct);
			LoginSys.instance.RspLogin();
		}
		else
		{
			GameRoot.AddTips("账号或密码为空");
		}
		//TODO 发送网络消息，请求登入
		//TODO Remove

	}

	public void ClickNoticeBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		GameRoot.AddTips("功能未开发完成");
		
	}
}