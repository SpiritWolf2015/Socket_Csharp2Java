using System;
using System.Text;

namespace TcpServer
{
    using System.IO;

    //李总群共享，夜莺QQ513994699

    public class ByteBuffer : ICloneable, IByteBuffer
    {

        public ByteBuffer(bool highEndian)
            : this()
        {
            this.highEndian = highEndian;
        }

        public virtual void position(int i)
        {
            readPos = writePos = i;
        }

        //JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void readFrom(InputStream inputstream) throws IOException
        public virtual void readFrom(Stream inputstream)
        {
            readFrom(inputstream, capacity() - Length());
        }

        public virtual void skipBytes(int i)
        {
            readPos += i;
        }

        //JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void readFrom(InputStream inputstream, int i) throws IOException
        public virtual void readFrom(Stream inputstream, int i)
        {
            ensureCapacity(writePos + i);
            for (int j = 0; j < i; )
            {
                int size = inputstream.Read(data, writePos + j, i - j);
                if (size == 0)
                {
                    i = j;
                    break;
                }
                j += size;
            }
            writePos += i;
        }

        public virtual int capacity()
        {
            return data.Length;
        }

        private void ensureCapacity(int i)
        {
            if (i > data.Length)
            {
                byte[] abyte0 = new byte[(i * 3) / 2];
                System.Array.Copy(data, 0, abyte0, 0, writePos);
                data = abyte0;
            }
        }

        //JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void writeTo(OutputStream outputstream) throws IOException
        public virtual void writeTo(Stream outputstream)
        {
            int i = available();
            outputstream.Write(data, readPos, i);
            readPos += i;
        }

        public virtual void pack()
        {
            if (readPos == 0)
                return;
            int i = available();
            for (int j = 0; j < i; j++)
                data[j] = data[readPos++];

            readPos = 0;
            writePos = i;
        }

        public virtual void writeByte(int i)
        {
            writeNumber(i, 1);
        }

        public virtual int readByte()
        {
            return ((int)data[readPos++] << 24) >> 24;
        }

        public virtual int readUnsignedByte()
        {
            return data[readPos++] & 0xff;
        }

        public virtual void read(byte[] abyte0, int i, int j, int k)
        {
            System.Array.Copy(data, k, abyte0, i, j);
        }

        public virtual int getReadPos()
        {
            return readPos;
        }

        public virtual void setReadPos(int i)
        {
            readPos = i;
        }

        public virtual void write(byte[] abyte0, int i, int j, int k)
        {
            ensureCapacity(k + j);
            System.Array.Copy(abyte0, i, data, k, j);
        }

        public virtual void writeChar(char c)
        {
            writeNumber(c, 2);
        }

        public virtual char readChar()
        {
            return (char)(int)(readNumber(2) & 65535L);
        }

        private void writeNumber(long l, int i)
        {
            if (highEndian)
                writeNumberHigh(l, i);
            else
                writeNumberLow(l, i);
        }

        private void writeNumberLow(long l, int i)
        {
            ensureCapacity(writePos + i);
            for (int j = 0; j < i; j++)
            {
                data[writePos++] = (byte)(int)l;
                l >>= 8;
            }

        }

        private void writeNumberHigh(long l, int i)
        {
            ensureCapacity(writePos + i);
            for (int j = i - 1; j >= 0; j--)
                data[writePos++] = (byte)(int)((ulong)l >> (j << 3));

        }

        private long readNumberHigh(int i)
        {
            long l = 0L;
            for (int j = i - 1; j >= 0; j--)
                l |= (long)(data[readPos++] & 255) << (j << 3);

            return l;
        }

        private long readNumberLow(int i)
        {
            long l = 0L;
            for (int j = 0; j < i; j++)
                l |= (long)(data[readPos++] & 255) << (j << 3);

            return l;
        }

        private long readNumber(int i)
        {
            if (highEndian)
                return readNumberHigh(i);
            else
                return readNumberLow(i);
        }

        public virtual byte[] getBytes()
        {
            byte[] abyte0 = new byte[Length()];
            System.Array.Copy(data, 0, abyte0, 0, abyte0.Length);
            return abyte0;
        }

        public virtual object Clone()
        {
            ByteBuffer bytebuffer = new ByteBuffer(writePos);
            System.Array.Copy(data, 0, bytebuffer.data, 0, writePos);
            bytebuffer.writePos = writePos;
            bytebuffer.readPos = readPos;
            return bytebuffer;
        }

