/****************************************************
    文件：IState.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：状态接口
*****************************************************/

public interface IState
{
    //可变参数
    void Enter(EntityBase entity,params object[]args);
    void Process(EntityBase entity, params object[] args);
    void Exit(EntityBase entity, params object[] args);

}

public enum AniState
{
    None,
    Born,
    Idle,
    Move,
    Attack,
    Hit,
    Die
}
