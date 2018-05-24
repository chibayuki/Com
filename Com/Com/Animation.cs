﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.Animation
Version 18.5.23.0000

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace Com
{
    /// <summary>
    /// 为动画的绘制提供静态方法。
    /// </summary>
    public static class Animation
    {
        /// <summary>
        /// 表示用于绘制动画的某一帧的方法。
        /// </summary>
        /// <param name="frameId">当前帧在动画所有帧中的次序数（从 1 开始计数）。</param>
        /// <param name="frameCount">动画的总帧数。</param>
        /// <param name="msPerFrame">动画每帧持续的毫秒数。</param>
        public delegate void Frame(int frameId, int frameCount, int msPerFrame);

        /// <summary>
        /// 按照指定的方法、总帧数、每帧的持续时长、关键帧密度与关键帧列表绘制动画。
        /// </summary>
        /// <param name="frame">用于绘制动画的某一帧的方法。</param>
        /// <param name="frameCount">动画的总帧数。</param>
        /// <param name="msPerFrame">动画每帧持续的毫秒数。</param>
        /// <param name="keyFrameDensity">表示自动画的第一帧起，每连续若干帧中的第一帧为关键帧。</param>
        /// <param name="keyFrameList">包含所有关键帧次序数的列表。</param>
        public static void Show(Frame frame, int frameCount, int msPerFrame, int keyFrameDensity, List<int> keyFrameList)
        {
            try
            {
                if (frame != null && frameCount > 0 && msPerFrame > 0)
                {
                    if (keyFrameList == null)
                    {
                        keyFrameList = new List<int>(0);
                    }

                    if (keyFrameList.Count > 0)
                    {
                        for (int i = keyFrameList.Count - 1; i >= 0; i--)
                        {
                            if (keyFrameList[i] <= 1 || keyFrameList[i] >= frameCount)
                            {
                                keyFrameList.RemoveAt(i);
                            }
                        }
                    }

                    if (keyFrameDensity > 0)
                    {
                        keyFrameList.Capacity = keyFrameList.Count + (frameCount - 1) / keyFrameDensity;

                        for (int frameId = 1 + keyFrameDensity; frameId < frameCount; frameId += keyFrameDensity)
                        {
                            if (!keyFrameList.Contains(frameId))
                            {
                                keyFrameList.Add(frameId);
                            }
                        }
                    }

                    if (keyFrameList.Count > 0)
                    {
                        keyFrameList.Sort();
                        keyFrameList.Reverse();

                        for (int i = keyFrameList.Count - 1; i > 0; i--)
                        {
                            if (keyFrameList[i] == keyFrameList[i - 1])
                            {
                                keyFrameList.RemoveAt(i);
                            }
                        }
                    }

                    //

                    int MSTotal = 0;

                    for (int frameId = 1; frameId <= frameCount; frameId++)
                    {
                        DateTime DTFrame = DateTime.Now;

                        frame(frameId, frameCount, msPerFrame);

                        if (keyFrameList.Count > 0)
                        {
                            if (keyFrameList[keyFrameList.Count - 1] == frameId)
                            {
                                keyFrameList.RemoveAt(keyFrameList.Count - 1);
                            }
                            else
                            {
                                while (keyFrameList.Count > 0 && keyFrameList[keyFrameList.Count - 1] <= frameId)
                                {
                                    keyFrameList.RemoveAt(keyFrameList.Count - 1);
                                }
                            }
                        }

                        if (frameId < frameCount)
                        {
                            int MSFrame = Math.Max(1, (int)Math.Round((DateTime.Now - DTFrame).TotalMilliseconds));

                            MSTotal += MSFrame;

                            if (MSTotal < (frameId - 0.5) * msPerFrame)
                            {
                                DateTime DTSleep = DateTime.Now;

                                Thread.Sleep(Math.Max(1, frameId * msPerFrame - MSTotal));

                                int MSSleep = Math.Max(1, (int)Math.Round((DateTime.Now - DTSleep).TotalMilliseconds));

                                MSTotal += MSSleep;
                            }
                            else if (frameId < frameCount - 1 && MSTotal > (frameId + 0.5) * msPerFrame)
                            {
                                int frameIdNew = Math.Min(frameCount, Math.Max((int)Math.Round((double)MSTotal / msPerFrame) + 1, frameId + 1));

                                if (keyFrameList.Count > 0)
                                {
                                    int NextKeyFrame = Math.Min(frameCount, Math.Max(keyFrameList[keyFrameList.Count - 1], frameId + 1));

                                    if (frameIdNew >= NextKeyFrame)
                                    {
                                        frameIdNew = NextKeyFrame;
                                    }
                                }

                                frameId = frameIdNew - 1;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 按照指定的方法、总帧数、每帧的持续时长与关键帧密度绘制动画。
        /// </summary>
        /// <param name="frame">用于绘制动画的某一帧的方法。</param>
        /// <param name="frameCount">动画的总帧数。</param>
        /// <param name="msPerFrame">动画每帧持续的毫秒数。</param>
        /// <param name="keyFrameDensity">表示自动画的第一帧起，每连续若干帧中的第一帧为关键帧。</param>
        public static void Show(Frame frame, int frameCount, int msPerFrame, int keyFrameDensity)
        {
            try
            {
                if (frame != null && frameCount > 0 && msPerFrame > 0)
                {
                    Show(frame, frameCount, msPerFrame, keyFrameDensity, null);
                }
            }
            catch { }
        }

        /// <summary>
        /// 按照指定的方法、总帧数、每帧的持续时长与关键帧列表绘制动画。
        /// </summary>
        /// <param name="frame">用于绘制动画的某一帧的方法。</param>
        /// <param name="frameCount">动画的总帧数。</param>
        /// <param name="msPerFrame">动画每帧持续的毫秒数。</param>
        /// <param name="keyFrameList">包含所有关键帧次序数的列表。</param>
        public static void Show(Frame frame, int frameCount, int msPerFrame, List<int> keyFrameList)
        {
            try
            {
                if (frame != null && frameCount > 0 && msPerFrame > 0)
                {
                    Show(frame, frameCount, msPerFrame, 0, keyFrameList);
                }
            }
            catch { }
        }

        /// <summary>
        /// 按照指定的方法、总帧数与每帧的持续时长绘制动画。
        /// </summary>
        /// <param name="frame">用于绘制动画的某一帧的方法。</param>
        /// <param name="frameCount">动画的总帧数。</param>
        /// <param name="msPerFrame">动画每帧持续的毫秒数。</param>
        public static void Show(Frame frame, int frameCount, int msPerFrame)
        {
            try
            {
                if (frame != null && frameCount > 0 && msPerFrame > 0)
                {
                    Show(frame, frameCount, msPerFrame, 0, null);
                }
            }
            catch { }
        }
    }
}