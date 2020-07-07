/****************************************************
    文件：LoginWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:42:1
	功能：Nothing
*****************************************************/

using PEProtocol;
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
		btnEnter.onClick.AddListener(OnClickEnterBtn);
		btnNotice.onClick.AddListener(OnClickNoticeBtn);
		
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



	public void OnClickEnterBtn()
	{
		audioSvc.PlayUIAudio(Constants.UILoginBtn);
		string _acct = iptAcct.text;
		string _pass = iptPass.text;
		if (_acct != "" && _pass != "")
		{
			PlayerPrefs.SetString("Acct",_acct);
			PlayerPrefs.SetString("Pass",_acct);
			
			netSvc.SendMsg(new GameMsg()
			{
				cmd = (int)CMD.ReqLogin,
				reqLogin = new ReqLogin()
				{
					acct = _acct,
					pass = _pass
				}
			});
		}
		else
		{
			GameRoot.AddTips("账号或密码为空");
		}

	}

	public void OnClickNoticeBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		GameRoot.AddTips("功能未开发完成");
		
	}
}