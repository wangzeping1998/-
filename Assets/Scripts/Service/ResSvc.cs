/****************************************************
    文件：ResSvc.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:18:17
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc instance = null;

    public void InitSvc()
    {
        instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        IniMonsterCfg(PathDefine.MonsterCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        IniTaskrewardCfg(PathDefine.TaskRewardCfg);
        IniSkillActionCfg(PathDefine.SkillActionCfg);
        IniSkillCfg(PathDefine.SkillCfg);
        IniSkillMoveCfg(PathDefine.SkillMoveCfg);

        Debug.Log("Init ResSvc...");
    }

    #region 加载场景
    private Action prgCb = null; //加载完成场景回调
    //异步加载场景，完成后回调
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        //打开加载场景UI
        GameRoot.instance.loadingWind.SetWindowState();

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCb = () =>
        {
            //设置进度条
            float val = sceneAsync.progress;
            GameRoot.instance.loadingWind.SetProgress(val);
            if (val == 1)
            {
                if (loaded != null)
                {
                    GC.Collect();
                    loaded.Invoke();
                }

                //加载完成设置加载回调为NULL
                prgCb = null;
                //清空异步加载
                sceneAsync = null;
                //关闭窗口
                GameRoot.instance.loadingWind.SetWindowState(false);
            }
        };
    }
    private void Update()
    {
        //当加载场景时，回调会在Update中调用，加载完成时回调会为NULL且停止调用
        if (prgCb != null)
        {
            prgCb.Invoke();
        }
    }
    #endregion

    #region 加载音频
    //音频缓存
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    //加载音频
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path, au);
            }
        }

        return au;
    }
    #endregion

    #region 加载游戏对象
    //游戏对象缓存
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    //加载游戏对象
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }

        return go;
    }
    #endregion

    #region 加载精灵
    //精灵缓存
    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    //加载精灵
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }

        return sp;
    }
    #endregion

    #region InitCfgs
    /*配置表数据与逻辑*/
    
    #region 随机名字配置
    private List<string> surnameLst = new List<string>();
    private List<string> mannLst = new List<string>();
    private List<string> womanLst = new List<string>();
    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameLst.Add(e.InnerText);
                            break;
                        case "man":
                            mannLst.Add(e.InnerText);
                            break;
                        case "woman":
                            womanLst.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }
    public string GetRDNameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surnameLst[PETools.RDInit(0, surnameLst.Count - 1)];
        if (man)
        {
            rdName += mannLst[PETools.RDInit(0, mannLst.Count - 1)];
        }
        else
        {
            rdName += womanLst[PETools.RDInit(0, womanLst.Count - 1)];
        }

        return rdName;
    }
    #endregion

    #region 地图配置
    private Dictionary<int, MapCfg> mapDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                MapCfg mc = new MapCfg()
                {
                    id = ID,
                    monsterLst = new List<MonsterSpawnData>(),
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "power":
                            mc.power = int.Parse(e.InnerText);
                            break;
                        case "mainCamPos":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;

                        case "mainCamRote":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;

                        case "playerBornPos":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mc.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;

                        case "playerBornRote":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mc.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;
                        case "monsterLst":
                        {
                            string[] monsteArr = e.InnerText.Split('#');
                            for (int waveIdx = 0; waveIdx < monsteArr.Length; waveIdx++)
                            {
                                if (string.IsNullOrEmpty(monsteArr[waveIdx]))
                                {
                                    continue;
                                }

                                string[] numberArr = monsteArr[waveIdx].Split('|');
                                for (int numIdx = 0; numIdx < numberArr.Length; numIdx++)
                                {
                                    if (string.IsNullOrEmpty(numberArr[numIdx]))
                                    {
                                        continue;
                                    }

                                    string[] strArr = numberArr[numIdx].Split(',');
                                    MonsterSpawnData msd = new MonsterSpawnData();
                                    msd.mWave = waveIdx;
                                    msd.mIndex = numIdx;
                                    msd.id = int.Parse(strArr[0]);
                                    msd.mBornpos = new Vector3(float.Parse(strArr[1]), float.Parse(strArr[2]),
                                        float.Parse(strArr[3]));
                                    msd.mBornRote = new Vector3(0, float.Parse(strArr[4]), 0);
                                    msd.mLevel = int.Parse(strArr[5]);
                                    msd.mCfg = GetMonsterCfg(msd.id);
                                    mc.monsterLst.Add(msd);
                                }
                            }
                        }
                            break;
                    }
                }

                mapDic.Add(ID, mc);
            }
        }
    }
    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data;
        if (mapDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #region 怪物配置
    private Dictionary<int, MonsterCfg> monsterDic = new Dictionary<int, MonsterCfg>();
    private void IniMonsterCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                MonsterCfg mc = new MonsterCfg()
                {
                    id = ID,
                    bps = new BattleProps(),
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mName":
                            mc.mName = e.InnerText;
                            break;
                        case "resPath":
                            mc.resPath = e.InnerText;
                            break;
                        case "isStop":
                            mc.isStop = e.InnerText.Equals("1");
                            break;
                        case "mType":
                            if (e.InnerText.Equals("1"))
                            {
                                mc.mType = MonsterType.Nomral;
                            }
                            else if (e.InnerText.Equals("2"))
                            {
                                mc.mType = MonsterType.Boss;
                            }
                            break;
                        case "skillID" :
                            mc.skillID = int.Parse(e.InnerText);
                            break;
                        case "atkDis" :
                            mc.atkDis = float.Parse(e.InnerText);
                            break;
                        case "hp" :
                            mc.bps.hp = int.Parse(e.InnerText);
                            break;
                        case "ad" :
                            mc.bps.ad = int.Parse(e.InnerText);
                            break;
                        case "ap" :
                            mc.bps.ap = int.Parse(e.InnerText);
                            break;
                        case "addef" :
                            mc.bps.addef = int.Parse(e.InnerText);
                            break;
                        case "apdef" :
                            mc.bps.apdef = int.Parse(e.InnerText);
                            break;
                        case "dodge" :
                            mc.bps.dodge = int.Parse(e.InnerText);
                            break;
                        case "pierce" :
                            mc.bps.pierce = int.Parse(e.InnerText);
                            break;
                        case "critical" :
                            mc.bps.critical = int.Parse(e.InnerText);
                            break;
                    }
                }

                monsterDic.Add(ID, mc);
            }
        }
    }
    public MonsterCfg GetMonsterCfg(int id)
    {
        MonsterCfg data;
        if (monsterDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                AutoGuideCfg agc = new AutoGuideCfg()
                {
                    id = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            agc.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            agc.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            agc.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            agc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            agc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }

                guideTaskDic.Add(ID, agc);
            }
        }
    }
    public AutoGuideCfg GetGuideCfgData(int id)
    {
        AutoGuideCfg data;
        if (guideTaskDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #region 强化配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongCfg = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                StrongCfg scg = new StrongCfg()
                {
                    id = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos":
                            scg.pos = val;
                            break;
                        case "starlv":
                            scg.starLv = val;
                            break;
                        case "addhp":
                            scg.addhp = val;
                            break;
                        case "addhurt":
                            scg.addhurt = val;
                            break;
                        case "adddef":
                            scg.adddef = val;
                            break;
                        case "minlv":
                            scg.minlv = val;
                            break;
                        case "coin":
                            scg.coin = val;
                            break;
                        case "crystal":
                            scg.crystal = val;
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongCfg.TryGetValue(scg.pos, out dic))
                {
                    dic.Add(scg.starLv, scg);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(scg.starLv, scg);
                    strongCfg.Add(scg.pos, dic);
                }
            }
        }
    }
    /// <summary>
    /// 查询装备强化加成信息
    /// </summary>
    /// <param name="pos">装备部位</param>
    /// <param name="starLv">星级</param>
    /// <returns></returns>
    public StrongCfg GetStrongCfgData(int pos, int starLv)
    {
        Dictionary<int, StrongCfg> dic;
        if (strongCfg.TryGetValue(pos, out dic))
        {
            //return data;
            StrongCfg data = null;
            if (dic.TryGetValue(starLv, out data))
            {
                return data;
            }
        }

        return null;
    }
    /// <summary>
    /// 查询装备强化加成信息属性
    /// </summary>
    /// <param name="pos">装备部位</param>
    /// <param name="starLv">星级</param>
    /// <param name="type">属性</param>
    /// <returns></returns>
    public int GetPropAddValPreLv(int pos, int starLv, int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if (strongCfg.TryGetValue(pos, out posDic))
        {
            for (int i = 1; i <= starLv; i++)
            {
                StrongCfg sc = null;
                if (posDic.TryGetValue(i, out sc))
                {
                    switch (type)
                    {
                        case 1:
                            //HP
                            val += sc.addhp;
                            break;

                        case 2:
                            val += sc.addhurt;
                            //Hurt
                            break;

                        case 3:
                            val += sc.adddef;
                            //Defense
                            break;
                    }
                }
            }
        }

        return val;
    }
    #endregion
    
    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskrewardDic = new Dictionary<int, TaskRewardCfg>();
    private void IniTaskrewardCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                TaskRewardCfg trc = new TaskRewardCfg()
                {
                    id = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "taskName":
                            trc.taskName = e.InnerText;
                            break;
                        case "coin":
                            trc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            trc.exp = int.Parse(e.InnerText);
                            break;
                        case "count":
                            trc.count = int.Parse(e.InnerText);
                            break;
                    }
                }

                taskrewardDic.Add(ID, trc);
            }
        }
    }
    public TaskRewardCfg GetTaskrewardCfg(int id)
    {
        TaskRewardCfg data;
        if (taskrewardDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #region 技能配置
    private Dictionary<int, SkillCfg> skillDic = new Dictionary<int, SkillCfg>();
    private void IniSkillCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                SkillCfg sc = new SkillCfg()
                {
                    id = ID,
                    skillMoveLst = new List<int>(),
                    skillActionLst = new List<int>(),
                    skillDamageLst = new List<int>(),
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "skillName":
                            sc.skillName = e.InnerText;
                            break;
                        case "skillTime":
                            sc.skillTime = long.Parse(e.InnerText);
                            break;
                        case "cdTime":
                            sc.cdTime = long.Parse(e.InnerText);
                            break;
                        case "isCollide":
                            sc.isCollide = e.InnerText.Equals("1");
                            break;
                        case "isBreak":
                            sc.isBreak = e.InnerText.Equals("1");
                            break;
                        case "aniAction":
                            sc.aniAction = int.Parse(e.InnerText);
                            break;
                        case "fx":
                            sc.fx = e.InnerText;
                            break;
                        case "skillMoveLst":
                        {
                            string[] strArr = e.InnerText.Split('|');
                            for (int j = 0; j < strArr.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(strArr[j]))
                                {
                                    sc.skillMoveLst.Add(int.Parse(strArr[j]));
                                }
                            }
                        }
                            break;
                        case "skillActionLst":
                        {
                            string[] skillActionArr = e.InnerText.Split('|');
                            for (int j = 0; j < skillActionArr.Length; j++)
                            {
                                if (string.IsNullOrEmpty(skillActionArr[j]))
                                {
                                    continue;
                                }

                                sc.skillActionLst.Add(int.Parse(skillActionArr[j]));
                            }
                        }
                            break;
                        case "skillDamageLst":
                        {
                            string[] skillDamageArr = e.InnerText.Split('|');
                            for (int j = 0; j < skillDamageArr.Length; j++)
                            {
                                if (string.IsNullOrEmpty(skillDamageArr[j]))
                                {
                                    continue;
                                }

                                sc.skillDamageLst.Add(int.Parse(skillDamageArr[j]));
                            }
                        }
                            break;
                        case "dmgType":
                            int dmgType = int.Parse(e.InnerText);
                            if (dmgType > 2)
                            {
                                PECommon.Log("Res  DamageType data error!", LogType.Error);
                            }

                            sc.dmgType = (DamageType) dmgType;
                            break;
                    }
                }

                skillDic.Add(ID, sc);
            }
        }
    }
    public SkillCfg GetSkillCfg(int id)
    {
        SkillCfg data;
        if (skillDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #region 技能位移配置
    private Dictionary<int, SkillMoveCfg> skillMoveDic = new Dictionary<int, SkillMoveCfg>();
    private void IniSkillMoveCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                SkillMoveCfg smc = new SkillMoveCfg()
                {
                    id = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            smc.delayTime = long.Parse(e.InnerText);
                            break;
                        case "moveTime":
                            smc.moveTime = long.Parse(e.InnerText);
                            break;
                        case "moveDis":
                            smc.moveDis = float.Parse(e.InnerText);
                            break;
                    }
                }

                skillMoveDic.Add(ID, smc);
            }
        }
    }
    public SkillMoveCfg GetSkillMoveCfg(int id)
    {
        SkillMoveCfg data;
        if (skillMoveDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #region 技能判定配置
    private Dictionary<int, SkillActionCfg> skillActionDic = new Dictionary<int, SkillActionCfg>();
    private void IniSkillActionCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("xml file " + path + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                SkillActionCfg sac = new SkillActionCfg()
                {
                    id = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            sac.delayTime = long.Parse(e.InnerText);
                            break;
                        case "radius":
                            sac.radius = float.Parse(e.InnerText);
                            break;
                        case "angle":
                            sac.angle = float.Parse(e.InnerText);
                            break;
                    }
                }

                skillActionDic.Add(ID, sac);
            }
        }
    }
    public SkillActionCfg GetSkillActionCfg(int id)
    {
        SkillActionCfg data;
        if (skillActionDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }
    #endregion

    #endregion
}