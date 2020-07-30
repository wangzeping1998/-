/****************************************************
    文件：EntityBase.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:21:10
	功能：逻辑实体基类
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase 
{
	public AniState currentAnimState = AniState.None;

	public BattleMgr battleMgr = null;
	public StateMgr stateMgr = null;
	public SkillMgr skillMgr = null;
	protected Controller controller = null;
	public EnitityType enitityType = EnitityType.None;
	public EntityState entityState = EntityState.None;

	public SkillCfg curtSkillCfg = null;

	public List<int> moveTaskLst = new List<int>();
	public List<int> actionTaskLst = new List<int>();
	public int skillEndCb = -1;
	
	private string mName;
	
	public string Name
	{
		get { return mName; }
		set { mName = value; }
	}
	
	//连招队列
	public Queue<int> comboQue = new Queue<int>();
	public int nextSkillID = 0;
	
	private BattleProps m_battleProps;
	
	
	public BattleProps battleProps
	{
		get { return m_battleProps;}
		protected set { m_battleProps = value; }
	}

	private int m_hp;

	//经过运算得到的最终血量
	public int HP
	{
		get { return m_hp;}
		set
		{
			SetHP(m_hp,value);
			m_hp = value;

			if (m_hp <= 0)
			{
				PECommon.Log("Unit Death!");
			}
		}
	}

	#region 初始化赋值

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
	
	public virtual void SetBattleProps(BattleProps battleProps)
	{
		this.m_battleProps = battleProps;
		this.m_hp = battleProps.hp;
	}

	#endregion
	

	//设置移动方向
	public virtual void SetDir(Vector2 dir)
	{
		if (controller!=null)
		{
			controller.Dir = dir;
		}
	}

	#region 设置动画
	
	
	public virtual void SetBlend(float blend)
	{
		if (controller!=null)
		{
			controller.SetBlend(blend);
		}
	}

	public virtual void SetAction(int action)
	{
		if (controller != null)
		{
			controller.SetAction(action);
		}
	}

	#endregion
	
	#region 状态切换

	//切换受击状态
	public void Hit()
	{
		stateMgr.ChangeStatus(this,AniState.Hit,null);
	}
	
	//切换死亡状态
	public void Die()
	{
		stateMgr.ChangeStatus(this,AniState.Die,null);
	}
	
	//切换出生状态
	public void Born()
	{
		stateMgr.ChangeStatus(this,AniState.Born,null);
	}
	
	//切换待机状态
	public void Idle()
	{
		stateMgr.ChangeStatus(this,AniState.Idle,null);
	}
	
	//切换移动状态
	public void Move()
	{
		stateMgr.ChangeStatus(this,AniState.Move,null);
	}

	//切换攻击状态
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

	//根据ID释放技能
	public void AttackDamage(int id)
	{
		if (skillMgr != null)
		{
			skillMgr.AttackDamage(this,id);
		}
	}
	
	//技能位移接口
	public virtual void SetSkillMove(bool isSkillMove,float speed = 0)
	{
		if (controller!= null)
		{
			controller.SetSkillMove(isSkillMove,speed);
		}
	}

	//播放粒子接口
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

	#region 获得位置与坐标属性

	public Vector3 GetPos()
	{
		return this.controller.transform.position;
	}

	public Transform GetTrans()
	{
		return this.controller.transform;
	}

	#endregion


	#region 伤害数值与动画

	public virtual void Critical(int hurt)
	{
		if (controller != null)
		{
			GameRoot.instance.dynamicWind.SetCritical(controller.name,hurt);
		}

	}

	public virtual void Dodge()
	{
		if (controller != null)
		{
			GameRoot.instance.dynamicWind.SetDodge(controller.name);
		}

	}

	public virtual void Hurt(int hurt)
	{
		if (controller != null)
		{
			GameRoot.instance.dynamicWind.SetHurt(controller.name,hurt);
		}
	}


	#endregion
	
	//设置当前血条
	public virtual void SetHP(int oldHp,int curtHp)
	{
		if (controller != null)
		{
			GameRoot.instance.dynamicWind.SetHP(Name,oldHp,curtHp);
		}
	}

	//获得所有动画片段
	public AnimationClip[] GetAnimClips()
	{
		if (controller != null)
		{
			return controller.anim.runtimeAnimatorController.animationClips;
		}

		return null;
	}

	//设置对象激活状态
	public void SetActive(bool isActive = true)
	{
		if (controller != null)
		{
			controller.gameObject.SetActive(isActive);
		}
	}

	//移除
	public virtual void Remove()
	{
		
	}

	public virtual AniState GetCurrentState()
	{
		return currentAnimState;
	}

	public void ExitCurtSkill()
	{

		if (!curtSkillCfg.isBreak)
		{
			entityState = EntityState.None;
		}
		if (comboQue.Count > 0)
		{
			 nextSkillID = comboQue.Dequeue();
		}
		else
		{
			nextSkillID = 0;
		}

	}

	public void SetIsCtrl(bool isCtrl)
	{
		this.controller.isCtrl = isCtrl;
	}

	public void SetAtkDir(Vector2 dir,bool isCamOffset = true)
	{
		if (controller != null)
		{
			if (isCamOffset)
			{
				this.controller.SetAtkDirCamOffset(dir);
			}
			else
			{
				this.controller.SetAtkDir(dir);
			}
			
		}
	}

	public virtual Vector2 CalcTargetDir()
	{
		return Vector2.zero;
	}
	
	public virtual void TickAILogic(){}

	public virtual AudioSource GetAudioSource()
	{
		return controller.GetAudioSource();
	}

	public virtual void AddMoveTask(int taskId)
	{
		moveTaskLst.Add(taskId);
	}

	public virtual void AddActionTask(int taskId)
	{
		actionTaskLst.Add(taskId);
	}

	public virtual void RemoveMoveTask(int taskId)
	{
		for (int i = 0; i < moveTaskLst.Count; i++)
		{
			if (moveTaskLst[i] == taskId)
			{
				moveTaskLst.Remove(taskId);
				break;
				
			}
		}
	}
	
	public virtual void RemoveActionTask(int taskId)
	{
		for (int i = 0; i < actionTaskLst.Count; i++)
		{
			if (actionTaskLst[i] == taskId)
			{
				actionTaskLst.Remove(taskId);
				break;
			}
		}
	}

	public virtual bool GetBreakState()
	{
		return true;
	}
}