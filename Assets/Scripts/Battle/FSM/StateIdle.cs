/****************************************************
    文件：StateIdle.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：待机状态
*****************************************************/

using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        entity.skEndCB = -1;
        //PECommon.Log("Enter StateIdle");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Exit StateIdle");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Process StateIdle");

        if(entity.nextSkillID != 0)//普攻连招
        {
            entity.Attack(entity.nextSkillID);
        }
        else
        {
            if (entity is EntityPlayer)
            {
                entity.canReleaseSkill = true;
            }

            if (entity.GetDirInput()!=Vector2.zero)
            {
                entity.Move();
                entity.SetDir(entity.GetDirInput());
            }
            else
            {
                entity.SetBlend(Constants.BlendIdle);            
            }

        }

    }
}
