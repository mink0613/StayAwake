using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StayAwake
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private bool _isStart;

        private System.Timers.Timer _timer;

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private bool RunTimer()
        {
            string time = tbInterval.Text;
            int interval = 0;
            if (int.TryParse(time, out interval) == false)
            {
                MessageBox.Show("Please enter valid interval in milli second");
                return false;
            }

            _timer = new System.Timers.Timer();
            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = interval;
            _timer.Start();

            return true;
        }

        private void StopTimer()
        {
            _timer.Stop();
            _timer = null;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Run!!");
            DoMouseClick();
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_isStart)
            {
                btnStart.Text = "Stop";
                if (!RunTimer())
                {
                    return;
                }
            }
            else
            {
                btnStart.Text = "Start";
                StopTimer();
            }

            _isStart = !_isStart;
            Console.WriteLine("Clicked!!");
        }
    }
}