        public virtual void writeAnsiString(String s)
        {
            if (s == null || s.Length == 0)
            {
                writeShort(0);
            }
            else
            {
                if (s.Length > 32767)
                    throw new System.ArgumentException("String over flow");
                byte[] abyte0 = Encoding.GetEncoding("GB2312").GetBytes(s);
                writeShort(abyte0.Length);
                writeBytes(abyte0);
            }
        }

        public virtual String readAnsiString()
        {
            int i = readUnsignedShort();
            if (i == 0)
            {
                return "";
            }
            else
            {
                unsafe
                {
                    byte[] abyte0 = readBytes(i);
                    sbyte[] data = new sbyte[abyte0.Length];
                    Buffer.BlockCopy(abyte0, 0, data, 0, data.Length);
                    fixed (sbyte* pData = data)
                    {
                        return new String(pData, 0, data.Length, Encoding.GetEncoding("GB2312"));
                    }
                }
            }
        }

        public virtual int Length()
        {
            return writePos;
        }

        public virtual void writeBoolean(bool flag)
        {
            writeByte(flag ? 1 : 0);
        }

        public virtual bool readBoolean()
        {
            return readByte() != 0;
        }

        public virtual void writeFloat(float i)
        {
            writeInt(floatBitsToInt(i));
        }

