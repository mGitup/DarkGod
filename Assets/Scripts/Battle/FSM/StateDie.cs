/****************************************************
    文件：StateDie.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/7/5 11:2:38
	功能：死亡状态
*****************************************************/

public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Die;
        entity.RemoveSkillCB();
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionDie);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            if(entity is EntityMonster)
            {
                entity.GetCC().enabled = false;
                entity.SetActive(false);

            }
        }, Constants.DieAniLength);
    }
}

