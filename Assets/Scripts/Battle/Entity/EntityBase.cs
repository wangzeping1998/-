/****************************************************
    文件：EntityBase.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:21:10
	功能：逻辑实体基类
*****************************************************/

using UnityEngine;

public class EntityBase : MonoBehaviour
{
	public AniState currentAnimState = AniState.None;

	public StateMgr stateMgr = null;
	public Controller controller = null;

	public void SetStateMgr( StateMgr stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void SetController(Controller controller)
	{
		this.controller = controller;
	}
	
	public void Idle()
	{
		stateMgr.ChangeStatus(this,AniState.Idle);
	}
	
	public void Move()
	{
		stateMgr.ChangeStatus(this,AniState.Move);
	}
}