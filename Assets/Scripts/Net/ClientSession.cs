/****************************************************
    文件：ClientSession.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/6 20:15:43
	功能：Nothing
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;

public class ClinetSession : PESession<GameMsg>
{
	protected override void OnConnected()
	{
		PECommon.Log("Server Connect");
	}

	protected override void OnReciveMsg(GameMsg msg)
	{
		PECommon.Log("Rcv Pack" + (CMD)msg.cmd);
		NetSvc.instance.AddMsg(msg);
		
	}

	protected override void OnDisConnected()
	{
		PECommon.Log("Server Connect");
	}
}