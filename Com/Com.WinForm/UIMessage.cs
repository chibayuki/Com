/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2024 chibayuki@foxmail.com

Com.WinForm.UIMessage
Version 20.10.27.1900

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Windows.Forms;

namespace Com.WinForm
{
    /// <summary>
    /// 表示当界面消息处理完成时应执行的方法。
    /// </summary>
    /// <param name="sender">发送界面消息的控件。</param>
    /// <param name="message">界面消息。</param>
    public delegate void UIMessageProcessedHandler(Control sender, UIMessage message);

    /// <summary>
    /// 界面消息。
    /// </summary>
    public class UIMessage
    {
        #region 非公开成员

        private static ReaderWriterLockSlim _UidLock = new ReaderWriterLockSlim(); // 用于生成唯一码的锁。
        private static ReaderWriterLockSlim _ReplyLock = new ReaderWriterLockSlim(); // 用于答复（引发回调方法）的锁。

        private ReaderWriterLockSlim _StateLock = new ReaderWriterLockSlim(); // 用于时刻、状态与异常的锁。
        private ReaderWriterLockSlim _DataLock = new ReaderWriterLockSlim(); // 用于请求数据与答复数据的锁。

        //

        private const long _MinUid = 0x100000000L; // 唯一码的最小值。
        private const long _MaxUid = long.MaxValue; // 唯一码的最大值。
        private static long _CurrentMaxUid = 0; // 已产生的唯一码的最大值。

        //

        private long _Uid; // 此界面消息的唯一码。
        private int _MessageCode; // 此界面消息的消息码。
        private bool _AllowAsync; // 此界面消息是否允许异步处理。
        private bool _AllowDiscard; // 此界面消息是否允许丢弃。
        private bool _NeedReply; // 此界面消息是否需要答复（引发回调方法）。
        private bool _ReplyWhenDiscarded; // 此界面消息被丢弃时是否需要答复（引发回调方法）。
        private Control _Sender; // 发送此界面消息的控件。
        private UIMessageProcessedHandler _CallbackMethod; // 此界面消息的回调方法。

        //

        private DateTime _CreateTime; // 此界面消息的创建时刻。
        private DateTime _RequestTime; // 此界面消息的请求时刻。
        private DateTime _ProcessStartTime; // 此界面消息开始处理的时刻。
        private DateTime _ProcessFinishTime; // 此界面消息处理完毕的时刻。
        private DateTime _ReplyTime; // 此界面消息的答复时刻。

        private UIMessageState _State; // 此界面消息的状态。

        private Exception _Exception; // 此界面消息在处理时发生的异常。

        //

        private object _RequestData; // 此界面消息的请求数据。
        private object _ReplyData; // 此界面消息的答复数据。

        //

        // 尝试更新此界面消息的状态。
        internal bool TryUpdateState(UIMessageState state)
        {
            _StateLock.EnterWriteLock();

            try
            {
                switch ((((long)_State) << 32) | (long)state)
                {
                    case (((long)UIMessageState.Created) << 32) | (long)UIMessageState.WaitingToProcess:
                        {
                            _RequestTime = DateTime.Now;
                            _State = state;
                        }
                        return true;

                    case (((long)UIMessageState.WaitingToProcess) << 32) | (long)UIMessageState.Processing:
                        {
                            _ProcessStartTime = DateTime.Now;
                            _State = state;
                        }
                        return true;

                    case (((long)UIMessageState.WaitingToProcess) << 32) | (long)UIMessageState.Discarded:
                        {
                            _ProcessStartTime = DateTime.Now;
                            _ProcessFinishTime = DateTime.Now;
                            _State = state;
                        }
                        return true;

                    case (((long)UIMessageState.Processing) << 32) | (long)UIMessageState.ProcessCompleted:
                        {
                            _ProcessFinishTime = DateTime.Now;
                            _State = state;
                        }
                        return true;

                    case (((long)UIMessageState.Processing) << 32) | (long)UIMessageState.ProcessFailed:
                        {
                            _ProcessFinishTime = DateTime.Now;
                            _State = state;
                        }
                        return true;

                    default:
                        return false;
                }
            }
            finally
            {
                _StateLock.ExitWriteLock();
            }
        }

