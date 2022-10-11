/****************************************************
    文件：BattleEndWnd.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/8/7 17:28:56
	功能：战斗结算界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleEndWnd : WindowRoot {

    #region UI Define
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;
    public Text txtTime;
    public Text txtRestHp;
    public Text txtReward;
    public Animation ani;
    #endregion

    private FBEndType endType = FBEndType.None;
    protected override void InitWnd()
    {
        base.InitWnd();

        RefreshUI();
    }
    private void RefreshUI()
    {
        switch (endType)
        {
            case FBEndType.Pause:
                SetActive(rewardTrans,false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                
                break;
            case FBEndType.Win:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject, false);
                SetActive(btnClose.gameObject, false);

               


                MapCfg mcfg = resSvc.GetMapCfg(fbid);
                int min = costTime / 60;
                int sec = costTime % 60;
                int coin = mcfg.coin;
                int exp = mcfg.exp;
                int crystal = mcfg.crystal;
                SetText(txtTime, "通关时间：" + min + ":" + sec);
                SetText(txtRestHp, "剩余血量：" + restHp);
                SetText(txtReward, "关卡奖励：" + Constants.Color(coin + "金币", TxtColor.Green) + Constants.Color(exp + "经验", TxtColor.Yellow) + Constants.Color(crystal + "水晶", TxtColor.Blue));

                timerSvc.AddTimeTask((int tid) =>
                {
                    SetActive(rewardTrans);
                    ani.Play();

                    timerSvc.AddTimeTask((int tid1) =>
                    {
                        audioSvc.PlayUIAudio(Constants.FBItem);
                        timerSvc.AddTimeTask((int tid2) =>
                        {
                            audioSvc.PlayUIAudio(Constants.FBItem);
                            timerSvc.AddTimeTask((int tid3) =>
                            {
                                audioSvc.PlayUIAudio(Constants.FBItem);
                                timerSvc.AddTimeTask((int tid4) =>
                                {
                                    audioSvc.PlayUIAudio(Constants.FBLogoEnter);
                                }, 300);
                            }, 270);
                        }, 270);
                    }, 325);
                }, 1000);
                break;
            case FBEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                audioSvc.PlayUIAudio(Constants.FBLose);
                break;
        }
    }

    public void ClickClose()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        SetWndState(false);
    }
    public void ClickExitBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        //进入主城，销毁当前战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        SetWndState(false );
    }
    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
        //进入主城，销毁当前战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        //打开副本界面
        FubenSys.Instance.EnterFuben();
    }
    public void SetWndType(FBEndType endType)
    {
        this.endType = endType;
    }


    private int fbid;
    private int costTime;
    private int restHp;

    public void SetBattleEndData(int fbid,int costTime,int restHp)
    {
        this.fbid = fbid;
        this.costTime = costTime;
        this.restHp = restHp;
    }

}

public enum FBEndType
{
    None,
    Pause,
    Win,
    Lose
}
