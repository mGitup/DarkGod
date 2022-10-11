/****************************************************
    文件：EntityBase.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：逻辑实体基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase
{
    public AniState currentAniState = AniState.None;

    public StateMgr stateMgr=null;
    public SkillMgr skillMgr=null;
    public BattleMgr battleMgr=null;
    protected Controller controller=null;

    public bool canControl = true;//玩家控制角色
    public bool canReleaseSkill = true;//不能同时放两个技能

    public EntityState entityState = EntityState.None;

    private BattleProps props;
    public BattleProps Props
    {
        get
        {
            return props;
        }
        protected set
        {
            props = value;
        }
    }

    private string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            //通知 UI 层 TODO
            //PECommon.Log("hp change:"+hp+"to"+value);
            SetHPVal(hp, value);
            hp = value;
        }
    }

    public Queue<int>comboQue = new Queue<int>();
    public int nextSkillID = 0;

    public SkillCfg curtSkillCfg;

    //技能位移的回调ID
    public List<int>skMoveCBList = new List<int>();
    //技能伤害计算回调ID
    public List<int>skActionCBList = new List<int>();
    //连招被攻击中断，技能回调
    public int skEndCB = -1;

    public void SetCtrl(Controller ctrl)
    {
        controller = ctrl;
    }
    public void SetActive(bool active = true)
    {
        if(controller != null)
        {
            controller.gameObject.SetActive(active);
        }
    }

    public virtual void SetBattleProps(BattleProps props)
    {
        HP = props.hp;
        Props = props;
    }
    public void Born()
    {
        stateMgr.ChangeStatus(this, AniState.Born, null);
    }
    public void Move()
    {
        stateMgr.ChangeStatus(this, AniState.Move,null);
    }
    public void Idle()
    {
        stateMgr.ChangeStatus(this,AniState.Idle, null);
    }
    public void Attack(int skillId)
    {
        stateMgr.ChangeStatus(this, AniState.Attack,skillId);
    }
    public void Hit()
    {
        stateMgr.ChangeStatus(this, AniState.Hit, null);
    }
    public void Die()
    {
        stateMgr.ChangeStatus(this, AniState.Die, null);
    }

    //AI
    public virtual void TickAILogic()
    {

    }

    public virtual void SetBlend(float blend)
    {
        if (controller != null)
        {
            controller.SetBlend(blend);
        }
    }

    public void SetDir(Vector2 dir)
    {
        if(controller != null)
        {
            controller.Dir = dir;
        }
    }

    public virtual void SetAction(int act)
    {
        if (controller != null)
        {
            controller.SetAction(act);
        }
    }
    //特效
    public virtual void SetFX(string name,float destroy)
    {
        if (controller != null)
        {
            controller.SetFX(name, destroy);
        }
    }
    public virtual void SetSkillMoveState(bool move,float speed=0f)
    {
        if (controller != null)
        {
            controller.SetSkillMoveState(move, speed);
        }
    }

    public virtual void SetAtkRotation(Vector2 dir,bool offset = false)
    {
        if (controller != null)
        {
            if (offset)
            {
                controller.SetAtkRotationCam(dir);

            }
            else
            {
                controller.SetAtkRotationLocal(dir);
            }
        }
    }

    #region 战斗信息显示
    public virtual void SetDodge()
    {
        if (controller != null)
            GameRoot.Instance.dynamicWnd.SetDodge(Name);
    }
    public virtual void SetCritical(int critical)
    {
        if (controller != null)
            GameRoot.Instance.dynamicWnd.SetCritical(Name,critical);
    }
    public virtual void SetHurt(int hurt)
    {
        if (controller != null)
            GameRoot.Instance.dynamicWnd.SetHurt(Name,hurt);
    }
    public virtual void SetHPVal(int oldVal,int newVal)
    {
        if(controller != null)
        GameRoot.Instance.dynamicWnd.SetHPVal(Name,oldVal,newVal);
    }
    #endregion

    public virtual void SkillAttack(int skillID)
    {
        skillMgr.SkillAttack(this, skillID);
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
              
    }

    public virtual Vector3 GetPos()
    {
        return controller.transform.position;
    }
    public virtual Transform GetTrans()
    {
        return controller.transform;
    }

    public AnimationClip[] GetAniClips()
    {
        if (controller != null)
        {
            return controller.ani.runtimeAnimatorController.animationClips;
        }
        return null;
    }

    public AudioSource GetAudio()
    {
        return controller.GetComponent<AudioSource>();
    }
    public CharacterController GetCC()
    {
        return controller.GetComponent<CharacterController>();
    }

    public virtual bool GetBreakState()
    {
        return true;
    }

    public void ExitCurtSkill()
    {
        canControl = true;

        if(curtSkillCfg != null)
        {
            if (!curtSkillCfg.isBreak)
            {
                entityState = EntityState.None;
            }

            //连招数据更新
            if (curtSkillCfg.isCombo)
            {
                if (comboQue.Count > 0)
                {
                    nextSkillID = comboQue.Dequeue();
                }
                else
                {
                    nextSkillID = 0;
                }
            }

            curtSkillCfg = null;

        }
            SetAction(Constants.ActionDefault);

    }

    public virtual Vector2 CalctargetDir()
    {
        
        return Vector2.zero;
    }    

    public void RemoveMoveCB(int tid)
    {
        int index = -1;
        for(int i = 0; i < skMoveCBList.Count; i++)
        {
            if (skMoveCBList[i] == tid)
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            skMoveCBList.RemoveAt(index);
        }
    }
    public void RemoveActionCB(int tid)
    {
        int index = -1;
        for (int i = 0; i < skActionCBList.Count; i++)
        {
            if (skActionCBList[i] == tid)
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            skActionCBList.RemoveAt(index);
        }
    }

    public void RemoveSkillCB()
    {
        //如果（普攻）技能被中断，那么技能的位移也应该停止
        SetDir(Vector2.zero);
        SetSkillMoveState(false);

        //在玩家攻击动画结束前，如果被攻击(打断）则取消攻击伤害，
        //但是我觉得攻击早就作用到目标物体上了，只是动画还没完，这个时候被攻击为什么不算伤害呢？
        //怪物攻击速度慢，即使同时A，玩家早就打到它身上了，为什么不算伤害？
        for (int i = 0; i < skMoveCBList.Count; i++)
        {
            int tid = skMoveCBList[i];
            TimerSvc.Instance.DelTask(tid);
        }

        for (int i = 0; i < skActionCBList.Count; i++)
        {
            int tid = skActionCBList[i];
            TimerSvc.Instance.DelTask(tid);
        }

        //攻击被迫中断，删除定时回调
        if (skEndCB != -1)
        {
            TimerSvc.Instance.DelTask(skEndCB);
            skEndCB = -1;
        }
        skMoveCBList.Clear();
        skActionCBList.Clear();

        //清空连招
        if (nextSkillID != 0 || comboQue.Count > 0)
        {
            nextSkillID = 0;
            comboQue.Clear();

            battleMgr.lastAtkTime = 0;
            battleMgr.comboIndex = 0;
        }
    }

}

