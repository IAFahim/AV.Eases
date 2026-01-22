# AV.Eases

[![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.md)
[![Version](https://img.shields.io/badge/Version-1.0.0-orange.svg)](CHANGELOG.md)

Comprehensive easing function library for Unity with enum-based selection and ScriptableObject configuration.

## Features

- 30+ easing functions (Linear, Sine, Quad, Cubic, Quart, Quint, Expo, Circ, Back, Elastic, Bounce)
- Inspector-friendly EEase enum for easy selection
- EaseConfig ScriptableObject for reusable easing configurations
- Mathematically accurate implementations
- Optimized for performance

## Installation

```
Window > Package Manager > + > Add package from git URL
```
```
https://github.com/IAFahim/AV.Eases.git
```

## Usage

```csharp
using AV.Eases.Runtime;

public class MyBehaviour : MonoBehaviour
{
    public EEase easeType = EEase.InOutQuad;
    public EaseConfig customEase;

    float ApplyEase(float t)
    {
        return EaseEvaluator.Evaluate(easeType, t);
    }
}
```

## Available Eases

- **Linear** - Straight diagonal line
- **Sine** - Smooth S-curves
- **Quad** - Gentle curves
- **Cubic** - Moderate curves
- **Quart** - Strong curves
- **Quint** - Very strong curves
- **Expo** - Extreme curves
- **Circ** - Circular motion
- **Back** - Overshoot
- **Elastic** - Elastic effect
- **Bounce** - Bouncy effect

Each comes in In, Out, and InOut variants.

## Code Quality

This package follows strict naming guidelines:
- ✅ **Descriptive variables**: `adjustedProgress` (not `f`)
- ✅ **Domain standards**: `t` for time/progress in easing functions (universally understood in animation mathematics)
- ✅ **Explicit intent**: Variable names clearly describe their purpose

## License

MIT License - see [LICENSE.md](LICENSE.md) for details.

## Author

**IAFahim** - [GitHub](https://github.com/IAFahim)
