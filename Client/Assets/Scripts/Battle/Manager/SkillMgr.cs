/****************************************************
    文件：BattleMgr.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：技能管理器
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;
    public void Init()
    {
        PECommon.Log("Init SkillMgr Done");
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    public void SkillAttack(EntityBase entity, int skillID)
    {
        entity.skMoveCBList.Clear();
        entity.skActionCBList.Clear();

        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);
    }
    /// <summary>
    /// 技能效果表现
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="skillID"></param>
    public void AttackEffect(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfg(skillID);

        if (!skillData.isCollide)
        {
            //忽略角色之间的刚体碰撞：利用 Layer 层次
            Physics.IgnoreLayerCollision(9, 10);
            timerSvc.AddTimeTask((int tid) =>
            {
                Physics.IgnoreLayerCollision(9, 10, false);
            }, skillData.skillTime);

        }

        //if (entity is EntityPlayer) PECommon.Log("entity is EntityPlayer");
        //if (entity is EntityMonster) PECommon.Log("entity is EntityMonster");
        //if (entity is EntityBase) PECommon.Log("entity is EntityBase");


        //只让玩家有智能锁定？
        //if(entity.entityType== EntityType.Player)
        
        //智能目标锁定
        if (entity.GetDirInput() == Vector2.zero)
        {
            //自动攻击最近的怪物
            Vector2 dir = entity.CalctargetDir();                       

            if (dir != Vector2.zero)
            {
                entity.SetAtkRotation(dir);
            }
        }
        else
        {            
            entity.SetAtkRotation(entity.GetDirInput(),true);
        }

        entity.SetAction(skillData.aniAction);
        entity.SetFX(skillData.fx, skillData.skillTime);
                      
        CalcSkillMove(entity,skillData);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);

        if (!skillData.isBreak)
        {
            entity.entityState = EntityState.BatiState;
        }

        entity.skEndCB = timerSvc.AddTimeTask((int tid) =>
        {
            entity.Idle();
            
        },skillData.skillTime);
    }
    private void CalcSkillMove(EntityBase entity,SkillCfg skillData)
    {
        List<int> skillMoveList = skillData.skillMoveList;
        int sum = 0;
        for (int i = 0; i < skillMoveList.Count; i++)
        {
            SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfg(skillData.skillMoveList[i]);
            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sum += skillMoveCfg.delayTime;
            if (sum > 0)
            {//技能位移延时
                int moveid =  timerSvc.AddTimeTask((int tid) =>
                {
                    entity.SetSkillMoveState(true, speed);

                    //在玩家攻击动画结束前，如果被攻击(打断）则取消攻击伤害，
                    //但是我觉得攻击早就作用到目标物体上了，只是动画还没完，这个时候被攻击为什么不算伤害呢？
                    entity.RemoveMoveCB(tid);

                }, sum);
                entity.skMoveCBList.Add(moveid);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }

            sum += skillMoveCfg.moveTime;
            int stopid = timerSvc.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);

                //在玩家攻击动画结束前，如果被攻击(打断）则取消攻击伤害，
                //但是我觉得攻击早就作用到目标物体上了，只是动画还没完，这个时候被攻击为什么不算伤害呢？
                entity.RemoveMoveCB(tid);

            }, sum);
            entity.skMoveCBList.Add(stopid);
        }
    }
    
    /// <summary>
    /// 技能伤害运算
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="skillID"></param>
    public void AttackDamage(EntityBase entity, int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfg(skillID);
        List<int> actionList = skillData.skillActionList;
        int sum = 0;//是否延时
        for(int i = 0; i < actionList.Count; i++)
        {
            SkillActionCfg skillAction = resSvc.GetSkillActionCfg(actionList[i]);
            sum += skillAction.delayTime;
            int index = i;

            //if (entity is EntityMonster) PECommon.Log("entity is EntityMonster");
            
            if (sum > 0)
            {                
                int actid = timerSvc.AddTimeTask((int tid) =>
                {
                    if (entity != null)
                    {
                        SkillAction(entity, skillData, index);

                        entity.RemoveActionCB(tid);

                    }

                },sum);

                //在玩家攻击动画结束前，如果被攻击(打断）则取消攻击伤害，
                //但是我觉得攻击早就作用到目标物体上了，只是动画还没完，这个时候被攻击为什么不算伤害呢？
                //算了
                entity.skActionCBList.Add(actid);
            }
            else
            {                
                //瞬时技能
                SkillAction(entity, skillData, index);
            }
        }
    }
    public void SkillAction(EntityBase caster,SkillCfg skillCfg,int index)
    {
        SkillActionCfg skillActionCfg = resSvc.GetSkillActionCfg(skillCfg.skillActionList[index]);

        int damage = skillCfg.skillDamageList[index];

        if(caster is EntityMonster)        
        {
            EntityPlayer target = caster.battleMgr.entitySelfPlayer;
            if (target == null)
            {
                return;
            }
            //判断距离、角度
            if (InRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius)
                && InAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle))
            {
                //if (caster is EntityMonster) PECommon.Log("entity is EntityMonster");
                //计算伤害
                CalcDamage(caster, target, skillCfg, damage);
                //PECommon.Log(damage.ToString());
            }
        }
        else if(caster is EntityPlayer)
        {
            //获取场景里所有怪物实体，遍历运算
            List<EntityMonster> monsterList = caster.battleMgr.GetEntityMonsters();
            for (int i = 0; i < monsterList.Count; i++)
            {
                EntityMonster em = monsterList[i];
                //判断距离、角度
                if (InRange(caster.GetPos(), em.GetPos(), skillActionCfg.radius)
                    &&InAngle(caster.GetTrans(),em.GetPos(),skillActionCfg.angle))
                {
                    //计算伤害
                    CalcDamage(caster,em,skillCfg,damage);
                }
            }
            
        }


    }

    System.Random rd = new System.Random();
    private void CalcDamage(EntityBase caster,EntityBase target,SkillCfg skillCfg ,int damage)
    {
        int dmgSum = damage;
        if (skillCfg.dmgType == DamageType.AD)
        {
            
            //计算闪避
            int dodgeNum = PETools.RDInt(1, 100, rd);
            if (dodgeNum <= target.Props.dodge)
            {
                //UI 显示闪避
                //PECommon.Log("闪避Rate:"+dodgeNum+"/"+target.Props.dodge);
                target.SetDodge();
                return;
            }

            //计算属性加成
            dmgSum += caster.Props.ad;

            //计算暴击
            int criticalNum = PETools.RDInt(1, 100, rd);
            if (criticalNum <= caster.Props.critical)
            {
                float criticalRate = 1+(PETools.RDInt(1, 100, rd)/100.0f);
                dmgSum = (int)criticalRate * dmgSum;
                //PECommon.Log("暴击Rate："+criticalNum+"/"+caster.Props.critical);
            }

            //计算穿甲
            int addef = (int) ((1 - caster.Props.pierce / 100.0f) * target.Props.addef);
            dmgSum -= addef;

            if (criticalNum <= caster.Props.critical)
            {
                target.SetCritical(dmgSum);
            }
        }
        else if (skillCfg.dmgType == DamageType.AP)
        {
            
            //计算属性加成
            dmgSum += caster.Props.ap;
            //计算魔法抗性
            dmgSum -= target.Props.apdef;
        }
        else {  }

        //最终伤害
        if (dmgSum < 0)
        {
            //PECommon.Log("怪物伤害："+dmgSum.ToString());
            //护甲再高也不能为0或负数，更不能直接return，否则没有受击动画和血量变化
            dmgSum = 1;            
        }
        //if (target is EntityPlayer) PECommon.Log("entity is EntityPlayer");
        //if (target is EntityMonster) PECommon.Log("entity is EntityMonster");
        target.SetHurt(dmgSum);
        if (target.HP <= dmgSum)
        {
            target.HP = 0;
            //目标死亡
            
            target.Die();

            if(target is EntityMonster)
            {
                target.battleMgr.RemoveMonster(target.Name);
            }
            else if (target is EntityPlayer)
            {
                target.battleMgr.EndBattle(false, 0);
                target.battleMgr.entitySelfPlayer = null;
            }
        }
        else
        {
            //PECommon.Log(target.ToString());
            target.HP -= dmgSum;

            //没有进入霸体状态并且是可以被中断的
            if(target.entityState == EntityState.None&& target.GetBreakState())
            {
                target.Hit();

            }
        }
    }
    private bool InRange(Vector3 from,Vector3 to,float range)
    {
        float dis = Vector3.Distance(from, to);
        if (dis <= range)
        {
            return true;
        }
        return false;
    }

    private bool InAngle(Transform trans,Vector3 to,float angle)
    {
        if (angle == 360)
        {
            return true;
        }
        else
        {
            Vector3 start = trans.forward;
            Vector3 dir = (to - trans.position).normalized;
            float ang = Vector3.Angle(start, dir);
            if (ang <= angle / 2)
            {
                return true;
            }
        }
        return false;
    }


}

