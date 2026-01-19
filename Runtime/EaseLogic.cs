using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;

namespace AV.Eases.Runtime
{
    /// <summary>
    /// Core logic for easing function evaluation.
    /// Burst-compatible static methods with strict in/out patterns.
    /// Follows DOD principles - pure computation without state.
    /// </summary>
    [BurstCompile]
    public static class EaseLogic
    {
        private const byte EaseMask = 0b0001_1111;

        #region Evaluation Logic

        /// <summary>
        /// The core evaluation method. Applies wrapping, reversal, and the base easing function.
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryComplete(in EaseConfig config, ref float elapsedTime, float duration, float step, out float easedNormalizedTime)
        {
            elapsedTime += step;
            if (elapsedTime > duration)
            {
                easedNormalizedTime = 1f;
                elapsedTime = 0f;
                return true;
            }

            var normalizedProgress = elapsedTime / duration;
            Evaluate(config, normalizedProgress, out easedNormalizedTime);
            return false;
        }

        /// <summary>
        /// Evaluates an easing function based on the configuration.
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Evaluate(in EaseConfig config, float normalizedTime, out float easedTime)
        {
            var baseEaseValue = config.Value & EaseMask;
            easedTime = baseEaseValue switch
            {
                (byte)EEase.InSine => InSine(normalizedTime),
                (byte)EEase.OutSine => OutSine(normalizedTime),
                (byte)EEase.InOutSine => InOutSine(normalizedTime),
                (byte)EEase.InQuad => InQuad(normalizedTime),
                (byte)EEase.OutQuad => OutQuad(normalizedTime),
                (byte)EEase.InOutQuad => InOutQuad(normalizedTime),
                (byte)EEase.InCubic => InCubic(normalizedTime),
                (byte)EEase.OutCubic => OutCubic(normalizedTime),
                (byte)EEase.InOutCubic => InOutCubic(normalizedTime),
                (byte)EEase.InQuart => InQuart(normalizedTime),
                (byte)EEase.OutQuart => OutQuart(normalizedTime),
                (byte)EEase.InOutQuart => InOutQuart(normalizedTime),
                (byte)EEase.InQuint => InQuint(normalizedTime),
                (byte)EEase.OutQuint => OutQuint(normalizedTime),
                (byte)EEase.InOutQuint => InOutQuint(normalizedTime),
                (byte)EEase.InExpo => InExpo(normalizedTime),
                (byte)EEase.OutExpo => OutExpo(normalizedTime),
                (byte)EEase.InOutExpo => InOutExpo(normalizedTime),
                (byte)EEase.InCirc => InCirc(normalizedTime),
                (byte)EEase.OutCirc => OutCirc(normalizedTime),
                (byte)EEase.InOutCirc => InOutCirc(normalizedTime),
                (byte)EEase.InElastic => InElastic(normalizedTime),
                (byte)EEase.OutElastic => OutElastic(normalizedTime),
                (byte)EEase.InOutElastic => InOutElastic(normalizedTime),
                (byte)EEase.InBack => InBack(normalizedTime),
                (byte)EEase.OutBack => OutBack(normalizedTime),
                (byte)EEase.InOutBack => InOutBack(normalizedTime),
                (byte)EEase.InBounce => InBounce(normalizedTime),
                (byte)EEase.OutBounce => OutBounce(normalizedTime),
                (byte)EEase.InOutBounce => InOutBounce(normalizedTime),
                (byte)EEase.Custom => 1f,
                _ => normalizedTime // EEase.Linear (default)
            };
        }

        /// <summary>
        /// Gets the leading 3 bits from the config value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetLeading3Bits(in EaseConfig config)
        {
            return (byte)((config.Value & ~EaseMask) >> 5);
        }

        #endregion

        #region Easing Functions

