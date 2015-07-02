using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace ShutdownManager
{
    public partial class ShutdownManager : Form
    {

        public int elapsedTimeSinceNotCharging { get; set; }
        public ShutdownManager()
        {
            InitializeComponent();
        }

        private void ShutdownManager_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
            handleNewBatteryState();
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            handleNewBatteryState();
        }

        private void handleNewBatteryState()
        {
            if ((SystemInformation.PowerStatus.BatteryChargeStatus & BatteryChargeStatus.Charging) == BatteryChargeStatus.Charging)
            {
                lblBatteryState.Text = "Charging, AC plugged";
                resetShutdownTimer();
            }
            else
            {
                lblBatteryState.Text = "Discharging, AC unplugged";
                launchShutdownTimer();
            }
        }

        private void launchShutdownTimer()
        {
            timer.Start();
        }

        private void resetShutdownTimer()
        {
            lblShutdownSeconds.Text = "No shutdown planned";
            timer.Stop();
            elapsedTimeSinceNotCharging = 0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            elapsedTimeSinceNotCharging++;
            lblShutdownSeconds.Text = (numTimeBeforeShutdown.Value - elapsedTimeSinceNotCharging) + "";
            if(elapsedTimeSinceNotCharging > numTimeBeforeShutdown.Value)
            {
                shutdown();
            }
        }

        private void shutdown()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        private void numTimeBeforeShutdown_ValueChanged(object sender, EventArgs e)
        {
            elapsedTimeSinceNotCharging = 0;
        }
    }
}
