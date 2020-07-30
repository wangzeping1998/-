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
		
		for (int i = 0; i < entity.actionTaskLst.Count ; i++)
		{
			TimerSvc.instance.RemoveTask(entity.actionTaskLst[i]);
		}
		
		for (int i = 0; i < entity.moveTaskLst.Count ; i++)
		{
			TimerSvc.instance.RemoveTask(entity.moveTaskLst[i]);
		}

		if (entity.skillEndCb != -1)
		{
			TimerSvc.instance.RemoveTask(entity.skillEndCb);
			entity.skillEndCb = -1;
		}
		
		//清空连招
		if (entity.nextSkillID !=0 ||entity.comboQue.Count>0)
		{
			entity.nextSkillID = 0;
			entity.comboQue.Clear();

			entity.battleMgr.lastAtkTime = 0;
			entity.battleMgr.comboIdx = 0;
		}
		
		entity.SetDir(Vector2.zero);
		entity.SetSkillMove(false);
	}

	public void Process(EntityBase entity, params object[] objs)
	{
		entity.SetAction(Constants.ActionHit);
		entity.SetIsCtrl(false);
		int animTime = (int) (GetHitAnimLength(entity) * 1000);
		TimerSvc.instance.AddTimeTask((taskId) =>
		{
			entity.SetAction(Constants.ActionDefault);
			entity.Idle();
			entity.SetIsCtrl(true);
		},animTime);
	}

	public void Exit(EntityBase entity, params object[] objs)
	{

	}

	private float GetHitAnimLength(EntityBase entity)
	{
		AnimationClip[] clips = entity.GetAnimClips();
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