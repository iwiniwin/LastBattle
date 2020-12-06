/*
 * @Author: iwiniwin
 * @Date: 2020-12-06 17:37:14
 * @LastEditors: iwiniwin
 * @LastEditTime: 2020-12-06 19:07:18
 * @Description: 消息包
 一个消息包由包头和包体构成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace UDK.Network 
{
    public class Message
    {
        public struct MessageHeader {
            public Int32 size;
            public Int32 protocalId;  // 消息对应的协议号
        }

        public static Int32 MessageHeaderSize = 8;
        private MemoryBuffer mBuffer = null;
        private Int32 mMessageSize;

        public Message(Int32 size, Int32? protocalId = 0) {
            //create memory stream and initialization the member variables.
            mBuffer = new MemoryBuffer(size);
            mMessageSize = size;

            //assign default value to the message header.
            MessageHeader header;
            header.size = 0;
            header.protocalId = (Int32)protocalId;

            Add(header.size);
            Add(header.protocalId);
            mBuffer.ReadPos = MessageHeaderSize;
            mBuffer.WritePos = MessageHeaderSize;
        }

        public byte[] GetBuffer() {
            return mBuffer.GetBuffer();
        }

        public Int32 GetBufferSize() {
            return BitConverter.ToInt32(GetBuffer(), 0);
        }

        public void SetProtocalID(Int32 protocalId){
            Int32 size = sizeof(Int32);
            mBuffer.ReWrite(BitConverter.GetBytes(protocalId), size, size);
        }

        public bool Reset() {
            mBuffer.ReadPos = 0;
            mBuffer.WritePos = 0;
            MessageHeader header;
            header.size = 0;
            header.protocalId = 0;
            Add(header.size);
            Add(header.protocalId);
            mBuffer.ReadPos = MessageHeaderSize;
            mBuffer.WritePos = MessageHeaderSize;
            return true;
        }

        public void Add(Boolean value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(byte value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(char value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(UInt16 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(Int16 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(UInt32 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(Int32 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(UInt64 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(Int64 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(float value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(double value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public void Add(string value) {
            Int32 count;
            byte[] toBytes = ToUTF8Bytes(value, out count);
            mBuffer.Add(toBytes, 0, count);
            UpdateMessageSize();
        }

        public void Add(string value, Int32 fixSize) {
            Int32 count;
            byte[] toBytes = ToUTF8Bytes(value, out count);
            if(count > fixSize) {
                count = fixSize;
            }
            mBuffer.Add(toBytes, 0, count);
            Int32 addByteNum = fixSize - count;
            if(addByteNum > 0) {
                for(Int32 i = 0; i < addByteNum; i ++){
                    mBuffer.Add((byte)0);
                }
            }
            UpdateMessageSize();
        }

        public void Add(byte[] array, Int32 offset, Int32 size) {
            mBuffer.Add(array, offset, size);
            UpdateMessageSize();
        }

        public void Add(ref UInt64 value) {
            mBuffer.Add(value);
            UpdateMessageSize();
        }

        public Boolean GetBoolean() {
            return mBuffer.GetBoolean();
        }

        public byte GetByte() {
            return mBuffer.GetByte();
        }

        public char GetChar() {
            return mBuffer.GetChar();
        }

        public UInt16 GetUInt16() {
            return mBuffer.GetUInt16();
        }

        public Int16 GetInt16() {
            return mBuffer.GetInt16();
        }

        public UInt32 GetUInt32() {
            return mBuffer.GetUInt32();
        }

        public Int32 GetInt32() {
            return mBuffer.GetInt32();
        }

        public UInt64 GetUInt64() {
            return mBuffer.GetUInt64();
        }

        public Int64 GetInt64() {
            return mBuffer.GetInt64();
        }

        public float GetFloat() {
            return mBuffer.GetFloat();
        }

        public double GetDouble() {
            return mBuffer.GetDouble();
        }

        public bool GetByteArray(byte[] array, Int32 size) {
            return mBuffer.GetByteArray(array, size);
        }

        public bool GetCharArray(char[] array, Int32 size) {
            byte[] tempBytes = new byte[size];
            if(!mBuffer.GetByteArray(tempBytes, size)){
                return false;
            }
            byte[] bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, tempBytes);
            Array.Copy(bytes, array, size);
            return true;
        }

        public string GetString(Int32 size) {
            byte[] tempBytes = new byte[size];
            if(!mBuffer.GetByteArray(tempBytes, size)) {
                return null;
            }
            byte[] bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, tempBytes);
            return Encoding.Unicode.GetString(bytes);
        }

        public string GetString() {
            return mBuffer.GetString();
        }

        // 更新MemoryStream头部记录的消息包大小
        private void UpdateMessageSize() {
            Int32 protocalIdSize = sizeof(Int32);
            byte[] bytes = BitConverter.GetBytes(mBuffer.WritePos);
            mBuffer.ReWrite(bytes, 0, protocalIdSize);
        }

        private byte[] ToUTF8Bytes(string value, out Int32 count) {
            byte[] fromBytes = Encoding.Unicode.GetBytes(value);
            byte[] toBytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, fromBytes);
            count = Encoding.UTF8.GetCharCount(toBytes);
            return toBytes;
        }
    }
}


