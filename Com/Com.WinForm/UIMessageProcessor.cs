/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

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

using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Com.WinForm
{
    /// <summary>
    /// 表示当界面消息处理器停止时应执行的方法。
    /// </summary>
    /// <param name="sender">调用 Stop 方法的控件。</param>
    /// <param name="messageProcessor">界面消息处理器。</param>
    public delegate void UIMessageProcessorStoppedHandler(Control sender, UIMessageProcessor messageProcessor);

    /// <summary>
    /// 界面消息处理器状态。
    /// </summary>
    public enum UIMessageProcessorState
    {
        /// <summary>
        /// 消息处理器尚未开始运行消息循环或已停止运行消息循环。
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// 消息处理器正在运行消息循环。
        /// </summary>
        Running,

        /// <summary>
        /// 消息处理器正在停止运行消息循环，仍有未处理完的消息在处理。
        /// </summary>
        Stopping
    }

    /// <summary>
    /// 界面消息处理器。
    /// </summary>
    public class UIMessageProcessor
    {
        #region 非公开成员

        private ReaderWriterLockSlim _MessageQueueLock = new ReaderWriterLockSlim(); // 用于消息队列的锁。
        private ReaderWriterLockSlim _StateLock = new ReaderWriterLockSlim(); // 用于状态的锁。

        //

        private Thread _MessageLoopThread; // 运行消息循环的线程。

        private Queue<UIMessage> _AsyncMessages; // 异步消息队列。
        private Queue<UIMessage> _SyncMessages; // 同步消息队列。

        private Control _Sender; // 调用 Stop 方法的控件。
        private UIMessageProcessorStoppedHandler _StoppedCallbackMethod; // 此界面消息处理器停止时的回调方法。

        //

        private UIMessageProcessorState _State; // 此界面消息处理器的状态。

        //

        private long _ProcessingAsyncMessageCount = 0; // 正在处理的异步消息的数量。

        // 获取正在处理的异步消息的数量。
        private long _GetProcessingAsyncMessageCount()
        {
            return Interlocked.Read(ref _ProcessingAsyncMessageCount);
        }

        // 增加正在处理的异步消息的数量。
        private void _IncreaseProcessingAsyncMessageCount()
        {
            Interlocked.Increment(ref _ProcessingAsyncMessageCount);
        }

        // 减少正在处理的异步消息的数量。
        private void _DecreaseProcessingAsyncMessageCount()
        {
            Interlocked.Decrement(ref _ProcessingAsyncMessageCount);
        }

        //

        // 消息循环线程执行的方法。
        private void _ThreadStartMethod()
        {
            Stopwatch sw = new Stopwatch();

            while (true)
            {
                if (State == UIMessageProcessorState.Stopping && _GetProcessingAsyncMessageCount() <= 0)
                {
                    break;
                }

                //

                sw.Restart();

                MessageLoop();

                sw.Stop();

                if (sw.ElapsedMilliseconds < 1)
                {
                    int count = 0;

                    _MessageQueueLock.EnterReadLock();

                    try
                    {
                        count = _AsyncMessages.Count + _SyncMessages.Count;
                    }
                    finally
                    {
                        _MessageQueueLock.ExitReadLock();
                    }

                    if (count > 0)
                    {
                        Thread.Sleep(0);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    Thread.Sleep(0);
                }
            }

            //

            _MessageQueueLock.EnterReadLock();

            try
            {
                _AsyncMessages.Clear();
                _SyncMessages.Clear();
            }
            finally
            {
                _MessageQueueLock.ExitReadLock();
            }

            State = UIMessageProcessorState.Stopped;

            //

            if (!(_Sender is null) && !(_StoppedCallbackMethod is null))
            {
                _Sender.Invoke(_StoppedCallbackMethod, _Sender, this);

                _Sender = null;
                _StoppedCallbackMethod = null;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 不使用任何参数初始化 UIMessageProcessor 的新实例。
        /// </summary>
        public UIMessageProcessor()
        {
            _MessageLoopThread = new Thread(new ThreadStart(_ThreadStartMethod));
            _MessageLoopThread.IsBackground = true;

            _AsyncMessages = new Queue<UIMessage>();
            _SyncMessages = new Queue<UIMessage>();

            _Sender = null;
            _StoppedCallbackMethod = null;

            _State = UIMessageProcessorState.Stopped;
        }

        //

        /// <summary>
        /// 在垃圾回收将此 UIMessageProcessor 对象回收前尝试释放资源并执行其他清理操作。
        /// </summary>
        ~UIMessageProcessor()
        {
            _MessageQueueLock.Dispose();
            _StateLock.Dispose();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取此界面消息的状态。
        /// </summary>
        public UIMessageProcessorState State
        {
            get
            {
                UIMessageProcessorState result;

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

            private set
            {
                _StateLock.EnterWriteLock();

                try
                {
                    _State = value;
                }
                finally
                {
                    _StateLock.ExitWriteLock();
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 为当前循环选择要处理的异步消息的数量与要丢弃的异步消息的次序号。
        /// </summary>
        /// <param name="messages">供选择的异步消息。</param>
        /// <param name="processCount">要处理的异步消息的数量（包括要丢弃的异步消息的数量）。</param>
        /// <param name="discardUids">要丢弃的异步消息的唯一码。</param>
        protected virtual void SelectAsyncMessagesForThisLoop(IEnumerable<UIMessage> messages, out int processCount, out HashSet<long> discardUids)
        {
            processCount = int.MaxValue;
            discardUids = null;
        }

        /// <summary>
        /// 为当前循环选择要处理的同步消息的数量与要丢弃的同步消息的次序号。
        /// </summary>
        /// <param name="messages">供选择的同步消息。</param>
        /// <param name="processCount">要处理的同步消息的数量（包括要丢弃的同步消息的数量）。</param>
        /// <param name="discardUids">要丢弃的同步消息的唯一码。</param>
        protected virtual void SelectSyncMessagesForThisLoop(IEnumerable<UIMessage> messages, out int processCount, out HashSet<long> discardUids)
        {
            processCount = int.MaxValue;
            discardUids = null;
        }

        /// <summary>
        /// 处理一个消息。
        /// </summary>
        /// <param name="message">消息。</param>
        protected virtual void ProcessMessage(UIMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行一次消息循环。
        /// </summary>
        protected virtual void MessageLoop()
        {
            if (State == UIMessageProcessorState.Running)
            {
                int asyncMessageCount;

                _MessageQueueLock.EnterReadLock();

                try
                {
                    asyncMessageCount = _AsyncMessages.Count;
                }
                finally
                {
                    _MessageQueueLock.ExitReadLock();
                }

                if (asyncMessageCount > 0)
                {
                    int processAsyncMessageCount;
                    HashSet<long> discardAsyncMessageUids;

                    _MessageQueueLock.EnterWriteLock();

                    try
                    {
                        SelectAsyncMessagesForThisLoop(_AsyncMessages, out processAsyncMessageCount, out discardAsyncMessageUids);
                    }
                    finally
                    {
                        _MessageQueueLock.ExitWriteLock();
                    }

                    if (processAsyncMessageCount < 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    else if (processAsyncMessageCount > 0)
                    {
                        if (processAsyncMessageCount > asyncMessageCount)
                        {
                            processAsyncMessageCount = asyncMessageCount;
                        }

                        //

                        List<UIMessage> asyncMessages = new List<UIMessage>(processAsyncMessageCount);

                        _MessageQueueLock.EnterWriteLock();

                        try
                        {
                            for (int i = 0; i < processAsyncMessageCount; i++)
                            {
                                asyncMessages.Add(_AsyncMessages.Dequeue());
                            }
                        }
                        finally
                        {
                            _MessageQueueLock.ExitWriteLock();
                        }

                        for (int i = 0; i < processAsyncMessageCount; i++)
                        {
                            if (State == UIMessageProcessorState.Stopping)
                            {
                                return;
                            }

                            UIMessage message = asyncMessages[i];

                            if (message.AllowDiscard && !(discardAsyncMessageUids is null) && discardAsyncMessageUids.Contains(message.Uid))
                            {
                                message.TryUpdateState(UIMessageState.Discarded);

                                message.TryReply();
                            }
                            else
                            {
                                Task.Run(() =>
                                {
                                    _IncreaseProcessingAsyncMessageCount();

                                    //

                                    message.TryUpdateState(UIMessageState.Processing);

                                    try
                                    {
                                        ProcessMessage(message);

                                        message.TryUpdateState(UIMessageState.ProcessCompleted);
                                    }
                                    catch (Exception ex)
                                    {
                                        message.Exception = ex;

                                        message.TryUpdateState(UIMessageState.ProcessFailed);
                                    }

                                    message.TryReply();

                                    //

                                    _DecreaseProcessingAsyncMessageCount();
                                });
                            }
                        }
                    }
                }
            }

            if (State == UIMessageProcessorState.Running)
            {
                int syncMessageCount;

                _MessageQueueLock.EnterReadLock();

                try
                {
                    syncMessageCount = _SyncMessages.Count;
                }
                finally
                {
                    _MessageQueueLock.ExitReadLock();
                }

                if (syncMessageCount > 0)
                {
                    int processSyncMessageCount;
                    HashSet<long> discardSyncMessageUids;

                    _MessageQueueLock.EnterWriteLock();

                    try
                    {
                        SelectSyncMessagesForThisLoop(_SyncMessages, out processSyncMessageCount, out discardSyncMessageUids);
                    }
                    finally
                    {
                        _MessageQueueLock.ExitWriteLock();
                    }

                    if (processSyncMessageCount < 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    else if (processSyncMessageCount > 0)
                    {
                        if (processSyncMessageCount > syncMessageCount)
                        {
                            processSyncMessageCount = syncMessageCount;
                        }

                        //

                        List<UIMessage> syncMessages = new List<UIMessage>(processSyncMessageCount);

                        _MessageQueueLock.EnterWriteLock();

                        try
                        {
                            for (int i = 0; i < processSyncMessageCount; i++)
                            {
                                syncMessages.Add(_SyncMessages.Dequeue());
                            }
                        }
                        finally
                        {
                            _MessageQueueLock.ExitWriteLock();
                        }

                        for (int i = 0; i < processSyncMessageCount; i++)
                        {
                            if (State == UIMessageProcessorState.Stopping)
                            {
                                return;
                            }

                            UIMessage message = syncMessages[i];

                            if (message.AllowDiscard && !(discardSyncMessageUids is null) && discardSyncMessageUids.Contains(message.Uid))
                            {
                                message.TryUpdateState(UIMessageState.Discarded);

                                message.TryReply();
                            }
                            else
                            {
                                message.TryUpdateState(UIMessageState.Processing);

                                try
                                {
                                    ProcessMessage(message);

                                    message.TryUpdateState(UIMessageState.ProcessCompleted);
                                }
                                catch (Exception ex)
                                {
                                    message.Exception = ex;

                                    message.TryUpdateState(UIMessageState.ProcessFailed);
                                }

                                message.TryReply();
                            }
                        }
                    }
                }
            }
        }

        //

        /// <summary>
        /// 开始在此界面消息处理器上运行消息循环。
        /// </summary>
        public void Start()
        {
            if (State != UIMessageProcessorState.Stopped)
            {
                throw new InvalidOperationException();
            }

            //

            _MessageLoopThread.Start();

            State = UIMessageProcessorState.Running;
        }

        /// <summary>
        /// 所有正在处理的消息处理完成后，停止在此界面消息处理器上运行消息循环。
        /// </summary>
        public void Stop()
        {
            if (State != UIMessageProcessorState.Running)
            {
                throw new InvalidOperationException();
            }

            //

            State = UIMessageProcessorState.Stopping;
        }

        /// <summary>
        /// 所有正在处理的消息处理完成后，停止在此界面消息处理器上运行消息循环。
        /// </summary>
        /// <param name="sender">调用此方法的控件。</param>
        /// <param name="callbackMethod">回调方法。</param>
        public void Stop(Control sender, UIMessageProcessorStoppedHandler callbackMethod)
        {
            if (sender is null || callbackMethod is null)
            {
                throw new ArgumentNullException();
            }

            if (State != UIMessageProcessorState.Running)
            {
                throw new InvalidOperationException();
            }

            //

            _Sender = sender;
            _StoppedCallbackMethod = callbackMethod;

            State = UIMessageProcessorState.Stopping;
        }

        //

        /// <summary>
        /// 向此界面消息处理器推送一条消息。
        /// </summary>
        /// <param name="message">消息。</param>
        public void PushMessage(UIMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException();
            }

            if (State != UIMessageProcessorState.Running)
            {
                throw new InvalidOperationException();
            }

            //

            if (!message.TryUpdateState(UIMessageState.WaitingToProcess))
            {
                throw new InvalidOperationException();
            }

            _MessageQueueLock.EnterWriteLock();

            try
            {
                Queue<UIMessage> messageQueue = (message.AllowAsync ? _AsyncMessages : _SyncMessages);

                messageQueue.Enqueue(message);
            }
            finally
            {
                _MessageQueueLock.ExitWriteLock();
            }
        }

        #endregion
    }
}