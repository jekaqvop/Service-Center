using Newtonsoft.Json;
using Service_Center.Models;
using Service_Center.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Center.ViewModels
{

    class ViewManager
    {
        private static ViewManager instance;
        User user;
        public User User 
        { 
            get => user ?? (user = new User()); 
            set 
            {
                user = value;
                Themes theme = Themes.InitializeComponent();
            } 
        }
        public static ViewManager GetInstance { get => instance; }
        Window window { get; set; }
        Window miniWindow { get; set; }
        private static object syncRoot = new Object();
        protected ViewManager(Window window)
        {
            this.window = window;
        }       
        public static ViewManager InitializeComponent(Window window)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    instance = new ViewManager(window);                    
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
