/****************************************************
    文件：GameStart.cs
	作者：#CreateAuthor#
    邮箱: wangzeping1998@gmail.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System;
using PENet;
using Protocol;
using UnityEngine;

public class GameStart : MonoBehaviour
{
	private PENet.PESocket<ClinetSession, NetMsg> client;
	private void Start()
	{
		client = new PESocket<ClinetSession, NetMsg>();
		client.StartAsClient(IPCig.srvIP,IPCig.srvPort);
		
		client.SetLog(true, (string msg, int lv) =>
		{
			switch (lv)
			{
				case 0:
					msg = "Log" + msg;
					Debug.Log(msg);
					break;
				case 1:
					msg = "Warn" + msg;
					Debug.LogWarning(msg);
					break;
				case 2:
					msg = "Error" + msg;
					Debug.LogError(msg);
					break;
				case 3:
					msg = "Info" + msg;
					Debug.Log(msg);
					break;
			}
			
		});
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			client.session.SendMsg(new NetMsg(){text = "Hello unity!"});
		}
	}
}