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
	public void Enter(EntityBase entity,params object[] objs)
	{
		entity.currentAnimState = AniState.Idle;
		entity.SetDir(Vector2.zero);
		PECommon.Log("Enter idle state.");
	}

	public void Process(EntityBase entity,params object[] objs)
	{
		PECommon.Log("Process idle state.");
		if (entity.nextSkillID != 0)
		{
			entity.Attack(entity.nextSkillID);
		}
		else
		{
			if (entity.GetMoveDir()!=Vector2.zero)
			{
				entity.Move();
				entity.SetDir(entity.GetMoveDir());
			}
			else
			{
				entity.SetBlend(0);
			}
		}
		
	}

	public void Exit(EntityBase entity,params object[] objs)
	{
		PECommon.Log("Exit idle state.");
	}
}