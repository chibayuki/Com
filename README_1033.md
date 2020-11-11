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

###### Language\(语言\): \[English\] / \[[中文](README.md)\]

# Com
Com is a common component, which is a DLL for supporting linear algebra, bit operation, large date-time, high precision nonlinear color processing, animation presentation, window management, etc.

## Static Classes
#### Com.Animation
> Presenting animation by frame rate, frame number and drawing method.
#### Com.BitOperation
> Scheme of rapid bit operation (up to 64 bits and faster than BitSet).
#### Com.ColorManipulation
> Scheme of nonlinear color processing based on ColorX.
#### Com.Geometry
> Basic geometry calculation.
#### Com.IO
> File operation.
#### Com.Painting2D
> Basic 2D drawing scheme.
#### Com.Painting3D
> Basic 3D drawing scheme.
#### Com.Statistics
> Basic statistical calculation.
#### Com.Text
> Text processing.
#### Com.WinForm.ControlSubstitution
> Scheme of substitution of control.

## Classes
#### Com.AffineTransformation
> Containing one or a sequence of affine transformation, supporting complex affine transformations.
#### Com.BitSet
> Managing set of bits (up to 2146434944 bits) and providing basic bit operations.
#### Com.FrequencyCounter
> Frequency counter, which can calculate the frequency of an event realtime.
#### Com.IndexableQueue
> A FIFO container, supporting access by index.
#### Com.Matrix
> Matrix, supporting linear transformations.
#### Com.Vector
> Column or row vector of any dimensions, supporting linear transformations of PointDxD.
#### Com.WinForm.FormManager
> Window manager of winform.
#### Com.WinForm.RecommendColors
> Color scheme provided by FormManager.
#### Com.WinForm.UIMessage
> Message of UI.
#### Com.WinForm.UIMessageProcessor
> Processor of UIMessage.

## Structures
#### Com.ColorX
> Floating-point precision color expression based on RGB, HSV, HSL, CMYK, LAB, YUV, etc.
#### Com.Complex
> Complex in the Cartesian coordinate system.
#### Com.DateTimeX
> Large date-time (25252756133808173 BC. ~ 25252756133808174 AD.).
#### Com.PointD
> Coordinate (vector) in 2D Euclidean space.
#### Com.PointD3D
> Coordinate (vector) in 3D Euclidean space.
#### Com.PointD4D
> Coordinate (vector) in 4D Euclidean space.
#### Com.PointD5D
> Coordinate (vector) in 5D Euclidean space.
#### Com.PointD6D
> Coordinate (vector) in 6D Euclidean space.
#### Com.Real
> Real number (floating-point number in the order of magnitude between ±999999999999999).
#### Com.TimeSpanX
> A timespan.

## Interfaces
#### Com.IAffineTransformable and Com.IAffineTransformable\<T\>
> Supporting affine transform.
#### Com.IEuclideanVector and Com.IEuclideanVector\<T\>
> Euclidean vector.
#### Com.ILinearAlgebraVector and Com.ILinearAlgebraVector\<T\>
> Linear algebra vector.
#### Com.IVector\<T\>
> Vector (a finite, ordered, and iterable set, which contains certain number of elements with clear values).

## Enumerates
#### Com.Vector.Type
> Type of vector.
#### Com.WinForm.Effect
> Effect of interaction of winform window.
#### Com.WinForm.FormState
> State of winform window.
#### Com.WinForm.FormStyle
> Style of winform window.
#### Com.WinForm.Theme
> Theme of color scheme of winform window.
#### Com.WinForm.UIMessageState
> State of UIMessage.
#### Com.WinForm.UIMessageProcessorState
> State of UIMessageProcessor.

## Delegates
#### Com.Animation.Frame
> Method of presenting animation frames.
#### Com.WinForm.UIMessageProcessedHandler
> Method of UIMessage processed.
#### Com.WinForm.UIMessageProcessorStoppedHandler
> Method of UIMessageProcessor stopped.

## License
Com is released under [GPLv3](Com/LicenseInfo/GPLv3.txt).
