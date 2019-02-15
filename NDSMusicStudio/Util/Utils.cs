using System;
using System.Globalization;

namespace Kermalis.NDSMusicStudio.Util
{
    public static class Utils
    {
        public static readonly string[] NoteNames = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        // 768 Values
        public static readonly ushort[] PitchTable = new ushort[]
        {
            0, 59, 118, 178, 237, 296, 356, 415,
            475, 535, 594, 654, 714, 773, 833, 893,
            953, 1013, 1073, 1134, 1194, 1254, 1314, 1375,
            1435, 1496, 1556, 1617, 1677, 1738, 1799, 1859,
            1920, 1981, 2042, 2103, 2164, 2225, 2287, 2348,
            2409, 2471, 2532, 2593, 2655, 2716, 2778, 2840,
            2902, 2963, 3025, 3087, 3149, 3211, 3273, 3335,
            3397, 3460, 3522, 3584, 3647, 3709, 3772, 3834,
            3897, 3960, 4022, 4085, 4148, 4211, 4274, 4337,
            4400, 4463, 4526, 4590, 4653, 4716, 4780, 4843,
            4907, 4971, 5034, 5098, 5162, 5226, 5289, 5353,
            5417, 5481, 5546, 5610, 5674, 5738, 5803, 5867,
            5932, 5996, 6061, 6125, 6190, 6255, 6320, 6384,
            6449, 6514, 6579, 6645, 6710, 6775, 6840, 6906,
            6971, 7037, 7102, 7168, 7233, 7299, 7365, 7431,
            7496, 7562, 7628, 7694, 7761, 7827, 7893, 7959,
            8026, 8092, 8159, 8225, 8292, 8358, 8425, 8492,
            8559, 8626, 8693, 8760, 8827, 8894, 8961, 9028,
            9096, 9163, 9230, 9298, 9366, 9433, 9501, 9569,
            9636, 9704, 9772, 9840, 9908, 9976, 10045, 10113,
            10181, 10250, 10318, 10386, 10455, 10524, 10592, 10661,
            10730, 10799, 10868, 10937, 11006, 11075, 11144, 11213,
            11283, 11352, 11421, 11491, 11560, 11630, 11700, 11769,
            11839, 11909, 11979, 12049, 12119, 12189, 12259, 12330,
            12400, 12470, 12541, 12611, 12682, 12752, 12823, 12894,
            12965, 13036, 13106, 13177, 13249, 13320, 13391, 13462,
            13533, 13605, 13676, 13748, 13819, 13891, 13963, 14035,
            14106, 14178, 14250, 14322, 14394, 14467, 14539, 14611,
            14684, 14756, 14829, 14901, 14974, 15046, 15119, 15192,
            15265, 15338, 15411, 15484, 15557, 15630, 15704, 15777,
            15850, 15924, 15997, 16071, 16145, 16218, 16292, 16366,
            16440, 16514, 16588, 16662, 16737, 16811, 16885, 16960,
            17034, 17109, 17183, 17258, 17333, 17408, 17483, 17557,
            17633, 17708, 17783, 17858, 17933, 18009, 18084, 18160,
            18235, 18311, 18387, 18462, 18538, 18614, 18690, 18766,
            18842, 18918, 18995, 19071, 19147, 19224, 19300, 19377,
            19454, 19530, 19607, 19684, 19761, 19838, 19915, 19992,
            20070, 20147, 20224, 20302, 20379, 20457, 20534, 20612,
            20690, 20768, 20846, 20924, 21002, 21080, 21158, 21236,
            21315, 21393, 21472, 21550, 21629, 21708, 21786, 21865,
            21944, 22023, 22102, 22181, 22260, 22340, 22419, 22498,
            22578, 22658, 22737, 22817, 22897, 22977, 23056, 23136,
            23216, 23297, 23377, 23457, 23537, 23618, 23698, 23779,
            23860, 23940, 24021, 24102, 24183, 24264, 24345, 24426,
            24507, 24589, 24670, 24752, 24833, 24915, 24996, 25078,
            25160, 25242, 25324, 25406, 25488, 25570, 25652, 25735,
            25817, 25900, 25982, 26065, 26148, 26230, 26313, 26396,
            26479, 26562, 26645, 26729, 26812, 26895, 26979, 27062,
            27146, 27230, 27313, 27397, 27481, 27565, 27649, 27733,
            27818, 27902, 27986, 28071, 28155, 28240, 28324, 28409,
            28494, 28579, 28664, 28749, 28834, 28919, 29005, 29090,
            29175, 29261, 29346, 29432, 29518, 29604, 29690, 29776,
            29862, 29948, 30034, 30120, 30207, 30293, 30380, 30466,
            30553, 30640, 30727, 30814, 30900, 30988, 31075, 31162,
            31249, 31337, 31424, 31512, 31599, 31687, 31775, 31863,
            31951, 32039, 32127, 32215, 32303, 32392, 32480, 32568,
            32657, 32746, 32834, 32923, 33012, 33101, 33190, 33279,
            33369, 33458, 33547, 33637, 33726, 33816, 33906, 33995,
            34085, 34175, 34265, 34355, 34446, 34536, 34626, 34717,
            34807, 34898, 34988, 35079, 35170, 35261, 35352, 35443,
            35534, 35626, 35717, 35808, 35900, 35991, 36083, 36175,
            36267, 36359, 36451, 36543, 36635, 36727, 36820, 36912,
            37004, 37097, 37190, 37282, 37375, 37468, 37561, 37654,
            37747, 37841, 37934, 38028, 38121, 38215, 38308, 38402,
            38496, 38590, 38684, 38778, 38872, 38966, 39061, 39155,
            39250, 39344, 39439, 39534, 39629, 39724, 39819, 39914,
            40009, 40104, 40200, 40295, 40391, 40486, 40582, 40678,
            40774, 40870, 40966, 41062, 41158, 41255, 41351, 41448,
            41544, 41641, 41738, 41835, 41932, 42029, 42126, 42223,
            42320, 42418, 42515, 42613, 42710, 42808, 42906, 43004,
            43102, 43200, 43298, 43396, 43495, 43593, 43692, 43790,
            43889, 43988, 44087, 44186, 44285, 44384, 44483, 44583,
            44682, 44781, 44881, 44981, 45081, 45180, 45280, 45381,
            45481, 45581, 45681, 45782, 45882, 45983, 46083, 46184,
            46285, 46386, 46487, 46588, 46690, 46791, 46892, 46994,
            47095, 47197, 47299, 47401, 47503, 47605, 47707, 47809,
            47912, 48014, 48117, 48219, 48322, 48425, 48528, 48631,
            48734, 48837, 48940, 49044, 49147, 49251, 49354, 49458,
            49562, 49666, 49770, 49874, 49978, 50082, 50187, 50291,
            50396, 50500, 50605, 50710, 50815, 50920, 51025, 51131,
            51236, 51341, 51447, 51552, 51658, 51764, 51870, 51976,
            52082, 52188, 52295, 52401, 52507, 52614, 52721, 52827,
            52934, 53041, 53148, 53256, 53363, 53470, 53578, 53685,
            53793, 53901, 54008, 54116, 54224, 54333, 54441, 54549,
            54658, 54766, 54875, 54983, 55092, 55201, 55310, 55419,
            55529, 55638, 55747, 55857, 55966, 56076, 56186, 56296,
            56406, 56516, 56626, 56736, 56847, 56957, 57068, 57179,
            57289, 57400, 57511, 57622, 57734, 57845, 57956, 58068,
            58179, 58291, 58403, 58515, 58627, 58739, 58851, 58964,
            59076, 59189, 59301, 59414, 59527, 59640, 59753, 59866,
            59979, 60092, 60206, 60319, 60433, 60547, 60661, 60774,
            60889, 61003, 61117, 61231, 61346, 61460, 61575, 61690,
            61805, 61920, 62035, 62150, 62265, 62381, 62496, 62612,
            62727, 62843, 62959, 63075, 63191, 63308, 63424, 63540,
            63657, 63774, 63890, 64007, 64124, 64241, 64358, 64476,
            64593, 64711, 64828, 64946, 65064, 65182, 65300, 65418
        };
        // 724 values
        public static readonly byte[] VolumeTable = new byte[]
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1,
            1, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4,
            4, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5,
            5, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 7,
            7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 9, 9, 9, 9,
            9, 9, 9, 9, 9, 10, 10, 10,
            10, 10, 10, 10, 10, 11, 11, 11,
            11, 11, 11, 11, 11, 12, 12, 12,
            12, 12, 12, 12, 13, 13, 13, 13,
            13, 13, 13, 14, 14, 14, 14, 14,
            14, 15, 15, 15, 15, 15, 16, 16,
            16, 16, 16, 16, 17, 17, 17, 17,
            17, 18, 18, 18, 18, 19, 19, 19,
            19, 19, 20, 20, 20, 20, 21, 21,
            21, 21, 22, 22, 22, 22, 23, 23,
            23, 23, 24, 24, 24, 25, 25, 25,
            25, 26, 26, 26, 27, 27, 27, 28,
            28, 28, 29, 29, 29, 30, 30, 30,
            31, 31, 31, 32, 32, 33, 33, 33,
            34, 34, 35, 35, 35, 36, 36, 37,
            37, 38, 38, 38, 39, 39, 40, 40,
            41, 41, 42, 42, 43, 43, 44, 44,
            45, 45, 46, 46, 47, 47, 48, 48,
            49, 50, 50, 51, 51, 52, 52, 53,
            54, 54, 55, 56, 56, 57, 58, 58,
            59, 60, 60, 61, 62, 62, 63, 64,
            65, 66, 66, 67, 68, 69, 70, 70,
            71, 72, 73, 74, 75, 75, 76, 77,
            78, 79, 80, 81, 82, 83, 84, 85,
            86, 87, 88, 89, 90, 91, 92, 93,
            94, 95, 96, 97, 98, 99, 101, 102,
            103, 104, 105, 106, 108, 109, 110, 111,
            113, 114, 115, 117, 118, 119, 121, 122,
            124, 125, 126, 127
        };
        // 128 Values of Attack
        public static readonly byte[] AttackTable = new byte[]
        {
            255, 254, 253, 252, 251, 250, 249, 248,
            247, 246, 245, 244, 243, 242, 241, 240,
            239, 238, 237, 236, 235, 234, 233, 232,
            231, 230, 229, 228, 227, 226, 225, 224,
            223, 222, 221, 220, 219, 218, 217, 216,
            215, 214, 213, 212, 211, 210, 209, 208,
            207, 206, 205, 204, 203, 202, 201, 200,
            199, 198, 197, 196, 195, 194, 193, 192,
            191, 190, 189, 188, 187, 186, 185, 184,
            183, 182, 181, 180, 179, 178, 177, 176,
            175, 174, 173, 172, 171, 170, 169, 168,
            167, 166, 165, 164, 163, 162, 161, 160,
            159, 158, 157, 156, 155, 154, 153, 152,
            151, 150, 149, 148, 147, 143, 137, 132,
            127, 123, 116, 109, 100, 92, 84, 73,
            63, 51, 38, 26, 14, 5, 1, 0
        };
        // 128 Values of Decay or Release
        public static readonly ushort[] DecayTable = new ushort[]
        {
            1, 3, 5, 7, 9, 11, 13, 15,
            17, 19, 21, 23, 25, 27, 29, 31,
            33, 35, 37, 39, 41, 43, 45, 47,
            49, 51, 53, 55, 57, 59, 61, 63,
            65, 67, 69, 71, 73, 75, 77, 79,
            81, 83, 85, 87, 89, 91, 93, 95,
            97, 99, 101, 102, 104, 105, 107, 108,
            110, 111, 113, 115, 116, 118, 120, 122,
            124, 126, 128, 130, 132, 135, 137, 140,
            142, 145, 148, 151, 154, 157, 160, 163,
            167, 171, 175, 179, 183, 187, 192, 197,
            202, 208, 213, 219, 226, 233, 240, 248,
            256, 265, 274, 284, 295, 307, 320, 334,
            349, 366, 384, 404, 427, 452, 480, 512,
            549, 591, 640, 698, 768, 853, 960, 1097,
            1280, 1536, 1920, 2560, 3840, 7680, 15360, 65535
        };
        // 128 values of Sustain
        public static readonly int[] SustainTable = new int[]
        {
            -92544, -92416, -92288, -83328, -76928, -71936, -67840, -64384,
            -61440, -58880, -56576, -54400, -52480, -50688, -49024, -47488,
            -46080, -44672, -43392, -42240, -41088, -40064, -39040, -38016,
            -36992, -36096, -35328, -34432, -33664, -32896, -32128, -31360,
            -30592, -29952, -29312, -28672, -28032, -27392, -26880, -26240,
            -25728, -25088, -24576, -24064, -23552, -23040, -22528, -22144,
            -21632, -21120, -20736, -20224, -19840, -19456, -19072, -18560,
            -18176, -17792, -17408, -17024, -16640, -16256, -16000, -15616,
            -15232, -14848, -14592, -14208, -13952, -13568, -13184, -12928,
            -12672, -12288, -12032, -11648, -11392, -11136, -10880, -10496,
            -10240, -9984, -9728, -9472, -9216, -8960, -8704, -8448,
            -8192, -7936, -7680, -7424, -7168, -6912, -6656, -6400,
            -6272, -6016, -5760, -5504, -5376, -5120, -4864, -4608,
            -4480, -4224, -3968, -3840, -3584, -3456, -3200, -2944,
            -2816, -2560, -2432, -2176, -2048, -1792, -1664, -1408,
            -1280, -1024, -896, -768, -512, -384, -128, 0
        };

