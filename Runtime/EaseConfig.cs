using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Properties;


namespace AV.Eases.Runtime
{
    /// <summary>
    ///     A high-performance, Burst-compatible struct for evaluating easing functions.
    ///     It efficiently packs the ease type, wrap mode, and a reversed flag into a single byte.
    ///     Layout: [R|WW|EEEEE] (Bit 7: Reversed | Bits 6-5: WrapMode | Bits 4-0: EaseType)
    /// </summary>
    [BurstCompile]
    [Serializable]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte Leading3Bit()
        {
            return (byte)((Value & ~EaseMask) >> 5);
        }

        #region Constructors & Operators

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

        #region Evaluation Logic

        /// <summary>
        ///     The core evaluation method. Applies wrapping, reversal, and the base easing function.
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryComplete(ref float elapsedTime, float duration, float step, out float easedT)
        {
            elapsedTime += step;
            if (elapsedTime > duration)
            {
                easedT = 1;
                elapsedTime = 0;
                return true;
            }

            var progress = elapsedTime / duration;
            Evaluate(progress, out easedT);
            return false;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Evaluate(float t, out float easedT)
        {
            var baseEaseValue = Value & EaseMask;
            easedT = baseEaseValue switch
            {
                (byte)EEase.InSine => InSine(t),
                (byte)EEase.OutSine => OutSine(t),
                (byte)EEase.InOutSine => InOutSine(t),
                (byte)EEase.InQuad => InQuad(t),
                (byte)EEase.OutQuad => OutQuad(t),
                (byte)EEase.InOutQuad => InOutQuad(t),
                (byte)EEase.InCubic => InCubic(t),
                (byte)EEase.OutCubic => OutCubic(t),
                (byte)EEase.InOutCubic => InOutCubic(t),
                (byte)EEase.InQuart => InQuart(t),
                (byte)EEase.OutQuart => OutQuart(t),
                (byte)EEase.InOutQuart => InOutQuart(t),
                (byte)EEase.InQuint => InQuint(t),
                (byte)EEase.OutQuint => OutQuint(t),
                (byte)EEase.InOutQuint => InOutQuint(t),
                (byte)EEase.InExpo => InExpo(t),
                (byte)EEase.OutExpo => OutExpo(t),
                (byte)EEase.InOutExpo => InOutExpo(t),
                (byte)EEase.InCirc => InCirc(t),
                (byte)EEase.OutCirc => OutCirc(t),
                (byte)EEase.InOutCirc => InOutCirc(t),
                (byte)EEase.InElastic => InElastic(t),
                (byte)EEase.OutElastic => OutElastic(t),
                (byte)EEase.InOutElastic => InOutElastic(t),
                (byte)EEase.InBack => InBack(t),
                (byte)EEase.OutBack => OutBack(t),
                (byte)EEase.InOutBack => InOutBack(t),
                (byte)EEase.InBounce => InBounce(t),
                (byte)EEase.OutBounce => OutBounce(t),
                (byte)EEase.InOutBounce => InOutBounce(t),
                (byte)EEase.Custom => 1f,
                _ => t // EEase.Linear (default)
            };
        }

        #endregion

        #region Private Easing Functions

        private const float PI = math.PI;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InSine(float t)
        {
            return 1f - math.cos(t * PI * 0.5f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutSine(float t)
        {
            return math.sin(t * PI * 0.5f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutSine(float t)
        {
            return -(math.cos(t * PI) - 1) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InQuad(float t)
        {
            return t * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutQuad(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : 1 - math.pow(-2 * t + 2, 2) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InCubic(float t)
        {
            return t * t * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutCubic(float t)
        {
            return 1 - math.pow(1 - t, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutCubic(float t)
        {
            return t < 0.5f ? 4 * t * t * t : 1 - math.pow(-2 * t + 2, 3) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InQuart(float t)
        {
            return t * t * t * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutQuart(float t)
        {
            return 1 - math.pow(1 - t, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutQuart(float t)
        {
            return t < 0.5f ? 8 * t * t * t * t : 1 - math.pow(-2 * t + 2, 4) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InQuint(float t)
        {
            return t * t * t * t * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutQuint(float t)
        {
            return 1 - math.pow(1 - t, 5);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutQuint(float t)
        {
            return t < 0.5f ? 16 * t * t * t * t * t : 1 - math.pow(-2 * t + 2, 5) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InExpo(float t)
        {
            return t == 0 ? 0 : math.pow(2, 10 * (t - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutExpo(float t)
        {
            return math.abs(t - 1) < float.Epsilon ? 1 : -math.pow(2, -10 * t) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutExpo(float t)
        {
            if (t == 0) return 0;
            if (math.abs(t - 1) < float.Epsilon) return 1;
            return t < 0.5f ? math.pow(2, 20 * t - 10) * 0.5f : (2 - math.pow(2, -20 * t + 10)) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InCirc(float t)
        {
            return 1 - math.sqrt(1 - math.pow(t, 2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutCirc(float t)
        {
            return math.sqrt(1 - math.pow(t - 1, 2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutCirc(float t)
        {
            return t < 0.5f
                ? (1 - math.sqrt(1 - math.pow(2 * t, 2))) * 0.5f
                : (math.sqrt(1 - math.pow(-2 * t + 2, 2)) + 1) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InElastic(float t)
        {
            if (t == 0) return 0;
            if (math.abs(t - 1) < float.Epsilon) return 1;
            return -math.sin(7.5f * PI * t) * math.pow(2, 10 * (t - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutElastic(float t)
        {
            if (t == 0) return 0;
            if (math.abs(t - 1) < float.Epsilon) return 1;
            return math.sin(-7.5f * PI * (t + 1)) * math.pow(2, -10 * t) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutElastic(float t)
        {
            if (t == 0) return 0;
            if (math.abs(t - 1) < float.Epsilon) return 1;
            return t < 0.5f
                ? 0.5f * math.sin(7.5f * PI * (2 * t)) * math.pow(2, 10 * (2 * t - 1))
                : 0.5f * (math.sin(-7.5f * PI * (2 * t - 1 + 1)) * math.pow(2, -10 * (2 * t - 1)) + 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InBack(float t)
        {
            return t * t * t - t * math.sin(t * PI);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutBack(float t)
        {
            return 1 - (math.pow(1 - t, 3) - (1 - t) * math.sin((1 - t) * PI));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutBack(float t)
        {
            if (t < 0.5f)
            {
                var adjustedProgress = 2 * t;
                return 0.5f * (adjustedProgress * adjustedProgress * adjustedProgress - adjustedProgress * math.sin(adjustedProgress * PI));
            }
            else
            {
                var adjustedProgress = 1 - (2 * t - 1);
                return 0.5f * (1 - (adjustedProgress * adjustedProgress * adjustedProgress - adjustedProgress * math.sin(adjustedProgress * PI))) + 0.5f;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InBounce(float t)
        {
            return 1 - OutBounce(1 - t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutBounce(float t)
        {
            if (t < 1 / 2.75f) return 7.5625f * t * t;

            if (t < 2 / 2.75f) return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;

            if (t < 2.5 / 2.75f) return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;

            return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutBounce(float t)
        {
            return t < 0.5f ? InBounce(t * 2) * 0.5f : OutBounce(t * 2 - 1) * 0.5f + 0.5f;
        }

        #endregion
    }
}
