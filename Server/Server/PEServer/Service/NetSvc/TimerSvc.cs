/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：计时器服务
*****************************************************/
using System;
using System.Collections.Generic;

class TimerSvc
{

    class TaskPck
    {
        public int id;
        public Action<int> cb;
        public TaskPck(int id, Action<int> cb)
        {
            this.id = id;
            this.cb = cb;
        }
    }

    private static TimerSvc instance;
    public static TimerSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimerSvc();
            }

            return instance;
        }
    }



    private PETimer pt = null;
    private Queue<TaskPck> tpQue = new Queue<TaskPck>();
    private static readonly string tpQueLock = "tpQueLock";
    public void Init()
    {

        tpQue.Clear();
        pt = new PETimer(100);
        //设置日志输出
        pt.SetLog((info) =>
        {
            PECommon.Log(info);
        });

        pt.SetHandle((cb,tId) => {
            if (cb != null)
            {
                lock (tpQueLock)
                {
                    tpQue.Enqueue(new TaskPck(tId, cb));
                }
            }
        });

        PECommon.Log("TimerSvc Init Done.");
    }


    public void Update()
    {
        if (tpQue.Count > 0)
        {
            TaskPck tp = null;
            lock (tpQueLock)
            {
                tp = tpQue.Dequeue();
            }

            if (tp != null)
            {
                tp.cb.Invoke(tp.id);
            }
        }
        
    }


    public int AddTimeTask(Action<int> cb, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int replay = 1)
    {
        return pt.AddTimeTask(cb, delay, timeUnit, replay);
    }

    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }
}