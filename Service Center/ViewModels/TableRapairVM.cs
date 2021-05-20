using Service_Center.Commands;
using Service_Center.Contexts;
using Service_Center.Converters;
using Service_Center.Models;
using Service_Center.Repository;
using Service_Center.Resources;
using Service_Center.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Service_Center.ViewModels
{

    class TableRapairVM : PropertysChanged
    {

        UnitOfWork unitOfWork;
        //Rapair selectRapair;
        public Rapair SelectRapair { get; set; }
        public ObservableCollection<string> ServiceTitleList { get; set; }
        public TableRapairVM()
        {
            unitOfWork = new UnitOfWork();
            Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList());
            ServiceTitleList = new ObservableCollection<string>();
            foreach (Service service in unitOfWork.Services.GetItemList())
            {
                ServiceTitleList.Add(service.Title);
            }
            OnPropertyChanged("ServiceTitleList");
            CompletedWorks = unitOfWork.Services.GetItemList().Where(p => p.Title == "Нет").First().ServiceId;
            OnPropertyChanged("CompletedWorks");
        }
        public User Rapair { get; set; }
        public string StatusNewRapair { get; set; } = "WaitingDiagnosis";
        string phoneNumber;
        string patternPhone = @"(?:\+375|80)\s?\(?\d\d\)?\s?\d\d(?:\d[\-\s]\d\d[\-\s]\d\d|[\-\s]\d\d[\-\s]\d\d\d|\d{5})";
        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (Regex.IsMatch(value, patternPhone, RegexOptions.IgnoreCase))
                {
                    phoneNumber = value;
                }
                else
                    MessageBox.Show("Введен неверный формат телефона / The number must be in the format +xxx-xx-xxx-xx-xx");
                OnPropertyChanged("Phone");
            }
        }
        string patternDevice = @"^([A-Za-zА-Яа-я1-9\s-]{1,25})$";
        string dervice;
        public string Device
        {
            get => dervice;
            set
            {
                if (Regex.IsMatch(value, patternDevice, RegexOptions.None))
                {
                    dervice = value;
                }
                else
                    MessageBox.Show("В марке устройства могут содержаться только буквы.\nКоличество симовлов от 1 до 25.");
                OnPropertyChanged("Device");
            }
        }
        string patternMalfunction = @"^([A-Za-zА-Яа-я1-9\s-]{1,50})$";
        string malfunction;
        public string Malfunction
        {
            get => malfunction;
            set
            {
                if (Regex.IsMatch(value, patternMalfunction, RegexOptions.IgnoreCase))
                {
                    malfunction = value;
                }
                else
                    MessageBox.Show("В строке неисправность могут содержаться только буквы. Длина строки от 1 до 50 символов.");
                OnPropertyChanged("Malfunction");
            }
        }
        public int CompletedWorks { get; set; }
        string patternSerialNumber = @"^([A-Za-zА-Я1-9]{4,25})$";
        string serialNumber;
        public string SerialNumber
        {
            get => serialNumber;
            set
            {
                if (Regex.IsMatch(value, patternSerialNumber, RegexOptions.IgnoreCase))
                {
                    serialNumber = value;
                }
                else
                    MessageBox.Show("В серийном номре устройства могут содержаться только буквы и цифры.\nКоличество символов от 4 до 25.");
                OnPropertyChanged("Device");
            }
        }
        public ObservableCollection<Rapair> rapairs;
        public ObservableCollection<Rapair> Rapairs { get => rapairs; set => Set<ObservableCollection<Rapair>>(ref rapairs, value); } 
        #region Command
        public ICommand SaveChanges
        {
            get => new DelegateCommand((obj) =>
            {
                unitOfWork.Save();
            });
        }
        #endregion
        public ICommand DeleteRows
        {
            get => new DelegateCommand((obj) =>
            {
                if (((Collection<object>)obj).Count > 0)
                {
                    Collection<object> objects = (Collection<object>)obj;
                    List<Rapair> list = objects.Cast<Rapair>().ToList();
                    list.ForEach(rapair =>
                    {
                        unitOfWork.Repairs.Delete(rapair.RapairID);
                        Rapairs.Remove(rapair);
                    });
                }
                else
                    MessageBox.Show("Не выбраны элементы для удаления");
            });
        }
        public ICommand UpdateDataGrid
        {
            get => new DelegateCommand((obj) =>
            {
                Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList());
                ServiceTitleList = new ObservableCollection<string>();
                foreach (Service service in unitOfWork.Services.GetItemList())
                {
                    ServiceTitleList.Add(service.Title);
                }
                OnPropertyChanged("ServiceTitleList");
                CompletedWorks = unitOfWork.Services.GetItemList().Where(p => p.Title == "Нет").First().ServiceId;
                OnPropertyChanged("CompletedWorks");
            });        
        }
        string search = "";
        public string Search
        {
            get => search;
            set
            {
                if (value != null && value.Length < 25)
                {
                    search = value;
                    if (search != "")
                    {
                        Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList().Where(r =>
                        (r.RapairID.ToString() +
                         r.Device +
                         r.Malfunction +
                         r.SerialNumber +
                         r.Status +
                         r.SumMoney.ToString() +
                         r.DateOfRaceipt.ToString()).IndexOf(search) > -1));
                        SelectRapair = Rapairs.Count > 0 ? Rapairs.First() : null;
                    }
                    else
                    {
                        Rapairs = new ObservableCollection<Rapair>(unitOfWork.Repairs.GetItemList());
                        SelectRapair = unitOfWork.Repairs.GetFirstItem();
                    }
                    OnPropertyChanged("Rapairs");
                }
                else
                    MessageBox.Show("Поисковая строка не должна быть пустой и не превышать длину 25 символов!");
            }
        }
        
        public ICommand CreateNewElement
        {
            get => new DelegateCommand((obj) =>
            {
                if(!CheckNotNull(typeof(string), StatusNewRapair, PhoneNumber, Device, Malfunction, SerialNumber))
                {
                    IEnumerable<User> users = unitOfWork.Users.GetItemList().Where(p => p.PhoneNumber == PhoneNumber);
                    if (users.Count() == 0)
                    {
                        DialogResult result = MessageBox.Show("Пользователя с таким номером телефона не существует!\n" +
                            "Вы хотите его добавить?",
                                        "Нужный пользователь отсутствует",
                                        MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {                            
                            ViewManager view = ViewManager.GetInstance;
                            view.OpenMiniWindow(new RegNewUserWind());
                        }
                        else
                        {
                            MessageBox.Show("В этом случае вы не сможете добавить заказ для несуществующего пользователя!\n" +
                                            "Если пользователь уже зарегестрировался, то проверьте корректность номера телефона!");
                        }
                    }
                    else if (users.Count() == 1)
                    {
                        addRapair(users.First());
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }               
            });
        }
        /// <summary>
        /// Выводит частичную информацию о пользователе и ремонте
        /// </summary>
        public ICommand ShowFullInfo
        {
            get => new DelegateCommand((obj) =>
            {
                if (obj != null && ((Collection<object>)obj).Count > 0)
                {
                    Collection<object> objects = (Collection<object>)obj;
                    List<Rapair> list = objects.Cast<Rapair>().ToList();
                    List<User> users = new List<User>();
                    list.ForEach(rapair =>
                    {
                        User user = unitOfWork.Users.GetItem(rapair.UserID);
                        MessageBox.Show($"Номер заказа: {rapair.RapairID}\n" +
                                    $"Устройство: {rapair.Device}\n" +
                                    $"Несправность: {rapair.Malfunction}\n" +
                                    $"ФИО клиента: {user.FullName}\n" +
                                    $"Email: {user.Email}\n" +
                                    $"Номер телефона: {user.PhoneNumber}");
                    });
                }
                else
                    MessageBox.Show("Не выбран Заказ для просмотра информации");
            });
        }
        public ICommand Notify
        {
            get => new DelegateCommand((obj) =>
            {
                if (obj != null && ((Collection<object>)obj).Count > 0)
                {
                    Collection<object> objects = (Collection<object>)obj;
                    List<Rapair> list = objects.Cast<Rapair>().ToList();
                    List<User> users = new List<User>();
                    list.ForEach(rapair =>
                    {
                        if (rapair.Status == StatusEnum.RepairsPaidFor.ToString())
                            users.Add(unitOfWork.Users.GetItemList().Where(u => u.UserId == rapair.UserID).First());
                        else
                            MessageBox.Show($"Заказ c Id {rapair.RapairID} не готов");
                    });
                    if(users.Count() > 0)
                    {
                        foreach (User us in users)
                        {
                            SendEmail("Ваше устпройство починено. Пожалуйста, оплатите услуги и заберите своё устройство.", us.Email);
                        }
                        MessageBox.Show("Оповещения успешно отправлены");
                    }
                   
                }
                else
                    MessageBox.Show("Не выбраны заказы, для уведомления клиентов");
            });
        }
        async void SendEmail(string body, string email)
        {
            try
            {
                MailAddress from = new MailAddress("ServiceCenterLaptop0@mail.ru", "ServiceCenterLaptop0");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to)
                {
                    Subject = "Service Center",
                    Body = body,
                    IsBodyHtml = true
                };
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587)
                {
                    UseDefaultCredentials = false,

                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("ServiceCenterLaptop0@mail.ru", "Laptop123")
                };
                await smtp.SendMailAsync(m);
              
            }
            catch
            {
                MessageBox.Show("Возникла ошибка при отправке сообщения!");
            }
        }
        Rapair rapair = new Rapair();
        void addRapair(User user)
        {
            Rapair rapair = new Rapair
            {
                Status = this.StatusNewRapair,
                Device = this.Device,
                Malfunction = this.Malfunction,
                CompletedWorks = this.CompletedWorks,
                SerialNumber = this.SerialNumber,
                DateOfRaceipt = DateTime.Now,
                SumMoney = 0,
                UserID = user.UserId
            };
            unitOfWork.Repairs.AddElemet(rapair);
            if (rapair.Status == this.rapair.Status &&
                rapair.Device == this.rapair.Device &&
                rapair.Malfunction == this.rapair.Malfunction &&
                rapair.CompletedWorks == this.CompletedWorks &&
                rapair.SerialNumber == this.rapair.SerialNumber &&
                rapair.SumMoney == this.rapair.SumMoney &&
                rapair.UserID == this.rapair.UserID)
                MessageBox.Show("Вы уже добавили такой заказ!");
            else
                Rapairs.Add(rapair);
        }
    }
}
