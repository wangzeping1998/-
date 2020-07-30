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
	
	public CharacterController ctrl;
	
	protected Dictionary<string,GameObject> fxDic = new Dictionary<string, GameObject>();

	protected AudioSource _audioSource = null;
	protected bool isMove = false;
	protected bool isSkillMove = false;
	public bool isCtrl = true;
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
		this.isCtrl = !isSkillMove;
		this.skillMoveSpeed = speed;
	}

	public virtual void SetAtkDirCamOffset(Vector2 dir)
	{
		
	}

	public virtual void SetAtkDir(Vector2 dir)
	{
		float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
		Vector3 eulerAngles = new Vector3(0, angle, 0);
		transform.localEulerAngles = eulerAngles;
	}

	public virtual AudioSource GetAudioSource()
	{
		return null;
	}
}