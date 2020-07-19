/****************************************************
    文件：StateHit.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/19 13:35:35
	功能：受击状态
*****************************************************/

using UnityEngine;

public class StateHit : IState 
{
	public void Enter(EntityBase entity, params object[] objs)
	{
		entity.currentAnimState = AniState.Hit;
	}

	public void Process(EntityBase entity, params object[] objs)
	{
		entity.SetAction(Constants.ActionHit);
		int animTime = (int) (GetHitAnimLength(entity) * 1000);
		TimerSvc.instance.AddTimeTask((taskId) =>
		{
			entity.SetAction(Constants.ActionDefault);
			entity.Idle();
		},animTime);
	}

	public void Exit(EntityBase entity, params object[] objs)
	{

	}

	private float GetHitAnimLength(EntityBase entity)
	{
		AnimationClip[] clips = entity.controller.anim.runtimeAnimatorController.animationClips;
		for (int i = 0; i < clips.Length; i++)
		{
			string animName = clips[i].name;
			if (animName.Contains("hit")||animName.Contains("Hit")||animName.Contains("HIT"))
			{
				return clips[i].length;
			}
		}

		return 1f;
	}
}