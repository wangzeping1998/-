/****************************************************
    文件：IState.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:19:39
	功能：状态接口
*****************************************************/

public interface IState
{
	void Enter(EntityBase entity,params object[] objs);
	void Process(EntityBase entity,params object[] objs);
	void Exit(EntityBase entity,params object[] objs);
}

public enum AniState
{
	None,
	Idle,
	Move,
	Attack
}