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
	
	private Queue<GameMsg> msgPackQue = new Queue<GameMsg>();	//消息队列
	public static readonly string queLock = "lock";				//线程锁
	
	//初始化
	public void InitSvc()
	{
		instance = this;
		Debug.Log("Init NetSvc...");
		
		client = new PESocket<ClinetSession, GameMsg>();
		
		//PECommon.Log 方法回调
		//根据PECommon不同的日志类型Unity打印出对应的Log信息
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
		//连接服务器
		client.StartAsClient(ServerCfg.srvIP,ServerCfg.srvPort);
	}
	
	//添加收到的消息放到队列
	public void AddMsg(GameMsg msg)
	{
		lock (queLock)
		{
			msgPackQue.Enqueue(msg);
		}
	}

	//发送消息
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
		//每一帧从队列中提取消息进行处理
		//这里加上了线程锁，避免线程竞争
		if (msgPackQue.Count > 0)
		{
			lock (queLock)
			{
				GameMsg msg = msgPackQue.Dequeue();
				ProgressMsg(msg);
			}
		}
	}
	
	//协议处理
	private void ProgressMsg(GameMsg msg)
	{
		
		CMD cmd = (CMD)msg.cmd;
		ErrorCode err = (ErrorCode)msg.err;
		//错误码处理
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
				case ErrorCode.UpdateDBError:
					//GameRoot.AddTips("更改异常");
					PECommon.Log("数据库更新异常",LogType.Error);
					GameRoot.AddTips("网络不稳定");
					break;
				case ErrorCode.ServerDataError:
					GameRoot.AddTips("客户端数据异常");
					break;
				case ErrorCode.MaxStar:
					GameRoot.AddTips("当前部位星级已满");
					break;
				case ErrorCode.LackLevel:
					GameRoot.AddTips("等级不够");
					break;
				case ErrorCode.LackCoin:
					GameRoot.AddTips("金币不足");
					break;
				case ErrorCode.LackDiamond:
					GameRoot.AddTips("钻石不足");
					break;
				case ErrorCode.LackCrystal:
					GameRoot.AddTips("水晶不足");
					break;
			}

			switch (cmd)
			{
				case CMD.RspBuy: MainCitySys.instance.RspBuyError(msg);
					break;
			}
			
			//接收错误码后消息不进行下一步处理
			return;
		}
		
		//这是无错误码的情况下进行进一步处理
		//将消息按类型分配给响应模块处理
		switch (cmd)
		{
			case CMD.RspLogin: LoginSys.instance.RspLogin(msg);
				break;
			case CMD.RspRename: LoginSys.instance.RspRename(msg);
				break;
			case CMD.RspGuide: MainCitySys.instance.RspGuide(msg);
				break;
			case CMD.RspStrong: MainCitySys.instance.RspStrong(msg);
				break;
			case CMD.PshChat: MainCitySys.instance.PshChat(msg);
				break;
			case CMD.RspBuy: MainCitySys.instance.RspBuy(msg);
				break;
			case CMD.PshPower: MainCitySys.instance.PshPower(msg);
				break;
					
			default:
				break;
		}
		
		

	}
}