﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Diagnostics;

namespace VPNClient {
    public partial class MainForm : Form {
        private ChromiumWebBrowser chrome { get; }
        private double factor = 1.0f;
        private JSCallback callback { get; }

        public MainForm() {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.chrome = new ChromiumWebBrowser("https://q64.co/endpoint/win") {
                Dock = DockStyle.Fill,
            };
            chrome.FrameLoadEnd += browserOnFrameLoadEnd;
            int currentDPI = 0;
            using (Graphics g = this.CreateGraphics()) {
                currentDPI = (int)g.DpiX;
            }
            this.factor = currentDPI / 96.0;
            this.Controls.Add(chrome);
            this.callback = new JSCallback(chrome);
            chrome.RegisterJsObject("cefobj", callback);

            Debug.WriteLine("Running CEF at scale factor " + factor + " for DPI");
        }

        private void browserOnFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs) {
            ChromiumWebBrowser browser = (ChromiumWebBrowser)sender;
            browser.SetZoomLevel(factor * 2);
        }
    }
}
