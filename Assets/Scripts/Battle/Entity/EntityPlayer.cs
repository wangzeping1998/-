/****************************************************
    文件：EntityPlayer.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:34:54
	功能：Nothing
*****************************************************/

using UnityEngine;

public class EntityPlayer : EntityBase 
{
	public override Vector2 GetMoveDir()
	{
		return battleMgr.GetInputDir();
	}
}