        public static readonly Random RNG = new Random();

        // 33 values
        static readonly sbyte[] sinTable = new sbyte[]
        {
            0, 6, 12, 19, 25, 31, 37, 43,
            49, 54, 60, 65, 71, 76, 81, 85,
            90, 94, 98, 102, 106, 109, 112, 115,
            117, 120, 122, 123, 125, 126, 126, 127,
            127
        };
        public static int Sin(int index)
        {
            if (index < 0x20)
            {
                return sinTable[index];
            }
            else if (index < 0x40)
            {
                return sinTable[0x20 - (index - 0x20)];
            }
            else if (index < 0x60)
            {
                return -sinTable[index - 0x40];
            }
            else // < 0x80
            {
                return -sinTable[0x20 - (index - 0x60)];
            }
        }

        public static ushort GetChannelTimer(ushort baseTimer, int pitch)
        {
            int remainder = 0;
            int pitchTableIndex = -pitch;
            // Positive pitch:
            while (pitchTableIndex < 0)
            {
                remainder--;
                pitchTableIndex += 768;
            }
            // Negative pitch:
            while (pitchTableIndex >= 768)
            {
                remainder++;
                pitchTableIndex -= 768;
            }
            ulong timer = (PitchTable[pitchTableIndex] + 0x10000uL) * baseTimer;
            remainder -= 0x10;
            if (remainder <= 0)
            {
                timer >>= -remainder;
            }
            else
            {
                if (remainder >= 0x20)
                {
                    return 0xFF;
                }
                if ((timer & (ulong.MaxValue << (0x20 - remainder))) != 0)
                {
                    return ushort.MaxValue;
                }
                timer <<= remainder;
            }
            return (ushort)timer.Clamp(0x10uL, ushort.MaxValue);
        }
        public static byte GetChannelVolume(int vol)
        {
            return VolumeTable[Clamp(vol / 0x80, -723, 0) + 723];
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }
            else if (val.CompareTo(max) > 0)
            {
                return max;
            }
            else
            {
                return val;
            }
        }
        public static bool TryParseValue(string value, out long outValue)
        {
            try { outValue = ParseValue(value); return true; }
            catch { outValue = 0; return false; }
        }
        public static long ParseValue(string value)
        {
            var provider = new CultureInfo("en-US");
            if (value.StartsWith("0x"))
            {
                if (long.TryParse(value.Substring(2), NumberStyles.HexNumber, provider, out long hexp))
                {
                    return hexp;
                }
            }
            if (long.TryParse(value, NumberStyles.Integer, provider, out long dec))
            {
                return dec;
            }
            if (long.TryParse(value, NumberStyles.HexNumber, provider, out long hex))
            {
                return hex;
            }
            throw new ArgumentException("\"value\" was invalid.");
        }
    }
}
