/****************************************************
    文件：StateMove.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:22:58
	功能：移动状态
*****************************************************/

using UnityEngine;

public class StateMove : IState 
{
	public void Enter(EntityBase entity,params object[] objs)
	{
		entity.currentAnimState = AniState.Move;
		PECommon.Log("Enter move state.");
	}

	public void Process(EntityBase entity,params object[] objs)
	{
		PECommon.Log("Process move state.");
		entity.SetBlend(1);
	}

	public void Exit(EntityBase entity,params object[] objs)
	{
		PECommon.Log("Exit move state.");
	}
}