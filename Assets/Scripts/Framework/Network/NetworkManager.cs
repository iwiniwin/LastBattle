using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.IO;
using UDK.Debug;
using System.Net;
using UDK.Event;

namespace UDK.Network
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        public enum EServerType
        {
            GateServer = 0,
            BalanceServer,
            LoginServer
        }

        private TcpClient mClient = null;
        private TcpClient mConnectingClient = null;
        private string mIP = "";
        private Int32 mPort = 40001;
        private Int32 mConnectTimes = 0;
        private EServerType mServerType = EServerType.BalanceServer;
        private float mCanConnectTime = 0f;
        private float mRecvOverTime = 0f;
        private float mRecvOverDelayTime = 2f;
        private float mConnectOverTime = 0f;
        private Int32 mConnectOverCount = 0;
        private Int32 mRecvOverCount = 0;
        private bool mCanReconnect = false;

        public byte[] mRecvBuffer = new byte[2 * 1024 * 1024];
        public Int32 mRecvPos = 0;
        IAsyncResult mRecvResult;
        IAsyncResult mConnectResult;

        // 发送数据stream
        public MemoryStream mSendStream = new MemoryStream();
        // 接收数据stream
        public List<int> mReceiveMsgIDs = new List<int>();
        public List<MemoryStream> mReceiveStreams = new List<MemoryStream>();
        public List<MemoryStream> mReceiveStreamsPool = new List<MemoryStream>();

        public NetworkManager()
        {
            for (int i = 0; i < 50; i++)
            {
                mReceiveStreamsPool.Add(new MemoryStream());
            }

            // 预先创建消息运行时类型模型
            // ProtoBuf.Serializer.PrepareSerializer<>();
        }

        ~NetworkManager()
        {
            foreach (MemoryStream ms in mReceiveStreams)
            {
                ms.Close();
            }
            foreach (MemoryStream ms in mReceiveStreamsPool)
            {
                ms.Close();
            }

            if (mSendStream != null)
            {
                mSendStream.Close();
            }

            if (mClient != null)
            {
                mClient.Client.Shutdown(SocketShutdown.Both);
                mClient.GetStream().Close();
                mClient.Close();
                mClient = null;
            }
        }

        public void Init(string ip, Int32 port, EServerType type)
        {
            DebugEx.Log("set network ip : " + ip + " port : " + port + " type : " + type);
            mIP = ip;
            mPort = port;
            mServerType = type;
            mConnectTimes = 0;
            mCanReconnect = true;
            mRecvPos = 0;
#if UNITY_EDITOR
            mRecvOverDelayTime = 20000f;
#endif
        }

        public void UnInit(){
            mCanReconnect = false;
        }

        public void Connect(){
            if(!mCanReconnect) return;

            if(mCanConnectTime > Time.time) return;

            if(mClient != null)
                throw new Exception("the socket is connecting, cannot connect again");

            if(mConnectingClient != null)
                throw new Exception("the socket is connecting, cannot connect again");

            IPAddress ipAddress = IPAddress.Parse(mIP);

            try
            {
                mConnectingClient = new TcpClient();
                mConnectResult = mConnectingClient.BeginConnect(mIP, mPort, null, null);
                mConnectOverCount = 0;
                mConnectOverTime = Time.time + 2;
            }
            catch (System.Exception exception)
            {
                DebugEx.LogError(exception.ToString());
                mClient = mConnectingClient;
                mConnectingClient = null;
                mConnectResult = null;
                OnConnectError(mClient, null);
                throw;
            }
        }

        public void Close(){
            if(mClient != null){
                OnClosed(mClient, null);
            }
        }

        public void Update(float deltaTime){
            if(mClient != null){
                DealWithMsg();

                if(mRecvResult != null){
                    if(mRecvOverCount > 200 && Time.time > mRecvOverTime){
                        DebugEx.LogError("recv data over 200, so close network");
                        Close();
                        return;
                    }

                    ++ mRecvOverCount;

                    if(mRecvResult.IsCompleted){
                        try
                        {
                            Int32 n32BytesRead = mClient.GetStream().EndRead(mRecvResult);
                            mRecvPos += n32BytesRead;
                            if(n32BytesRead == 0){
                                DebugEx.LogError("can't recv data now, so close network");
                                Close();
                                return;
                            }
                        }
                        catch (System.Exception exception)
                        {
                            DebugEx.LogError(exception.ToString());
                            Close();
                            return;
                        }

                        OnDataReceived(null, null);

                        if(mClient != null){
                            try
                            {
                                mRecvResult = mClient.GetStream().BeginRead(mRecvBuffer, mRecvPos, mRecvBuffer.Length - mRecvPos, null, null);
                                mRecvOverTime = Time.time + mRecvOverDelayTime;
                                mRecvOverCount = 0;
                            }
                            catch (System.Exception exception)
                            {
                                DebugEx.LogError(exception.ToString());
                                Close();
                                return;
                            }
                        }
                    }
                }
                if(mClient != null && mClient.Connected == false){
                    DebugEx.LogError("client is close by system, so close it now");
                    Close();
                    return;
                }
            }
            else if(mConnectingClient != null){
                if(mConnectOverCount > 200 && Time.time > mConnectOverTime){
                    DebugEx.LogError("can't connect, so close network");
                    mClient = mConnectingClient;
                    mConnectingClient = null;
                    mConnectResult = null;
                    OnConnectError(mClient, null);
                    return;
                }

                ++ mConnectOverCount;
                if(mConnectResult.IsCompleted){
                    mClient = mConnectingClient;
                    mConnectingClient = null;

                    mConnectResult = null;

                    if (mClient.Connected)
                    {
                        try
                        {
                            mClient.NoDelay = true;

                            mClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 2000);

                            mClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);

                            mClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                            mRecvResult = mClient.GetStream().BeginRead(mRecvBuffer, 0, mRecvBuffer.Length, null, null);

                            mRecvOverTime = Time.time + mRecvOverDelayTime;

                            mRecvOverCount = 0;

                            OnConnected(mClient, null);
                        }
                        catch (Exception exception)
                        {
                            DebugEx.LogError(exception.ToString());
                            Close();
                            return;
                        }
                    }
                    else
                    {
                        OnConnectError(mClient, null);
                    }
                }
            }
            else
            {
                Connect();
            }
        }

        public void SendMsg(ProtoBuf.IExtensible pMsg, Int32 n32MsgID)
        {
            if (mClient != null)
            {
                //清除stream
                mSendStream.SetLength(0);
                mSendStream.Position = 0;

               
                //序列到stream
                ProtoBuf.Serializer.Serialize(mSendStream, pMsg);
                // todo
//                 CMsg pcMsg = new CMsg((int)mSendStream.Length);
//                 pcMsg.SetProtocalID(n32MsgID);
//                 pcMsg.Add(mSendStream.ToArray(), 0, (int)mSendStream.Length);
//                 //ms.Close();
// #if UNITY_EDITOR
// #else
//                 try
//                 {
// #endif

// #if LOG_FILE && UNITY_EDITOR
//                 if (n32MsgID != 8192 && n32MsgID != 16385)
//                 {
//                     string msgName = "";
//                     if (Enum.IsDefined(typeof(GCToBS.MsgNum), n32MsgID))
//                     {
//                         msgName = ((GCToBS.MsgNum)n32MsgID).ToString();
//                     }
//                     else if (Enum.IsDefined(typeof(GCToCS.MsgNum), n32MsgID))
//                     {
//                         msgName = ((GCToCS.MsgNum)n32MsgID).ToString();
//                     }
//                     else if (Enum.IsDefined(typeof(GCToLS.MsgID), n32MsgID))
//                     {
//                         msgName = ((GCToLS.MsgID)n32MsgID).ToString();
//                     }
//                     else if (Enum.IsDefined(typeof(GCToSS.MsgNum), n32MsgID))
//                     {
//                         msgName = ((GCToSS.MsgNum)n32MsgID).ToString();
//                     }

//                     using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"E:\Log.txt", true))
//                     {
//                         sw.WriteLine(Time.time + "   发送消息：\t" + n32MsgID + "\t" + msgName);
//                     }
//                 }
// #endif
//                 mClient.GetStream().Write(pcMsg.GetMsgBuffer(), 0, (int)pcMsg.GetMsgSize());
#if UNITY_EDITOR
#else
                }
                catch (Exception exc)
                {
                    DebugEx.LogError(exc.ToString());
                    Close();
                }
#endif
            }
        }

        public void OnConnected(object sender, EventArgs e)
        {
            // todo
            // switch (mServerType)
            // {
                // case ServerType.BalanceServer:
                //     {
                //         CGLCtrl_GameLogic.Instance.BsOneClinetLogin();
                //     }
                //     break;
                // case ServerType.GateServer:
                //     {
                //         ++mConnectTimes;
                //         if (mConnectTimes > 1)
                //         {
                //             CGLCtrl_GameLogic.Instance.EmsgTocsAskReconnect();
                //         }
                //         else
                //         {
                //             CGLCtrl_GameLogic.Instance.GameLogin();
                //         }
                //         EventSystem.Broadcast(GameEvent.GameEvent_ConnectServerSuccess);
                //     }
                //     break;
                // case ServerType.LoginServer:
                //     {
                //         CGLCtrl_GameLogic.Instance.EmsgToLs_AskLogin();
                //     }
                //     break;
            // }
        }

        public void OnConnectError(object sender, ErrorEventArgs e)
        {
            DebugEx.Log("OnConnectError begin");
            
            try
            {
                mClient.Client.Shutdown(SocketShutdown.Both);
                mClient.GetStream().Close();          
                mClient.Close();
                mClient = null;
            }
            catch (Exception exc)
            {
                DebugEx.Log(exc.ToString());
            }
            mRecvResult = null;
            mClient = null;
            mRecvPos = 0;
            mRecvOverCount = 0;
            mConnectOverCount = 0;

            // EventSystem.Broadcast(EGameEvent.GameEvent_ConnectServerFail);

            DebugEx.Log("OnConnectError end");
        }

        public void OnClosed(object sender, EventArgs e)
        {
            // EventSystem.Broadcast(EGameEvent.GameEvent_ConnectServerFail);
            
            try
            {
                mClient.Client.Shutdown(SocketShutdown.Both);
                mClient.GetStream().Close();
                mClient.Close();
                mClient = null;
            }
            catch (Exception exc)
            {
                DebugEx.Log(exc.ToString());
            }

            mRecvResult = null;
            mClient = null;
            mRecvPos = 0;
            mRecvOverCount = 0;
            mConnectOverCount = 0;
            mReceiveStreams.Clear();
            mReceiveMsgIDs.Clear();
        }

        public void DealWithMsg()
        {
            while (mReceiveMsgIDs.Count>0 && mReceiveStreams.Count>0)
            {
                int type = mReceiveMsgIDs[0];
                System.IO.MemoryStream iostream = mReceiveStreams[0];
                mReceiveMsgIDs.RemoveAt(0);
                mReceiveStreams.RemoveAt(0);
#if UNITY_EDITOR
#else
                try
                {
#endif
#if LOG_FILE && UNITY_EDITOR
                if (type != 1)
                {
                    string msgName = "";
                    if (Enum.IsDefined(typeof(BSToGC.MsgID), type))
                    {
                        msgName = ((BSToGC.MsgID)type).ToString();
                    }
                    else if (Enum.IsDefined(typeof(LSToGC.MsgID), type))
                    {
                        msgName = ((LSToGC.MsgID)type).ToString();
                    }
                    else if (Enum.IsDefined(typeof(GSToGC.MsgID), type))
                    {
                        msgName = ((GSToGC.MsgID)type).ToString();
                    }

                   using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"E:\Log.txt", true))
                   {
                       sw.WriteLine(Time.time + "  收到消息：\t" + type + "\t" + msgName);
                    }
                }
#endif
                // todo
                // CGLCtrl_GameLogic.Instance.HandleNetMsg(iostream, type);
                if (mReceiveStreamsPool.Count<100)
                {
                    mReceiveStreamsPool.Add(iostream);
                }
                else
                {
                    iostream = null;
                }

#if UNITY_EDITOR
#else
                }
                catch (Exception ecp)
                {
                    DebugEx.LogError("Handle Error msgid: " + type);
                }