        // 尝试答复此界面消息（引发回调方法）。
        internal bool TryReply()
        {
            bool result = false;

            if (_NeedReply)
            {
                UIMessageState state = State;

                if (state != UIMessageState.Discarded || (state == UIMessageState.Discarded && _ReplyWhenDiscarded))
                {
                    _ReplyLock.EnterWriteLock();

                    try
                    {
                        ReplyTime = DateTime.Now;

                        _Sender.Invoke(_CallbackMethod, _Sender, this);

                        result = true;
                    }
                    finally
                    {
                        _ReplyLock.ExitWriteLock();
                    }
                }
            }

            return result;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用消息码初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        public UIMessage(int messageCode)
        {
            _UidLock.EnterWriteLock();

            try
            {
                _Uid = unchecked(_CurrentMaxUid + 1);

                if (_Uid > _MaxUid || _Uid < _MinUid)
                {
                    _Uid = _MinUid;
                }

                _CurrentMaxUid = _Uid;
            }
            finally
            {
                _UidLock.ExitWriteLock();
            }

            //

            _MessageCode = messageCode;

            _AllowAsync = false;
            _AllowDiscard = false;
            _NeedReply = false;
            _ReplyWhenDiscarded = false;
            _Sender = null;
            _CallbackMethod = null;

            //

            _CreateTime = DateTime.Now;
            _RequestTime = DateTime.MaxValue;
            _ProcessStartTime = DateTime.MaxValue;
            _ProcessFinishTime = DateTime.MaxValue;
            _ReplyTime = DateTime.MaxValue;

            _State = UIMessageState.Created;

            _Exception = null;

            //

            _RequestData = null;
            _ReplyData = null;
        }

        /// <summary>
        /// 使用消息码与表示是否允许异步处理的布尔值初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        /// <param name="allowAsync">是否允许异步处理。</param>
        public UIMessage(int messageCode, bool allowAsync) : this(messageCode) => _AllowAsync = allowAsync;

        /// <summary>
        /// 使用消息码、表示是否允许异步处理的布尔值与表示是否允许丢弃的布尔值初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        /// <param name="allowAsync">是否允许异步处理。</param>
        /// <param name="allowDiscard">是否允许丢弃。</param>
        public UIMessage(int messageCode, bool allowAsync, bool allowDiscard) : this(messageCode)
        {
            _AllowAsync = allowAsync;
            _AllowDiscard = allowDiscard;
        }

        /// <summary>
        /// 使用消息码、发送界面消息的控件与回调方法初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        /// <param name="sender">发送界面消息的控件。</param>
        /// <param name="callbackMethod">回调方法。</param>
        public UIMessage(int messageCode, Control sender, UIMessageProcessedHandler callbackMethod) : this(messageCode)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (callbackMethod is null)
            {
                throw new ArgumentNullException(nameof(callbackMethod));
            }

            //

            _NeedReply = true;
            _Sender = sender;
            _CallbackMethod = callbackMethod;
        }

        /// <summary>
        /// 使用消息码、发送界面消息的控件、回调方法与表示是否允许异步处理的布尔值初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        /// <param name="sender">发送界面消息的控件。</param>
        /// <param name="callbackMethod">回调方法。</param>
        /// <param name="allowAsync">是否允许异步处理。</param>
        public UIMessage(int messageCode, Control sender, UIMessageProcessedHandler callbackMethod, bool allowAsync) : this(messageCode, sender, callbackMethod) => _AllowAsync = allowAsync;

        /// <summary>
        /// 使用消息码、发送界面消息的控件、回调方法、表示是否允许异步处理的布尔值与表示是否允许丢弃的布尔值初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        /// <param name="sender">发送界面消息的控件。</param>
        /// <param name="callbackMethod">回调方法。</param>
        /// <param name="allowAsync">是否允许异步处理。</param>
        /// <param name="allowDiscard">是否允许丢弃。</param>
        public UIMessage(int messageCode, Control sender, UIMessageProcessedHandler callbackMethod, bool allowAsync, bool allowDiscard) : this(messageCode, sender, callbackMethod)
        {
            _AllowAsync = allowAsync;
            _AllowDiscard = allowDiscard;
        }

        /// <summary>
        /// 使用消息码、发送界面消息的控件、回调方法、表示是否允许异步处理的布尔值、表示是否允许丢弃的布尔值与表示被丢弃时是否需要答复的布尔值初始化 UIMessage 的新实例。
        /// </summary>
        /// <param name="messageCode">消息码。</param>
        /// <param name="sender">发送界面消息的控件。</param>
        /// <param name="callbackMethod">回调方法。</param>
        /// <param name="allowAsync">是否允许异步处理。</param>
        /// <param name="allowDiscard">是否允许丢弃。</param>
        /// <param name="replyWhenDiscarded">被丢弃时是否需要答复。</param>
        public UIMessage(int messageCode, Control sender, UIMessageProcessedHandler callbackMethod, bool allowAsync, bool allowDiscard, bool replyWhenDiscarded) : this(messageCode, sender, callbackMethod)
        {
            _AllowAsync = allowAsync;
            _AllowDiscard = allowDiscard;
            _ReplyWhenDiscarded = replyWhenDiscarded;
        }