        private const float PI = math.PI;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InSine(float normalizedTime)
        {
            return 1f - math.cos(normalizedTime * PI * 0.5f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutSine(float normalizedTime)
        {
            return math.sin(normalizedTime * PI * 0.5f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutSine(float normalizedTime)
        {
            return -(math.cos(normalizedTime * PI) - 1) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InQuad(float normalizedTime)
        {
            return normalizedTime * normalizedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutQuad(float normalizedTime)
        {
            return 1 - (1 - normalizedTime) * (1 - normalizedTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutQuad(float normalizedTime)
        {
            return normalizedTime < 0.5f ? 2 * normalizedTime * normalizedTime : 1 - math.pow(-2 * normalizedTime + 2, 2) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InCubic(float normalizedTime)
        {
            return normalizedTime * normalizedTime * normalizedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutCubic(float normalizedTime)
        {
            return 1 - math.pow(1 - normalizedTime, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutCubic(float normalizedTime)
        {
            return normalizedTime < 0.5f ? 4 * normalizedTime * normalizedTime * normalizedTime : 1 - math.pow(-2 * normalizedTime + 2, 3) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InQuart(float normalizedTime)
        {
            return normalizedTime * normalizedTime * normalizedTime * normalizedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutQuart(float normalizedTime)
        {
            return 1 - math.pow(1 - normalizedTime, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutQuart(float normalizedTime)
        {
            return normalizedTime < 0.5f ? 8 * normalizedTime * normalizedTime * normalizedTime * normalizedTime : 1 - math.pow(-2 * normalizedTime + 2, 4) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InQuint(float normalizedTime)
        {
            return normalizedTime * normalizedTime * normalizedTime * normalizedTime * normalizedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutQuint(float normalizedTime)
        {
            return 1 - math.pow(1 - normalizedTime, 5);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutQuint(float normalizedTime)
        {
            return normalizedTime < 0.5f ? 16 * normalizedTime * normalizedTime * normalizedTime * normalizedTime * normalizedTime : 1 - math.pow(-2 * normalizedTime + 2, 5) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InExpo(float normalizedTime)
        {
            return normalizedTime == 0 ? 0 : math.pow(2, 10 * (normalizedTime - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutExpo(float normalizedTime)
        {
            return math.abs(normalizedTime - 1) < float.Epsilon ? 1 : -math.pow(2, -10 * normalizedTime) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutExpo(float normalizedTime)
        {
            if (normalizedTime == 0) return 0;
            if (math.abs(normalizedTime - 1) < float.Epsilon) return 1;
            return normalizedTime < 0.5f ? math.pow(2, 20 * normalizedTime - 10) * 0.5f : (2 - math.pow(2, -20 * normalizedTime + 10)) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InCirc(float normalizedTime)
        {
            return 1 - math.sqrt(1 - math.pow(normalizedTime, 2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutCirc(float normalizedTime)
        {
            return math.sqrt(1 - math.pow(normalizedTime - 1, 2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutCirc(float normalizedTime)
        {
            return normalizedTime < 0.5f
                ? (1 - math.sqrt(1 - math.pow(2 * normalizedTime, 2))) * 0.5f
                : (math.sqrt(1 - math.pow(-2 * normalizedTime + 2, 2)) + 1) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InElastic(float normalizedTime)
        {
            if (normalizedTime == 0) return 0;
            if (math.abs(normalizedTime - 1) < float.Epsilon) return 1;
            return -math.sin(7.5f * PI * normalizedTime) * math.pow(2, 10 * (normalizedTime - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutElastic(float normalizedTime)
        {
            if (normalizedTime == 0) return 0;
            if (math.abs(normalizedTime - 1) < float.Epsilon) return 1;
            return math.sin(-7.5f * PI * (normalizedTime + 1)) * math.pow(2, -10 * normalizedTime) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutElastic(float normalizedTime)
        {
            if (normalizedTime == 0) return 0;
            if (math.abs(normalizedTime - 1) < float.Epsilon) return 1;
            return normalizedTime < 0.5f
                ? 0.5f * math.sin(7.5f * PI * (2 * normalizedTime)) * math.pow(2, 10 * (2 * normalizedTime - 1))
                : 0.5f * (math.sin(-7.5f * PI * (2 * normalizedTime - 1 + 1)) * math.pow(2, -10 * (2 * normalizedTime - 1)) + 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InBack(float normalizedTime)
        {
            return normalizedTime * normalizedTime * normalizedTime - normalizedTime * math.sin(normalizedTime * PI);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutBack(float normalizedTime)
        {
            return 1 - (math.pow(1 - normalizedTime, 3) - (1 - normalizedTime) * math.sin((1 - normalizedTime) * PI));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutBack(float normalizedTime)
        {
            if (normalizedTime < 0.5f)
            {
                var f = 2 * normalizedTime;
                return 0.5f * (f * f * f - f * math.sin(f * PI));
            }
            else
            {
                var f = 1 - (2 * normalizedTime - 1);
                return 0.5f * (1 - (f * f * f - f * math.sin(f * PI))) + 0.5f;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InBounce(float normalizedTime)
        {
            return 1 - OutBounce(1 - normalizedTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OutBounce(float normalizedTime)
        {
            if (normalizedTime < 1 / 2.75f) return 7.5625f * normalizedTime * normalizedTime;

            if (normalizedTime < 2 / 2.75f) return 7.5625f * (normalizedTime -= 1.5f / 2.75f) * normalizedTime + 0.75f;

            if (normalizedTime < 2.5 / 2.75f) return 7.5625f * (normalizedTime -= 2.25f / 2.75f) * normalizedTime + 0.9375f;

            return 7.5625f * (normalizedTime -= 2.625f / 2.75f) * normalizedTime + 0.984375f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InOutBounce(float normalizedTime)
        {
            return normalizedTime < 0.5f ? InBounce(normalizedTime * 2) * 0.5f : OutBounce(normalizedTime * 2 - 1) * 0.5f + 0.5f;
        }

        #endregion
    }
}
