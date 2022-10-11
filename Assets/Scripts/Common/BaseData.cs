/****************************************************
	文件：BaseData.cs
	作者：洛辰
	邮箱: 893271704@qq.com
	日期：2022/06/15 21:50   	
	功能：配置数据类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class MonsterData : BaseData<MonsterData>
{
	public int mWave;//怪物产生的批次
	public int mIndex;//序号
	public MonsterCfg mCfg;
	public Vector3 mBornPos;
	public Vector3 mBornRote;
	public int mLevel;
}
public class MonsterCfg : BaseData<MonsterCfg>
{
	public string mName;
	public MonsterType mType;//1：普通怪物，2：boss
	public bool isStop;//怪物是否能被玩家的攻击中断当前的状态
	public string resPath;
	public int skillID;
	public float atkDis;
	public BattleProps bps;
}

public class SkillActionCfg : BaseData<SkillActionCfg>
{
	public int delayTime;
	public float radius;//伤害计算范围
	public int angle;//伤害有效角度
}
public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
	public int cdTime;
    public int skillTime;
    public int aniAction;
	public string fx;//动画
	public bool isCombo;
	public bool isCollide;//技能碰撞
	public bool isBreak;//打断攻击
	public int skillMove;
	public DamageType dmgType;
	public List<int> skillMoveList;
	public List<int> skillActionList;
	public List<int> skillDamageList;
}

public class SkillMoveCfg : BaseData<SkillCfg>
{
    public int moveTime;
    public float moveDis;
	public int delayTime;
}

public class StrongCfg : BaseData<StrongCfg>
{
	public int pos;
	public int starlv;
	public int addhp;
	public int addhurt;
	public int adddef;
	public int minlv;
	public int coin;
	public int crystal;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
	public int npcID;//触发任务目标 NPC 引导
	public string dilogArr;
	public int actID;
	public int coin;
	public int exp;
}

public class MapCfg : BaseData<MapCfg>
{
	public string mapName;
	public string sceneName;
	public int power;
	public Vector3 mainCamPos;
	public Vector3 mainCamRote;
	public Vector3 playerBornPos;
	public Vector3 playerBornRote;
	public List<MonsterData> monsterLst;

	public int coin;
	public int exp;
	public int crystal;
}
public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
	public string taskName;
	public int count;
	public int exp;
	public int coin;
}
public class TaskRewardData : BaseData<TaskRewardCfg>
{
	public int prgs;//任务进度
	public bool taked;
}

public class BaseData<T>
{
	public int ID;
}

public class BattleProps
{
	public int hp;
	public int ad;
	public int ap;
	public int addef;
	public int apdef;
	public int dodge;
	public int pierce;
	public int critical;
}


