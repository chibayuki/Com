# Com
Com是一个DLL，用于支持线性代数运算、位运算、天文学日期时间计算、高精度非线性色彩处理、动画呈现、窗口管理等功能。

### 公开的类与结构
- Com.BitSet 管理位（Bit）的集合（至多68719472640位），提供基本的位运算功能
- Com.ColorX 浮点精度的色彩解决方案，支持RGB、HSV、HSL、CMYK、LAB等色彩空间
- Com.Complex 直角坐标形式的二元复数
- Com.DateTimeX 大型日期时间（范围B.C.25252756133808173~A.D.25252756133808174）
- Com.PointD 二维欧式空间中表示的坐标（向量）
- Com.PointD3D 三维欧式空间中表示的坐标（向量）
- Com.PointD4D 四维欧式空间中表示的坐标（向量）
- Com.WinForm.FormManager 窗口管理器
- Com.WinForm.RecommendColors 窗口管理器提供的建议配色方案

### 公开的静态类
- Com.Animation 使用指定的帧率、帧数与绘制方法呈现动画
- Com.BitOperation 快速的位运算方案（性能远高于BitSet，但至多64位）
- Com.ColorManipulation 提供基于ColorX的非线性色彩处理方案
- Com.Geometry 提供几何学的基本计算
- Com.IO 提供文件操作功能
- Com.Matrix2D 提供基于二维矩阵的线性代数运算
- Com.Painting2D 提供基本的2D绘图方案
- Com.Painting3D 提供基本的3D绘图方案
- Com.Statistics 提供统计学的基本计算
- Com.Text 提供文本处理功能
- Com.WinForm.ControlSubstitution 提供控件的替代使用方案

### License
Com is released under [GPLv3](Com/LicenseInfo/GPLv3.txt).
