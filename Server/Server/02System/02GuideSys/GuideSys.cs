/****************************************************
	文件：GuideSys.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2018/12/17 6:40   	
	功能：引导业务系统
*****************************************************/

using PEProtocol;

public class GuideSys {
    private static GuideSys instance = null;
    public static GuideSys Instance {
        get {
            if (instance == null) {
                instance = new GuideSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init() {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("GuideSys Init Done.");
    }

    public void ReqGuide(MsgPack pack) {
        ReqGuide data = pack.msg.reqGuide;

        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspGuide
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        GuideCfg gc = cfgSvc.GetGuideCfg(data.guideid);

        //更新引导ID
        if (pd.guideid == data.guideid) {

            //检测是否为智者点拔任务
            if (pd.guideid == 1001) {
                TaskSys.Instance.CalcTaskPrgs(pd, 1);
            }
            pd.guideid += 1;

            //更新玩家数据
            pd.coin += gc.coin;
            PECommon.CalcExp(pd, gc.exp);

            if (!cacheSvc.UpdatePlayerData(pd.id, pd)) {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else {
                msg.rspGuide = new RspGuide {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp
                };
            }
        }
        else {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }


}
