/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：网络会话连接
*****************************************************/

using PENet;
using PEProtocol;

public class ServerSession:PESession<GameMsg>
{
    public int sessionID = 0;
    //连接成功
    protected override void OnConnected()
    {
        sessionID = ServerRoot.GetSessionID();
        PECommon.Log("Client Connect");
    }

    //接受消息
    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Client CMD:" + (CMD)msg.cmd);
        NetSvc.Instance.AddMsg(new MsgPack(this,msg));
    }

    //断开连接
    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("Client DisConnect");
    }
}