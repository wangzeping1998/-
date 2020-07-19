/****************************************************
    文件：MapMgr.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 21:9:47
	功能：Nothing
*****************************************************/

using UnityEngine;

public class MapMgr : MonoBehaviour 
{
	public void Init()
	{
		PECommon.Log("MapMgr init...");
		LoadMonster(1);
	}

	public void LoadMonster(int wave)
	{
		BattleSys.instance.LoadMonster(wave);
	}
}