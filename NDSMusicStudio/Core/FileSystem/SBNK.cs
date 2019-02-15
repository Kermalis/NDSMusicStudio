using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.NDSMusicStudio.Core.FileSystem
{
    class SBNK
    {
        public class Instrument : IBinarySerializable
        {
            public class DefaultData
            {
                public InstrumentData.DataParam Param;
            }
            public class DrumSetData : IBinarySerializable
            {
                public byte MinNote;
                public byte MaxNote;
                public InstrumentData[] SubInstruments;

                public void Read(EndianBinaryReader er)
                {
                    MinNote = er.ReadByte();
                    MaxNote = er.ReadByte();
                    SubInstruments = new InstrumentData[MaxNote - MinNote + 1];
                    for (int i = 0; i < SubInstruments.Length; i++)
                    {
                        SubInstruments[i] = er.ReadObject<InstrumentData>();
                    }
                }
                public void Write(EndianBinaryWriter ew)
                {
                    throw new NotImplementedException();
                }
            }
            public class KeySplitData : IBinarySerializable
            {
                public byte[] KeyRegions;
                public InstrumentData[] SubInstruments;

                public void Read(EndianBinaryReader er)
                {
                    KeyRegions = er.ReadBytes(8);
                    int numSubInstruments = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        numSubInstruments++;
                        if (KeyRegions[i] == 0)
                        {
                            break;
                        }
                    }
                    SubInstruments = new InstrumentData[numSubInstruments];
                    for (int i = 0; i < numSubInstruments; i++)
                    {
                        SubInstruments[i] = er.ReadObject<InstrumentData>();
                    }
                }
                public void Write(EndianBinaryWriter ew)
                {
                    throw new NotImplementedException();
                }
            }

            public InstrumentType Type;
            public ushort DataOffset;
            public byte Padding;

            public object Data;

            public void Read(EndianBinaryReader er)
            {
                Type = (InstrumentType)er.ReadByte();
                DataOffset = er.ReadUInt16();
                Padding = er.ReadByte();

                long p = er.BaseStream.Position;
                switch (Type)
                {
                    case InstrumentType.Drum: Data = er.ReadObject<DrumSetData>(DataOffset); break;
                    case InstrumentType.KeySplit: Data = er.ReadObject<KeySplitData>(DataOffset); break;
                    default: Data = er.ReadObject<DefaultData>(DataOffset); break;
                }
                er.BaseStream.Position = p;
            }
            public void Write(EndianBinaryWriter ew)
            {
                throw new NotImplementedException();
            }
        }

        public FileHeader FileHeader; // "SBNK"
        public string BlockType; // "DATA"
        public int BlockSize;
        public byte[] Padding;
        public int NumInstruments;
        public Instrument[] Instruments;

        public SWAR[] SWARs = new SWAR[4];

        public SBNK(byte[] bytes)
        {
            using (var er = new EndianBinaryReader(new MemoryStream(bytes)))
            {
                FileHeader = er.ReadObject<FileHeader>();
                BlockType = er.ReadString(4);
                BlockSize = er.ReadInt32();
                Padding = er.ReadBytes(32);
                NumInstruments = er.ReadInt32();
                Instruments = new Instrument[NumInstruments];
                for (int i = 0; i < NumInstruments; i++)
                {
                    Instruments[i] = er.ReadObject<Instrument>();
                }
            }
        }

        public InstrumentData GetInstrumentData(int voice, int key)
        {
            if (voice >= NumInstruments)
            {
                return null;
            }
            else
            {
                switch (Instruments[voice].Type)
                {
                    case InstrumentType.PCM:
                    case InstrumentType.PSG:
                    case InstrumentType.Noise:
                        {
                            var d = (Instrument.DefaultData)Instruments[voice].Data;
                            // TODO: Better way?
                            return new InstrumentData
                            {
                                Type = Instruments[voice].Type,
                                Param = d.Param
                            };
                        }
                    case InstrumentType.Drum:
                        {
                            var d = (Instrument.DrumSetData)Instruments[voice].Data;
                            if (key < d.MinNote || key > d.MaxNote)
                            {
                                return null;
                            }
                            else
                            {
                                return d.SubInstruments[key - d.MinNote];
                            }
                        }
                    case InstrumentType.KeySplit:
                        {
                            var d = (Instrument.KeySplitData)Instruments[voice].Data;
                            for (int i = 0; i < 8; i++)
                            {
                                if (key <= d.KeyRegions[i])
                                {
                                    return d.SubInstruments[i];
                                }
                            }
                            return null;
                        }
                    default: return null;
                }
            }
        }

        public SWAVInfo GetWave(int swarIndex, int swavIndex)
        {
            SWAR swar = SWARs[swarIndex];
            if (swar != null && swavIndex < swar.NumWaves)
            {
                return swar.Waves[swavIndex];
            }
            else
            {
                return null;
            }
        }
    }
}
