using System;
namespace TcpServer {
    public interface IByteBuffer {
        int available ( );
        int bytesAvailable { get; }
        int capacity ( );
        void Clear ( );
        byte[ ] getBytes ( );
        byte[ ] getRawBytes ( );
        int getReadPos ( );
        int getWritePos ( );
        bool isHighEndian ( );
        int Length ( );
        void pack ( );
        int position ( );
        void position (int i);
        void read (byte[ ] abyte0, int i, int j, int k);
        string readAnsiString ( );
        bool readBoolean ( );
        int readByte ( );
        void readBytes (IByteBuffer data0, int offset, int length);
        byte[ ] readBytes (int i);
        char readChar ( );
        byte[ ] readData ( );
        double readDouble ( );
        float readFloat ( );
        void readFrom (System.IO.Stream inputstream);
        void readFrom (System.IO.Stream inputstream, int i);
        int readInt ( );
        long readLong ( );
        int readShort ( );
        string readShortAnsiString ( );
        int readUnsignedByte ( );
        int readUnsignedShort ( );
        string readUTF ( );
        void reset ( );
        void setHighEndian (bool highEndian);
        void setReadPos (int i);
        void setWritePos (int i);
        void skipBytes (int i);
        byte this[int index] { get; set; }
        
        void write (byte[ ] abyte0, int i, int j, int k);
        void writeAnsiString (string s);
        void writeBoolean (bool flag);
        void writeByte (int i);
        void writeByteBuffer (IByteBuffer bytebuffer);
        void writeByteBuffer (IByteBuffer bytebuffer, int i);
        void writeBytes (IByteBuffer data0, int offset, int length);
        void writeBytes (byte[ ] abyte0);
        void writeBytes (byte[ ] abyte0, int i, int j);
        void writeChar (char c);
        void writeData (byte[ ] data);
        void writeData (byte[ ] data, int pos, int len);
        void writeDouble (double i);
        void writeFloat (float i);
        void writeInt (int i);
        void writeLong (long l);
        void writeShort (int i);
        void writeShortAnsiString (string s);
        void writeTo (System.IO.Stream outputstream);
        void writeUTF (string s);
    }
}
