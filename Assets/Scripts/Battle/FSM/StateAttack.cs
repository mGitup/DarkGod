/****************************************************
    文件：StateAttack.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：攻击状态
*****************************************************/

using UnityEngine;

public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        entity.curtSkillCfg = ResSvc.Instance.GetSkillCfg((int)args[0]);
        //PECommon.Log("Enter StateAttack");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Exit StateAttack");
        entity.ExitCurtSkill();
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if(entity is EntityPlayer)
        {
            entity.canReleaseSkill = false;
        }
        //技能效果表现与伤害运算
        entity.SkillAttack((int)args[0]);
        
        //PECommon.Log("Process StateAttack");
    }

}
