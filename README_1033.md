###### Language\(语言\): \[English\] / \[[中文](README.md)\]

# Com
Com is a common component, which is a DLL for supporting linear algebra, bit operation, large date-time, high precision nonlinear color processing, animation presentation, window management, etc.

## Public Static Class
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

## Public Class
#### Com.BitSet
> Managing set of bits (up to 2146434944 bits) and providing basic bit operations.
#### Com.Matrix
> Matrix, supporting linear transformations.
#### Com.Vector
> Column or row vector of any dimensions, supporting linear transformations of PointDxD.
#### Com.WinForm.FormManager
> Window manager of winform.
#### Com.WinForm.RecommendColors
> Color scheme provided by FormManager.

## Public Structure
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

## Public Interface
#### Com.IAffine and Com.IAffine\<T\>
> Method for affine transform.
#### Com.IEuclideanVector and Com.IEuclideanVector\<T\>
> Euclidean vector.
#### Com.ILinearAlgebraVector and Com.ILinearAlgebraVector\<T\>
> Linear algebra vector.
#### Com.IVector\<T\>
> Vector (a finite, ordered, and iterable set, which contains certain number of elements with clear values).

## Public Enumerate
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

## Public Delegate
#### Com.Animation.Frame
> Method of presenting animation frames.

## License
Com is released under [GPLv3](Com/LicenseInfo/GPLv3.txt).