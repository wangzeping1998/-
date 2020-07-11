/****************************************************
    文件：MainCitySys.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/13 10:41:13
	功能：出城系统
*****************************************************/

using System;
using PEProtocol;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MainCitySys : SystemRoot
{
    public static MainCitySys instance;

    //窗口
    public MainCityWind maincityWind;    //主城窗口
    public InfoWind infoWind;            //角色信息窗口
    public GuideWind guideWind;          //引导任务窗口
    public StrongWind strongWind;        //强化窗口
    public ChatWind chatWind;            //聊天窗口
    public BuyWind buyWind;              //购买窗口
    public TaskWind taskWind;
    
    //数据
    private PlayerController m_playerCtrl;    //玩家角色信息
    private Transform m_charShowCam;          //角色展示相机
    private AutoGuideCfg m_currTaskData;      //当前引导任务数据
    private Transform[] m_npcPosTrans;        //NPC坐标点
    private NavMeshAgent m_nav;               //Navgation导航
    private bool isNavGuide = false;          //导航标志位

    public override void InitSys()
    {
        base.InitSys();
        instance = this;
        Debug.Log("Init MainCitySys...");
    }

    //进入主城
    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.mainCityID);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity");
            //加载主角
            LoadPlayer(mapData);
            //打开主城UI
            maincityWind.SetWindowState();
            //播放主题音乐
            audioSvc.PlayBgMusic(Constants.BGMainCity);
            GameObject map = GameObject.FindWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            m_npcPosTrans = mcm.NpcPosTrans;

            if (m_charShowCam != null)
            {
                m_charShowCam.gameObject.SetActive(false);
            }
        });
    }

    //创建角色
    public void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one * 1.5f;
        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        m_playerCtrl = player.GetComponent<PlayerController>();
        m_playerCtrl.Init();

        m_nav = player.GetComponent<NavMeshAgent>();
    }

    //角色移动，UI与角色控制中转
    public void SetMoveDir(Vector2 dir)
    {
        StopNavGuide();
        if (dir == Vector2.zero)
        {
            m_playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            m_playerCtrl.SetBlend(Constants.BlendWalk);
        }

        m_playerCtrl.Dir = dir;
    }

    //打开信息窗口
    public void OpenInfoWind()
    {
        StopNavGuide();
        //找到UI角色相机
        if (m_charShowCam == null)
        {
            m_charShowCam = GameObject.FindWithTag("charShowCam").transform;
        }

        //设置相机位置、旋转、缩放、激活
        m_charShowCam.localPosition = m_playerCtrl.transform.position + m_playerCtrl.transform.forward * 2.8f +
                                      new Vector3(0, 1.2f, 0);
        m_charShowCam.localEulerAngles = new Vector3(0, 180 + m_playerCtrl.transform.localEulerAngles.y, 0);
        m_charShowCam.localScale = Vector3.one;
        m_charShowCam.gameObject.SetActive(true);

        //打开信息面板
        infoWind.SetWindowState();
    }
    
    //打开对话窗口
    private void OpenGuideWind()
    {
        guideWind.SetWindowState();
    }

    //打开强化窗口
    public void OpenStrongWind()
    {
        strongWind.SetWindowState();
        StopNavGuide();
    }

    //打开聊天窗口
    public void OpenChatWind()
    {
        chatWind.SetWindowState();
        StopNavGuide();
    }
    
    //打开购买窗口
    public void OpenBuyWind(int type)
    {
        buyWind.SetBuyType(type);
        buyWind.SetWindowState();
        StopNavGuide();
    }

    public void OpenTaskWind()
    {
        taskWind.SetWindowState();
        StopNavGuide();
    }

    public void EnterFuben()
    {
        FubenSys.instance.EnterFuben();
        StopNavGuide();
    }
    
    //关闭角色信息面板
    public void CloseInfoWind()
    {
        if (m_charShowCam != null)
        {
            m_charShowCam.gameObject.SetActive(false);
            infoWind.SetWindowState(false);
        }
    }

    #region 旋转角色信息界面UI模型功能

    private float m_startPos;

    public void SetStartRote()
    {
        m_startPos = m_playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRote(float rote)
    {
        m_playerCtrl.transform.localEulerAngles = new Vector3(0, m_startPos + rote, 0);
    }

    #endregion

    
    
    #region 任务引导

    //执行引导任务
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            m_currTaskData = agc;
        }

        //解析任务数据,执行操作
        if (m_currTaskData.npcID != -1)
        {
            m_nav.enabled = true;
            m_nav.isStopped = false;
            isNavGuide = true;
            m_nav.speed = Constants.PlayerMoveSpeed;
            m_nav.SetDestination(m_npcPosTrans[m_currTaskData.npcID].position);
            m_playerCtrl.SetBlend(Constants.BlendWalk);
        }
        else
        {
            OpenGuideWind();
        }
    }

    private void Update()
    {
        //在寻路中一直调用
        //其中包括：相机跟随、判断是否找到目标
        if (isNavGuide)
        {
            NavGuideing();
            m_playerCtrl.SetCam();
        }
    }

    //寻路中持续调用
    private void NavGuideing()
    {
        float dis = Vector3.Distance(m_playerCtrl.transform.position, m_npcPosTrans[m_currTaskData.npcID].position);
        if (dis < 0.5f)
        {
            m_nav.isStopped = true;
            //已经找到NPC
            m_nav.enabled = false;
            isNavGuide = false;
            m_playerCtrl.SetBlend(Constants.BlendIdle);
            OpenGuideWind();
        }
    }
    
    //停止寻路
    private void StopNavGuide()
    {
        //只要玩家使用摇杆移动，当处于正在寻路中则立即停止寻路
        if (isNavGuide)
        {
            m_nav.isStopped = true;
            m_nav.enabled = false;
            isNavGuide = false;
            m_playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    
    
    //获得当前任务数据
    public AutoGuideCfg GetTaskData()
    {
        return m_currTaskData;
    }
    #endregion

    //任务完成服务器响应
    public void RspGuide(GameMsg msg)
    {
        RspGuide rspGuie = msg.rspGuide;
        GameRoot.AddTips("任务完成奖励金币+"+m_currTaskData.coin + "经验值+"+m_currTaskData.exp);
        switch (m_currTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            
            case 1:
                EnterFuben();
                //进入副本
                break;
            
            case 2:
                OpenStrongWind();
                //进入强化界面
                break;
            
            case 3:
                OpenBuyWind(0);
                //进入体力购买
                break;
            
            case 4:
                //进入金币制造
                OpenBuyWind(1);
                break;
            
            case 5:
                //进入世界聊天
                OpenChatWind();
                break;
        }
        
        GameRoot.instance.SetPlayerDataByGuide(rspGuie);
        maincityWind.RefreshUI();
    }
    
    //接收服务器强化响应
    public void RspStrong(GameMsg msg)
    {
        int fightPre = PECommon.GetFightProps(GameRoot.instance.playerData);
        RspStrong rspStrong = msg.rspStrong;
        GameRoot.instance.SetPlayerDataByStrong(rspStrong);
        int fightNow = PECommon.GetFightProps(GameRoot.instance.playerData);
        GameRoot.AddTips("战斗力提升+"+(fightNow - fightPre));
        audioSvc.PlayUIAudio(Constants.UIFbitem);
        strongWind.UpdateUI();
    }

    //接收服务器聊天响应
    public void PshChat(GameMsg msg)
    {
        PshChat pshChat = msg.pshChat;
        chatWind.AddChatMsg(pshChat.name, pshChat.chat);
    }

    public void RspBuy(GameMsg msg)
    {
        RspBuy rspBuy = msg.rspBuy;
        GameRoot.instance.SetPlayerDataByBuy(rspBuy);
        buyWind.SetWindowState(false);
        maincityWind.RefreshUI();
    }

    public void RspBuyError(GameMsg msg)
    {
        buyWind.SetWindowState(false);
    }

    public void PshPower(GameMsg msg)
    {
        PshPower pshPower = msg.pshPower;
        GameRoot.instance.SetPlayerDataByPower(pshPower);
        maincityWind.RefreshUI();
    }

    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTaskReward data = msg.rspTaskReward;
        GameRoot.instance.SetPlayerDataByTask(data);
        taskWind.Refresh();
        maincityWind.RefreshUI();
    }

    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.pshTaskPrgs;
        GameRoot.instance.SetPlayerDataByTaskPrgs(data);
        taskWind.Refresh();
        maincityWind.RefreshUI();
    }
}