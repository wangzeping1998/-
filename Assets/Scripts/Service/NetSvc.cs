/****************************************************
    文件：NetSvc.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/6 20:10:47
	功能：网络服务模块
*****************************************************/

using System.Collections.Generic;
using PENet;
using PEProtocol;
using UnityEngine;

public class NetSvc : MonoBehaviour 
{
	public static NetSvc instance = null;
	
	private PENet.PESocket<ClinetSession, GameMsg> client;
	
	private Queue<GameMsg> msgPackQue = new Queue<GameMsg>();
	public static readonly string queLock = "lock";
	
	public void InitSvc()
	{
		instance = this;
		Debug.Log("Init NetSvc...");
		
		client = new PESocket<ClinetSession, GameMsg>();
		client.SetLog(true, (string msg, int lv) => {
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
		client.StartAsClient(ServerCfg.srvIP,ServerCfg.srvPort);
	}
	
	public void AddMsg(GameMsg msg)
	{
		lock (queLock)
		{
			msgPackQue.Enqueue(msg);
		}
	}

	public void SendMsg(GameMsg msg)
	{
		if (client.session != null)
		{
			client.session.SendMsg(msg);
		}
		else
		{
			GameRoot.AddTips("服务器未连接");
			PECommon.Log("服务器未连接");
			InitSvc();
		}
	}
	
	public void Update()
	{
		if (msgPackQue.Count > 0)
		{
			lock (queLock)
			{
				GameMsg msg = msgPackQue.Dequeue();
				ProgressMsg(msg);
			}
		}
	}
	
	private void ProgressMsg(GameMsg msg)
	{
		CMD cmd = (CMD)msg.cmd;
		ErrorCode err = (ErrorCode)msg.err;
		if (err!= ErrorCode.None)
		{
			switch (err)
			{
				case ErrorCode.AcctIsOnline:
					GameRoot.AddTips("当前账号已经在线");
					break;
				case ErrorCode.WrongPass:
					GameRoot.AddTips("账号或密码错误");
					break;
				case  ErrorCode.UpdateDBError:
					//GameRoot.AddTips("更改异常");
					PECommon.Log("数据库更新异常",LogType.Error);
					GameRoot.AddTips("网络不稳定");
					break;
			}
			return;
		}
		
		switch (cmd)
		{
			case CMD.RspLogin: LoginSys.instance.RspLogin(msg);
				break;
			case CMD.RspRename: LoginSys.instance.RspRename(msg);
				break;
			default:
				break;
		}
		
		

	}
}