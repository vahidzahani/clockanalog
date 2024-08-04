using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace clockanalog
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer clockTimer;
        private int lastMinute;

        // Import the MessageBeep function from user32.dll
        [DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000; // 1 second
            clockTimer.Tick += new EventHandler(this.OnTimerTick);
            clockTimer.Start();

            lastMinute = DateTime.Now.Minute;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {

            DateTime now = DateTime.Now;
            if (now.Minute != lastMinute)
            {
                MessageBeep(0xFFFFFFFF); // Play the system beep sound
                lastMinute = now.Minute;
            }

            this.Invalidate(); // Force the form to be redrawn
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Center of the clock
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Clock radius
            int radius = Math.Min(centerX, centerY) - 10;

            // Draw clock face with thicker pen
            Pen thickPen = new Pen(Color.Black, 10);
            g.DrawEllipse(thickPen, centerX - radius, centerY - radius, radius * 2, radius * 2);

            // Draw clock ticks
            for (int i = 1; i <= 12; i++)
            {
                double angle = i * Math.PI / 6; // 30 degrees in radians
                int tickStartX = centerX + (int)(Math.Cos(angle) * (radius - 10));
                int tickStartY = centerY - (int)(Math.Sin(angle) * (radius - 10));
                int tickEndX = centerX + (int)(Math.Cos(angle) * (radius - 20));
                int tickEndY = centerY - (int)(Math.Sin(angle) * (radius - 20));
                g.DrawLine(thickPen, tickStartX, tickStartY, tickEndX, tickEndY);
            }

            // Get current time
            DateTime now = DateTime.Now;
            int hour = now.Hour % 12;
            int minute = now.Minute;
            int second = now.Second;

            // Calculate hand angles
            double hourAngle = (hour + minute / 60.0) * 30 * Math.PI / 180; // 30 degrees per hour
            double minuteAngle = (minute + second / 60.0) * 6 * Math.PI / 180; // 6 degrees per minute
            double secondAngle = second * 6 * Math.PI / 180; // 6 degrees per second

            // Draw hour hand
            DrawHand(g, centerX, centerY, hourAngle, radius * 0.5f, 15, Brushes.Black);

            // Draw minute hand
            DrawHand(g, centerX, centerY, minuteAngle, radius * 0.75f, 15, Brushes.Black);

            // Draw second hand
            DrawHand(g, centerX, centerY, secondAngle, radius * 0.9f, 5, Brushes.Red);

            // Draw center circle
            int centerCircleRadius = 20;
            g.FillEllipse(Brushes.Black, centerX - centerCircleRadius, centerY - centerCircleRadius, centerCircleRadius * 2, centerCircleRadius * 2);
        }

        private void DrawHand(Graphics g, int centerX, int centerY, double angle, float length, int width, Brush brush)
        {
            int x = centerX + (int)(Math.Sin(angle) * length);
            int y = centerY - (int)(Math.Cos(angle) * length);
            Pen pen = new Pen(brush, width);
            g.DrawLine(pen, centerX, centerY, x, y);
        }
    }
}
