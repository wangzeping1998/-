/****************************************************
    文件：StateDie.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/18 22:28:50
	功能：死亡状态
*****************************************************/

using UnityEngine;

public class StateDie : IState 
{
	public void Enter(EntityBase entity, params object[] objs)
	{
		entity.currentAnimState = AniState.Die;
	}

	public void Process(EntityBase entity, params object[] objs)
	{
		entity.SetAction(Constants.ActionDie);
		TimerSvc.instance.AddTimeTask((taskId) =>
		{
			entity.SetActive(false);
		}, Constants.DieAnimLength);
	}

	public void Exit(EntityBase entity, params object[] objs)
	{

	}
}