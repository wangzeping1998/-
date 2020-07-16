/****************************************************
    文件：Controller.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:39:33
	功能：表现实体基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{

	public Animator anim;
	
	protected Dictionary<string,GameObject> fxDic = new Dictionary<string, GameObject>();
	
	protected bool isMove = false;
	protected bool isSkillMove = false;
	protected float skillMoveSpeed = 0;
	private Vector2 _dir = Vector2.zero;

	public Vector2 Dir
	{
		get { return _dir; }
		set
		{
			if (value == Vector2.zero)
			{
				isMove = false;
			}
			else
			{
				isMove = true;
			}

			_dir = value;
		}
	}

	protected TimerSvc timerSvc = null;

	public virtual void Init()
	{
		timerSvc = TimerSvc.instance;
	}
	
	public virtual void SetBlend(float blend)
	{
		anim.SetFloat("Blend", blend);
	}

	public virtual void SetAction(int action)
	{
		anim.SetInteger("Action",action);
	}
	

	public virtual void SetFx(string name, float destroy)
	{
		
	}

	public virtual void SetSkillMove(bool isSkillMove,float speed)
	{
		this.isSkillMove = isSkillMove;
		this.skillMoveSpeed = speed;
	}
}