/****************************************************
    文件：IState.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:19:39
	功能：Nothing
*****************************************************/

public interface IState
{
	void Enter(EntityBase entity);
	void Process(EntityBase entity);
	void Exit(EntityBase entity);
}

public enum AniState
{
	None,
	Idle,
	Move
}