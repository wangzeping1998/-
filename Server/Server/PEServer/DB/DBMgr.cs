/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：数据库管理器
*****************************************************/
using MySql.Data.MySqlClient;
using PEProtocol;
using System.Threading.Tasks;

class DBMgr
{
    private static DBMgr instance;
    public static DBMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBMgr();
            }

            return instance;
        }
    }
    private MySqlConnection conn;

    public void Init()
    {
        PECommon.Log("DBMgr init done!");
        conn = new MySqlConnection("server = localhost;User Id=root;password = Qq123123;Database = darkgod;Charset = utf8");
        conn.Open();
    }

    /// <summary>
    /// 根据用户名和密码查询玩家数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public PlayerData QueryPlayerData(string acct,string pass)
    {
        bool isNew = true;
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                //账号存在
                isNew = false;
                //获得密码信息
                string _pass = reader.GetString("pass");
                //密码正确
                //为数据结构赋值
                if (_pass.Equals(pass))
                {
                    //密码正确返回玩家数据
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        crystal = reader.GetInt32("crystal"),
                        hp = reader.GetInt32("diamond"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),
                        guideId = reader.GetInt32("guideId"),
                        time = reader.GetInt64("time"),
                        fuben = reader.GetInt32("fuben"),
                };

                    #region strong
                    //星级数据结构  10#10#10#10#10#8#
                    string[] strongStrArr = reader.GetString("strong").Split('#');
                    int[] _strongArr = new int[6];
                    for (int i = 0; i < strongStrArr.Length; i++)
                    {
                        if (strongStrArr[i] == "")
                        {
                            continue;
                        }

                        if (int.TryParse(strongStrArr[i],out int starLv))
                        {
                            _strongArr[i] = starLv;
                        }
                        else
                        {
                            PECommon.Log("Parse strong data error!", LogType.Error);
                        }
                    }
                    playerData.strongArr = _strongArr;
                    #endregion

                    string[] taskStrArr = reader.GetString("task").Split('#');
                    string[] taskArr = new string[6];
                    for (int i = 0; i < taskStrArr.Length; i++)
                    {
                        if (taskStrArr[i] == string.Empty)
                        {
                            continue;
                        }
                        else if(taskStrArr[i].Length >= 5)
                        {
                            taskArr[i] = taskStrArr[i];
                        }
                        else
                        {
                            throw new System.Exception("taskArr error!");
                        }
                    }
                    playerData.taskArr = taskArr;


                }
            }
            else
            {
                //账号不存在
                //是新账号，自动创建
                if (isNew)
                {
                    playerData = new PlayerData
                    {
                        id = -1,
                        name = "",
                        lv = 1,
                        exp = 0,
                        power = 150,
                        coin = 5000,
                        diamond = 500,
                        crystal = 500,
                        hp = 2000,
                        ad = 275,
                        ap = 265,
                        addef = 67,
                        apdef = 43,
                        dodge = 7,
                        pierce = 5,
                        critical = 2,
                        guideId = 1001,
                        strongArr = new int[6],
                        time = TimerSvc.Instance.GetNowTime(),
                        taskArr = new string[6],
                        fuben = 10001,
                    };

                    string[] taskArr = playerData.taskArr;
                    for (int i = 0; i < taskArr.Length; i++)
                    { 
                        taskArr[i] = (i + 1) + "|0|0";
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }

                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }


        }
        catch (System.Exception e)
        {
            PECommon.Log("Query PlayerData By Acct&pass Error:" + e, LogType.Error);
        }

        if (reader != null)
        {
            reader.Close();
        }

        return playerData;
    }

    /// <summary>
    /// 插入数据，返回ID
    /// </summary>
    private int InsertNewAcctData(string acct,string pass ,PlayerData pd)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct=@acct, pass=@pass, name=@name, level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,strong=@strong,guideId = @guideId,time = @time,task = @task,fuben=@fuben", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);
            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideId", pd.guideId);

            #region strong
            //写入强化星级
            //0#0#0#0#0#0#
            string strongStr = string.Empty;
            int[] strong = pd.strongArr;
            for (int i = 0; i < strong.Length; i++)
            {
                strongStr += strong[i];
                strongStr += "#";
            }


            cmd.Parameters.AddWithValue("strong", strongStr);
            #endregion

            cmd.Parameters.AddWithValue("time", pd.time);

            #region Taskreward
            string[] taskArr = pd.taskArr;
            string taskStr = string.Empty;
            for (int i = 0; i < taskArr.Length; i++)
            {
                taskStr += taskArr[i] + "#";
            }

            cmd.Parameters.AddWithValue("task", taskStr);
            cmd.Parameters.AddWithValue("fuben", pd.fuben);
            #endregion


            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (System.Exception e)
        {
            PECommon.Log("Insert PlayerData Error:" + e, LogType.Error);
        }
        return id;
    }

    /// <summary>
    /// 查询是否存在
    /// </summary>
    public bool QueryNameData(string name)
    {
        bool exist = false;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name = @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                exist = true;
            }
        }
        catch (System.Exception ex)
        {
            PECommon.Log("Query name Error:" + ex, LogType.Error);
        }
        if (reader != null)
        {
            reader.Close();
        }

        return exist;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public bool UpdatePlayerData(int id,PlayerData pd)
    {

        try
        {
            MySqlCommand cmd = new MySqlCommand("update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideId = @guideId,strong =@strong,time =@time,task =@task,fuben = @fuben where id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);
            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideId", pd.guideId);

            //写入强化星级
            //0#0#0#0#0#0#
            string strongStr = string.Empty;
            int[] strong = pd.strongArr;
            for (int i = 0; i < strong.Length; i++)
            {
                strongStr += strong[i];
                strongStr += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongStr);
            cmd.Parameters.AddWithValue("time", pd.time);

            #region Taskreward
            string[] taskArr = pd.taskArr;
            string taskStr = string.Empty;
            for (int i = 0; i < taskArr.Length; i++)
            {
                taskStr += taskArr[i] + "#";
            }
            cmd.Parameters.AddWithValue("task", taskStr);
            cmd.Parameters.AddWithValue("fuben", pd.fuben);
            #endregion

            cmd.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
            PECommon.Log("Updat ePlayerData Error:" + ex, LogType.Error);
            return false;
        }

        return true;
    }
}
