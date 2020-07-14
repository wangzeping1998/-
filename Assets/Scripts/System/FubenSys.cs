/****************************************************
    文件：FubenSys.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/10 19:27:26
	功能：副本系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class FubenSys : SystemRoot
{
	public static FubenSys instance;
	public FubenWind fubenWind;
	public override void InitSys()
	{
		base.InitSys();
		instance = this;
	}

	public void EnterFuben()
	{
		OpenFubenWind();
	}

	#region FuBenWind
	
	public void OpenFubenWind()
	{
		fubenWind.SetWindowState();
	}
	

	#endregion

	public void RspFBFight(GameMsg msg)
	{
		RspFBFight data = msg.rspFBFight;
		GameRoot.instance.SetPlayerDataByFuben(data);
		MainCitySys.instance.maincityWind.SetWindowState(false);
		fubenWind.SetWindowState(false);
		BattleSys.instance.StartBattle(data.id);
	}
}