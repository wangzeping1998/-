/****************************************************
    文件：MainCitySys.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/13 10:41:13
	功能：出城系统
*****************************************************/

using UnityEngine;

public class MainCitySys : SystemRoot 
{
	public static MainCitySys instance;

	public MainCityWind maincityWind;
	private PlayerController m_playerCtrl;
	public override void InitSys()
	{
		base.InitSys();
		instance = this;
		Debug.Log("Init MainCitySys...");
	}

	public void EnterMainCity()
	{
		MapCfg mapData = resSvc.GetMapCfgData(Constants.mainCityID);
		resSvc.AsyncLoadScene(mapData.sceneName, () =>
		{
			PECommon.Log("Enter MainCity");
			//加载主角
			LoadPlayer(mapData);
			//打开主城UI
			maincityWind.SetWindowState();
			//播放主题音乐
			audioSvc.PlayBgMusic(Constants.BGMainCity);
		});
	}

	public void LoadPlayer(MapCfg mapData)
	{
		GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab,true);
		player.transform.position = mapData.playerBornPos;
		player.transform.localEulerAngles = mapData.playerBornRote;
		player.transform.localScale = Vector3.one * 1.5f;
		//相机初始化
		Camera.main.transform.position = mapData.mainCamPos;
		Camera.main.transform.localEulerAngles = mapData.mainCamRote;

		m_playerCtrl = player.GetComponent<PlayerController>();
		m_playerCtrl.Init();
	}

	public void SetMoveDir(Vector2 dir)
	{
		if (dir == Vector2.zero)
		{
			m_playerCtrl.SetBlend(Constants.BlendIdle);
		}
		else
		{
			m_playerCtrl.SetBlend(Constants.BlendWalk);
		}

		m_playerCtrl.Dir = dir;
	}
}