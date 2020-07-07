
/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：配置表服务
*****************************************************/
using System;
using System.Collections.Generic;
using System.Xml;

class CfgRvc
{
    private static CfgRvc instance;
    public static CfgRvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CfgRvc();
            }

            return instance;
        }
    }



    public void Init()
    {
        PECommon.Log("CfgRvc Init Done.");
        InitGuideCfg();
		InitStrongCfg();

	}

    #region 引导任务配置


    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\GitDirectory\暗黑战神\Assets\Resources\ResCfgs\guide.xml");
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
	private void InitStrongCfg()
	{

		XmlDocument doc = new XmlDocument();
		doc.Load(@"D:\GitDirectory\暗黑战神\Assets\Resources\ResCfgs\strong.xml");
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
			for (int i = 0; i < starLv; i++)
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

}

#region 数据结构
public class BaseData<T>
{
	public int id;
}

/// <summary>
/// 引导任务配置数据结构类
/// </summary>
public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
	public int npcID;            //触发任务目标NPC索引号
	public string dilogArr;      //对话内容
	public int actID;            //完成后新的任务ID
	public int coin;             //金币
	public int exp;              //经验
}


/// <summary>
/// 强化配置
/// </summary>
public class StrongCfg : BaseData<StrongCfg>
{
	public int pos;                //位置
	public int starLv;             //星级
	public int addhp;              //增加生命值
	public int addhurt;            //增加伤害
	public int adddef;             //增加防御
	public int minlv;              //最小等级
	public int coin;               //消耗金币
	public int crystal;            //消耗材料
}
#endregion