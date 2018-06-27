/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.WinForm.ControlSubstitution
Version 18.6.28.2100

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Com.WinForm
{
    /// <summary>
    /// 为控件的替代使用提供静态方法。
    /// </summary>
    public static class ControlSubstitution
    {
        private static Hashtable _EventKeyHashtable = new Hashtable(); // 用于存储事件键值的哈希表。
        private static EventHandlerList _Events = new EventHandlerList(); // 用于存储事件委托的列表。

        private static void _AddEventHandler(Control control, string eventName, Delegate eventHandler) // 向控件的指定事件添加一个委托。
        {
            try
            {
                if (control != null && !string.IsNullOrWhiteSpace(eventName) && eventHandler != null)
                {
                    switch (eventName)
                    {
                        case "AutoSizeChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.AutoSizeChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "BackColorChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.BackColorChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "BackgroundImageChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.BackgroundImageChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "BackgroundImageLayoutChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.BackgroundImageLayoutChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "BindingContextChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.BindingContextChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "CausesValidationChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.CausesValidationChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ChangeUICues":
                            {
                                UICuesEventHandler _EH = eventHandler as UICuesEventHandler;

                                if (_EH != null)
                                {
                                    control.ChangeUICues += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Click":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.Click += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ClientSizeChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.ClientSizeChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ContextMenuChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.ContextMenuChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ContextMenuStripChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.ContextMenuStripChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ControlAdded":
                            {
                                ControlEventHandler _EH = eventHandler as ControlEventHandler;

                                if (_EH != null)
                                {
                                    control.ControlAdded += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ControlRemoved":
                            {
                                ControlEventHandler _EH = eventHandler as ControlEventHandler;

                                if (_EH != null)
                                {
                                    control.ControlRemoved += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "CursorChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.CursorChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DockChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.DockChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DoubleClick":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.DoubleClick += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DpiChangedAfterParent":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.DpiChangedAfterParent += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DpiChangedBeforeParent":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.DpiChangedBeforeParent += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DragDrop":
                            {
                                DragEventHandler _EH = eventHandler as DragEventHandler;

                                if (_EH != null)
                                {
                                    control.DragDrop += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DragEnter":
                            {
                                DragEventHandler _EH = eventHandler as DragEventHandler;

                                if (_EH != null)
                                {
                                    control.DragEnter += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DragLeave":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.DragLeave += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "DragOver":
                            {
                                DragEventHandler _EH = eventHandler as DragEventHandler;

                                if (_EH != null)
                                {
                                    control.DragOver += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "EnabledChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.EnabledChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Enter":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.Enter += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "FontChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.FontChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ForeColorChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.ForeColorChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "GiveFeedback":
                            {
                                GiveFeedbackEventHandler _EH = eventHandler as GiveFeedbackEventHandler;

                                if (_EH != null)
                                {
                                    control.GiveFeedback += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "GotFocus":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.GotFocus += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "HandleCreated":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.HandleCreated += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "HandleDestroyed":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.HandleDestroyed += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "HelpRequested":
                            {
                                HelpEventHandler _EH = eventHandler as HelpEventHandler;

                                if (_EH != null)
                                {
                                    control.HelpRequested += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ImeModeChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.ImeModeChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Invalidated":
                            {
                                InvalidateEventHandler _EH = eventHandler as InvalidateEventHandler;

                                if (_EH != null)
                                {
                                    control.Invalidated += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "KeyDown":
                            {
                                KeyEventHandler _EH = eventHandler as KeyEventHandler;

                                if (_EH != null)
                                {
                                    control.KeyDown += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "KeyPress":
                            {
                                KeyPressEventHandler _EH = eventHandler as KeyPressEventHandler;

                                if (_EH != null)
                                {
                                    control.KeyPress += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "KeyUp":
                            {
                                KeyEventHandler _EH = eventHandler as KeyEventHandler;

                                if (_EH != null)
                                {
                                    control.KeyUp += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Layout":
                            {
                                LayoutEventHandler _EH = eventHandler as LayoutEventHandler;

                                if (_EH != null)
                                {
                                    control.Layout += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Leave":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.Leave += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "LocationChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.LocationChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "LostFocus":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.LostFocus += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MarginChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.MarginChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseCaptureChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.MouseCaptureChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseClick":
                            {
                                MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                if (_EH != null)
                                {
                                    control.MouseClick += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseDoubleClick":
                            {
                                MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                if (_EH != null)
                                {
                                    control.MouseDoubleClick += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseDown":
                            {
                                MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                if (_EH != null)
                                {
                                    control.MouseDown += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseEnter":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.MouseEnter += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseHover":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.MouseHover += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseLeave":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.MouseLeave += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseMove":
                            {
                                MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                if (_EH != null)
                                {
                                    control.MouseMove += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseUp":
                            {
                                MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                if (_EH != null)
                                {
                                    control.MouseUp += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "MouseWheel":
                            {
                                MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                if (_EH != null)
                                {
                                    control.MouseWheel += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Move":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.Move += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "PaddingChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.PaddingChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Paint":
                            {
                                PaintEventHandler _EH = eventHandler as PaintEventHandler;

                                if (_EH != null)
                                {
                                    control.Paint += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "ParentChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.ParentChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "PreviewKeyDown":
                            {
                                PreviewKeyDownEventHandler _EH = eventHandler as PreviewKeyDownEventHandler;

                                if (_EH != null)
                                {
                                    control.PreviewKeyDown += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "QueryAccessibilityHelp":
                            {
                                QueryAccessibilityHelpEventHandler _EH = eventHandler as QueryAccessibilityHelpEventHandler;

                                if (_EH != null)
                                {
                                    control.QueryAccessibilityHelp += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "QueryContinueDrag":
                            {
                                QueryContinueDragEventHandler _EH = eventHandler as QueryContinueDragEventHandler;

                                if (_EH != null)
                                {
                                    control.QueryContinueDrag += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "RegionChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.RegionChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Resize":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.Resize += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "RightToLeftChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.RightToLeftChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "SizeChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.SizeChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "StyleChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.StyleChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "SystemColorsChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.SystemColorsChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "TabIndexChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.TabIndexChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "TabStopChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.TabStopChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "TextChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.TextChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Validated":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.Validated += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "Validating":
                            {
                                CancelEventHandler _EH = eventHandler as CancelEventHandler;

                                if (_EH != null)
                                {
                                    control.Validating += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        case "VisibleChanged":
                            {
                                EventHandler _EH = eventHandler as EventHandler;

                                if (_EH != null)
                                {
                                    control.VisibleChanged += _EH;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;

                        default:
                            return;
                    }

                    string hashKey = string.Concat(control.GetType().FullName, "|", control.GetHashCode(), "|", control.Name, "|", eventName);

                    if (!_EventKeyHashtable.Contains(hashKey))
                    {
                        _EventKeyHashtable.Add(hashKey, new object());
                    }

                    object eventKey = _EventKeyHashtable[hashKey];

                    _Events.AddHandler(eventKey, eventHandler);
                }
            }
            catch { }
        }

        private static void _RemoveEventHandler(Control control, string eventName) // 删除控件的指定事件的所有委托。
        {
            try
            {
                if (control != null && !string.IsNullOrWhiteSpace(eventName))
                {
                    string hashKey = string.Concat(control.GetType().FullName, "|", control.GetHashCode(), "|", control.Name, "|", eventName);

                    if (_EventKeyHashtable.Contains(hashKey))
                    {
                        object eventKey = _EventKeyHashtable[hashKey];

                        Delegate delegates = _Events[eventKey];

                        if (delegates != null)
                        {
                            Delegate[] delegateList = delegates.GetInvocationList();

                            foreach (Delegate eventHandler in delegateList)
                            {
                                switch (eventName)
                                {
                                    case "AutoSizeChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.AutoSizeChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "BackColorChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.BackColorChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "BackgroundImageChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.BackgroundImageChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "BackgroundImageLayoutChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.BackgroundImageLayoutChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "BindingContextChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.BindingContextChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "CausesValidationChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.CausesValidationChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "ChangeUICues":
                                        {
                                            UICuesEventHandler _EH = eventHandler as UICuesEventHandler;

                                            if (_EH != null)
                                            {
                                                control.ChangeUICues -= _EH;
                                            }
                                        }
                                        break;

                                    case "Click":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.Click -= _EH;
                                            }
                                        }
                                        break;

                                    case "ClientSizeChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.ClientSizeChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "ContextMenuChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.ContextMenuChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "ContextMenuStripChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.ContextMenuStripChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "ControlAdded":
                                        {
                                            ControlEventHandler _EH = eventHandler as ControlEventHandler;

                                            if (_EH != null)
                                            {
                                                control.ControlAdded -= _EH;
                                            }
                                        }
                                        break;

                                    case "ControlRemoved":
                                        {
                                            ControlEventHandler _EH = eventHandler as ControlEventHandler;

                                            if (_EH != null)
                                            {
                                                control.ControlRemoved -= _EH;
                                            }
                                        }
                                        break;

                                    case "CursorChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.CursorChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "DockChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.DockChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "DoubleClick":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.DoubleClick -= _EH;
                                            }
                                        }
                                        break;

                                    case "DpiChangedAfterParent":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.DpiChangedAfterParent -= _EH;
                                            }
                                        }
                                        break;

                                    case "DpiChangedBeforeParent":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.DpiChangedBeforeParent -= _EH;
                                            }
                                        }
                                        break;

                                    case "DragDrop":
                                        {
                                            DragEventHandler _EH = eventHandler as DragEventHandler;

                                            if (_EH != null)
                                            {
                                                control.DragDrop -= _EH;
                                            }
                                        }
                                        break;

                                    case "DragEnter":
                                        {
                                            DragEventHandler _EH = eventHandler as DragEventHandler;

                                            if (_EH != null)
                                            {
                                                control.DragEnter -= _EH;
                                            }
                                        }
                                        break;

                                    case "DragLeave":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.DragLeave -= _EH;
                                            }
                                        }
                                        break;

                                    case "DragOver":
                                        {
                                            DragEventHandler _EH = eventHandler as DragEventHandler;

                                            if (_EH != null)
                                            {
                                                control.DragOver -= _EH;
                                            }
                                        }
                                        break;

                                    case "EnabledChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.EnabledChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "Enter":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.Enter -= _EH;
                                            }
                                        }
                                        break;

                                    case "FontChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.FontChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "ForeColorChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.ForeColorChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "GiveFeedback":
                                        {
                                            GiveFeedbackEventHandler _EH = eventHandler as GiveFeedbackEventHandler;

                                            if (_EH != null)
                                            {
                                                control.GiveFeedback -= _EH;
                                            }
                                        }
                                        break;

                                    case "GotFocus":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.GotFocus -= _EH;
                                            }
                                        }
                                        break;

                                    case "HandleCreated":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.HandleCreated -= _EH;
                                            }
                                        }
                                        break;

                                    case "HandleDestroyed":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.HandleDestroyed -= _EH;
                                            }
                                        }
                                        break;

                                    case "HelpRequested":
                                        {
                                            HelpEventHandler _EH = eventHandler as HelpEventHandler;

                                            if (_EH != null)
                                            {
                                                control.HelpRequested -= _EH;
                                            }
                                        }
                                        break;

                                    case "ImeModeChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.ImeModeChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "Invalidated":
                                        {
                                            InvalidateEventHandler _EH = eventHandler as InvalidateEventHandler;

                                            if (_EH != null)
                                            {
                                                control.Invalidated -= _EH;
                                            }
                                        }
                                        break;

                                    case "KeyDown":
                                        {
                                            KeyEventHandler _EH = eventHandler as KeyEventHandler;

                                            if (_EH != null)
                                            {
                                                control.KeyDown -= _EH;
                                            }
                                        }
                                        break;

                                    case "KeyPress":
                                        {
                                            KeyPressEventHandler _EH = eventHandler as KeyPressEventHandler;

                                            if (_EH != null)
                                            {
                                                control.KeyPress -= _EH;
                                            }
                                        }
                                        break;

                                    case "KeyUp":
                                        {
                                            KeyEventHandler _EH = eventHandler as KeyEventHandler;

                                            if (_EH != null)
                                            {
                                                control.KeyUp -= _EH;
                                            }
                                        }
                                        break;

                                    case "Layout":
                                        {
                                            LayoutEventHandler _EH = eventHandler as LayoutEventHandler;

                                            if (_EH != null)
                                            {
                                                control.Layout -= _EH;
                                            }
                                        }
                                        break;

                                    case "Leave":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.Leave -= _EH;
                                            }
                                        }
                                        break;

                                    case "LocationChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.LocationChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "LostFocus":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.LostFocus -= _EH;
                                            }
                                        }
                                        break;

                                    case "MarginChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.MarginChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseCaptureChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseCaptureChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseClick":
                                        {
                                            MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseClick -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseDoubleClick":
                                        {
                                            MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseDoubleClick -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseDown":
                                        {
                                            MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseDown -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseEnter":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseEnter -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseHover":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseHover -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseLeave":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseLeave -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseMove":
                                        {
                                            MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseMove -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseUp":
                                        {
                                            MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseUp -= _EH;
                                            }
                                        }
                                        break;

                                    case "MouseWheel":
                                        {
                                            MouseEventHandler _EH = eventHandler as MouseEventHandler;

                                            if (_EH != null)
                                            {
                                                control.MouseWheel -= _EH;
                                            }
                                        }
                                        break;

                                    case "Move":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.Move -= _EH;
                                            }
                                        }
                                        break;

                                    case "PaddingChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.PaddingChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "Paint":
                                        {
                                            PaintEventHandler _EH = eventHandler as PaintEventHandler;

                                            if (_EH != null)
                                            {
                                                control.Paint -= _EH;
                                            }
                                        }
                                        break;

                                    case "ParentChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.ParentChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "PreviewKeyDown":
                                        {
                                            PreviewKeyDownEventHandler _EH = eventHandler as PreviewKeyDownEventHandler;

                                            if (_EH != null)
                                            {
                                                control.PreviewKeyDown -= _EH;
                                            }
                                        }
                                        break;

                                    case "QueryAccessibilityHelp":
                                        {
                                            QueryAccessibilityHelpEventHandler _EH = eventHandler as QueryAccessibilityHelpEventHandler;

                                            if (_EH != null)
                                            {
                                                control.QueryAccessibilityHelp -= _EH;
                                            }
                                        }
                                        break;

                                    case "QueryContinueDrag":
                                        {
                                            QueryContinueDragEventHandler _EH = eventHandler as QueryContinueDragEventHandler;

                                            if (_EH != null)
                                            {
                                                control.QueryContinueDrag -= _EH;
                                            }
                                        }
                                        break;

                                    case "RegionChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.RegionChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "Resize":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.Resize -= _EH;
                                            }
                                        }
                                        break;

                                    case "RightToLeftChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.RightToLeftChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "SizeChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.SizeChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "StyleChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.StyleChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "SystemColorsChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.SystemColorsChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "TabIndexChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.TabIndexChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "TabStopChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.TabStopChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "TextChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.TextChanged -= _EH;
                                            }
                                        }
                                        break;

                                    case "Validated":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.Validated -= _EH;
                                            }
                                        }
                                        break;

                                    case "Validating":
                                        {
                                            CancelEventHandler _EH = eventHandler as CancelEventHandler;

                                            if (_EH != null)
                                            {
                                                control.Validating -= _EH;
                                            }
                                        }
                                        break;

                                    case "VisibleChanged":
                                        {
                                            EventHandler _EH = eventHandler as EventHandler;

                                            if (_EH != null)
                                            {
                                                control.VisibleChanged -= _EH;
                                            }
                                        }
                                        break;
                                }
                            }

                            _Events.RemoveHandler(eventKey, delegates);
                        }

                        _EventKeyHashtable.Remove(hashKey);
                    }
                }
            }
            catch { }
        }

        //

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Font font, Font mouseOverFont, Font mouseDownFont, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    if (backColor.IsEmpty)
                    {
                        mouseOverBackColor = mouseDownBackColor = Color.Empty;
                    }
                    else
                    {
                        label.BackColor = backColor;

                        if (mouseOverBackColor.IsEmpty)
                        {
                            mouseOverBackColor = backColor;
                        }

                        if (mouseDownBackColor.IsEmpty)
                        {
                            mouseDownBackColor = backColor;
                        }
                    }

                    if (foreColor.IsEmpty)
                    {
                        mouseOverForeColor = mouseDownForeColor = Color.Empty;
                    }
                    else
                    {
                        label.ForeColor = foreColor;

                        if (mouseOverForeColor.IsEmpty)
                        {
                            mouseOverForeColor = foreColor;
                        }

                        if (mouseDownForeColor.IsEmpty)
                        {
                            mouseDownForeColor = foreColor;
                        }
                    }

                    if (font == null)
                    {
                        mouseOverFont = mouseDownFont = null;
                    }
                    else
                    {
                        label.Font = font;

                        if (mouseOverFont == null)
                        {
                            mouseOverFont = font;
                        }

                        if (mouseDownFont == null)
                        {
                            mouseDownFont = font;
                        }
                    }

                    //

                    EventHandler MouseEnter = (sender, e) =>
                    {
                        if (!mouseOverBackColor.IsEmpty)
                        {
                            label.BackColor = mouseOverBackColor;
                        }

                        if (!mouseOverForeColor.IsEmpty)
                        {
                            label.ForeColor = mouseOverForeColor;
                        }

                        if (mouseOverFont != null)
                        {
                            label.Font = mouseOverFont;
                        }

                        //

                        if (mouseEnterEvent != null)
                        {
                            mouseEnterEvent(sender, e);
                        }
                    };

                    EventHandler MouseLeave = (sender, e) =>
                    {
                        if (!backColor.IsEmpty)
                        {
                            label.BackColor = backColor;
                        }

                        if (!foreColor.IsEmpty)
                        {
                            label.ForeColor = foreColor;
                        }

                        if (font != null)
                        {
                            label.Font = font;
                        }

                        //

                        if (mouseLeaveEvent != null)
                        {
                            mouseLeaveEvent(sender, e);
                        }
                    };

                    MouseEventHandler MouseDown = (sender, e) =>
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (!mouseDownBackColor.IsEmpty)
                            {
                                label.BackColor = mouseDownBackColor;
                            }

                            if (!mouseDownForeColor.IsEmpty)
                            {
                                label.ForeColor = mouseDownForeColor;
                            }

                            if (mouseDownFont != null)
                            {
                                label.Font = mouseDownFont;
                            }
                        }
                    };

                    MouseEventHandler MouseUp = (sender, e) =>
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (Geometry.CursorIsInControl(label))
                            {
                                if (!mouseOverBackColor.IsEmpty)
                                {
                                    label.BackColor = mouseOverBackColor;
                                }

                                if (!mouseOverForeColor.IsEmpty)
                                {
                                    label.ForeColor = mouseOverForeColor;
                                }

                                if (mouseOverFont != null)
                                {
                                    label.Font = mouseOverFont;
                                }
                            }
                            else
                            {
                                if (!backColor.IsEmpty)
                                {
                                    label.BackColor = backColor;
                                }

                                if (!foreColor.IsEmpty)
                                {
                                    label.ForeColor = foreColor;
                                }

                                if (font != null)
                                {
                                    label.Font = font;
                                }
                            }
                        }
                    };

                    MouseEventHandler MouseClick = (sender, e) =>
                    {
                        if (clickEvent != null && e.Button == MouseButtons.Left)
                        {
                            clickEvent(sender, EventArgs.Empty);
                        }
                    };

                    MouseEventHandler MouseDoubleClick = (sender, e) =>
                    {
                        if (doubleClickEvent != null && e.Button == MouseButtons.Left)
                        {
                            doubleClickEvent(sender, EventArgs.Empty);
                        }
                    };

                    //

                    _RemoveEventHandler(label, "MouseEnter");
                    _RemoveEventHandler(label, "MouseLeave");
                    _RemoveEventHandler(label, "MouseDown");
                    _RemoveEventHandler(label, "MouseUp");
                    _RemoveEventHandler(label, "MouseClick");
                    _RemoveEventHandler(label, "MouseDoubleClick");

                    _AddEventHandler(label, "MouseEnter", MouseEnter);
                    _AddEventHandler(label, "MouseLeave", MouseLeave);
                    _AddEventHandler(label, "MouseDown", MouseDown);
                    _AddEventHandler(label, "MouseUp", MouseUp);
                    _AddEventHandler(label, "MouseClick", MouseClick);
                    _AddEventHandler(label, "MouseDoubleClick", MouseDoubleClick);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Font font, Font mouseOverFont, Font mouseDownFont)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, backColor, mouseOverBackColor, mouseDownBackColor, font, mouseOverFont, mouseDownFont, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Font font, Font mouseOverFont, Font mouseDownFont, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, Color.Empty, Color.Empty, Color.Empty, font, mouseOverFont, mouseDownFont, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, Color.Empty, Color.Empty, Color.Empty, null, null, null, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Font font, Font mouseOverFont, Font mouseDownFont, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, null, null, backColor, mouseOverBackColor, mouseDownBackColor, font, mouseOverFont, mouseDownFont, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Font font, Font mouseOverFont, Font mouseDownFont)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, null, null, backColor, mouseOverBackColor, mouseDownBackColor, font, mouseOverFont, mouseDownFont, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, Font font, Font mouseOverFont, Font mouseDownFont, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, null, null, Color.Empty, Color.Empty, Color.Empty, font, mouseOverFont, mouseDownFont, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, null, null, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, null, null, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, EventHandler doubleClickEvent)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, doubleClickEvent, null, null, Color.Empty, Color.Empty, Color.Empty, null, null, null, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Font font, Font mouseOverFont, Font mouseDownFont, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, null, null, null, backColor, mouseOverBackColor, mouseDownBackColor, font, mouseOverFont, mouseDownFont, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Font font, Font mouseOverFont, Font mouseDownFont)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, null, null, null, backColor, mouseOverBackColor, mouseDownBackColor, font, mouseOverFont, mouseDownFont, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="font">控件的字体。</param>
        /// <param name="mouseOverFont">鼠标位于控件内时的字体。</param>
        /// <param name="mouseDownFont">鼠标按下控件时的字体。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, Font font, Font mouseOverFont, Font mouseDownFont, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, null, null, null, Color.Empty, Color.Empty, Color.Empty, font, mouseOverFont, mouseDownFont, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="foreColor">控件的前景色。</param>
        /// <param name="mouseOverForeColor">鼠标位于控件内时的前景色。</param>
        /// <param name="mouseDownForeColor">鼠标按下控件时的前景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Color foreColor, Color mouseOverForeColor, Color mouseDownForeColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, null, null, null, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null, foreColor, mouseOverForeColor, mouseDownForeColor);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, null, null, null, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 Label 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="label">Label 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        public static void LabelAsButton(Label label, EventHandler clickEvent)
        {
            try
            {
                if (label != null)
                {
                    LabelAsButton(label, clickEvent, null, null, null, Color.Empty, Color.Empty, Color.Empty, null, null, null, Color.Empty, Color.Empty, Color.Empty);
                }
            }
            catch { }
        }

        //

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="image">控件的图像。</param>
        /// <param name="mouseOverImage">鼠标位于控件内时的图像。</param>
        /// <param name="mouseDownImage">鼠标按下控件时的图像。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Image image, Image mouseOverImage, Image mouseDownImage)
        {
            try
            {
                if (pictureBox != null)
                {
                    if (backColor.IsEmpty)
                    {
                        mouseOverBackColor = mouseDownBackColor = Color.Empty;
                    }
                    else
                    {
                        pictureBox.BackColor = backColor;

                        if (mouseOverBackColor.IsEmpty)
                        {
                            mouseOverBackColor = backColor;
                        }

                        if (mouseDownBackColor.IsEmpty)
                        {
                            mouseDownBackColor = backColor;
                        }
                    }

                    if (image == null)
                    {
                        mouseOverImage = mouseDownImage = null;
                    }
                    else
                    {
                        pictureBox.Image = image;

                        if (mouseOverImage == null)
                        {
                            mouseOverImage = image;
                        }

                        if (mouseDownImage == null)
                        {
                            mouseDownImage = image;
                        }
                    }

                    //

                    EventHandler MouseEnter = (sender, e) =>
                    {
                        if (!mouseOverBackColor.IsEmpty)
                        {
                            pictureBox.BackColor = mouseOverBackColor;
                        }

                        if (mouseOverImage != null)
                        {
                            pictureBox.Image = mouseOverImage;
                        }

                        //

                        if (mouseEnterEvent != null)
                        {
                            mouseEnterEvent(sender, e);
                        }
                    };

                    EventHandler MouseLeave = (sender, e) =>
                    {
                        if (!backColor.IsEmpty)
                        {
                            pictureBox.BackColor = backColor;
                        }

                        if (image != null)
                        {
                            pictureBox.Image = image;
                        }

                        //

                        if (mouseLeaveEvent != null)
                        {
                            mouseLeaveEvent(sender, e);
                        }
                    };

                    MouseEventHandler MouseDown = (sender, e) =>
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (!mouseDownBackColor.IsEmpty)
                            {
                                pictureBox.BackColor = mouseDownBackColor;
                            }

                            if (mouseDownImage != null)
                            {
                                pictureBox.Image = mouseDownImage;
                            }
                        }
                    };

                    MouseEventHandler MouseUp = (sender, e) =>
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (Geometry.CursorIsInControl(pictureBox))
                            {
                                if (!mouseOverBackColor.IsEmpty)
                                {
                                    pictureBox.BackColor = mouseOverBackColor;
                                }

                                if (mouseOverImage != null)
                                {
                                    pictureBox.Image = mouseOverImage;
                                }
                            }
                            else
                            {
                                if (!backColor.IsEmpty)
                                {
                                    pictureBox.BackColor = backColor;
                                }

                                if (image != null)
                                {
                                    pictureBox.Image = image;
                                }
                            }
                        }
                    };

                    MouseEventHandler MouseClick = (sender, e) =>
                    {
                        if (clickEvent != null && e.Button == MouseButtons.Left)
                        {
                            clickEvent(sender, EventArgs.Empty);
                        }
                    };

                    MouseEventHandler MouseDoubleClick = (sender, e) =>
                    {
                        if (doubleClickEvent != null && e.Button == MouseButtons.Left)
                        {
                            doubleClickEvent(sender, EventArgs.Empty);
                        }
                    };

                    //

                    _RemoveEventHandler(pictureBox, "MouseEnter");
                    _RemoveEventHandler(pictureBox, "MouseLeave");
                    _RemoveEventHandler(pictureBox, "MouseDown");
                    _RemoveEventHandler(pictureBox, "MouseUp");
                    _RemoveEventHandler(pictureBox, "MouseClick");
                    _RemoveEventHandler(pictureBox, "MouseDoubleClick");

                    _AddEventHandler(pictureBox, "MouseEnter", MouseEnter);
                    _AddEventHandler(pictureBox, "MouseLeave", MouseLeave);
                    _AddEventHandler(pictureBox, "MouseDown", MouseDown);
                    _AddEventHandler(pictureBox, "MouseUp", MouseUp);
                    _AddEventHandler(pictureBox, "MouseClick", MouseClick);
                    _AddEventHandler(pictureBox, "MouseDoubleClick", MouseDoubleClick);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        /// <param name="image">控件的图像。</param>
        /// <param name="mouseOverImage">鼠标位于控件内时的图像。</param>
        /// <param name="mouseDownImage">鼠标按下控件时的图像。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent, Image image, Image mouseOverImage, Image mouseDownImage)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, Color.Empty, Color.Empty, Color.Empty, image, mouseOverImage, mouseDownImage);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="mouseEnterEvent">鼠标进入控件时引发的事件。</param>
        /// <param name="mouseLeaveEvent">鼠标离开控件时引发的事件。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, EventHandler mouseEnterEvent, EventHandler mouseLeaveEvent)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, mouseEnterEvent, mouseLeaveEvent, Color.Empty, Color.Empty, Color.Empty, null, null, null);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="image">控件的图像。</param>
        /// <param name="mouseOverImage">鼠标位于控件内时的图像。</param>
        /// <param name="mouseDownImage">鼠标按下控件时的图像。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Image image, Image mouseOverImage, Image mouseDownImage)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, null, null, backColor, mouseOverBackColor, mouseDownBackColor, image, mouseOverImage, mouseDownImage);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, null, null, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        /// <param name="image">控件的图像。</param>
        /// <param name="mouseOverImage">鼠标位于控件内时的图像。</param>
        /// <param name="mouseDownImage">鼠标按下控件时的图像。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent, Image image, Image mouseOverImage, Image mouseDownImage)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, null, null, Color.Empty, Color.Empty, Color.Empty, image, mouseOverImage, mouseDownImage);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="doubleClickEvent">鼠标双击控件时引发的事件。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, EventHandler doubleClickEvent)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, doubleClickEvent, null, null, Color.Empty, Color.Empty, Color.Empty, null, null, null);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        /// <param name="image">控件的图像。</param>
        /// <param name="mouseOverImage">鼠标位于控件内时的图像。</param>
        /// <param name="mouseDownImage">鼠标按下控件时的图像。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor, Image image, Image mouseOverImage, Image mouseDownImage)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, null, null, null, backColor, mouseOverBackColor, mouseDownBackColor, image, mouseOverImage, mouseDownImage);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="backColor">控件的背景色。</param>
        /// <param name="mouseOverBackColor">鼠标位于控件内时的背景色。</param>
        /// <param name="mouseDownBackColor">鼠标按下控件时的背景色。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, Color backColor, Color mouseOverBackColor, Color mouseDownBackColor)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, null, null, null, backColor, mouseOverBackColor, mouseDownBackColor, null, null, null);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        /// <param name="image">控件的图像。</param>
        /// <param name="mouseOverImage">鼠标位于控件内时的图像。</param>
        /// <param name="mouseDownImage">鼠标按下控件时的图像。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent, Image image, Image mouseOverImage, Image mouseDownImage)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, null, null, null, Color.Empty, Color.Empty, Color.Empty, image, mouseOverImage, mouseDownImage);
                }
            }
            catch { }
        }

        /// <summary>
        /// 将 PictureBox 控件作为 Button 控件使用。
        /// </summary>
        /// <param name="pictureBox">PictureBox 控件。</param>
        /// <param name="clickEvent">鼠标单击控件时引发的事件。</param>
        public static void PictureBoxAsButton(PictureBox pictureBox, EventHandler clickEvent)
        {
            try
            {
                if (pictureBox != null)
                {
                    PictureBoxAsButton(pictureBox, clickEvent, null, null, null, Color.Empty, Color.Empty, Color.Empty, null, null, null);
                }
            }
            catch { }
        }
    }
}