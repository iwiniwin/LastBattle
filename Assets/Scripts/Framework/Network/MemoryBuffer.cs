using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

namespace UDK.Network  
{
    public class MemoryBuffer
    {
        private Int32 mBufferSize = 0;
        private MemoryStream mStream = null;
        public Int32 ReadPos {get; set;} = 0;
        public Int32 WritePos {get; set;} = 0;

        public MemoryBuffer(Int32 size) {
            mStream = new MemoryStream(size);
            mBufferSize = size;
        }

        public Int32 AddWritePos(Int32 size) {
            if(mBufferSize >= WritePos + size) {
                WritePos += size;
            }
            return WritePos;
        }

        public Int32 AddReadPos(Int32 size) {
            if(mBufferSize >= ReadPos + size) {
                ReadPos += size;
            }
            return ReadPos;
        }

        public Int32 GetBufferSize() {
            return mBufferSize;
        }

        public Boolean GetBoolean() {
            Boolean value = BitConverter.ToBoolean(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(Boolean);
            return value;
        }

        public byte[] GetBuffer() {
            return mStream.GetBuffer();
        }

        public byte GetByte() {
            mStream.Seek(ReadPos, SeekOrigin.Begin);
            byte value = Convert.ToByte(mStream.ReadByte());
            ReadPos ++;
            return value;
        }

        public char GetChar() {
            char value = BitConverter.ToChar(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(char);
            return value;
        }

        public UInt16 GetUInt16() {
            UInt16 value = BitConverter.ToUInt16(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(UInt16);
            return value;
        }

        public Int16 GetInt16() {
            Int16 value = BitConverter.ToInt16(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(Int16);
            return value;
        }

        public UInt32 GetUInt32() {
            UInt32 value = BitConverter.ToUInt32(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(UInt32);
            return value;
        }

        public Int32 GetInt32() {
            Int32 value = BitConverter.ToInt32(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(Int32);
            return value;
        }

        public UInt64 GetUInt64() {
            UInt64 value = BitConverter.ToUInt64(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(UInt64);
            return value;
        }

        public Int64 GetInt64() {
            Int64 value = BitConverter.ToInt64(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(Int64);
            return value;
        }

        public float GetFloat() {
            float value = BitConverter.ToSingle(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(float);
            return value;
        }

        public double GetDouble() {
            double value = BitConverter.ToDouble(mStream.GetBuffer(), ReadPos);
            ReadPos += sizeof(double);
            return value;
        }

        public bool GetByteArray(byte[] array, Int32 size) {
            if(ReadPos + size > WritePos){
                return false;
            }
            mStream.Seek(ReadPos, SeekOrigin.Begin);
            mStream.Read(array, 0, size);
            ReadPos += size;
            return true;
        }

        public bool GetCharArray(char[] array, Int32 size) {
            if(ReadPos + size * sizeof(char) > WritePos){
                return false;
            }
            for(Int32 i = 0; i < size; i ++){
                array[i] = GetChar();
            }
            return true;
        }

        public string GetString() {
            StringBuilder sb = new StringBuilder(10);
            char c = GetChar();
            while(c != '\0') {
                sb.Append(c);
                c = GetChar();
            }
            return sb.ToString();
        }

        public string GetString(Int32 fixSize) {
            Int32 tempSize = fixSize;
            if(tempSize == 0) {
                tempSize = 100;
            }
            Int32 getSize = 0;
            StringBuilder sb = new StringBuilder(tempSize);
            char c = GetChar();
            getSize ++;
            while(c != '\0') {
                sb.Append(c);
                c = GetChar();
                getSize ++;
                if(fixSize > 0 && getSize >= fixSize){
                    break;
                }
            }
            if(fixSize > 0 && getSize < fixSize) {
                Int32 writePos = fixSize - getSize;
                WritePos += writePos;
            }
            return sb.ToString();
        }

        public void Add(Boolean value) {
            AddData(BitConverter.GetBytes(value), sizeof(Boolean));
        }

        public void Add(byte value) {
            AddData(BitConverter.GetBytes(value), sizeof(byte));
        }

        public void Add(char value) {
            AddData(BitConverter.GetBytes(value), sizeof(char));
        }

        public void Add(UInt16 value) {
            AddData(BitConverter.GetBytes(value), sizeof(UInt16));
        }

        public void Add(Int16 value) {
            AddData(BitConverter.GetBytes(value), sizeof(Int16));
        }

        public void Add(UInt32 value) {
            AddData(BitConverter.GetBytes(value), sizeof(UInt32));
        }

        public void Add(Int32 value) {
            AddData(BitConverter.GetBytes(value), sizeof(Int32));
        }

        public void Add(UInt64 value) {
            AddData(BitConverter.GetBytes(value), sizeof(UInt64));
        }

        public void Add(Int64 value) {
            AddData(BitConverter.GetBytes(value), sizeof(Int64));
        }

        public void Add(float value) {
            AddData(BitConverter.GetBytes(value), sizeof(float));
        }

        public void Add(double value) {
            AddData(BitConverter.GetBytes(value), sizeof(double));
        }

        public void Add(string value) {
            for(Int32 i = 0; i < value.Length; i ++){
                Add(value[i]);
            }
            Add('\0');  // '\0'表示字符串结束
        }

        public void Add(string value, UInt32 fixSize) {
            for(Int32 i = 0; i < fixSize; i ++){
                if(i < value.Length){
                    Add(value[i]);
                }else{
                    Add('\0');
                }
            }
        }

        public void Add(char[] array, Int32 size) {
            for(Int32 i = 0; i < size; i ++){
                Add(array[i]);
            }
        }

        public void Add(byte[] array, Int32 offset, Int32 size) {
            for(Int32 i = offset; i < size; i ++) {
                if(i < array.Length) {
                    Add(array[i]);
                }else{
                    Add((byte)0);
                }
            }
        }

        public Int32 Resize(Int32 newSize) {
            if(newSize <= 0) return 0;
            if(WritePos >= newSize) return 0;

            MemoryStream old = mStream;
            mStream = new MemoryStream(newSize);
            mBufferSize = newSize;
            mStream.Seek(0, SeekOrigin.Begin);
            mStream.Write(old.GetBuffer(), 0, WritePos);
            return newSize;
        }

        public Boolean ReWrite(byte[] array, Int32 pos, Int32 size) {
            if(size + pos > mBufferSize) {
                return false;
            }
            mStream.Seek(pos, SeekOrigin.Begin);
            mStream.Write(array, 0, size);
            return true;
        }

        private void AddData(byte[] bytes, Int32 size) {
            if(mBufferSize <= WritePos + size) {
                Int32 increaseSize = size * 2;
                Resize(mBufferSize + increaseSize);
            }

            // write data
            mStream.Seek(WritePos, SeekOrigin.Begin);
            mStream.Write(bytes, 0, size);
            WritePos += size;
        }
    }
}


