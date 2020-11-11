/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

Com.IndexableQueue
Version 20.10.27.1900

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Com
{
    /// <summary>
    /// 允许通过索引访问元素的队列。
    /// </summary>
    public sealed class IndexableQueue<T> : IList, IList<T>
    {
        #region 非公开成员

        private static readonly T[] _EmptyData = new T[0]; // 表示 Empty 对象的数据数组。

        //

        private bool _AutoExpand;
        private int _Capacity; // 容量。
        private int _Count; // 元素数目。
        private int _HeadIndex; // 队首元素的索引。
        private int _TailIndex; // 队尾元素的索引。
        private T[] _TArray; // 数据数组。

        //

        // 获取元素，不做索引越界检查。
        private T _GetItemWithoutCheckBounds(int index)
        {
            return _TArray[(_HeadIndex + index) % _Capacity];
        }

        // 设置元素，不做索引越界检查。
        private void _SetItemWithoutCheckBounds(int index, T value)
        {
            _TArray[(_HeadIndex + index) % _Capacity] = value;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 不使用任何参数初始化 IndexableQueue 的新实例。
        /// </summary>
        public IndexableQueue()
        {
            _AutoExpand = true;
            _Capacity = 0;
            _Count = 0;
            _HeadIndex = 0;
            _TailIndex = 0;
            _TArray = _EmptyData;
        }

        /// <summary>
        /// 使用指定的容量与表示是否自动扩展容量的布尔值初始化 IndexableQueue 的新实例。
        /// </summary>
        /// <param name="capacity">容量。</param>
        /// <param name="autoExpand">是否自动扩展容量，如果不自动扩容，队列满后每添加一个元素都将从队首删除一个元素。</param>
        public IndexableQueue(int capacity, bool autoExpand)
        {
            if (capacity < 0)
            {
                throw new OverflowException();
            }

            //

            _AutoExpand = autoExpand;
            _Capacity = capacity;
            _Count = 0;
            _HeadIndex = 0;
            _TailIndex = 0;

            if (_Capacity > 0)
            {
                _TArray = new T[_Capacity];
            }
            else
            {
                _TArray = _EmptyData;
            }
        }

        /// <summary>
        /// 使用指定的容量初始化 IndexableQueue 的新实例。
        /// </summary>
        /// <param name="capacity">容量。</param>
        public IndexableQueue(int capacity) : this(capacity, true)
        {
        }

        /// <summary>
        /// 使用包含多个元素的集合与表示是否自动扩展容量的布尔值初始化 IndexableQueue 的新实例。
        /// </summary>
        /// <param name="autoExpand">是否自动扩展容量，如果不自动扩容，队列满后每添加一个元素都将从队首删除一个元素。</param>
        /// <param name="items">包含多个元素的集合。</param>
        public IndexableQueue(IEnumerable<T> items, bool autoExpand)
        {
            if (items is null)
            {
                throw new ArgumentNullException();
            }

            //

            _AutoExpand = autoExpand;
            _Capacity = items.Count();
            _Count = 0;
            _HeadIndex = 0;
            _TailIndex = 0;

            if (_Capacity > 0)
            {
                _TArray = new T[_Capacity];

                Enqueue(items);
            }
            else
            {
                _TArray = _EmptyData;
            }
        }

        /// <summary>
        /// 使用包含多个元素的集合初始化 IndexableQueue 的新实例。
        /// </summary>
        /// <param name="items">包含多个元素的集合。</param>
        public IndexableQueue(IEnumerable<T> items) : this(items, true)
        {
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置此 IndexableQueue 对象的指定索引的元素。
        /// </summary>
        /// <param name="index">索引。</param>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _Count)
                {
                    throw new IndexOutOfRangeException();
                }

                //

                return _GetItemWithoutCheckBounds(index);
            }

            set
            {
                if (index < 0 || index >= _Count)
                {
                    throw new IndexOutOfRangeException();
                }

                //

                _SetItemWithoutCheckBounds(index, value);
            }
        }

        //

        /// <summary>
        /// 获取或设置此 IndexableQueue 对象的队首元素。
        /// </summary>
        public T Head
        {
            get
            {
                if (_Count <= 0)
                {
                    throw new InvalidOperationException();
                }

                //

                return _TArray[_HeadIndex];
            }

            set
            {
                if (_Count <= 0)
                {
                    throw new InvalidOperationException();
                }

                //

                _TArray[_HeadIndex] = value;
            }
        }

        /// <summary>
        /// 获取或设置此 IndexableQueue 对象的队尾元素。
        /// </summary>
        public T Tail
        {
            get
            {
                if (_Count <= 0)
                {
                    throw new InvalidOperationException();
                }

                //

                return _TArray[_TailIndex];
            }

            set
            {
                if (_Count <= 0)
                {
                    throw new InvalidOperationException();
                }

                //

                _TArray[_TailIndex] = value;
            }
        }

        //

        /// <summary>
        /// 获取表示此 IndexableQueue 对象是否自动扩展容量的布尔值。
        /// </summary>
        public bool AutoExpand
        {
            get
            {
                return _AutoExpand;
            }
        }

        /// <summary>
        /// 获取此 IndexableQueue 对象的容量。
        /// </summary>
        public int Capacity
        {
            get
            {
                return _Capacity;
            }
        }

        /// <summary>
        /// 获取此 IndexableQueue 对象的元素数目。
        /// </summary>
        public int Count
        {
            get
            {
                return _Count;
            }
        }

        //

        /// <summary>
        /// 获取表示此 IndexableQueue 对象是否为空的布尔值。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_Count <= 0);
            }
        }

        /// <summary>
        /// 获取表示此 IndexableQueue 对象是否已满的布尔值。
        /// </summary>
        public bool IsFull
        {
            get
            {
                return (_Capacity > 0 && _Count >= _Capacity);
            }
        }

        /// <summary>
        /// 获取表示此 IndexableQueue 是否只读的布尔值。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取表示此 IndexableQueue 是否具有固定的大小的布尔值。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 遍历此 IndexableQueue 的所有元素并返回第一个与指定值相等的元素的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的元素的索引。</returns>
        public int IndexOf(T item)
        {
            if (IsEmpty)
            {
                return -1;
            }
            else
            {
                return IndexOf(item, 0, _Count);
            }
        }

        /// <summary>
        /// 从指定的索引开始遍历此 IndexableQueue 的所有元素并返回第一个与指定值相等的元素的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的元素的索引。</returns>
        public int IndexOf(T item, int startIndex)
        {
            if (_Count <= 0 || (startIndex < 0 || startIndex >= _Count))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return IndexOf(item, startIndex, _Count - startIndex);
        }

        /// <summary>
        /// 从指定的索引开始遍历此 IndexableQueue 指定数量的元素并返回第一个与指定值相等的元素的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的元素数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的元素的索引。</returns>
        public int IndexOf(T item, int startIndex, int count)
        {
            if (_Count <= 0 || (startIndex < 0 || startIndex >= _Count) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            int right = Math.Min(_Count, startIndex + count);

            for (int i = startIndex; i < right; i++)
            {
                if (_GetItemWithoutCheckBounds(i).Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 逆序遍历此 IndexableQueue 的所有元素并返回第一个与指定值相等的元素的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的元素的索引。</returns>
        public int LastIndexOf(T item)
        {
            if (IsEmpty)
            {
                return -1;
            }
            else
            {
                return LastIndexOf(item, _Count - 1, _Count);
            }
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 IndexableQueue 的所有元素并返回第一个与指定值相等的元素的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的元素的索引。</returns>
        public int LastIndexOf(T item, int startIndex)
        {
            if (_Count <= 0 || (startIndex < 0 || startIndex >= _Count))
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            return LastIndexOf(item, startIndex, startIndex + 1);
        }

        /// <summary>
        /// 从指定的索引开始逆序遍历此 IndexableQueue 指定数量的元素并返回第一个与指定值相等的元素的索引。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <param name="startIndex">起始索引。</param>
        /// <param name="count">遍历的元素数量。</param>
        /// <returns>32 位整数，表示第一个与指定值相等的元素的索引。</returns>
        public int LastIndexOf(T item, int startIndex, int count)
        {
            if (_Count <= 0 || (startIndex < 0 || startIndex >= _Count) || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            int left = Math.Max(0, startIndex - count + 1);

            for (int i = startIndex; i >= left; i++)
            {
                if (_GetItemWithoutCheckBounds(i).Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 遍历此 IndexableQueue 的所有元素并返回表示是否存在与指定值相等的元素的布尔值。
        /// </summary>
        /// <param name="item">用于检索的值。</param>
        /// <returns>布尔值，表示是否存在与指定值相等的元素。</returns>
        public bool Contains(T item)
        {
            if (IsEmpty)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < _Count; i++)
                {
                    if (_GetItemWithoutCheckBounds(i).Equals(item))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        //

        /// <summary>
        /// 将此 IndexableQueue 转换为数组。
        /// </summary>
        /// <returns>T 数组，数组元素表示此 IndexableQueue 的元素。</returns>
        public T[] ToArray()
        {
            if (IsEmpty)
            {
                return new T[0];
            }
            else
            {
                T[] result = new T[_Count];

                if (_HeadIndex + _Count <= _Capacity)
                {
                    Array.Copy(_TArray, _HeadIndex, result, 0, _Count);
                }
                else
                {
                    int part1Count = _Capacity - _HeadIndex;

                    Array.Copy(_TArray, _HeadIndex, result, 0, part1Count);
                    Array.Copy(_TArray, 0, result, part1Count, _Count - part1Count);
                }

                return result;
            }
        }

        /// <summary>
        /// 将此 IndexableQueue 转换为列表。
        /// </summary>
        /// <returns>T 列表，列表元素表示此 IndexableQueue 的元素。</returns>
        public List<T> ToList()
        {
            if (IsEmpty)
            {
                return new List<T>(0);
            }
            else
            {
                return new List<T>(ToArray());
            }
        }

        //

        /// <summary>
        /// 向此 IndexableQueue 对象的队尾添加一个元素。
        /// </summary>
        /// <param name="item">添加的元素。</param>
        public void Enqueue(T item)
        {
            if (_Count >= _Capacity)
            {
                if (_AutoExpand)
                {
                    Resize(Math.Max(_Capacity * 2, _Capacity + 4));

                    int newTailIndex = (_TailIndex + 1) % _Capacity;

                    _TArray[newTailIndex] = item;
                    _TailIndex = newTailIndex;
                    _Count++;
                }
                else
                {
                    if (_Capacity > 0)
                    {
                        int newTailIndex = (_TailIndex + 1) % _Capacity;

                        _TArray[newTailIndex] = item;
                        _HeadIndex = (_HeadIndex + 1) % _Capacity;
                        _TailIndex = newTailIndex;
                    }
                }
            }
            else
            {
                if (_Count > 0)
                {
                    int newTailIndex = (_TailIndex + 1) % _Capacity;

                    _TArray[newTailIndex] = item;
                    _TailIndex = newTailIndex;
                    _Count++;
                }
                else
                {
                    _TArray[_TailIndex] = item;
                    _Count++;
                }
            }
        }

        /// <summary>
        /// 向此 IndexableQueue 对象的队尾添加多个元素。
        /// </summary>
        /// <param name="items">添加的元素。</param>
        public void Enqueue(params T[] items)
        {
            if (items is null)
            {
                throw new ArgumentNullException();
            }

            //

            for (int i = 0; i < items.Length; i++)
            {
                Enqueue(items[i]);
            }
        }

        /// <summary>
        /// 向此 IndexableQueue 对象的队尾添加多个元素。
        /// </summary>
        /// <param name="items">添加的元素。</param>
        public void Enqueue(IEnumerable<T> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (T item in items)
            {
                Enqueue(item);
            }
        }

        /// <summary>
        /// 从此 IndexableQueue 对象的队首取出一个元素。
        /// </summary>
        /// <returns>T，取出的元素。</returns>
        public T Dequeue()
        {
            if (_Count <= 0)
            {
                throw new InvalidOperationException();
            }

            //

            T result = _TArray[_HeadIndex];

            _TArray[_HeadIndex] = default(T);
            _HeadIndex = (_HeadIndex + 1) % _Capacity;
            _Count--;

            if (_Count == 0)
            {
                _HeadIndex = 0;
                _TailIndex = 0;
            }

            return result;
        }

        /// <summary>
        /// 从此 IndexableQueue 对象的队首取出多个元素。
        /// </summary>
        /// <param name="count">取出元素的数目。</param>
        /// <param name="items">取出的元素。</param>
        /// <returns>T，取出的元素。</returns>
        public void Dequeue(int count, ICollection<T> items)
        {
            if (_Count <= 0)
            {
                throw new InvalidOperationException();
            }

            if (count <= 0 || count > _Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (items is null)
            {
                for (int i = 0; i < count; i++)
                {
                    _TArray[_HeadIndex] = default(T);
                    _HeadIndex = (_HeadIndex + 1) % _Capacity;
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    T item = _TArray[_HeadIndex];

                    _TArray[_HeadIndex] = default(T);
                    _HeadIndex = (_HeadIndex + 1) % _Capacity;

                    items.Add(item);
                }
            }

            _Count -= count;

            if (_Count == 0)
            {
                _HeadIndex = 0;
                _TailIndex = 0;
            }
        }

        //

        /// <summary>
        /// 重新设置此 IndexableQueue 对象的容量。
        /// </summary>
        /// <param name="capacity">容量。</param>
        public void Resize(int capacity)
        {
            if (capacity < 0)
            {
                throw new OverflowException();
            }

            //

            if (capacity != _Capacity)
            {
                if (capacity <= 0)
                {
                    _Capacity = 0;
                    _Count = 0;
                    _HeadIndex = 0;
                    _TailIndex = 0;
                    _TArray = _EmptyData;
                }
                else
                {
                    if (_Count <= 0)
                    {
                        _Capacity = capacity;
                        _Count = 0;
                        _HeadIndex = 0;
                        _TailIndex = 0;
                        _TArray = new T[_Capacity];
                    }
                    else
                    {
                        T[] array = new T[capacity];

                        _Count = Math.Min(capacity, _Count);

                        if (_HeadIndex + _Count <= _Capacity)
                        {
                            Array.Copy(_TArray, _HeadIndex, array, 0, _Count);
                        }
                        else
                        {
                            int part1Count = _Capacity - _HeadIndex;

                            Array.Copy(_TArray, _HeadIndex, array, 0, part1Count);
                            Array.Copy(_TArray, 0, array, part1Count, _Count - part1Count);
                        }

                        _Capacity = capacity;
                        _HeadIndex = 0;
                        _TailIndex = _Count - 1;
                        _TArray = array;
                    }
                }
            }
        }

        /// <summary>
        /// 删除此 IndexableQueue 对象的所有元素。
        /// </summary>
        public void Clear()
        {
            _Count = 0;
            _HeadIndex = 0;
            _TailIndex = 0;

            for (int i = 0; i < _Capacity; i++)
            {
                _TArray[i] = default(T);
            }
        }

        #endregion

        #region 显式接口成员实现

        #region System.Collections.IList

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                if (default(T) == null)
                {
                    if (!(value is T))
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    if (value is null || !(value is T))
                    {
                        throw new ArgumentNullException();
                    }
                }

                //

                this[index] = (T)value;
            }
        }

        int IList.Add(object item)
        {
            if (default(T) == null)
            {
                if (!(item is T))
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                if (item is null || !(item is T))
                {
                    throw new ArgumentNullException();
                }
            }

            //

            Enqueue((T)item);

            return _Count - 1;
        }

        bool IList.Contains(object item)
        {
            if (default(T) == null)
            {
                if (!(item is T))
                {
                    return false;
                }
            }
            else
            {
                if (item is null || !(item is T))
                {
                    return false;
                }
            }

            return Contains((T)item);
        }

        int IList.IndexOf(object item)
        {
            if (default(T) == null)
            {
                if (!(item is T))
                {
                    return -1;
                }
            }
            else
            {
                if (item is null || !(item is T))
                {
                    return -1;
                }
            }

            return IndexOf((T)item);
        }

        void IList.Insert(int index, object item)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.ICollection

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException();
            }

            if (array.Rank != 1)
            {
                throw new RankException();
            }

            if (array.Length - index < _Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (_Count > 0)
            {
                ToArray().CopyTo(array, index);
            }
        }

        #endregion

        #region System.Collections.IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private sealed class Enumerator : IEnumerator // 实现 System.Collections.IEnumerator 的迭代器。
        {
            private IndexableQueue<T> _Queue;
            private int _Index;

            internal Enumerator(IndexableQueue<T> queue)
            {
                _Queue = queue;
                _Index = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_Queue is null)
                    {
                        throw new ArgumentNullException();
                    }

                    if (_Queue.IsEmpty || (_Index < 0 || _Index >= _Queue._Count))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Queue._GetItemWithoutCheckBounds(_Index);
                }
            }

            bool IEnumerator.MoveNext()
            {
                if ((_Queue is null || _Queue.IsEmpty) || _Index >= _Queue._Count - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }
        }

        #endregion

        #region System.Collections.Generic.IList<T>

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T>

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void ICollection<T>.Add(T item)
        {
            Enqueue(item);
        }

        void ICollection<T>.CopyTo(T[] array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException();
            }

            if (array.Length - index < _Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            if (_Count > 0)
            {
                ToArray().CopyTo(array, index);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region System.Collections.Generic.IEnumerable<out T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new GenericEnumerator(this);
        }

        private sealed class GenericEnumerator : IEnumerator<T> // 实现 System.Collections.Generic.IEnumerator<out T> 的迭代器。
        {
            private IndexableQueue<T> _Queue;
            private int _Index;

            internal GenericEnumerator(IndexableQueue<T> queue)
            {
                _Queue = queue;
                _Index = -1;
            }

            void IDisposable.Dispose()
            {
                _Queue = null;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_Queue is null)
                    {
                        throw new ArgumentNullException();
                    }

                    if (_Queue.IsEmpty || _Index < 0 || _Index >= _Queue._Count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Queue._GetItemWithoutCheckBounds(_Index);
                }
            }

            bool IEnumerator.MoveNext()
            {
                if ((_Queue is null || _Queue.IsEmpty) || _Index >= _Queue._Count - 1)
                {
                    return false;
                }
                else
                {
                    _Index++;

                    return true;
                }
            }

            void IEnumerator.Reset()
            {
                _Index = -1;
            }

            T IEnumerator<T>.Current
            {
                get
                {
                    if (_Queue is null)
                    {
                        throw new ArgumentNullException();
                    }

                    if (_Queue.IsEmpty || (_Index < 0 || _Index >= _Queue._Count))
                    {
                        throw new IndexOutOfRangeException();
                    }

                    //

                    return _Queue._GetItemWithoutCheckBounds(_Index);
                }
            }
        }

        #endregion

        #endregion
    }
}