[![GitHub stars](https://img.shields.io/github/stars/chibayuki/Com.svg?style=social&label=Stars)](https://github.com/chibayuki/Com/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/chibayuki/Com.svg?style=social&label=Fork)](https://github.com/chibayuki/Com/network/members)
[![GitHub watchers](https://img.shields.io/github/watchers/chibayuki/Com.svg?style=social&label=Watch)](https://github.com/chibayuki/Com/watchers)
[![GitHub followers](https://img.shields.io/github/followers/chibayuki.svg?style=social&label=Follow)](https://github.com/chibayuki?tab=followers)

[![GitHub issues](https://img.shields.io/github/issues/chibayuki/Com.svg)](https://github.com/chibayuki/Com/issues)
[![GitHub license](https://img.shields.io/github/license/chibayuki/Com.svg)](https://github.com/chibayuki/Com/Com/LicenseInfo/GPLv3.txt)
[![GitHub last commit](https://img.shields.io/github/last-commit/chibayuki/Com.svg)](https://github.com/chibayuki/Com/commits)
[![GitHub release](https://img.shields.io/github/release/chibayuki/Com.svg)](https://github.com/chibayuki/Com/releases)
[![GitHub repo size in bytes](https://img.shields.io/github/repo-size/chibayuki/Com.svg)](https://github.com/chibayuki/Com)
[![HitCount](http://hits.dwyl.io/chibayuki/Com.svg)](http://hits.dwyl.io/chibayuki/Com)
[![Language](https://img.shields.io/badge/language-C%23-green.svg)](https://github.com/chibayuki/Com)

###### 语言\(Language\): \[中文\] / \[[English](README_1033.md)\]

# Com
Com是通用组件，是用于支持线性代数运算、位运算、大型日期时间、高精度非线性色彩处理、动画呈现、窗口管理等功能的DLL。

## 静态类
#### Com.Animation
> 使用指定的帧率、帧数与绘制方法呈现动画。
#### Com.BitOperation
> 快速的位运算方案（性能高于BitSet，但至多64位）。
#### Com.ColorManipulation
> 提供基于ColorX的非线性色彩处理方案。
#### Com.Geometry
> 提供几何学的基本计算。
#### Com.IO
> 提供文件操作功能。
#### Com.Painting2D
> 提供基本的2D绘图方案。
#### Com.Painting3D
> 提供基本的3D绘图方案。
#### Com.Statistics
> 提供统计学的基本计算。
#### Com.Text
> 提供文本处理功能。
#### Com.WinForm.ControlSubstitution
> 提供控件的替代使用方案。

## 类
#### Com.AffineTransformation
> 表示仿射变换或仿射变换序列，用于支持复杂的仿射变换。
#### Com.BitSet
> 管理位（Bit）的集合（至多2146434944位），提供基本的位运算功能。
#### Com.FrequencyCounter
> 频率计数器，用于实时计算在过去一小段时间间隔内某一事件发生的频率。
#### Com.IndexableQueue
> 允许通过索引访问元素的先进先出容器。
#### Com.Matrix
> 表示矩阵，并为线性变换提供实现。
#### Com.Vector
> 表示任意维度的列向量或行向量，并为PointDxD的线性变换提供实现。
#### Com.WinForm.FormManager
> 窗口管理器。
#### Com.WinForm.RecommendColors
> 窗口管理器提供的建议配色方案。
#### Com.WinForm.UIMessage
> 界面消息。
#### Com.WinForm.UIMessageProcessor
> 界面消息处理器。

## 结构
#### Com.ColorX
> 浮点精度的色彩表示方案，支持RGB、HSV、HSL、CMYK、LAB、YUV等色彩空间。
#### Com.Complex
> 直角坐标形式的二元复数。
#### Com.DateTimeX
> 表示较大范围的日期时间（范围25252756133808173 BC. ~ 25252756133808174 AD.）。
#### Com.PointD
> 二维欧式空间中表示的坐标（向量）。
#### Com.PointD3D
> 三维欧式空间中表示的坐标（向量）。
#### Com.PointD4D
> 四维欧式空间中表示的坐标（向量）。
#### Com.PointD5D
> 五维欧式空间中表示的坐标（向量）。
#### Com.PointD6D
> 六维欧式空间中表示的坐标（向量）。
#### Com.Real
> 表示实数（数量级介于±999999999999999的浮点数）。
#### Com.TimeSpanX
> 表示较大范围的时间间隔。

## 接口
#### Com.IAffineTransformable 与 Com.IAffineTransformable\<T\>
> 表示支持仿射变换。
#### Com.IEuclideanVector 与 Com.IEuclideanVector\<T\>
> 表示欧几里得向量。
#### Com.ILinearAlgebraVector 与 Com.ILinearAlgebraVector\<T\>
> 表示线性代数向量。
#### Com.IVector\<T\>
> 表示向量（包含确定数量与值的元素的可迭代的有限有序集合）。

## 枚举
#### Com.Vector.Type
> 表示向量类型。
#### Com.WinForm.Effect
> 表示窗口交互过程显示的效果。
#### Com.WinForm.FormState
> 表示窗口状态。
#### Com.WinForm.FormStyle
> 表示窗口样式。
#### Com.WinForm.Theme
> 表示窗口配色主题。
#### Com.WinForm.UIMessageState
> 界面消息状态。
#### Com.WinForm.UIMessageProcessorState
> 界面消息处理器状态。

## 委托
#### Com.Animation.Frame
> 表示用于绘制动画的某一帧的方法。
#### Com.WinForm.UIMessageProcessedHandler
> 表示当界面消息处理完成时应执行的方法。
#### Com.WinForm.UIMessageProcessorStoppedHandler
> 表示当界面消息处理器停止时应执行的方法。

## 许可
Com基于[GPLv3](Com/LicenseInfo/GPLv3.txt)发布。
