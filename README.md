# AV.Eases

![Header](documentation_header.svg)

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-000000.svg?style=flat-square&logo=unity)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE.md)

Comprehensive, Burst-compatible easing library for Unity.

## ✨ Features

- **Burst Compatible**: `EaseConfig` struct is designed for high-performance Burst jobs.
- **Extensive Library**: 30+ easing functions (Sine, Quad, Cubic, Elastic, Bounce, etc.).
- **Optimized Struct**: Packs ease type, wrap mode, and flags into a single byte.
- **Direct Math**: Access to raw math functions via static methods.

## 📦 Installation

Install via Unity Package Manager (git URL).

### Dependencies
- **Unity.Mathematics**

## 🚀 Usage

### 1. Burst Job Usage
```csharp
[BurstCompile]
public struct MoveJob : IJob
{
    public EaseConfig Ease;
    public float Time;
    
    public void Execute()
    {
        Ease.Evaluate(Time, out float result);
    }
}
```

### 2. Standard Usage
```csharp
float val = EaseConfig.OutBounce(0.5f);
```

## ⚠️ Status

- 🧪 **Tests**: Missing.
- 📘 **Samples**: None.
