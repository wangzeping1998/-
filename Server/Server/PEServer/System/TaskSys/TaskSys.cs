/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：任务系统
*****************************************************/
using PEProtocol;

public class TaskSys
{
    private static TaskSys instance;
    public static TaskSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TaskSys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;
    private CfgRvc cfgSvc;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgRvc.Instance;
        PECommon.Log("TaskSys Init Done.");
    }

    public void ReqTaskReward(MsgPack msgPack)
    {
        ReqTaskReward data = msgPack.msg.reqTaskReward;
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspTaskReward
        };


        PlayerData pd = cacheSvc.GetPlayerDataBySession(msgPack.session);
        //获得任务模板
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfgData(data.taskId);
        //获得玩家当前任务信息
        TaskRewardData trd = CalcTaskRewardData(pd, data.taskId);
        if ((trd.prgs == trc.count) && (!trd.taked))
        {
            //任务进度完成，且奖励未被领取
            pd.coin += trc.coin;
            PECommon.CalExp(pd, trc.exp);
            trd.taked = true;
            CalcUpdatePlayerTaskArr(pd, trd);

            if (!cacheSvc.UpdataPlayerData(pd.id,pd))
            {
                //数据库写入失败
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspTaskReward = new RspTaskReward
                {
                    coin = pd.coin,
                    exp = pd.exp,
                    lv = pd.lv,
                    taskArr = pd.taskArr
                };
            }
        }
        else
        {
            //客户端数据异常
            msg.err = (int)ErrorCode.ClientDataError;
        }

        msgPack.session.SendMsg(msg);

    }

    //根据rid获取玩家的任务奖励数据
    public TaskRewardData CalcTaskRewardData(PlayerData pd,int rid)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskInfo[0]) == rid)
            {
                //获得玩家当前任务信息
                trd = new TaskRewardData
                {
                    id = rid,
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1"),
                };
                break;
            }
        }
        return trd;
    }

    //更新玩家任务进度数据
    public void CalcUpdatePlayerTaskArr(PlayerData pd,TaskRewardData trd)
    {
        string result = trd.id + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
        int index = 0;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskInfo[0]) == trd.id)
            {
                index = i;
                break;
            }
        }
        pd.taskArr[index] = result;
    }

    /// <summary>
    /// 更新玩家数据的任务进度
    /// </summary>
    /// <param name="pd">玩家数据</param>
    /// <param name="tid">进度id</param>
    public void CalcTaskPrgs(PlayerData pd,int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);   //获得玩家当前任务信息
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfgData(tid);   //获得任务模板

        if (trd.prgs < trc.count)
        {
            //更新任务进度
            trd.prgs++;
            CalcUpdatePlayerTaskArr(pd, trd);

            ServerSession session = cacheSvc.GetOnlineServerSession(pd.id);
            if (session != null)
            {
                session.SendMsg(
                    new GameMsg
                    {
                        cmd = (int)CMD.PshTaskPrgs,
                        pshTaskPrgs = new PshTaskPrgs
                        {
                            taskArr =pd.taskArr
                        }
                    }
                ); 
            }
        }

    }
}