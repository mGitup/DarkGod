/****************************************************
    文件：StateBorn.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/7/5 11:2:38
	功能：出生状态
*****************************************************/


public class StateBorn : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Born;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //播放出生动画
        entity.SetAction(Constants.ActionBorn);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constants.ActionDefault);
        }, 500);
    }
}

