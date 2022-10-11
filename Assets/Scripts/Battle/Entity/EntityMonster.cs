/****************************************************
    文件：EntityMonster.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/7/5 11:2:38
	功能：怪物逻辑实体
*****************************************************/


using UnityEngine;

public class EntityMonster: EntityBase
{

    public MonsterData md;
    private float checkTime = 1.2f; //第一次检测间隔时间
    private float checkCountTime = 0;

    private float atkTime = 1.2f; //检测间隔时间
    private float atkCountTime = 0;
    public override void SetBattleProps(BattleProps props)
    {
        int level = md.mLevel;

        BattleProps p = new BattleProps
        {
            hp = props.hp * level,
            ad = props.ad * level,
            ap = props.ap * level,
            addef = props.addef * level,
            apdef = props.apdef * level,
            dodge = props.dodge * level,
            pierce = props.pierce * level,
            critical = props.critical * level,

        };

        Props = p;
        HP = p.hp;
    }

    bool runAI = true;
    public override void TickAILogic()
    {
        if (!runAI)
        {
            return;
        }
        if(currentAniState == AniState.Idle || currentAniState == AniState.Move)
        {
            if (battleMgr.isPauseGame)
            {
                Idle();
                return;
            }

            float delta = Time.deltaTime;
            checkCountTime += delta;
            if (checkCountTime < checkTime)
            {
                return;
            }
            else//找到玩家并攻击
            {
                //计算目标方向
                Vector2 dir = CalctargetDir();

                //判断目标是否在攻击范围
                if (!InAtkRange())
                {
                    //不在：则设置移动方向，并进入移动状态
                    SetDir(dir);
                    Move();
                }
                else
                {
                    //在：则停止移动，进行攻击
                    SetDir(Vector2.zero);
                    //判断攻击间隔
                    atkCountTime += checkCountTime;
                    if(atkCountTime > atkTime)
                    {
                        //达到攻击时间，转向并攻击
                        SetAtkRotation(dir);
                        Attack(md.mCfg.skillID);
                        atkCountTime = 0;
                    }
                    else
                    {
                        //未达到攻击时间，Idle 等待
                        Idle();
                    }

                }
                checkCountTime = 0;
                //随机检测间隔时间，让怪物动作不那么统一
                checkTime = PETools.RDInt(1, 5)*1.0f / 10;

            }



            
        }
    }
    public override Vector2 CalctargetDir()
    {
        EntityPlayer entityPlayer = battleMgr.entitySelfPlayer;
        if(entityPlayer == null || entityPlayer.currentAniState == AniState.Die)
        {
            runAI = false;
            return Vector2.zero;
        }
        else
        {
            Vector3 target = entityPlayer.GetPos();
            Vector3 self = GetPos();
            return new Vector2(target.x - self.x, target.z - self.z).normalized;
        }
    }
    private bool InAtkRange()
    {
        EntityPlayer entityPlayer = battleMgr.entitySelfPlayer;
        if(entityPlayer == null || entityPlayer.currentAniState == AniState.Die)
        {
            runAI = false; 
            return false;
        }
        else
        {
            Vector3 target = entityPlayer.GetPos();
            Vector3 self = GetPos();
            target.y = 0;
            self.y = 0;
            float dis = Vector3.Distance(target, self);
            if (dis <= md.mCfg.atkDis)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

    public override bool GetBreakState()
    {
        if (md.mCfg.isStop)
        {
            if (curtSkillCfg != null)
            {
                return curtSkillCfg.isBreak;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public override void SetHPVal(int oldVal, int newVal)
    {
        if (md.mCfg.mType == MonsterType.Boss)
        {
            BattleSys.Instance.playerCtrlWnd.SetBossHPBarVal(oldVal, newVal, Props.hp);
        }
        else
        {
            base.SetHPVal(oldVal, newVal);
        }
    }
}

