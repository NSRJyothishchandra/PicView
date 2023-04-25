﻿using AutoUpdaterDotNET;
using Microsoft.VisualBasic.Logging;
using PicView.Animations;
using PicView.ConfigureSettings;
using PicView.Shortcuts;
using PicView.SystemIntegration;
using PicView.UILogic;
using PicView.UILogic.Sizing;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace PicView.Views.Windows
{
    public partial class InfoWindow : Window
    {
        private double startheight, extendedheight;

        public InfoWindow()
        {
            InitializeComponent();

            extendedheight = 750;
            startheight = Height;

            // Get version
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            appVersion.Text += " " + fvi.FileVersion;

            ContentRendered += Window_ContentRendered;

            MaxWidth = MinWidth = 565 * WindowSizing.MonitorInfo.DpiScaling;
            if (double.IsNaN(Width)) // Fixes if user opens window when loading from startup
            {
                WindowSizing.MonitorInfo = MonitorSize.GetMonitorSize();
                MaxHeight = WindowSizing.MonitorInfo.WorkArea.Height;
                Width *= WindowSizing.MonitorInfo.DpiScaling;
                MaxWidth = MinWidth = 565 * WindowSizing.MonitorInfo.DpiScaling;
            }
        }

        public void ChangeColor()
        {
            Logo.ChangeColor();
            AccentBrush.Brush = new SolidColorBrush(ConfigColors.GetSecondaryAccentColor);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            WindowBlur.EnableBlur(this);
            ChangeColor();

            MouseLeftButtonDown += (_, e) =>
            { if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); } };

            // ExpandButton
            ExpandButton.MouseEnter += (_, _) => MouseOverAnimations.ButtonMouseOverAnim(chevronDownBrush);
            ExpandButton.MouseEnter += (_, _) => AnimationHelper.MouseEnterBgTexColor(ExpandButtonBg);
            ExpandButton.MouseLeave += (_, _) => MouseOverAnimations.ButtonMouseLeaveAnim(chevronDownBrush);
            ExpandButton.MouseLeave += (_, _) => AnimationHelper.MouseLeaveBgTexColor(ExpandButtonBg);

            ExpandButton.Click += (_, _) => UIHelper.ExtendOrCollopase(Height, startheight, extendedheight, this, Scroller, xGeo);

            PreviewMouseWheel += (_, e) => // Collapse when scrolling down
            {
                if (e.Delta < 0 && Height == startheight)
                {
                    UIHelper.ExtendOrCollopase(Height, startheight, extendedheight, this, Scroller, xGeo);
                }
            };

            KeyDown += (_, e) => GenericWindowShortcuts.KeysDown(Scroller, e, this);
            Scroller.MouseWheel += (_, e) => GenericWindowShortcuts.Window_MouseWheel(Scroller, e);
            TitleBar.MouseLeftButtonDown += (_, _) => DragMove();

            CloseButton.TheButton.Click += delegate { Hide(); };
            MinButton.TheButton.Click += delegate { SystemCommands.MinimizeWindow(this); };

            Iconic.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, IconicBrush, false); };
            Iconic.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, IconicBrush, false); };

            Ionic.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, IonicBrush, false); };
            Ionic.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, IonicBrush, false); };

            FontAwesome.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, FontAwesomeBrush, false); };
            FontAwesome.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, FontAwesomeBrush, false); };


            License.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, LicenseBrush, false); };
            License.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, LicenseBrush, false); };

            zondicons.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, zondiconsBrush, false); };
            zondicons.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, zondiconsBrush, false); };

            freepik.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, FreepikBrush, false); };
            freepik.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, FreepikBrush, false); };

            PicViewSite.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, PicViewBrush, false); };
            PicViewSite.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, PicViewBrush, false); };

            UpdateButton.MouseLeftButtonDown += delegate
            {
                AutoUpdater.ShowRemindLaterButton = false;
                AutoUpdater.ReportErrors = true;
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.Start("https://picview.org/update.xml");
            };

            UpdateButton.MouseEnter += delegate { AnimationHelper.MouseOverColorEvent(255, 255, 255, 255, UpdateText, false); };
            UpdateButton.MouseEnter += delegate { AnimationHelper.MouseEnterBgTexColor(UpdateBrush); };
            UpdateButton.MouseLeave += delegate { AnimationHelper.MouseLeaveColorEvent(255, 255, 255, 255, UpdateText, false); };
            UpdateButton.MouseLeave += delegate { AnimationHelper.MouseLeaveBgTexColor(UpdateBrush); };
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var ps = new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }
    }
}