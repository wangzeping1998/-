/****************************************************
    文件：LoginSys.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:16:1
	功能：登入系统业务逻辑
*****************************************************/

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

	public void RspLogin()
	{
		GameRoot.AddTips("登入成功");
		
		//打开角色创建界面
		createWind.SetWindowState();
		loginWind.SetWindowState(false);
	}
}