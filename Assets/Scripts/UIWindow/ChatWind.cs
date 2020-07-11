/****************************************************
    文件：ChatWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/3 15:34:30
	功能：聊天窗口
*****************************************************/

using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class ChatWind : WindowRoot
{
    public InputField iptChat;

    public Text txtChat;

    public Image imgWolrd;
    public Image imgGuid;
    public Image imgFriend;

    private Button m_btnWorld;
    private Button m_btnGuid;
    private Button m_btnFriend;
    public Button btnSend;
    public Button btnClose;

    private int m_chatType;
    private List<string> m_chatLis = new List<string>();

    private bool m_canSend = true;

    protected override void InitWind()
    {
        base.InitWind();
        m_chatType = 0;

        m_btnWorld = imgWolrd.GetComponent<Button>();
        m_btnGuid = imgGuid.GetComponent<Button>();
        m_btnFriend = imgFriend.GetComponent<Button>();

        btnSend.onClick.AddListener(OnClickSendBtn);
        btnClose.onClick.AddListener(OnClickCloseBtn);
        m_btnWorld.onClick.AddListener(OnClickWorldChatBtn);
        m_btnGuid.onClick.AddListener(OnClickGuidChatBtn);
        m_btnFriend.onClick.AddListener(OnClickFriendChatBtn);

        RefreshUI();
    }

    //刷新UI
    public void RefreshUI()
    {
        if (m_chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < m_chatLis.Count; i++)
            {
                chatMsg += m_chatLis[i] + "\n";
            }

            SetText(txtChat, chatMsg);
            SetSprite(imgWolrd, PathDefine.btnType1);
            SetSprite(imgGuid, PathDefine.btnType2);
            SetSprite(imgFriend, PathDefine.btnType2);
        }
        else if (m_chatType == 1)
        {
            SetText(txtChat, "尚未加入公会");
            SetSprite(imgWolrd, PathDefine.btnType2);
            SetSprite(imgGuid, PathDefine.btnType1);
            SetSprite(imgFriend, PathDefine.btnType2);
        }
        else if (m_chatType == 2)
        {
            SetText(txtChat, "目前没有好友");
            SetSprite(imgWolrd, PathDefine.btnType2);
            SetSprite(imgGuid, PathDefine.btnType2);
            SetSprite(imgFriend, PathDefine.btnType1);
        }
    }

    protected override void ClearWind()
    {
        base.ClearWind();
        btnClose.onClick.RemoveListener(OnClickCloseBtn);
    }

    //点击发送聊天消息按钮
    private void OnClickSendBtn()
    {
        if (!m_canSend)
        {
            GameRoot.AddTips("每次发送聊天消息需要间隔5秒哦");
            return;
        }
        
        if (!string.IsNullOrEmpty(iptChat.text) && iptChat.text != " ")
        {
            if (iptChat.text.Length < 12)
            {
                //发送网络消息
                GameMsg msg = new GameMsg
                {
                    cmd = (int) CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text
                    }
                };
                //清空内容
                iptChat.text = string.Empty;
                netSvc.SendMsg(msg);
                m_canSend = false;
                timerSvc.AddTimeTask((id) => { m_canSend = true; },5, PETimeUnit.Second);
            }
            else
            {
                GameRoot.AddTips("输入聊天信息内容不能超过12个字");
            }
        }
        else
        {
            GameRoot.AddTips("未输入聊天信息");
        }
    }


    //关闭聊天界面按钮
    private void OnClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        m_chatType = 0;
        SetWindowState(false);
    }

    //世界聊天按钮
    private void OnClickWorldChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        m_chatType = 0;
        RefreshUI();
    }

    //公会聊天按钮
    private void OnClickGuidChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        m_chatType = 1;
        RefreshUI();
    }

    //好友聊天按钮
    private void OnClickFriendChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        m_chatType = 2;
        RefreshUI();
    }

    public void AddChatMsg(string name, string chat)
    {

        m_chatLis.Add(Constants.Color(name + ":", TxtColor.Blue) + chat);
        if (m_chatLis.Count > 12)
        {
            m_chatLis.RemoveAt(0);
        }

        if (GetWindowState())
        {
            RefreshUI();
        }
    }
}