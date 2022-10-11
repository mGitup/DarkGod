/****************************************************
    文件：FubenSys.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：副本业务系统
*****************************************************/
using PEProtocol;

public class FubenSys:SystemRoot
{
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;

    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
        PECommon.Log("Init FubenSys...");
    }

    public void EnterFuben()
    {
        OpenFubenWnd();
    }

    #region FubenWnd
    public void OpenFubenWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        fubenWnd.SetWndState();
    }
    #endregion

    public void RspFBFight(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByFBStart(msg.rspFBFight);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);

        fubenWnd.SetWndState(false);
        BattleSys.Instance.StartBattle(msg.rspFBFight.fbid);
    }
}

