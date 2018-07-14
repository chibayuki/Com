# Com
用于支持线性代数运算、位运算、大型日期时间、高精度非线性色彩处理、动画呈现、窗口管理等功能的DLL / DLL for supporting linear algebra, bit operation, large date-time, high precision nonlinear color processing, animation presentation, window management, etc.

### 公开的类与结构 / Public Class & Struct
- Com.BitSet 管理位（Bit）的集合（至多2147483520位），提供基本的位运算功能 / Managing set of bits (up to 2147483520 bits) and providing basic bit operations.
- Com.ColorX 浮点精度的色彩解决方案，支持RGB、HSV、HSL、CMYK、LAB等色彩空间 / Floating-point precision color solution based on RGB, HSV, HSL, CMYK, LAB, etc.
- Com.Complex 直角坐标形式的二元复数 / Complex in the Cartesian coordinate system.
- Com.DateTimeX 大型日期时间（范围25252756133808173 BC. ~ 25252756133808174 AD.） / Large date-time (25252756133808173 BC. ~ 25252756133808174 AD.).
- Com.PointD 二维欧式空间中表示的坐标（向量） / Coordinate (vector) in 2D Euclidean space.
- Com.PointD3D 三维欧式空间中表示的坐标（向量） / Coordinate (vector) in 3D Euclidean space.
- Com.PointD4D 四维欧式空间中表示的坐标（向量） / Coordinate (vector) in 4D Euclidean space.
- Com.WinForm.FormManager 窗口管理器 / Window manager of winform.
- Com.WinForm.RecommendColors 窗口管理器提供的建议配色方案 / Color scheme provided by FormManager.

### 公开的静态类 / Public Static Class
- Com.Animation 使用指定的帧率、帧数与绘制方法呈现动画 / Presenting animation by frame rate, frame number and drawing method.
- Com.BitOperation 快速的位运算方案（性能远高于BitSet，但至多64位） / Scheme of rapid bit operation (up to 64 bits and far faster than BitSet).
- Com.ColorManipulation 提供基于ColorX的非线性色彩处理方案 / Scheme of nonlinear color processing based on ColorX.
- Com.Geometry 提供几何学的基本计算 / Basic geometry calculation.
- Com.IO 提供文件操作功能 / File operation.
- Com.Matrix2D 提供基于二维矩阵的线性代数运算 / Linear algebra calculation based on 2D matrix.
- Com.Painting2D 提供基本的2D绘图方案 / Basic 2D drawing scheme.
- Com.Painting3D 提供基本的3D绘图方案 / Basic 3D drawing scheme.
- Com.Statistics 提供统计学的基本计算 / Basic statistical calculation.
- Com.Text 提供文本处理功能 / Text processing.
- Com.WinForm.ControlSubstitution 提供控件的替代使用方案 / Scheme of substitution of control.

### 许可 / License
Com基于[GPLv3](Com/LicenseInfo/GPLv3.txt)发布 / Com is released under [GPLv3](Com/LicenseInfo/GPLv3.txt).
