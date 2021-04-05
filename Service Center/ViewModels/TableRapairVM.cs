using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Models;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Service_Center.ViewModels
{
    class TableRapairVM : INotifyPropertyChanged
    {
        public List<string> Status { get; set; }
        #region OnpropertyChanget
        Context context = new Context();
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public TableRapairVM() 
        {
                
        }
        TableRapair TableRapairs { get; set; }
        public TableRapairVM(TableRapair TableRapairs)
        {
            this.TableRapairs = TableRapairs; 
            context.Rapairs.Load();
            TableRapairs.RapairGrid.ItemsSource = context.Rapairs.Local.ToBindingList();
            this.Status = new List<string>();
            this.Status.Add("Ожидание диагностики");
            this.Status.Add("На согласовании с клиентом");
            this.Status.Add("Производится ремонт");
            this.Status.Add("Ожидание оплаты");            
            TableRapairs.Status.ItemsSource = this.Status;
            //Rapairs = context.Rapairs.Local;      
        }
        Rapair rapair;
        public User Rapair { get; set; }
        ObservableCollection<Rapair> rapairs;
        public ObservableCollection<Rapair> Rapairs { get; set; } = new ObservableCollection<Rapair>();
        #region Command
        public ICommand SaveChanges
        {
            get => new DelegateCommand((obj) =>
            {
                context.SaveChanges();
            });
        }
        #endregion
        public ICommand DeleteRow
        {
            get => new DelegateCommand((obj) =>
            {
                if (TableRapairs.RapairGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < TableRapairs.RapairGrid.SelectedItems.Count; i++)
                    {
                        Rapair rapair = TableRapairs.RapairGrid.SelectedItems[i] as Rapair;
                        if (rapair != null)
                        {
                            context.Rapairs.Remove(rapair);
                        }
                    }
                }
                context.SaveChanges();
            });
        }
        public ICommand UpdateDataGrid
        {
            get => new DelegateCommand((obj) =>
            {
                this.TableRapairs = TableRapairs;
                context.Rapairs.Load();
                TableRapairs.RapairGrid.ItemsSource = context.Rapairs.Local.ToBindingList();
            });        
        }
    }

}
