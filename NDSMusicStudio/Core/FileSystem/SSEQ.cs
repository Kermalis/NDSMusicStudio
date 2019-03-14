using Kermalis.EndianBinaryIO;
using System.IO;

namespace Kermalis.NDSMusicStudio.Core.FileSystem
{
    class SSEQ
    {
        public FileHeader FileHeader; // "SSEQ"
        public string BlockType; // "DATA"
        public int BlockSize;
        public int DataOffset;

        public byte[] Data;

        public SSEQ(byte[] bytes)
        {
            using (var s = new MemoryStream(bytes))
            using (var er = new EndianBinaryReader(s))
            {
                FileHeader = er.ReadObject<FileHeader>();
                BlockType = er.ReadString(4);
                BlockSize = er.ReadInt32();
                DataOffset = er.ReadInt32();

                Data = er.ReadBytes(FileHeader.FileSize - DataOffset, DataOffset);
            }
        }
    }
}
