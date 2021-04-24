using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Service_Center.ViewModels
{
    class ViewController
    {
        private static ViewController instance;
        public static ViewController GetInstance { get => instance; }
        public Window window { get; private set; }
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
                    if (instance == null)
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
    }
}
