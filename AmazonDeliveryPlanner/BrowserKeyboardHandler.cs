﻿using CefSharp;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public class BrowserKeyboardHandler : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            bool result = false;
            Debug.WriteLine(String.Format("OnKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
            // TODO: Handle MessageNeeded cases here somehow.
            return result;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int WM_SYSKEYUP = 0x105;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int VK_TAB = 0x9;

            bool result = false;

            isKeyboardShortcut = false;

            // Don't deal with TABs by default:
            // TODO: Are there any additional ones we need to be careful of?
            // i.e. Escape, Return, etc...?
            if (windowsKeyCode == VK_TAB)
            {
                return result;
            }

            Control control = browserControl as Control;
            int msgType = 0;
            switch (type)
            {
                case KeyType.RawKeyDown:
                    if (isSystemKey)
                    {
                        msgType = WM_SYSKEYDOWN;
                    }
                    else
                    {
                        msgType = WM_KEYDOWN;
                    }
                    break;
                case KeyType.KeyUp:
                    if (isSystemKey)
                    {
                        msgType = WM_SYSKEYUP;
                    }
                    else
                    {
                        msgType = WM_KEYUP;
                    }
                    break;
                case KeyType.Char:
                    if (isSystemKey)
                    {
                        msgType = WM_SYSCHAR;
                    }
                    else
                    {
                        msgType = WM_CHAR;
                    }
                    break;
                default:
                    Trace.Assert(false);
                    break;
            }
            // We have to adapt from CEF's UI thread message loop to our fronting WinForm control here.
            // So, we have to make some calls that Application.Run usually ends up handling for us:
            PreProcessControlState state = PreProcessControlState.MessageNotNeeded;
            // We can't use BeginInvoke here, because we need the results for the return value
            // and isKeyboardShortcut. In theory this shouldn't deadlock, because
            // atm this is the only synchronous operation between the two threads.
            control.Invoke(new Action(() =>
            {
                Message msg = new Message() { HWnd = control.Handle, Msg = msgType, WParam = new IntPtr(windowsKeyCode), LParam = new IntPtr(nativeKeyCode) };

                // First comes Application.AddMessageFilter related processing:
                // 99.9% of the time in WinForms this doesn't do anything interesting.
                bool processed = Application.FilterMessage(ref msg);
                if (processed)
                {
                    state = PreProcessControlState.MessageProcessed;
                    // state = PreProcessControlState.MessageNeeded;
                }
                else
                {
                    // Next we see if our control (or one of its parents)
                    // wants first crack at the message via several possible Control methods.
                    // This includes things like Mnemonics/Accelerators/Menu Shortcuts/etc...
                    state = control.PreProcessControlMessage(ref msg);
                }
            }));
            if (state == PreProcessControlState.MessageNeeded)
            {
                // TODO: Determine how to track MessageNeeded for OnKeyEvent.
                isKeyboardShortcut = true;
            }
            else if (state == PreProcessControlState.MessageProcessed)
            {
                // Most of the interesting cases get processed by PreProcessControlMessage.
                result = true; //-- ?
                result = false;
            }
            Debug.WriteLine(String.Format("OnPreKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
            Debug.WriteLine(String.Format("OnPreKeyEvent PreProcessControlState: {0}", state));
            return result;
        }
    }
}
