using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace UDK.Network
{
    public delegate void ConnectAction();

    public delegate void SerializeAction(MemoryStream stream, object msg);

    public delegate void MessageHandler(MemoryStream stream, int id);

    public class NetworkManager : Singleton<NetworkManager>
    {
        enum EConnectState {
            None = 0,
            Connecting,  // 正在连接
            Connected,  // 已连接
        }

        // 是否启动连接
        public bool EnableConnect { get; set; } = false;

        private TcpClient mClient = null;
        private string mIP = "";
        private Int32 mPort = 40001;
        private Int32 mConnectTimes = 0;
        private float mCanConnectTime = 0f;
        // 预创建的数据接收流数量
        private int mReceiveStreamsPoolSize = 50;

        /* 数据接收 */

        // 数据接收结束时间
        private float mReceiveOverTime = 0f;
        // 数据接收历经次数（经过了多少帧）
        private Int32 mReceiveCount = 0;
        // 数据接收历经的最大帧数，在此数值内未接收成功，则接收失败
        private Int32 mMaxReceiveCount = 200;
        // 数据接收最大时长
        private float mMaxReceiveDuration = 2f;
        // 接收结果
        IAsyncResult mRecvResult;
        // 数据接收缓冲
        public byte[] mRecvBuffer = new byte[2 * 1024 * 1024];
        // mRecvBuffer中有效数据的位置，即已读取数据大小
        public Int32 mRecvPos = 0;

        /* 连接 */

        // 当前连接状态
        private EConnectState mConnectState = EConnectState.None;
        // 连接结束时间
        private float mConnectOverTime = 0f;
        // 连接最大时长，在此时间内未连接成功，则连接失败
        private float mMaxConnectDuration = 2f;
        // 连接历经次数（经过了多少帧）
        private Int32 mConnectCount = 0;
        // 连接历经的最大帧数，在此数值内未连接成功，则连接失败
        private Int32 mMaxConnectCount = 200;
        // 连接结果
        IAsyncResult mConnectResult;

        // 发送数据stream
        public MemoryStream mSendStream = new MemoryStream();

        // 消息队列，存放已读取消息的标识与数据流
        public Queue<KeyValuePair<int, MemoryStream>> mMessageQueue = new Queue<KeyValuePair<int, MemoryStream>>();

        // 数据读取流池，存放空闲的数据读取流
        public List<MemoryStream> mReceiveStreamsPool = new List<MemoryStream>();

        public event ConnectAction OnConnectServerSuccess;
        public event ConnectAction OnConnectServerFailed;
        public event ConnectAction OnConnectClosed;
        public event MessageHandler OnReceiveMessage;

        private SerializeAction mSerializer = null;

        public NetworkManager()
        {
            for (int i = 0; i < mReceiveStreamsPoolSize; i++)
            {
                mReceiveStreamsPool.Add(new MemoryStream());
            }
        }

        ~NetworkManager()
        {
            foreach (var ms in mMessageQueue)
            {
                ms.Value.Close();
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

        public void Init(SerializeAction serializer) {
            mSerializer = serializer;
            Close();
        }

        public void Configure(string ip, Int32 port)
        {
            DebugEx.Log("configure network -> ip : " + ip + " port : " + port);
            mIP = ip;
            mPort = port;
            mConnectTimes = 0;
            mRecvPos = 0;
#if UNITY_EDITOR
            mMaxReceiveDuration = 20000f;
#endif
            Close();
        }

        // 异步连接远程主机
        public void Connect()
        {
            if (!EnableConnect) return;
            if (mCanConnectTime > Time.time) return;

            if (mClient != null)
                throw new Exception("the socket is connecting, cannot connect again");

            DebugEx.Log("satrt connect ip : " + mIP + " port : " + mPort);

            IPAddress ipAddress = IPAddress.Parse(mIP);

            try
            {
                mClient = new TcpClient();
                mConnectResult = mClient.BeginConnect(mIP, mPort, null, null);
                mConnectCount = 0;
                mConnectOverTime = Time.time + mMaxConnectDuration;
                mConnectState = EConnectState.Connecting;
            }
            catch (System.Exception exception)
            {
                DebugEx.LogError("connect exception : " + exception.ToString());
                mConnectResult = null;
                OnConnectError(mClient, null);
                throw;
            }
        }

        public void Close()
        {
            EnableConnect = false;
            if (mClient != null)
            {
                OnClosed(mClient, null);
            }
        }

        public void Update(float deltaTime)
        {
            if (mConnectState == EConnectState.Connected)  // 已连接状态
            {
                DealWithMsg();

                if (mRecvResult != null)
                {
                    if (mReceiveCount > mMaxReceiveCount && Time.time > mReceiveOverTime)
                    {
                        DebugEx.LogError("recv data over 200, so close network");
                        Close();
                        return;
                    }

                    ++mReceiveCount;

                    if (mRecvResult.IsCompleted)
                    {
                        try
                        {
                            Int32 readSize = mClient.GetStream().EndRead(mRecvResult);  // 从流中读取的字节数
                            mRecvPos += readSize;
                            if (readSize == 0)
                            {
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

                        OnDataReceived();

                        if (mClient != null)
                        {
                            try
                            {
                                // 从流中读取数据存放到mRecvBuffer中，从mRecvPos位置开始存放
                                mRecvResult = mClient.GetStream().BeginRead(mRecvBuffer, mRecvPos, mRecvBuffer.Length - mRecvPos, null, null);
                                mReceiveOverTime = Time.time + mMaxReceiveDuration;
                                mReceiveCount = 0;
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
                if (mClient != null && mClient.Connected == false)
                {
                    DebugEx.LogError("client is close by system, so close it now");
                    Close();
                    return;
                }
            }
            else if (mConnectState == EConnectState.Connecting)  // 正在连接状态
            {
                if (mConnectCount > mMaxConnectCount && Time.time > mConnectOverTime)
                {
                    DebugEx.LogError("can't connect, so close network");
                    mConnectResult = null;
                    OnConnectError(mClient, null);
                    return;
                }
                // 记录连接历经的帧数
                ++mConnectCount;
                if (mConnectResult.IsCompleted)  // 连接完成
                {
                    mConnectState = EConnectState.Connected;

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

                            mReceiveOverTime = Time.time + mMaxReceiveDuration;

                            mReceiveCount = 0;

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

        // 发送消息
        public void SendMsg(object msg, Int32 n32MsgID)
        {
            if (mSerializer == null)
            {
                DebugEx.LogError("missing serialization method");
                return;
            }
            if (mClient != null)
            {
                //清除stream
                mSendStream.SetLength(0);
                mSendStream.Position = 0;

                //序列到stream
                mSerializer(mSendStream, msg);

                Message message = new Message((int)mSendStream.Length, n32MsgID);
                message.Add(mSendStream.ToArray(), 0, (int)mSendStream.Length);
#if UNITY_EDITOR
#else
                try
                {
#endif
                mClient.GetStream().Write(message.GetBuffer(), 0, (int)message.GetBufferSize());
#if UNITY_EDITOR
#else
                }
                catch (Exception exc)
                {
                    DebugEx.LogError("send msg exception : " + exc.ToString());
                    Close();
                }
#endif
            }
        }

        // 连接成功
        public void OnConnected(object sender, EventArgs e)
        {
            if(OnConnectServerSuccess != null)
                OnConnectServerSuccess();
        }

        public void OnConnectError(object sender, ErrorEventArgs e)
        {
            mConnectState = EConnectState.None;
            DebugEx.Log("connect error, ready to close");

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
            mReceiveCount = 0;
            mConnectCount = 0;

            if(OnConnectServerFailed != null)
                OnConnectServerFailed();
        }

        // 关闭连接
        public void OnClosed(object sender, EventArgs e)
        {
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

            EnableConnect = false;
            mConnectState = EConnectState.None;
            mRecvResult = null;
            mClient = null;
            mRecvPos = 0;
            mReceiveCount = 0;
            mConnectCount = 0;
            mMessageQueue.Clear();

            if(OnConnectClosed != null)
                OnConnectClosed();
        }

        // 处理读取到的一条消息
        public void DealWithMsg()
        {
            while (mMessageQueue.Count > 0)
            {
                KeyValuePair<int, MemoryStream> pair = mMessageQueue.Dequeue();
                int type = pair.Key;
                System.IO.MemoryStream iostream = pair.Value;
#if UNITY_EDITOR
#else
                try
                {
#endif
                if(OnReceiveMessage != null) {
                    OnReceiveMessage(iostream, type);
                }
                if (mReceiveStreamsPool.Count < mReceiveStreamsPoolSize)
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


        // 收到数据，解析并读取数据
        public void OnDataReceived()
        {
            int curPos = 0;
            while (mRecvPos - curPos >= 8)
            {
                // 数据长度
                int len = BitConverter.ToInt32(mRecvBuffer, curPos);  // 将数组中指定位置的四个字节转换为Int32
                // 数据类型
                int type = BitConverter.ToInt32(mRecvBuffer, curPos + 4);
                if (len > mRecvBuffer.Length)
                {
                    DebugEx.LogError("can't parse message" + "type=" + type + "len=" + len);
                    break;
                }
                if (len > mRecvPos - curPos)
                {
                    break; //wait net recv more buffer to parse.
                }
                //获取stream
                System.IO.MemoryStream tempStream = null;
                if (mReceiveStreamsPool.Count > 0)
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
                mMessageQueue.Enqueue(new KeyValuePair<int, MemoryStream>(type, tempStream));
            }
            if (curPos > 0)  // 读取了大小为curPos数据
            {
                mRecvPos -= curPos;  // 剩下未读取的数据量

                if (mRecvPos < 0)
                {
                    DebugEx.LogError("mRecvPos < 0");
                }

                if (mRecvPos > 0)
                {
                    // 将剩下未读取的数据移动到mRecvBuffer的头部
                    Buffer.BlockCopy(mRecvBuffer, curPos, mRecvBuffer, 0, mRecvPos);
                }
            }
        }
    }
}


