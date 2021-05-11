using Service_Center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class ViewController
    {
        private static ViewController instance;
        User user;
        public User User { get => user ?? (user = new User()); set => user = value; }
        public static ViewController GetInstance { get => instance; }
        Window window { get; set; }
        Window miniWindow { get; set; }
        private static object syncRoot = new Object();
        protected ViewController(Window window)
        {
            this.window = window;
        }       
        public static ViewController InitializeComponent(Window window)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    instance = new ViewController(window);
                }
            }
            return instance;
        }
        public void CloseAndShow(Window window)
        {
            this.window.Close();
            this.window = window;
            this.window.Show();
        }
        public void OpenMiniWindow(Window miniWindow)
        {
            this.miniWindow = miniWindow;
            miniWindow.ShowDialog();
        }
        public void CloseMiniWindow()
        {
            this.miniWindow.Close();
        }
        public void MinWindow()
        {
            this.window.WindowState = WindowState.Minimized;
        }       
    }
}
