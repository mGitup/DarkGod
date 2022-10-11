/****************************************************
	文件：NetSvc.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2018/12/07 4:40   	
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack {
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session, GameMsg msg) {
        this.session = session;
        this.msg = msg;
    }
}

public class NetSvc {
    private static NetSvc instance = null;
    public static NetSvc Instance {
        get {
            if (instance == null) {
                instance = new NetSvc();
            }
            return instance;
        }
    }
    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init() {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);

        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsgQue(ServerSession session, GameMsg msg) {
        lock (obj) {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    public void Update() {
        if (msgPackQue.Count > 0) {
            //PECommon.Log("QueCount:" + msgPackQue.Count);
            lock (obj) {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack) {
        switch ((CMD)pack.msg.cmd) {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(pack);
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
            case CMD.ReqGuide:
                GuideSys.Instance.ReqGuide(pack);
                break;
            case CMD.ReqStrong:
                StrongSys.Instance.ReqStrong(pack);
                break;
            case CMD.SndChat:
                ChatSys.Instance.SndChat(pack);
                break;
            case CMD.ReqBuy:
                BuySys.Instance.ReqBuy(pack);
                break;
            case CMD.ReqTakeTaskReward:
                TaskSys.Instance.ReqTakeTaskReward(pack);
                break;
            case CMD.ReqFBFight:
                FubenSys.Instance.ReqFBFight(pack);
                break;
            case CMD.ReqFBFightEnd:
                FubenSys.Instance.ReqFBFightEnd(pack);
                break;
        }
    }
}
