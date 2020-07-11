/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：聊天系统
*****************************************************/

using PENet;
using PEProtocol;
using System;
using System.Collections.Generic;

class ChatSys
{
    private static ChatSys instance;
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;


    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("ChatSys Init Done.");
    }

    public void SndChat(MsgPack pack)
    {
        SndChat data = pack.msg.sndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        //更新任务进度
        TaskSys.Instance.CalcTaskPrgs(pd, 6);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat
            {
                name = pd.name,
                chat = data.chat
            }
        };

        List<ServerSession> lst = cacheSvc.GetOnlineServerSessions();

        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for (int i = 0; i < lst.Count; i++)
        {
            lst[i].SendMsg(bytes);
        }
    }
}