using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.NDSMusicStudio.Core.FileSystem
{
    class SWAVInfo : IBinarySerializable
    {
        public SWAVFormat Format;
        public bool DoesLoop;
        public ushort SampleRate;
        public ushort Timer; // (ARM7_CLOCK / SampleRate) [ARM7_CLOCK: 33.513982MHz / 2 = 1.6756991 E +7]
        public ushort LoopOffset;
        public int Length;

        public byte[] Samples;

        public void Read(EndianBinaryReader er)
        {
            Format = (SWAVFormat)er.ReadByte();
            DoesLoop = er.ReadBoolean();
            SampleRate = er.ReadUInt16();
            Timer = er.ReadUInt16();
            LoopOffset = er.ReadUInt16();
            Length = er.ReadInt32();

            Samples = er.ReadBytes((LoopOffset * 4) + (Length * 4));
        }
        public void Write(EndianBinaryWriter ew)
        {
            throw new NotImplementedException();
        }
    }
}
