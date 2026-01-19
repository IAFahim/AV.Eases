using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Properties;

namespace AV.Eases.Runtime
{
    /// <summary>
    ///     A high-performance, Burst-compatible struct for storing easing configuration.
    ///     It efficiently packs the ease type, wrap mode, and a reversed flag into a single byte.
    ///     Layout: [R|WW|EEEEE] (Bit 7: Reversed | Bits 6-5: WrapMode | Bits 4-0: EaseType)
    ///
    ///     This is a pure data structure (Layer A). All logic is in EaseLogic (Layer B).
    /// </summary>
    [BurstCompile]
    public struct EaseConfig
    {
        /// <summary>
        ///     The raw byte value storing the ease type and modifier flags.
        /// </summary>
        public byte Value;

#if UNITY_EDITOR
        [CreateProperty] internal byte Leading3 => (byte)((Value & ~EaseMask) >> 5);
#endif
        // --- EaseConfig Type (lower 5 bits) ---
        private const byte EaseMask = 0b0001_1111;

        #region Constructors

        /// <summary>
        ///     Creates an EaseConfig struct from its constituent parts. This is the recommended factory method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EaseConfig New(EEase ease, byte leading3Bit)
        {
            var easeByte = (byte)ease;
            var value = easeByte | (leading3Bit << 5);

            return new EaseConfig
            {
                Value = (byte)value
            };
        }

        #endregion
    }
}
