/****************************************************
    文件：LoginSys.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:16:1
	功能：登入系统业务逻辑
*****************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSys : SystemRoot
{
	public static LoginSys instance;
	
	public CreateWind createWind;
	public LoginWind loginWind;

	public override void InitSys()
	{
		base.InitSys();
		instance = this;
		Debug.Log("Init LoginSys...");
	}

	/// <summary>
	/// 进入登入场景
	/// </summary>
	public void EnterLogin()
	{
		//异步加载登入场景
		//显示加载进度条
		//加载完成后打开注册登入界面
		resSvc.AsyncLoadScene(Constants.SceneLogin,OpenLoginWind);
		//播放背景音乐
		audioSvc.PlayBgMusic(Constants.BGLogin);
	}

	public void OpenLoginWind()
	{
		loginWind.SetWindowState();
	}

	public void RspLogin(GameMsg msg)
	{
		GameRoot.AddTips("登入成功");
		
		GameRoot.instance.SetPlayerData(msg.rspLogin);
		if (msg.rspLogin.playerData.name == "")
		{
			//没有名字信息，打开创建角色窗口
			createWind.SetWindowState();
		}
		else
		{
			//进入主城
			MainCitySys.instance.EnterMainCity();
		}
		
		loginWind.SetWindowState(false);
	}

	public void RspRename(GameMsg msg)
	{
		GameRoot.instance.SetPlayerName(msg.rspRename.name);
		
		MainCitySys.instance.EnterMainCity();
		
		createWind.SetWindowState(false);
	}
	
	
	
}