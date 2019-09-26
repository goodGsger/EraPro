using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Framework
{
    public class ByteArray
    {
        private MemoryStream _memoryStream;
        private BinaryWriter _dataOutput;
        private BinaryReader _dataInput;

        /// <summary>
        /// Initializes a new instance of the ByteArray class.
        /// </summary>
        public ByteArray()
        {
            _memoryStream = new MemoryStream();
            _dataOutput = new BinaryWriter(_memoryStream);
            _dataInput = new BinaryReader(_memoryStream);
        }
        public ByteArray(int size)
        {
            _memoryStream = new MemoryStream(size);
            _dataOutput = new BinaryWriter(_memoryStream);
            _dataInput = new BinaryReader(_memoryStream);
        }
        /// <summary>
        /// Initializes a new instance of the ByteArray class.
        /// </summary>
        /// <param name="ms">The MemoryStream from which to create the current ByteArray.</param>
        public ByteArray(MemoryStream ms)
        {
            _memoryStream = ms;
            _dataOutput = new BinaryWriter(_memoryStream);
            _dataInput = new BinaryReader(_memoryStream);
        }
        /// <summary>
        /// Initializes a new instance of the ByteArray class.
        /// </summary>
        /// <param name="buffer">The array of unsigned bytes from which to create the current ByteArray.</param>
        public ByteArray(byte[] buffer)
        {
            _memoryStream = new MemoryStream();
            _memoryStream.Write(buffer, 0, buffer.Length);
            _memoryStream.Position = 0;
            _dataOutput = new BinaryWriter(_memoryStream);
            _dataInput = new BinaryReader(_memoryStream);
        }
        /// <summary>
        /// Gets the length of the ByteArray object, in bytes.
        /// </summary>
        public uint Length
        {
            get
            {
                return (uint)_memoryStream.Length;
            }
        }

        /// <summary>
        /// Gets or sets the current position, in bytes, of the file pointer into the ByteArray object.
        /// </summary>
        public int Position
        {
            get { return (int)_memoryStream.Position; }
            set { _memoryStream.Position = value; }
        }

        public uint BytesAvailable
        {
            get { return (uint)_memoryStream.Length - (uint)_memoryStream.Position; }
        }

        public void Clear()
        {
            _memoryStream.SetLength(0);
        }

        /// <summary>
        /// Returns the array of unsigned bytes from which this ByteArray was created.
        /// </summary>
        /// <returns>The byte array from which this ByteArray was created, or the underlying array if a byte array was not provided to the ByteArray constructor during construction of the current instance.</returns>
        public byte[] GetBuffer()
        {
            return _memoryStream.GetBuffer();
        }
        //
        /// <summary>
        /// Gets the MemoryStream from which this ByteArray was created.
        /// </summary>
        internal MemoryStream MemoryStream { get { return _memoryStream; } }

        #region IDataInput Members

        /// <summary>
        /// Reads a Boolean from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            return _dataInput.ReadBoolean();
        }
        /// <summary>
        /// Reads a signed byte from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return _dataInput.ReadByte();
        }
        /// <summary>
        /// Reads length bytes of data from the byte stream or byte array. 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public byte[] ReadBytes(int count)
        {
            return _dataInput.ReadBytes(count);
        }

        /// <summary>
        /// Reads length bytes of data from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            return ReadBytes((int)Length - Position);
        }
        /// <summary>
        /// Reads an IEEE 754 double-precision floating point number from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            byte[] bytes = _dataInput.ReadBytes(8);
            double value;
            if (BigEndian)
            {
                byte[] reverse = new byte[8];

                //Grab the bytes in reverse order 
                for (int i = 7, j = 0; i >= 0; i--, j++)
                {
                    reverse[j] = bytes[i];
                }
                value = BitConverter.ToDouble(reverse, 0);
                return value;
            }
            value = BitConverter.ToDouble(bytes, 0);
            return value;
        }
        /// <summary>
        /// Reads an IEEE 754 single-precision floating point number from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            float value;
            byte[] bytes = _dataInput.ReadBytes(4);
            if (BigEndian)
            {
                byte[] invertedBytes = new byte[4];
                //Grab the bytes in reverse order from the backwards index
                for (int i = 3, j = 0; i >= 0; i--, j++)
                {
                    invertedBytes[j] = bytes[i];
                }
                value = BitConverter.ToSingle(invertedBytes, 0);
                return value;
            }
            value = BitConverter.ToSingle(bytes, 0);
            return value;
        }
        /// <summary>
        /// Reads a signed 32-bit integer from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            int value;

            byte[] bytes = _dataInput.ReadBytes(4);
            if (BigEndian)
            {

                value = (int)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
                return value;
            }
            value = BitConverter.ToInt32(bytes, 0);
            return value;
        }

        public uint ReadUInt()
        {
            uint value;

            byte[] bytes = _dataInput.ReadBytes(4);
            if (BigEndian)
            {

                value = (uint)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
                return value;
            }
            value = BitConverter.ToUInt32(bytes, 0);
            return value;
        }

        /// <summary>
        /// Reads a signed 16-bit integer from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            //Read the next 2 bytes, shift and add.
            byte[] bytes = _dataInput.ReadBytes(2);
            if (BigEndian)
            {
                return (short)((bytes[0] << 8) | bytes[1]);
            }
            return (short)((bytes[1] << 8) | bytes[0]);
        }

        public ushort ReadUShort()
        {
            //Read the next 2 bytes, shift and add.
            byte[] bytes = _dataInput.ReadBytes(2);
            if (BigEndian)
            {
                return (ushort)((bytes[0] << 8) | bytes[1]);
            }
            return (ushort)((bytes[1] << 8) | bytes[0]);
        }
        /// <summary>
        /// Reads a UTF-8 string from the byte stream or byte array. 
        /// </summary>
        /// <returns></returns>
        public string ReadUTF()
        {
            int length = ReadShort();
            return ReadUTFBytes(length);
        }
        /// <summary>
        /// Reads a sequence of length UTF-8 bytes from the byte stream or byte array, and returns a string. 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadUTFBytes(int length)
        {
            if (length == 0)
                return string.Empty;

            byte[] encodedBytes = this.ReadBytes(length);
            //
            string decodedString = utf8.GetString(encodedBytes, 0, length);
            return decodedString;
        }
        static UTF8Encoding utf8 = new UTF8Encoding(false, true);
        #endregion

        #region IDataOutput Members

        /// <summary>
        /// Writes a Boolean value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteBoolean(bool value)
        {
            _dataOutput.Write(value ? ((byte)1) : ((byte)0));
        }
        /// <summary>
        /// Writes a byte.
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            _dataOutput.Write(value);
        }
        /// <summary>
        /// Writes a sequence of length bytes from the specified byte array, bytes, starting offset(zero-based index) bytes into the byte stream.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void WriteBytes(byte[] bytes, int offset, int length)
        {
            _dataOutput.Write(bytes, offset, length);
        }
        public void WriteBytes(byte[] bytes)
        {
            _dataOutput.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// Writes an IEEE 754 double-precision (64-bit) floating point number.
        /// </summary>
        /// <param name="value"></param>
        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteBigEndian(bytes);
        }
        /// <summary>
        /// Writes an IEEE 754 single-precision (32-bit) floating point number.
        /// </summary>
        /// <param name="value"></param>
        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteBigEndian(bytes);
        }
        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="value"></param>
        /// 
        byte[] intbytes = new byte[4];
        public void WriteInt(int u)
        {
            //byte[] intbytes = BitConverter.GetBytes(u);
            intbytes[0] = (byte)(u);
            intbytes[1] = (byte)(u >> 8);
            intbytes[2] = (byte)(u >> 16);
            intbytes[3] = (byte)(u >> 24);
            WriteBigEndian(intbytes);
        }

        public void WriteUInt(uint u)
        {
            //byte[] intbytes = BitConverter.GetBytes(u);
            intbytes[0] = (byte)(u);
            intbytes[1] = (byte)(u >> 8);
            intbytes[2] = (byte)(u >> 16);
            intbytes[3] = (byte)(u >> 24);
            WriteBigEndian(intbytes);
        }
        /// <summary>
        /// Writes a 16-bit integer.
        /// </summary>
        /// <param name="value"></param>
        /// 
        byte[] shortbytes = new byte[2];
        public void WriteShort(short u)
        {
            //byte[] bytes = BitConverter.GetBytes(u);
            shortbytes[0] = (byte)(u);
            shortbytes[1] = (byte)(u >> 8);
            WriteBigEndian(shortbytes);
        }

        public void WriteUShort(ushort u)
        {
            //byte[] bytes = BitConverter.GetBytes(u);
            shortbytes[0] = (byte)(u);
            shortbytes[1] = (byte)(u >> 8);
            WriteBigEndian(shortbytes);
        }
        /// <summary>
        /// Writes a UTF-8 string to the byte stream. 
        /// The length of the UTF-8 string in bytes is written first, as a 16-bit integer, followed by 
        /// the bytes representing the characters of the string.
        /// </summary>
        /// <param name="value"></param>
        ///
        Encoding utf8Encoding = Encoding.UTF8;
        public void WriteUTF(string value)
        {
            int byteCount = utf8Encoding.GetByteCount(value);
            WriteShort((short)byteCount);
            WriteUTFBytes(value);

        }
        /// <summary>
        /// Writes a UTF-8 string. Similar to writeUTF, but does not prefix the string with a 16-bit length word.
        /// </summary>
        /// <param name="value"></param>
        public void WriteUTFBytes(string value)
        {
            byte[] buffer = utf8Encoding.GetBytes(value);
            if (buffer.Length > 0)
                this.MemoryStream.Write(buffer, 0, buffer.Length);
        }
        public static bool BigEndian = false;
        private void WriteBigEndian(byte[] bytes)
        {
            if (bytes == null)
                return;
            if (BigEndian)
            {
                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    this.MemoryStream.WriteByte(bytes[i]);
                }
            }
            else
            {
                WriteBytes(bytes, 0, bytes.Length);
            }
        }
        #endregion
    }
}
