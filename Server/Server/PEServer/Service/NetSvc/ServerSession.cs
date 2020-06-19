/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：网络会话连接
*****************************************************/

using PENet;
using PEProtocol;

class ServerSession:PESession<GameMsg>
{
    public int sessionID = 0;
    protected override void OnConnected()
    {
        sessionID = ServerRoot.GetSessionID();
        PECommon.Log("Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Client CMD:" + (CMD)msg.cmd);
        NetSvc.Instance.AddMsg(new MsgPack(this,msg));
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("Client DisConnect");
    }
}