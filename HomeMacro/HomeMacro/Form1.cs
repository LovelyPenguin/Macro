/*
 * C#폼을 자세히 모르는 덕분에 코드 가독성이 존나 개판임
 * 매크로 기능과 좌표 저장, 색 추출 정도는 가능
 * 스크린의 색과 저장된 색 정보를 대조하는 기능이 필요함
 * Pixel Search라는 방법이 있는 모양
 * 하 ㅅㅂ
 * ㅅㅂ
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

        Color testCol;

        ColorCodeClass color = new ColorCodeClass();

        bool macroStart = false;

        bool isFindColor = false;

        int count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = 1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MouseLocation.Text = "x : " + Cursor.Position.X.ToString() + "\n" + "y : " + Cursor.Position.Y.ToString();

            //if (macroStart)
            //{
            //    MouseMoveAndClick(mouseXpos, mouseYpos);
            //    count++;
            //    if (count >= 100)
            //    {
            //        macroStart = false;
            //    }
            //}

            if (isFindColor && macroStart)
            {
                Thread.Sleep(500);
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
                isFindColor = PixelSearch(0, 0, 688, 923, testCol);
                MouseMoveAndClick(mouseXpos, mouseYpos);
            }

            /*
            Point cursor = new Point();
            GetCursorPos(ref cursor);
            var c = GetColorAt(Cursor.Position);
            this.BackColor = c;

            if (c.R == color.red && c.G == color.green && c.B == color.blue)
            {
                Detector.Text = "Detect!";
            }
            else
                Detector.Text = "Not found";
            */
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
                Color colorCode;

                colorCode = ScreenColor(Cursor.Position.X, Cursor.Position.Y);

                testCol = GetColorAt(new Point(Cursor.Position.X, Cursor.Position.Y));

                Keyboard.Text = colorCode.R.ToString() + "\n";
                Keyboard.Text += colorCode.G.ToString() + "\n";
                Keyboard.Text += colorCode.B.ToString() + "\n";
            }

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

        /// <summary>
        /// 마우스 포인터를 기준으로 색을 잡아옴
        /// 그렇다고 1920 * 1080 연산을 돌려버릴 수는 없음.
        /// </summary>
        //Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        //public Color GetColorAt(Point location)
        //{
        //    using (Graphics gdest = Graphics.FromImage(screenPixel))
        //    {
        //        using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
        //        {
        //            IntPtr hSrcDC = gsrc.GetHdc();
        //            IntPtr hDC = gdest.GetHdc();
        //            int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
        //            gdest.ReleaseHdc();
        //            gsrc.ReleaseHdc();
        //        }
        //    }

        //    return screenPixel.GetPixel(0, 0);
        //}

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

        // 어케 쓰는거지...
        public Bitmap CaptureFromScreen(Rectangle rect)
        {
            Bitmap bmpScreenCapture = null;

            if (rect == Rectangle.Empty)//capture the whole screen
            {
                rect = Screen.PrimaryScreen.Bounds;
            }

            bmpScreenCapture = new Bitmap(rect.Width, rect.Height);

            Graphics p = Graphics.FromImage(bmpScreenCapture);


            p.CopyFromScreen(rect.X,
                     rect.Y,
                     0, 0,
                     rect.Size,
                     CopyPixelOperation.SourceCopy);


            p.Dispose();

            return bmpScreenCapture;
        }

        /// <summary>
        /// 배껴쓰는 거라 난 잘 모르겠다
        /// 속도가 아주 심히 느리다...
        /// 다른 방법을 찾아보는 것도 좋을 것 같다.
        /// 마우스 포인터의 위치도 이상함
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
    }
}
