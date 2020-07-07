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
    private Action prgCb = null; //加载完成场景回调

    public void InitSvc()
    {
        instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        Debug.Log("Init ResSvc...");
    }

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
                    id = ID
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

    #endregion
}