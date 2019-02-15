using Kermalis.EndianBinaryIO;

namespace Kermalis.NDSMusicStudio.Core.FileSystem
{
    class InstrumentData
    {
        public class DataParam
        {
            [BinaryArrayFixedLength(2)]
            public ushort[] Info;
            public byte BaseKey;
            public byte Attack;
            public byte Decay;
            public byte Sustain;
            public byte Release;
            public byte Pan;
        }

        public InstrumentType Type;
        public byte Padding;
        public DataParam Param;
    }
}
