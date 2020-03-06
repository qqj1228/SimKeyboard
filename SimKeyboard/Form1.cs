using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimKeyboard {
    public partial class Form1 : Form {
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetFocus();

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        static extern int PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        // 定义相关窗口消息
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_CHAR = 0x0102;

        private readonly HotKeyClass m_hotKey;
        private readonly int m_keyID;
        private StringBuilder m_keyString;
        private readonly Config m_cfg;
        public Form1() {
            InitializeComponent();
            m_hotKey = new HotKeyClass();
            m_keyID = 10;
            m_cfg = new Config();
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            if (m_keyString != null) {
                if (m_keyString.Length > 0) {
                    m_hotKey.UnRegist(this.Handle, CallBack);
                }
                if (Regist(m_keyString.ToString())) {
                    lblInfo.Text = m_keyString.ToString() + " 注册成功";
                    m_cfg.Setting.Data.HotKey = m_keyString.ToString();
                    if (!m_cfg.Setting.Data.SendTexts.Contains(this.cmbBoxInput.Text) && this.cmbBoxInput.Text.Length > 0) {
                        m_cfg.Setting.Data.SendTexts.Add(this.cmbBoxInput.Text);
                        this.cmbBoxInput.Items.Add(this.cmbBoxInput.Text);
                    }
                    m_cfg.SaveConfig(m_cfg.Setting);
                } else {
                    lblInfo.Text = "";
                }
            }
        }

        private bool Regist(string str) {
            if (str.Length == 0) {
                return false;
            }
            int modifiers = 0;
            Keys vk = Keys.None;
            foreach (string value in str.Split('+')) {
                if (value.Trim() == "Ctrl") {
                    modifiers += (int)HotKeyClass.HotkeyModifiers.Control;
                } else if (value.Trim() == "Alt") {
                    modifiers += (int)HotKeyClass.HotkeyModifiers.Alt;
                } else if (value.Trim() == "Shift") {
                    modifiers += (int)HotKeyClass.HotkeyModifiers.Shift;
                } else {
                    if (Regex.IsMatch(value, @"[0-9]")) {
                        vk = (Keys)Enum.Parse(typeof(Keys), "D" + value.Trim());
                    } else {
                        vk = (Keys)Enum.Parse(typeof(Keys), value.Trim());
                    }
                }
            }
            try {
                m_hotKey.Regist(this.Handle, m_keyID, modifiers, vk, CallBack);
                return true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        // 按下热键时被调用的方法
        public void CallBack() {
            // 获得当前激活的窗口句柄
            IntPtr hForeWnd = GetForegroundWindow();
            // 获取本身的线程ID
            uint dwSelfThreadId = GetCurrentThreadId();
            // 根据窗口句柄获取线程ID
            uint dwForeThreadId = GetWindowThreadProcessId(hForeWnd, (IntPtr)null);
            // 附加线程
            if (AttachThreadInput(dwForeThreadId, dwSelfThreadId, true)) {
                // 获取具有输入焦点的窗口句柄
                IntPtr hFocus = GetFocus();
                string input = this.cmbBoxInput.Text.Replace("\\n", "\n").Replace("\\r", "\r");
                foreach (char item in input) {
                    PostMessage(hFocus, WM_CHAR, item, 0);
                }
                // 取消附加的线程
                AttachThreadInput(dwForeThreadId, dwSelfThreadId, false);
                this.lblInfo.Text = "已发送：\n" + this.cmbBoxInput.Text;
            }
            // SendKeys必须在本程序主窗口或子窗口处于激活状态下才有效，
            // 例如使用MessageBox.Show()先强制用户激活本程序子窗口
            // keybd_event()也是同样道理
        }

        protected override void WndProc(ref Message m) {
            // 窗口消息处理函数
            base.WndProc(ref m);
            m_hotKey.ProcessHotKey(m);
        }

        private void TxtBoxHotKey_KeyDown(object sender, KeyEventArgs e) {
            m_keyString = new StringBuilder {
                Length = 0
            };
            m_keyString.Append("");
            if (e.Modifiers != 0) {
                if (e.Control) {
                    m_keyString.Append("Ctrl + ");
                }
                if (e.Alt) {
                    m_keyString.Append("Alt + ");
                }
                if (e.Shift) {
                    m_keyString.Append("Shift + ");
                }
            }
            if ((e.KeyValue >= 0x21 && e.KeyValue <= 0x28) ||  // 编辑键
                (e.KeyValue >= 0x41 && e.KeyValue <= 0x5A) ||  // 字母键
                (e.KeyValue >= 0x70 && e.KeyValue <= 0x7B)) {  // F1-F12
                m_keyString.Append(e.KeyCode);
            } else if ((e.KeyValue >= 0x30 && e.KeyValue <= 0x39)) {  // 数字键
                m_keyString.Append(e.KeyCode.ToString().Substring(1));
            }
            ((TextBox)sender).Text = m_keyString.ToString();
        }

        private void TxtBoxHotKey_KeyUp(object sender, KeyEventArgs e) {
            string str = ((TextBox)sender).Text.TrimEnd();
            if (str.Length >= 1 && str.Substring(str.Length - 1) == "+") {
                ((TextBox)sender).Text = "";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            m_hotKey.UnRegist(this.Handle, CallBack);
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.Text += " - v" + MainFileVersion.AssemblyVersion;
            this.txtBoxHotKey.Text = m_cfg.Setting.Data.HotKey;
            m_keyString = new StringBuilder(m_cfg.Setting.Data.HotKey);
            this.cmbBoxInput.Items.AddRange(m_cfg.Setting.Data.SendTexts.ToArray());
        }
    }

    public class HotKeyClass {
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        /// <summary>
        /// 注册全局热键 Win32 API
        /// 如果函数执行成功，返回值不为0。
        /// 如果函数执行失败，返回值为0。要得到详细错误信息，调用GetLastError。
        /// </summary>
        /// <param name="hWnd">定义热键的窗口的句柄</param>
        /// <param name="id">定义热键 ID（不能与其它ID重复）</param>
        /// <param name="modifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
        /// <param name="vk">定义热键的内容</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);

        /// <summary>
        /// 注销全局热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的 ID </param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 定义WM_HOTKEY窗口消息
        const int WM_HOTKEY = 0x312;

        public delegate void HotKeyCallBackHanlder();
        private readonly Dictionary<int, HotKeyCallBackHanlder> keymap; // 每一个key对于一个处理函数

        /// <summary>
        /// 组合控制键
        /// </summary>
        [Flags]
        public enum HotkeyModifiers : int {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public HotKeyClass() {
            keymap = new Dictionary<int, HotKeyCallBackHanlder>();
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="id">热键 ID</param>
        /// <param name="modifiers">组合控制键</param>
        /// <param name="vk">热键</param>
        /// <param name="callBack">回调函数</param>
        public void Regist(IntPtr hWnd, int id, int modifiers, Keys vk, HotKeyCallBackHanlder callBack) {
            if (!RegisterHotKey(hWnd, id, modifiers, vk)) {
                if (Marshal.GetLastWin32Error() == 1409) {
                    throw new ApplicationException("热键被占用 ！");
                } else {
                    throw new ApplicationException("注册失败！");
                }
            }
            keymap[id] = callBack;
        }

        /// <summary>
        /// 注销热键，使用回调函数识别
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="callBack">热键回调函数</param>
        public void UnRegist(IntPtr hWnd, HotKeyCallBackHanlder callBack) {
            foreach (KeyValuePair<int, HotKeyCallBackHanlder> item in keymap) {
                if (item.Value == callBack) {
                    UnregisterHotKey(hWnd, item.Key);
                    keymap.Remove(item.Key);
                    return;
                }
            }
        }

        /// <summary>
        /// 注销热键，使用热键 ID 识别
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="keyID">热键 ID</param>
        public void UnRegist(IntPtr hWnd, int keyID) {
            UnregisterHotKey(hWnd, keyID);
            keymap.Remove(keyID);
            return;
        }

        /// <summary>
        /// 热键消息处理
        /// </summary>
        /// <param name="m">窗口消息</param>
        public void ProcessHotKey(Message m) {
            if (m.Msg == WM_HOTKEY) {
                int id = m.WParam.ToInt32();
                if (keymap.TryGetValue(id, out HotKeyCallBackHanlder callback)) {
                    callback();
                }
            }
        }
    }

    public static class MainFileVersion {
        public static Version AssemblyVersion {
            get { return ((Assembly.GetEntryAssembly()).GetName()).Version; }
        }

        public static Version AssemblyFileVersion {
            get { return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion); }
        }

        public static string AssemblyInformationalVersion {
            get { return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion; }
        }
    }

}
