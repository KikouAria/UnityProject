using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class NetWorkScript
{
    private static NetWorkScript instance;
    private Socket socket;
    private string ip = "127.0.0.1";
    private int port = 10100;
    private byte[] Size = new byte[4];
    private List<SocketModel> messageList = new List<SocketModel>();
    protected static readonly object SyncRoot = new object();


    public static NetWorkScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetWorkScript();
            }
            return instance;
        }
    }

    public void Init()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);

            Debug.Log("服务器连接成功");
            socket.BeginReceive(Size, 0, Size.Length, SocketFlags.None, ReceiveCallBack, socket);
        }
        catch
        {
            Debug.Log("服务器连接失败");
        }
    }


    public void SendMessage(int type, int command, string message)
    {
        byte[] t = BitConverter.GetBytes(type);
        byte[] c = BitConverter.GetBytes(command);
        byte[] buffer = UTF8Encoding.UTF8.GetBytes(message);
        int messagelength = buffer.Length;
        byte[] Length = BitConverter.GetBytes(messagelength);
        //算出整个消息的大小
        int size = t.Length + c.Length + Length.Length + buffer.Length;
        byte[] z = BitConverter.GetBytes(size);
        List<byte> temp = new List<byte>(z.Length + t.Length + c.Length + Length.Length + buffer.Length);
        temp.AddRange(z);
        temp.AddRange(t);
        temp.AddRange(c);
        temp.AddRange(Length);
        temp.AddRange(buffer);
        byte[] data = temp.ToArray();
        try
        {
            socket.Send(data, 0, data.Length, 0);
        }
        catch
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log("网络连接断开");
        }
    }

    private void ReceiveCallBack(System.IAsyncResult ar)
    {
        try
        {
            //消息体长度
            int readCount = 0;
            readCount = socket.EndReceive(ar);
            while (readCount < 4)
            {
                readCount += socket.Receive(Size, readCount, Size.Length - readCount, 0);
            }
            ByteArray ioBuff = new ByteArray();
            byte[] temp = new byte[readCount];
            Buffer.BlockCopy(Size, 0, temp, 0, readCount);
            Size = new byte[4];
            ioBuff.WriteBytes(temp);
            ioBuff.Postion = 0;//设置流的当前位置为0
            onData(socket, ioBuff);//消息读取完成
            socket.BeginReceive(Size, 0, Size.Length, 0, ReceiveCallBack, socket);
        }
        catch
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log("网络连接断开");
        }
    }

    public void onData(Socket Oc, ByteArray ioBuff)
    {
        int dataSize = 0;
       //读取消息体大小
        dataSize = ioBuff.ReadInt();
        ByteArray ioData = ioBuff;
        int len = 0;
        byte[] buffer = new byte[dataSize];
        while (len < dataSize)
        {
            len += Oc.Receive(buffer, len, buffer.Length - len, 0);
        }
        ioData.ClearStream();
        ioData.WriteBytes(buffer);
        ioData.Postion = 0;
        int type = ioData.ReadInt();//表示消息类型  我们这里有两种
        int command = ioData.ReadInt();//模块内部协议---具体稍后描述
        len = ioData.ReadInt();

        string m = null;
        if (len > 0) { m = ioData.ReadUTFBytes((uint)len); }//这里开始就是读取服务器传过来的消息对象了 是一串json字符串
        //转换为Socket消息模型
        SocketModel model = new SocketModel();
        model.type = type;
        model.command = command;
        model.message = m;
        //Debug.Log(type + "   " + area + "  " + command + "length" + (16 + len));

        lock (SyncRoot)
        {
            //消息接收完毕后，存入收到消息队列
            messageList.Add(model);
        }
        ioData.ClearStream();
    }

    public SocketModel GetListData()
    {
        lock (SyncRoot)
        {
            if (messageList.Count > 0)
            {
                SocketModel model = messageList[0];
                messageList.RemoveAt(0);
                return model;
            }
            
        }
      
        return null;
    }

    /*
    public void SendMessageTest()
    {
        byte[] v = {1};
        socket.Send(v, 0, 1, 0);
    }
    */
}