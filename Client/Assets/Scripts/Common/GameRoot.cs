/****************************************************
    文件：GameRoot.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/4 17:13:51
	功能：游戏启动入口
*****************************************************/

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour {

    public static GameRoot Instance = null;
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start()
    {
        //GameRoot 不可销毁
        DontDestroyOnLoad(this);
        Instance = this;
        PECommon.Log("Game Start...");

        ClearUIRoot();
        Init();
    }
    
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for(int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        //服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();
        FubenSys fuben = GetComponent<FubenSys>();
        fuben.InitSys();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCity = GetComponent<MainCitySys>();
        mainCity.InitSys();
        BattleSys battle = GetComponent<BattleSys>();
        battle.InitSys();

        dynamicWnd.SetWndState();

        //进入登录场景并加载 UI
        login.EnterLogin();

    }

    //哪里需要加Tips，通过GameRoot加就行了，而不用在GameRoot里面加
    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }        
    }

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name)
    {
        PlayerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data)
    {
        PlayerData.coin = data.coin;
        PlayerData.critical = data.crystal;
        PlayerData.hp = data.hp;
        PlayerData.ad = data.ad;
        PlayerData.ap = data.ap;
        PlayerData.addef = data.addef;
        PlayerData.apdef = data.apdef;

        PlayerData.strongArr = data.strongArr;
    }
    public void SetPlayerDataByBuy(RspBuy data)
    {
        PlayerData.coin = data.coin;
        PlayerData.diamond = data.dimond;
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByPower(PshPower data)
    {
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTask(PshTaskPrgs data)
    {
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByFBStart(RspFBFight data)
    {
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByFBEnd(RspFBFightEnd data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.crystal = data.crystal;
        PlayerData.fuben = data.fuben;
    }

}
