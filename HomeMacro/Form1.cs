/*
 * C#폼을 자세히 모르는 덕분에 코드 가독성이 존나 개판임
 * 매크로 기능과 좌표 저장, 색 추출 정도는 가능.
 * 스크린의 색과 저장된 색 정보를 대조하는 기능이 필요함.
 * 큰 메모리 누수는 해결했지만 작은 메모리 누수가 일어나는 모양이다.
 * 티켓링크 전용임
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace HomeMacro
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte vk, byte scan, int flags, ref int extrainfo);
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        private const uint MOUSEMOVE = 0x0001;   // 마우스 이동
        private const uint ABSOLUTEMOVE = 0x8000;   // 전역 위치
        private const uint LBUTTONDOWN = 0x0002;   // 왼쪽 마우스 버튼 눌림
        private const uint LBUTTONUP = 0x0004;   // 왼쪽 마우스 버튼 떼어짐

        int mouseXpos;
        int mouseYpos;

        int nextBtnXpos;
        int nextBtnYpos;

        int saveSeatPositionXpos;
        int saveSeatPositionYpos;

        Color colorData;
        Color popupColorData;
        bool macroStart = false;
        bool isFindColor = false;
        bool isPopup = false;

        private int MYACTION_HOTKEY_ID;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, 2, (int)Keys.F12);
            timer1.Start();
            timer1.Interval = 1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (isFindColor && macroStart)
            {
                //Thread.Sleep(300);
                MouseMoveAndClick(saveSeatPositionXpos, saveSeatPositionYpos);
                Thread.Sleep(300);
                MouseMoveAndClick(saveSeatPositionXpos, saveSeatPositionYpos);

                MouseMoveAndClick(nextBtnXpos, nextBtnYpos);
                macroStart = false;
                isFindColor = false;
            }
            if (!isFindColor && macroStart)
            {
                Detector.Text = "not found";
                isFindColor = PixelSearch(0, 0, 688, 923, colorData);
                //isPopup = PixelSearch(0, 0, 688, 923, popupColorData);
                MouseMoveAndClick(mouseXpos, mouseYpos);
            }

            if (isPopup && macroStart)
            {

            }

            // 강제 메모리 삭제
            System.GC.Collect(0, GCCollectionMode.Forced);
            System.GC.WaitForFullGCComplete();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // 좌표 가져오기
            if (e.KeyCode.ToString() == "R")
            {
                mouseXpos = Cursor.Position.X;
                mouseYpos = Cursor.Position.Y;

                SaveMousePoint.Text = "save x : " + mouseXpos.ToString() + "\n" + "save y : " + mouseYpos.ToString();
            }

            // 다음버튼 가져오기
            if (e.KeyCode.ToString() == "N")
            {
                nextBtnXpos = Cursor.Position.X;
                nextBtnYpos = Cursor.Position.Y;

                NextBtnLocation.Text = "save x : " + nextBtnXpos.ToString() + "\n" + "save y : " + nextBtnYpos.ToString();
            }

            // 컬러 가져오기
            if (e.KeyCode.ToString() == "C")
            {
                colorData = GetColorAt(new Point(Cursor.Position.X, Cursor.Position.Y));

                RGBDataLabel.Text = colorData.R.ToString() + "\n";
                RGBDataLabel.Text += colorData.G.ToString() + "\n";
                RGBDataLabel.Text += colorData.B.ToString();
            }

            // 컬러 가져오기
            //if (e.KeyCode.ToString() == "P")
            //{
            //    colorData = Color.FromArgb(225, 225, 225);
            //}

            // 매크로 스타트
            if (e.KeyCode.ToString() == "S")
            {
                macroStart = true;
            }
        }

        /// <summary>
        /// 마우스의 x,y좌표를 받고 클릭을 대행해줌
        /// </summary>
        /// <param name="xpos"> 마우스 포인터 좌표 x </param>
        /// <param name="ypos"> 마우스 포인터 좌표 y </param>
        private void MouseMoveAndClick(int xpos, int ypos)
        {
            Cursor.Position = new Point(xpos, ypos);
            mouse_event(LBUTTONDOWN, 0, 0, 0, 0);
            mouse_event(LBUTTONUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 스크린의 컬러를 받아옴
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Color ScreenColor(int x, int y)
        {
            Size sz = new Size(1, 1);
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(x, y, 0, 0, sz);
            return bmp.GetPixel(0, 0);
        }

        public Color GetColorAt(Point location)
        {
            Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        /// <summary>
        /// 배껴쓰는 거라 난 잘 모르겠다
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool PixelSearch(int left, int top, int right, int bottom, Color data)
        {
            int width = right - left;
            int height = bottom - top;
            Bitmap screenBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (Graphics gdest = Graphics.FromImage(screenBitmap))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, left, top, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color c = screenBitmap.GetPixel(i, j);
                            if (c == data)
                            {
                                Detector.Text = i + " " + j;
                                saveSeatPositionXpos = i + 5;
                                saveSeatPositionYpos = j + 5;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                // My hotkey has been typed

                // Do what you want here
                // ...
                macroStart = false;
                //Application.Exit();
                MessageBox.Show("Stop");
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 폼이 닫혔을 때 (즉 실행이 꺼졌을 때)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
