/****************************************************
    文件：GameRoot.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:14:38
	功能：游戏启动入口
*****************************************************/

using System;
using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
	public static GameRoot instance = null;
	
	public LoadingWind loadingWind;
	public DynamicWind dynamicWind;
	
	private PlayerData _playerData = null;

	public PlayerData playerData
	{
		get { return _playerData; }
	}

	private void Start()
	{
		instance = this;
		DontDestroyOnLoad(this);
		Debug.Log("Game Start...");
		ClearUIRoot();
		Init();
	}

	private void ClearUIRoot()
	{
		Transform canvas = transform.Find("Canvas");
		for (int i = 0; i < canvas.childCount ; i++)
		{
			canvas.GetChild(i).gameObject.SetActive(false);
		}
		dynamicWind.SetWindowState();
	}

	private void Init()
	{
		//服务模块初始化
		ResSvc res = GetComponent<ResSvc>();
		res.InitSvc();
		NetSvc net = GetComponent<NetSvc>();
		net.InitSvc();
		AudioSvc audio = GetComponent<AudioSvc>();
		audio.InitSvc();
		
		//业务系统初始化
		LoginSys login = GetComponent<LoginSys>();
		login.InitSys();
		MainCitySys mainCitySys = GetComponent<MainCitySys>();
		mainCitySys.InitSys();
		//进入登入场景并加载UI
		login.EnterLogin();
		
		
	}

	public static void AddTips(string tips)
	{
		instance.dynamicWind.AddTips(tips);
	}

	public void SetPlayerData(RspLogin msg)
	{
		this._playerData = msg.playerData;
	}

	public void SetPlayerName(string name)
	{
		this._playerData.name = name;
	}
}