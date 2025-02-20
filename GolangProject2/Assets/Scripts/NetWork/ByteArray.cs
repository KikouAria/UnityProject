﻿using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;

public class ByteArray
{
    private MemoryStream m_Stream = new MemoryStream();
    private BinaryReader m_Reader = null;
    private BinaryWriter m_Writer = null;

    public ByteArray()
    {
        Init();
    }
    public ByteArray(MemoryStream ms)
    {
        m_Stream = ms;
        Init();
    }
    public ByteArray(byte[] buffer)
    {
        m_Stream = new MemoryStream(buffer);
        Init();
    }
    private void Init()
    {
        m_Writer = new BinaryWriter(m_Stream);
        m_Reader = new BinaryReader(m_Stream);
    }
    public int Length
    {
        get { return (int)m_Stream.Length; }
    }

    public int Postion
    {
        get { return (int)m_Stream.Position; }
        set { m_Stream.Position = value; }
    }

    public byte[] Buffer
    {
        get { return m_Stream.GetBuffer(); }
    }

    internal MemoryStream MemoryStream { get { return m_Stream; } }
    //读取boolean
    public bool ReadBoolean()
    {
        return m_Reader.ReadBoolean();
    }
    //读取byte
    public byte ReadByte()
    {
        return m_Reader.ReadByte();
    }
    //读取byte[]
    public void ReadBytes(byte[] bytes, uint offset, uint length)
    {
        byte[] tmp = m_Reader.ReadBytes((int)length);
        for (int i = 0; i < tmp.Length; i++)
            bytes[i + offset] = tmp[i];
    }
    //读取double
    public double ReadDouble()
    {
        return m_Reader.ReadDouble();
    }
    //读取float
    public float ReadFloat()
    {
        byte[] bytes = m_Reader.ReadBytes(4);
        byte[] invertedBytes = new byte[4];
        for (int i = 3, j = 0; i >= 0; i--, j++)
        {
            invertedBytes[j] = bytes[i];
        }
        float value = BitConverter.ToSingle(invertedBytes, 0);
        return value;
    }
    //读取int
    public int ReadInt()
    {
        return m_Reader.ReadInt32();
    }
    //读取短整型
    public short ReadShort()
    {
        return m_Reader.ReadInt16();
    }
    //读取正byte
    public byte ReadUnsignedByte()
    {
        return m_Reader.ReadByte();
    }
    //读取正整型
    public uint ReadUnsignedInt()
    {
        return (uint)m_Reader.ReadInt32();
    }
    //读取正短整型
    public ushort ReadUnsignedShort()
    {
        return m_Reader.ReadUInt16();
    }
    //读取utf字符串到bytearray结尾
    public string ReadUTF()
    {
        return m_Reader.ReadString();
    }
    //读取指定长度的字符串
    public string ReadUTFBytes(uint length)
    {
        if (length == 0)
            return string.Empty;
        UTF8Encoding utf8 = new UTF8Encoding(false, true);
        byte[] encodedBytes = m_Reader.ReadBytes((int)length);
        string decodedString = utf8.GetString(encodedBytes, 0, encodedBytes.Length);
        return decodedString;
    }
    //写入boolean
    public void WriteBoolean(bool value)
    {
        m_Writer.BaseStream.WriteByte(value ? ((byte)1) : ((byte)0));
    }
    //写入byte
    public void WriteByte(byte value)
    {
        m_Writer.BaseStream.WriteByte(value);
    }
    //写入byte[]
    public void WriteBytes(byte[] buffer)
    {
        for (int i = 0; buffer != null && i < buffer.Length; i++)
            m_Writer.BaseStream.WriteByte(buffer[i]);
    }
    //写入byte[] 指定开始及结束位置--相当于截取byte[]中的部分
    public void WriteBytes(byte[] bytes, int offset, int length)
    {
        for (int i = offset; i < offset + length; i++)
            m_Writer.BaseStream.WriteByte(bytes[i]);
    }
    //写入double
    public void WriteDouble(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
    }
    //写入float
    public void WriteFloat(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
    }
    //写入高字节序byte[]
    private void WriteBigEndian(byte[] bytes)
    {
        if (bytes == null)
            return;
        for (int i = 0; i < bytes.Length; i++)
        {
            m_Writer.BaseStream.WriteByte(bytes[i]);
        }
    }
    //写入int32
    public void WriteInt32(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
    }
    //写入int
    public void WriteInt(int value)
    {
        WriteInt32(value);
    }
    //写入短整型
    public void WriteShort(int value)
    {
        byte[] bytes = BitConverter.GetBytes((ushort)value);
        WriteBigEndian(bytes);
    }
    //写入正整型
    public void WriteUnsignedInt(uint value)
    {
        WriteInt32((int)value);
    }
    //写入字符串
    public void WriteUTF(string value)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        int byteCount = utf8Encoding.GetByteCount(value);
        byte[] buffer = utf8Encoding.GetBytes(value);
        WriteShort(byteCount);
        if (buffer.Length > 0)
            m_Writer.Write(buffer);
    }
    //写入字符串
    public void WriteUTFBytes(string value)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        byte[] buffer = utf8Encoding.GetBytes(value);
        if (buffer.Length > 0)
            m_Writer.Write(buffer);
    }
    //写入带长度的字符串
    public void WriteStringBytes(string value)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        byte[] buffer = utf8Encoding.GetBytes(value);
        if (buffer.Length > 0)
        {
            m_Writer.Write(buffer.Length);
            m_Writer.Write(buffer);
        }
    }

    public void ClearStream()
    {
        m_Stream.Position = 0;
        m_Stream.SetLength(0);
    }
}