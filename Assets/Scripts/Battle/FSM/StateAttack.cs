/****************************************************
    文件：StateAttack.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/14 20:37:43
	功能：攻击状态
*****************************************************/


public class StateAttack : IState 
{
	public void Enter(EntityBase entity,params object[] objs)
	{
		entity.currentAnimState = AniState.Attack;
		PECommon.Log("Enter attck state.");
	}

	public void Process(EntityBase entity,params object[] objs)
	{
		PECommon.Log("Process attck state.");
		entity.AttackEffect((int)objs[0]);
		entity.AttackDamage((int)objs[0]);
		

	}

	public void Exit(EntityBase entity,params object[] objs)
	{
		PECommon.Log("Exit attck state.");
		entity.ExitCurtSkill();
		entity.SetAction(Constants.ActionDefault);

	}
}