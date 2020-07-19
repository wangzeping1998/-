/****************************************************
    文件：StateBorn.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/18 22:13:22
	功能：出生状态
*****************************************************/

using UnityEngine;

public class StateBorn : IState 
{
	public void Enter(EntityBase entity, params object[] objs)
	{
		entity.currentAnimState = AniState.Born;
	}

	public void Process(EntityBase entity, params object[] objs)
	{
		entity.SetAction(Constants.ActionBorn);
		TimerSvc.instance.AddTimeTask((taskId) =>
		{
			entity.SetAction(Constants.ActionDefault);
		}, 500);
	}

	public void Exit(EntityBase entity, params object[] objs)
	{

	}
}