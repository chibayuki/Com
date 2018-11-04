# Com
用于支持线性代数运算、位运算、大型日期时间、高精度非线性色彩处理、动画呈现、窗口管理等功能的DLL / DLL for supporting linear algebra, bit operation, large date-time, high precision nonlinear color processing, animation presentation, window management, etc.

### 公开的类 / Public Class
- Com.BitSet 管理位（Bit）的集合（至多2146434944位），提供基本的位运算功能 / Managing set of bits (up to 2147483520 bits) and providing basic bit operations.
- Com.Matrix 表示矩阵，并为所有线性变换提供实现 / Matrix, supporting all linear transformations.
- Com.Vector 表示任意维度的列向量或行向量，并为PointDnD的线性变换提供实现 / Column or row vector of any dimensions, supporting linear transformations of PointDnD.
- Com.WinForm.FormManager 窗口管理器 / Window manager of winform.
- Com.WinForm.RecommendColors 窗口管理器提供的建议配色方案 / Color scheme provided by FormManager.

### 公开的结构 / Public Structure
- Com.ColorX 浮点精度的色彩解决方案，支持RGB、HSV、HSL、CMYK、LAB等色彩空间 / Floating-point precision color solution based on RGB, HSV, HSL, CMYK, LAB, etc.
- Com.Complex 直角坐标形式的二元复数 / Complex in the Cartesian coordinate system.
- Com.DateTimeX 大型日期时间（范围25252756133808173 BC. ~ 25252756133808174 AD.） / Large date-time (25252756133808173 BC. ~ 25252756133808174 AD.).
- Com.PointD 二维欧式空间中表示的坐标（向量） / Coordinate (vector) in 2D Euclidean space.
- Com.PointD3D 三维欧式空间中表示的坐标（向量） / Coordinate (vector) in 3D Euclidean space.
- Com.PointD4D 四维欧式空间中表示的坐标（向量） / Coordinate (vector) in 4D Euclidean space.
- Com.PointD5D 五维欧式空间中表示的坐标（向量） / Coordinate (vector) in 5D Euclidean space.
- Com.PointD6D 六维欧式空间中表示的坐标（向量） / Coordinate (vector) in 6D Euclidean space.

### 公开的接口 / Public Interface
- Com.IAffine 表示用于支持仿射变换的方法 / Method for affine transform.
- Com.IAffine\<T\> 表示用于支持仿射变换的方法 / Method for affine transform.
- Com.IEuclideanVector 表示欧几里得向量 / Euclidean vector.
- Com.IEuclideanVector\<T\> 表示欧几里得向量 / Euclidean vector.
- Com.ILinearAlgebraVector 表示线性代数向量 / Linear algebra vector.
- Com.ILinearAlgebraVector\<T\> 表示线性代数向量 / Linear algebra vector.
- Com.IVector\<T\> 表示向量（包含确定数量与值的元素的可迭代的有限有序集合） / Vector (a finite and ordered set, which contains certain number of elements with clear values).

### 公开的静态类 / Public Static Class
- Com.Animation 使用指定的帧率、帧数与绘制方法呈现动画 / Presenting animation by frame rate, frame number and drawing method.
- Com.BitOperation 快速的位运算方案（性能远高于BitSet，但至多64位） / Scheme of rapid bit operation (up to 64 bits and far faster than BitSet).
- Com.ColorManipulation 提供基于ColorX的非线性色彩处理方案 / Scheme of nonlinear color processing based on ColorX.
- Com.Geometry 提供几何学的基本计算 / Basic geometry calculation.
- Com.IO 提供文件操作功能 / File operation.
- Com.Painting2D 提供基本的2D绘图方案 / Basic 2D drawing scheme.
- Com.Painting3D 提供基本的3D绘图方案 / Basic 3D drawing scheme.
- Com.Statistics 提供统计学的基本计算 / Basic statistical calculation.
- Com.Text 提供文本处理功能 / Text processing.
- Com.WinForm.ControlSubstitution 提供控件的替代使用方案 / Scheme of substitution of control.

### 公开的枚举 / Public Enumerate
- Com.Vector.Type 表示向量类型 / Type of vector.
- Com.WinForm.Effect 表示窗口交互过程显示的效果 / Effect of interaction of winform window.
- Com.WinForm.FormState 表示窗口状态 / State of winform window.
- Com.WinForm.FormStyle 表示窗口样式 / Style of winform window.
- Com.WinForm.Theme 表示窗口配色主题 / Theme of color scheme of winform window.

### 公开的委托 / Public Delegate
- Com.Animation.Frame 表示用于绘制动画的某一帧的方法 / Method of presenting animation frames.

### 许可 / License
Com基于[GPLv3](Com/LicenseInfo/GPLv3.txt)发布 / Com is released under [GPLv3](Com/LicenseInfo/GPLv3.txt).
