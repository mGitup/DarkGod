/****************************************************
    文件：MainCitySys.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:33:12
	功能：主城业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    private bool isNavGuide=false;

    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;
    public TaskWnd taskWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;
    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
        PECommon.Log("Init MainCitySys...");
    }

    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfg(Constants.MainCityMapID);

        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity...");

            //加载游戏主角
            LoadPlayer(mapData);

            //打开主城场景UI
            mainCityWnd.SetWndState();

            GameRoot.Instance.GetComponent<AudioListener>().enabled = false;

            //播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;

            //设置人物展示相机
            if(charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }

        });                
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.loadPrefab(PathDefine.AssassinCityPlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f,1.5f,1.5f);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;
    
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if(dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendMove);
        }

        playerCtrl.Dir = dir;
    }

    #region FubenWnd
    public void EnterFuben()
    {
        StopNavTask();
        FubenSys.Instance.EnterFuben();
    }
    #endregion

    #region InfoWnd
    public void OpenInfoWnd()
    {
        StopNavTask();
        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        //设置人物展示相机相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0); ;
        charCamTrans.localEulerAngles=new Vector3(0,180+playerCtrl.transform.localEulerAngles.y,0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        charCamTrans.gameObject.SetActive(false);
        infoWnd.SetWndState(false);
    }

    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.x;
    }
    public void SetPlayerRoate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }
    #endregion


    #region GuideWnd
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }
        //解析任务数据
        nav.enabled = true;
        if (curtTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd();
            }
            else
            {
                isNavGuide=true;
                nav.enabled = true;
                nav.speed = Constants.PlayMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtrl.SetBlend(Constants.BlendMove);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            IsArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.8f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;

            OpenGuideWnd();
        }
    }

    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    //打开引导界面
    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curtTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;
        GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curtTaskData.coin+" 经验+"+curtTaskData.exp,TxtColor.Blue));

        switch (curtTaskData.actID)
        {

            case 0:
                //与智者对话
                break;
            case 1:
                EnterFuben();
                break;
            case 2:
                //进入强化界面
                OpenStrongWnd();
                break;
            case 3:
                //进行体力购买
                OpenBuyWnd(0);
                break;
            case 4:
                //进行金币铸造
                OpenBuyWnd(1);
                break;
            case 5:
                //进入世界聊天
                OpenChatWnd();
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }
    #endregion

    #region StrongWnd
    public void OpenStrongWnd()
    {
        StopNavTask();
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        strongWnd.SetWndState();
    }
    public void CloseStrongWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        strongWnd.SetWndState(false);
    }

    public void RspStrong(GameMsg msg)
    {
        int zhanliPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int zhanliNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.Color("战力提升"+(zhanliNow-zhanliPre), TxtColor.Blue));

        strongWnd.UpdateUI();
        mainCityWnd.RefreshUI();
    }

    #endregion

    #region Chat Wnd
    public void OpenChatWnd()
    {
        StopNavTask();
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        chatWnd.SetWndState();
    }
    public void PshChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.chat);
    }
    #endregion


    #region BuyWnd
    public void OpenBuyWnd(int type)
    {
        StopNavTask();
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState();
    }

    public void RspBuy(GameMsg msg)
    {
        RspBuy rspBuy = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(rspBuy);
        GameRoot.AddTips("购买成功");
        mainCityWnd.RefreshUI();
        buyWnd.SetWndState(false);

        if (msg.pshTaskPrgs != null)
        { 
            GameRoot.Instance.SetPlayerDataByTask(msg.pshTaskPrgs);
            if (taskWnd.GetWndState())
            {
                taskWnd.RefreshUI();
            }
        }
    }

    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.pshPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        if (mainCityWnd.GetWndState())
        {
            mainCityWnd.RefreshUI();

        }
    }
    #endregion


    #region TaskWnd
    public void OpenTaskRewardWnd()
    {
        StopNavTask();
        taskWnd.SetWndState();
    }

    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.rspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTask(data);

        taskWnd.RefreshUI();
        mainCityWnd.RefreshUI();
    }
    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.pshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByTask(data);
        if (taskWnd.GetWndState())
        {
            taskWnd.RefreshUI();
        }
        
    }
    #endregion

}