#endif
            }
        }

        public void OnDataReceived(object sender, DataEventArgs e)
        {
            int curPos = 0;
            while (mRecvPos - curPos >= 8)
            {
                int len = BitConverter.ToInt32(mRecvBuffer, curPos);
                int type = BitConverter.ToInt32(mRecvBuffer, curPos + 4);
                if (len > mRecvBuffer.Length)
                {
                    DebugEx.LogError("can't pause message" + "type=" + type + "len=" + len);
                    break;
                }
                if (len > mRecvPos - curPos)
                {
                    break;//wait net recv more buffer to parse.
                }
                //获取stream
                System.IO.MemoryStream tempStream = null;
                if (mReceiveStreamsPool.Count>0)
                {
                    tempStream = mReceiveStreamsPool[0];
                    tempStream.SetLength(0);
                    tempStream.Position = 0;
                    mReceiveStreamsPool.RemoveAt(0);
                }
                else
                {
                    tempStream = new System.IO.MemoryStream();
                }
                //往stream填充网络数据
                tempStream.Write(mRecvBuffer, curPos + 8, len - 8);
                tempStream.Position = 0;
                curPos += len;
                mReceiveMsgIDs.Add(type);
                mReceiveStreams.Add(tempStream);
            }
            if (curPos > 0)
            {
                mRecvPos = mRecvPos - curPos;

                if (mRecvPos < 0)
                {
                    DebugEx.LogError("mRecvPos < 0");
                }

                if (mRecvPos > 0)
                {
                    Buffer.BlockCopy(mRecvBuffer, curPos, mRecvBuffer, 0, mRecvPos);                    
                }
            }
        }
    }
}


