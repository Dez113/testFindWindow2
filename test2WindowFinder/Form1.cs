using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
// вариант 2 с работы
namespace test2WindowFinder
{
    public partial class Form1 : Form
    {
        string ip = "172.17.101.2";
        string port = "4001";
        string subnet = "255.255.255.192";
        string gw = "172.17.100.99";
        const int WM_SETTEXT = 12;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public struct Coords
        {
            public int[] Ip {get; set;}
            public int[] Port { get; set; }
            public int[] Subnet { get; set; }
            public int[] Gw { get; set; }
            public int[] Settings { get; set; }

        }
       
        public Form1()
        {
            InitializeComponent();

        }
        // private const int BN_CLICKED = 245;
        public static class WindowsFinder
        {
            public const uint WM_SETTEXT = 0x000C;
            public const int WM_GETTEXTLENGTH = 0x000E;
            public const int EM_SETSEL = 0x00B1;
            public const int EM_REPLACESEL = 0x00C2;

            [DllImport("user32.dll")]
            public static extern bool SetCursorPos(int x, int y);
            //[DllImport("user32.dll")]
            //public static extern bool SetWindowText(IntPtr hWnd, string text);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, string lParam);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SetForegroundWindow(IntPtr hWnd);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
            [DllImport("user32.dll")]
            public static extern IntPtr WindowFromPoint(System.Drawing.Point p);
            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hwndChild = IntPtr.Zero;
            IntPtr hwndFrame = IntPtr.Zero;

            Rect rect = new Rect();
            //Get a handle for the Calculator Application main window
            hwnd = WindowsFinder.FindWindow(null, "WIZ100SR/105SR/110SR Configuration Tool ver 3.0.2");
            //Console.WriteLine(hwnd);

            if (hwnd == IntPtr.Zero)
                {
                    if (MessageBox.Show("Не могу найти конфигуратор УСПД!\n" +
                                       "Запустить?",
                                       ":-)",
                                       MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                    System.Diagnostics.Process.Start(@"");
                }
            }
            else
            {
                WindowsFinder.SetForegroundWindow(hwnd);
                hwndFrame = WindowsFinder.FindWindowEx(hwnd, IntPtr.Zero, "TabStrip20WndClass", null);
                if (hwndFrame != IntPtr.Zero)
                {
                    WindowsFinder.GetWindowRect(hwndFrame, ref rect);
                }
                else
                {
                    return;
                }
                //Console.WriteLine(hwndChild);
                Coords coords = new Coords();                                                   // 
                GetBox(hwndFrame, ref coords);
                //WindowsFinder.SetCursorPos(976, 452);
                //WindowsFinder.mouse_event(2 | 4, 976, 452,0 ,0);
                hwndChild  = WindowsFinder.WindowFromPoint(new Point(976, 452));

                SetValue(WindowsFinder.WindowFromPoint(new Point(coords.Ip[0], coords.Ip[1])), ip);         //  устанавливаем значения
                SetValue(WindowsFinder.WindowFromPoint(new Point(coords.Port[0], coords.Port[1])), port);
                SetValue(WindowsFinder.WindowFromPoint(new Point(coords.Subnet[0], coords.Subnet[1])), subnet);
                SetValue(WindowsFinder.WindowFromPoint(new Point(coords.Gw[0], coords.Gw[1])), gw);
                //Console.WriteLine(coords.Settings[0].ToString()+ coords.Settings[1].ToString());
                WindowsFinder.SetCursorPos(coords.Settings[0], coords.Settings[1]);
                WindowsFinder.mouse_event(2 | 4, coords.Settings[0], coords.Settings[1], 0, 0);


                //Console.WriteLine("Lenght: "+(WindowsFinder.SendMessage(hwndChild, WindowsFinder.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero)).ToString());
               
                //hwndChild = WindowsFinder.FindWindowEx(hwnd, IntPtr.Zero, "ThunderRT6TextBox", null); //ThunderRT6TextBox", "msvb_lib_toolbar");//
                //Console.WriteLine(hwndChild);
                
            }
        }
        public static void GetBox(IntPtr Frame, ref Coords coords)                          // получение координат всех полей ввода и запись их в структуру
        {
            Rect r = new Rect();

            WindowsFinder.GetWindowRect(Frame, ref r);
            //Position p = new Position();
            coords.Ip = new int[2] { r.Left + 112 + 5, r.Top + 88 + 5 };
            coords.Port = new int[2] { r.Left + 312 + 5, r.Top + 88 + 5 };
            coords.Subnet = new int[2] { r.Left + 112 + 5, r.Top + 112 + 5};
            coords.Gw = new int[2] { r.Left + 112 + 5, r.Top + 136 + 5};
            coords.Settings = new int[2] {r.Left + 112 + 5, r.Top + 368 + 5};
        }
        public static void SetValue(IntPtr textbox, string value)                           // выделяем текст в тектсбоксе, заменяем
        {
            int lengthText = WindowsFinder.SendMessage(textbox, WindowsFinder.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            WindowsFinder.SendMessage(textbox, WindowsFinder.EM_SETSEL, IntPtr.Zero, (IntPtr)(lengthText));
            WindowsFinder.SendMessage(textbox, WindowsFinder.EM_REPLACESEL, IntPtr.Zero, value);
        }
    }
}