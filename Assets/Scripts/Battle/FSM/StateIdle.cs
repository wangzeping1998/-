/****************************************************
    文件：StateIdle.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:22:44
	功能：待机状态
*****************************************************/

using UnityEngine;

public class StateIdle : IState 
{
	public void Enter(EntityBase entity)
	{
		entity.currentAnimState = AniState.Idle;
		PECommon.Log("Enter idle state.");
	}

	public void Process(EntityBase entity)
	{
		PECommon.Log("Process idle state.");
	}

	public void Exit(EntityBase entity)
	{
		PECommon.Log("Exit idle state.");
	}
}