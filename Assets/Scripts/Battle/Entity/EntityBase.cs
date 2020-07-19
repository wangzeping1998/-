/****************************************************
    文件：EntityBase.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:21:10
	功能：逻辑实体基类
*****************************************************/

using System;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
	public AniState currentAnimState = AniState.None;

	public BattleMgr battleMgr = null;
	public StateMgr stateMgr = null;
	public SkillMgr skillMgr = null;
	public Controller controller = null;

	private BattleProps m_battleProps;

	public BattleProps battleProps
	{
		get { return m_battleProps;}
		protected set { m_battleProps = value; }
	}

	private int m_hp;

	public int HP
	{
		get { return m_hp;}
		set
		{
			m_hp = value;
			if (m_hp <= 0)
			{
				PECommon.Log("Unit Death!");
			}
		}
	}

	public virtual void SetBattleProps(BattleProps battleProps)
	{
		this.m_battleProps = battleProps;
		this.m_hp = battleProps.hp;
	}
	
	public void SetBattleMgr(BattleMgr battleMgr)
	{
		this.battleMgr = battleMgr;
	}
	
	public void SetStateMgr( StateMgr stateMgr)
	{
		this.stateMgr = stateMgr;
	}
	
	public void SetSkillMgr(SkillMgr skillMgr)
	{
		this.skillMgr = skillMgr;
	}

	public void SetController(Controller controller)
	{
		this.controller = controller;
	}
	

	public virtual void SetBlend(float blend)
	{
		if (controller!=null)
		{
			controller.SetBlend(blend);
		}
	}

	public virtual void SetDir(Vector2 dir)
	{
		if (controller!=null)
		{
			controller.Dir = dir;
		}
	}

	public virtual void SetAction(int action)
	{
		if (controller != null)
		{
			controller.SetAction(action);
		}
	}

	#region 状态切换

	public void Hit()
	{
		stateMgr.ChangeStatus(this,AniState.Hit,null);
	}
	
	public void Die()
	{
		stateMgr.ChangeStatus(this,AniState.Die,null);
	}
	
	public void Born()
	{
		stateMgr.ChangeStatus(this,AniState.Born,null);
	}
	
	public void Idle()
	{
		stateMgr.ChangeStatus(this,AniState.Idle,null);
	}
	
	public void Move()
	{
		stateMgr.ChangeStatus(this,AniState.Move,null);
	}

	public void Attack(int skillId)
	{
		stateMgr.ChangeStatus(this,AniState.Attack,skillId);
	}
	
	public void AttackEffect(int id)
	{
		if (skillMgr != null)
		{
			skillMgr.AttackEffect(this,id);
		}
	}
	
	#endregion

	public void AttackDamage(int id)
	{
		if (skillMgr != null)
		{
			skillMgr.AttackDamage(this,id);
		}
	}

	public virtual void SetSkillMove(bool isSkillMove,float speed = 0)
	{
		if (controller!= null)
		{
			controller.SetSkillMove(isSkillMove,speed);
		}
	}

	public virtual void SetFx(string name,float destroy)
	{
		if (controller != null)
		{
			controller.SetFx(name, destroy);
		}
	}

	public virtual Vector2 GetMoveDir()
	{
		return Vector2.zero;
	}

	public Vector3 GetPos()
	{
		return this.controller.transform.position;
	}

	public Transform GetTrans()
	{
		return this.controller.transform;
	}
}