        public static int floatBitsToInt(float i)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(i), 0);
        }

        public virtual float readFloat()
        {
            int i = readInt();
            return intBitsToFloat(i);
        }

        public static float intBitsToFloat(int i)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
        }

        public virtual double readDouble()
        {
            long i = readLong();
            return BitConverter.Int64BitsToDouble(i);
        }
        public virtual void writeDouble(double i)
        {
            long d = BitConverter.DoubleToInt64Bits(i);
            writeLong(d);
        }
        public virtual void reset()
        {
            readPos = 0;
        }

        public virtual void writeLong(long l)
        {
            writeNumber(l, 8);
        }

        public ByteBuffer()
            : this(1024)
        {
        }

        public ByteBuffer(int i)
        {
            highEndian = true;
            if (i > 2457600)
            {
                throw new System.ArgumentException((new StringBuilder("data overflow ")).Append(i).ToString());
            }
            else
            {
                data = new byte[i];
                return;
            }
        }

        public ByteBuffer(byte[] abyte0)
            : this(abyte0, 0, abyte0.Length)
        {
        }

        public ByteBuffer(byte[] abyte0, int i, int j)
        {
            highEndian = true;
            data = abyte0;
            readPos = i;
            writePos = i + j;
        }

        public virtual void writeShortAnsiString(String s)
        {
            if (s == null || s.Length == 0)
            {
                writeByte(0);
            }
            else
            {
                byte[] abyte0 = Encoding.GetEncoding("GB2312").GetBytes(s);
                if (abyte0.Length > 255)
                    throw new System.ArgumentException("short String over flow");
                writeByte(abyte0.Length);
                writeBytes(abyte0);
            }
        }

        public virtual long readLong()
        {
            return readNumber(8);
        }

        public virtual void writeShort(int i)
        {
            writeNumber(i, 2);
        }

        public virtual int readShort()
        {
            return (short)(int)(readNumber(2) & 65535L);
        }

        public virtual void writeByteBuffer(IByteBuffer bytebuffer)
        {
            writeByteBuffer(bytebuffer, bytebuffer.available());
        }

        public virtual void writeByteBuffer(IByteBuffer bytebuffer, int i)
        {
            ensureCapacity(Length() + i);
            byte[] sourceData = bytebuffer.getRawBytes();
            int sourceReadPos = bytebuffer.getReadPos();
            System.Array.Copy(sourceData, sourceReadPos, data, writePos, i);
            setWritePos(writePos + i);
            bytebuffer.setReadPos(sourceReadPos + i);
        }

        public virtual void writeBytes(byte[] abyte0)
        {
            writeBytes(abyte0, 0, abyte0.Length);
        }

        public virtual byte[] readData()
        {
            int len = readInt();
            if (len < 0)
                return null;
            if (len > 2457600)
                throw new System.ArgumentException((new StringBuilder()).Append(this).Append(" readData, data overflow:").Append(len).ToString());
            else
                return readBytes(len);
        }

        public virtual void writeData(byte[] data)
        {
            writeData(data, 0, data == null ? 0 : data.Length);
        }

        public virtual void writeData(byte[] data, int pos, int len)
        {
            if (data == null)
            {
                writeInt(0);
                return;
            }
            else
            {
                writeInt(len);
                writeBytes(data);
                return;
            }
        }

        public virtual void writeBytes(byte[] abyte0, int i, int j)
        {
            ensureCapacity(writePos + j);
            for (int k = 0; k < j; k++)
                data[writePos++] = abyte0[i++];

        }

        public virtual byte[] readBytes(int i)
        {
            byte[] abyte0 = new byte[i];
            for (int j = 0; j < i; j++)
                abyte0[j] = data[readPos++];

            return abyte0;
        }

        public virtual int readUnsignedShort()
        {
            return (int)(readNumber(2) & 65535L);
        }

        public virtual String readShortAnsiString()
        {
            int i = readUnsignedByte();
            if (i == 0)
            {
                return "";
            }
            else
            {
                byte[] abyte0 = readBytes(i);
                sbyte[] data = new sbyte[abyte0.Length];
                Buffer.BlockCopy(abyte0, 0, data, 0, data.Length);
                unsafe
                {
                    fixed (sbyte* pData = data)
                    {
                        return new String(pData);
                    }
                }
            }
        }

        public virtual int available()
        {
            return writePos - readPos;
        }

        public override String ToString()
        {
            byte[] abyte0 = data;
            sbyte[] data0 = new sbyte[abyte0.Length];
            Buffer.BlockCopy(abyte0, 0, data0, 0, data0.Length);
            unsafe
            {
                fixed (sbyte* pData = data0)
                {
                    return new String(pData, 0, writePos);
                }
            }
        }

        public virtual int getWritePos()
        {
            return writePos;
        }

        public virtual void setWritePos(int i)
        {
            writePos = i;
        }

        public virtual byte[] getRawBytes()
        {
            return data;
        }

        public virtual void writeUTF(String s)
        {
            if (s == null)
                s = "";
            int i = s.Length;
            int j = 0;
            for (int k = 0; k < i; k++)
            {
                char c = s[k];
                if (c < 127)
                    j++;
                else
                    if (c > '\u07FF')
                        j += 3;
                    else
                        j += 2;
            }

            if (j > 65535)
                throw new System.ArgumentException((new StringBuilder("the String is too long:")).Append(i).ToString());
            ensureCapacity(writePos + j + 2);
            writeShort(j);
            for (int l = 0; l < i; l++)
            {
                char c1 = s[l];
                if (c1 < 127)
                    data[writePos++] = (byte)c1;
                else
                    if (c1 > '\u07FF')
                    {
                        data[writePos++] = (byte)(224 | c1 >> 12 & 15);
                        data[writePos++] = (byte)(128 | c1 >> 6 & 63);
                        data[writePos++] = (byte)(128 | c1 & 63);
                    }
                    else
                    {
                        data[writePos++] = (byte)(192 | c1 >> 6 & 31);
                        data[writePos++] = (byte)(128 | c1 & 63);
                    }
            }

        }

        public virtual String readUTF()
        {
            int i = readUnsignedShort();
            if (i == 0)
                return "";
            char[] ac = new char[i];
            int j = 0;
            for (int l = readPos + i; readPos < l; )
            {
                int k = data[readPos++] & 0xff;
                if (k < 127)
                    ac[j++] = (char)k;
                else
                    if (k >> 5 == 7)
                    {
                        byte byte0 = data[readPos++];
                        byte byte2 = data[readPos++];
                        ac[j++] = (char)((k & 15) << 12 | (byte0 & 63) << 6 | byte2 & 63);
                    }
                    else
                    {
                        byte byte1 = data[readPos++];
                        ac[j++] = (char)((k & 31) << 6 | byte1 & 63);
                    }
            }
            return new String(ac, 0, j);
        }

        public virtual void Clear()
        {
            writePos = readPos = 0;
        }

        public virtual void writeInt(int i)
        {
            writeNumber(i, 4);
        }

        public virtual int readInt()
        {
            return (int)(readNumber(4) & -1L);
        }

        public virtual int position()
        {
            return readPos;
        }

        public virtual bool isHighEndian()
        {
            return highEndian;
        }

        public virtual void setHighEndian(bool highEndian)
        {
            this.highEndian = highEndian;
        }

        private int readPos;
        private int writePos;
        private byte[] data;
        private bool highEndian;
        public const int MAX_DATA_LENGTH = 2457600;

        public byte this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }

        public void readBytes(IByteBuffer data0, int offset, int length)
        {
            data0.setWritePos(data0.getWritePos() + offset);
            data0.writeByteBuffer(this, length);
        }

        public void writeBytes(IByteBuffer data0, int offset, int length)
        {
            data0.setReadPos(data0.getReadPos() + offset);
            writeByteBuffer(data0, length);
        }

        public int bytesAvailable
        {
            get { return this.available(); }
        }
    }
}