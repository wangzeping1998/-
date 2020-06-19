/****************************************************
    文件：ClinetSession.cs
	作者：#CreateAuthor#
    邮箱: wangzeping1998@gmail.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using PENet;
using Protocol;
using UnityEngine;

public class ClinetSession : PENet.PESession<NetMsg>
{
	protected override void OnConnected()
	{
		Debug.Log("Server Connect");
	}

	protected override void OnReciveMsg(NetMsg msg)
	{
		Debug.Log(msg.text);
	}

	protected override void OnDisConnected()
	{
		Debug.Log("Server Connect");
	}
}