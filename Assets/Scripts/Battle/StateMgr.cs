/****************************************************
    文件：StateMgr.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 21:9:33
	功能：状态机管理器
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class StateMgr : MonoBehaviour 
{
	private Dictionary<AniState,IState> fsm = new Dictionary<AniState, IState>();
	
	public void Init()
	{
		PECommon.Log("StateMgr init done.");
		fsm.Add(AniState.Born,new StateBorn());
		fsm.Add(AniState.Idle,new StateIdle());
		fsm.Add(AniState.Move,new StateMove());
		fsm.Add(AniState.Attack,new StateAttack());
		fsm.Add(AniState.Die,new StateDie());
		fsm.Add(AniState.Hit,new StateHit());
	}

	//切换状态
	public void ChangeStatus(EntityBase entity,AniState targetState,params object[] objs)
	{
		if (entity.currentAnimState == targetState)
		{
			return;
		}

		if (fsm.ContainsKey(targetState))
		{
			if (entity.currentAnimState != AniState.None)
			{
				fsm[entity.currentAnimState].Exit(entity,objs);
			}
			fsm[targetState].Enter(entity,objs);
			fsm[targetState].Process(entity,objs);

		}
	}
}