        //

        /// <summary>
        /// 在垃圾回收将此 UIMessage 对象回收前尝试释放资源并执行其他清理操作。
        /// </summary>
        ~UIMessage()
        {
            _StateLock.Dispose();
            _DataLock.Dispose();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取此界面消息的唯一码。
        /// </summary>
        public long Uid => _Uid;

        /// <summary>
        /// 获取此界面消息的消息码。
        /// </summary>
        public int MessageCode => _MessageCode;

        /// <summary>
        /// 获取表示此界面消息是否允许异步处理的布尔值。
        /// </summary>
        public bool AllowAsync => _AllowAsync;

        /// <summary>
        /// 获取表示此界面消息是否允许丢弃的布尔值。
        /// </summary>
        public bool AllowDiscard => _AllowDiscard;

        /// <summary>
        /// 获取表示此界面消息是否需要答复（引发回调方法）的布尔值。
        /// </summary>
        public bool NeedReply => _NeedReply;

        /// <summary>
        /// 获取表示此界面消息被丢弃时是否需要答复（引发回调方法）的布尔值。
        /// </summary>
        public bool ReplyWhenDiscarded => _ReplyWhenDiscarded;

        //

        /// <summary>
        /// 获取此界面消息的创建时刻。
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                DateTime result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _CreateTime;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }
        }

        /// <summary>
        /// 获取此界面消息的请求时刻。
        /// </summary>
        public DateTime RequestTime
        {
            get
            {
                DateTime result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _RequestTime;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }
        }

        /// <summary>
        /// 获取此界面消息开始处理的时刻。
        /// </summary>
        public DateTime ProcessStartTime
        {
            get
            {
                DateTime result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _ProcessStartTime;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }
        }

        /// <summary>
        /// 获取此界面消息处理完毕的时刻。
        /// </summary>
        public DateTime ProcessFinishTime
        {
            get
            {
                DateTime result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _ProcessFinishTime;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }
        }

        /// <summary>
        /// 获取此界面消息的答复时刻。
        /// </summary>
        public DateTime ReplyTime
        {
            get
            {
                DateTime result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _ReplyTime;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }

            private set
            {
                _StateLock.EnterWriteLock();

                try
                {
                    _ReplyTime = value;
                }
                finally
                {
                    _StateLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// 获取此界面消息的状态。
        /// </summary>
        public UIMessageState State
        {
            get
            {
                UIMessageState result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _State;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }
        }

        /// <summary>
        /// 获取此界面消息在处理时发生的异常。
        /// </summary>
        public Exception Exception
        {
            get
            {
                Exception result;

                _StateLock.EnterReadLock();

                try
                {
                    result = _Exception;
                }
                finally
                {
                    _StateLock.ExitReadLock();
                }

                return result;
            }

            internal set
            {
                _StateLock.EnterWriteLock();

                try
                {
                    _Exception = value;
                }
                finally
                {
                    _StateLock.ExitWriteLock();
                }
            }
        }

        //

        /// <summary>
        /// 获取或设置此界面消息的请求数据。
        /// </summary>
        public object RequestData
        {
            get
            {
                object result;

                _DataLock.EnterReadLock();

                try
                {
                    result = _RequestData;
                }
                finally
                {
                    _DataLock.ExitReadLock();
                }

                return result;
            }

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                //

                if (State == UIMessageState.Created)
                {
                    _DataLock.EnterWriteLock();

                    try
                    {
                        _RequestData = value;
                    }
                    finally
                    {
                        _DataLock.ExitWriteLock();
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// 获取或设置此界面消息的答复数据。
        /// </summary>
        public object ReplyData
        {
            get
            {
                object result;

                _DataLock.EnterReadLock();

                try
                {
                    result = _ReplyData;
                }
                finally
                {
                    _DataLock.ExitReadLock();
                }

                return result;
            }

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                //

                if (State == UIMessageState.Processing)
                {
                    _DataLock.EnterWriteLock();

                    try
                    {
                        _ReplyData = value;
                    }
                    finally
                    {
                        _DataLock.ExitWriteLock();
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        #endregion
    }
}