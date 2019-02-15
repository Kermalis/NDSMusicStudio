using Kermalis.NDSMusicStudio.Util;
using NAudio.Wave;
using System.Linq;

namespace Kermalis.NDSMusicStudio.Core
{
    class SoundMixer
    {
        public static SoundMixer Instance { get; } = new SoundMixer();

        public float MasterVolume;

        public readonly bool[] Mutes;
        public Channel[] Channels;

        readonly BufferedWaveProvider buffer;
        readonly IWavePlayer @out;

        private SoundMixer()
        {
            Channels = new Channel[0x10];
            for (byte i = 0; i < 0x10; i++)
            {
                Channels[i] = new Channel(i);
            }

            Mutes = new bool[0x10];

            buffer = new BufferedWaveProvider(new WaveFormat(65456, 16, 2))
            {
                DiscardOnBufferOverflow = true,
                BufferLength = 0x5540
            };
            @out = new WasapiOut();
            @out.Init(buffer);
            @out.Play();
        }

        public Channel AllocateChannel(InstrumentType type, Track track)
        {
            ushort allowedChannels;
            switch (type)
            {
                case InstrumentType.PCM: allowedChannels = 0b1111111111111111; break; // All channels
                case InstrumentType.PSG: allowedChannels = 0b0011111100000000; break; // Only 8 9 10 11 12 13
                case InstrumentType.Noise: allowedChannels = 0b1100000000000000; break; // Only 14 15
                default: return null;
            }
            Channel nChn = null;
            IOrderedEnumerable<Channel> byOwner = Channels.Where(c => (allowedChannels & (1 << c.Index)) != 0).OrderByDescending(c => c.Owner == null ? 0xFF : c.Owner.Index);
            foreach (Channel i in byOwner) // Find free
            {
                if (i.Owner == null)
                {
                    nChn = i;
                    break;
                }
            }
            if (nChn == null) // Find releasing
            {
                foreach (Channel i in byOwner)
                {
                    if (i.State == EnvelopeState.Release)
                    {
                        nChn = i;
                        break;
                    }
                }
            }
            if (nChn == null) // Find prioritized
            {
                foreach (Channel i in byOwner)
                {
                    if (track.Priority > i.Owner.Priority)
                    {
                        nChn = i;
                        break;
                    }
                }
            }
            if (nChn == null) // None available
            {
                Channel lowest = byOwner.First(); // Kill lowest track's instrument if the track is lower than this one
                if (lowest.Owner.Index >= track.Index)
                {
                    nChn = lowest;
                }
            }
            if (nChn != null)
            {
                if (nChn.Owner != null)
                {
                    nChn.Owner.Channels.Remove(nChn);
                }
                nChn.Owner = track;
            }
            return nChn;
        }

        public void ChannelTick()
        {
            for (int i = 0; i < 0x10; i++)
            {
                Channel chan = Channels[i];
                if (chan.Owner != null)
                {
                    // If channel stopped, clean it up
                    if (!chan.Enabled)
                    {
                        chan.Owner.Channels.Remove(chan);
                        chan.Owner = null;
                        chan.Volume = 0;
                        continue;
                    }
                    int pan = chan.StartingPan; // TODO: Mod
                    chan.StepEnvelope();
                    int chanVolume = Utils.SustainTable[chan.NoteVelocity] + chan.Velocity + chan.TrackVolume; // TODO: Mod
                    int pitch = ((chan.Key - chan.BaseKey) << 6) + chan.SweepMain() + chan.Owner.GetPitch(); // "<< 6" is "* 0x40"
                    if (chan.State != EnvelopeState.Release || chanVolume > -92544)
                    {
                        chan.Volume = Utils.GetChannelVolume(chanVolume);
                        chan.Timer = Utils.GetChannelTimer(chan.BaseTimer, pitch);
                        chan.Pan = (sbyte)Utils.Clamp(pan + chan.TrackPan, -0x40, 0x40);
                    }
                    else // EnvelopeState.Dying
                    {
                        chan.Owner.Channels.Remove(chan);
                        chan.Owner = null;
                        chan.Volume = 0;
                    }
                }
            }
        }

        // Called 192 times a second
        public void Process()
        {
            for (int i = 0; i < 0x155; i++) // 0x155 (SamplesPerBuffer) == 0x5540/0x40
            {
                int left = 0, right = 0;
                for (int j = 0; j < 0x10; j++)
                {
                    Channel chan = Channels[j];
                    if (chan != null && chan.Enabled)
                    {
                        chan.Process(out short channelLeft, out short channelRight);
                        if (chan.Owner != null && !Mutes[chan.Owner.Index])
                        {
                            left += channelLeft;
                            right += channelRight;
                        }
                    }
                }
                left = (int)Utils.Clamp(left * MasterVolume, short.MinValue, short.MaxValue);
                right = (int)Utils.Clamp(right * MasterVolume, short.MinValue, short.MaxValue);
                // Convert two shorts to four bytes
                buffer.AddSamples(new byte[] { (byte)(left & 0xFF), (byte)((left >> 8) & 0xFF), (byte)(right & 0xFF), (byte)((right >> 8) & 0xFF) }, 0, 4);
            }
        }
    }
}
