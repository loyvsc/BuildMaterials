using BuildMaterials.Models;
using BuildMaterials.Other;
using BuildMaterials.Views;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<Material> materials;
        private List<Employee> employees;
        private List<Customer> customers;
        private List<Provider> providers;
        private List<Trade> trades;
        private List<TTN> ttns;
        private List<Account> accounts;
        private List<Contract> contracts;

        public bool CanUserEditEmployeeConf => CurrentEmployee.AccessLevel.Equals(3);

        public List<Material> MaterialsList
        {
            get => materials;
            set
            {
                materials = value;
                OnPropertyChanged(nameof(MaterialsList));
            }
        }
        public List<Employee> EmployeesList
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged(nameof(EmployeesList));
            }
        }
        public List<Customer> CustomersList
        {
            get => customers;
            set
            {
                customers = value;
                OnPropertyChanged(nameof(CustomersList));
            }
        }
        public List<Provider> ProvidersList
        {
            get => providers;
            set
            {
                providers = value;
                OnPropertyChanged(nameof(ProvidersList));
            }
        }
        public List<Trade> TradesList
        {
            get => trades;
            set
            {
                trades = value;
                OnPropertyChanged(nameof(TradesList));
            }
        }
        public List<TTN> TTNList
        {
            get => ttns;
            set
            {
                ttns = value;
                OnPropertyChanged(nameof(TTNList));
            }
        }
        public List<Account> AccountsList
        {
            get => accounts;
            set
            {
                accounts = value;
                OnPropertyChanged(nameof(AccountsList));
            }
        }
        public List<Contract> ContractsList
        {
            get => contracts;
            set
            {
                contracts = value;
                OnPropertyChanged(nameof(ContractsList));
            }
        }

        public EmployeeFIO[] SellersFIO => App.DBContext.Employees.Local.Select(x => new EmployeeFIO() { Name = x.Name, Surname = x.SurName, Pathnetic = x.Pathnetic }).ToArray();

        public ICommand AboutProgrammCommand => new RelayCommand((sender) => OpenAboutProgram());
        public ICommand ExitCommand => new RelayCommand((sener) => Application.Current.MainWindow.Close());
        public ICommand SettingsCommand => new RelayCommand((sener) => OpenSettings());
        public ICommand AddRowCommand => new RelayCommand((sender) => AddRow());
        public ICommand DeleteRowCommand => new RelayCommand((sender) => DeleteRow());
        public ICommand PrintCommand => new RelayCommand((sender) => PrintContract());
        public ICommand SaveChangesCommand => new RelayCommand((sender) => SaveChanges());

        private string _searchtext;
        public string SearchText
        {
            get => _searchtext;
            set
            {
                _searchtext = value.Trim();
                Search(_searchtext);
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public Employee CurrentEmployee { get; set; }
        public Settings Settings { get; private set; }

        public int SelectedRowIndex;

        public bool? CanAdd_EditConfidentional => CurrentEmployee.AccessLevel.Equals(3);
        public Visibility IsConfidentionNotView { get => CurrentEmployee.AccessLevel < 2 ? Visibility.Collapsed : Visibility.Visible; }
        public bool IsSaved
        {
            get => isSaved;
            set
            {
                if (value)
                {
                    Application.Current.MainWindow.Title = savedTitle;
                }
                else
                {
                    Application.Current.MainWindow.Title = unsavedTitle;
                }
                isSaved = value;
            }
        }
        public Visibility IsPrintEnabled
        {
            get => isPrintEnabled;
            set
            {
                isPrintEnabled = value;
                OnPropertyChanged(nameof(IsPrintEnabled));
            }
        }

        private bool isSaved = true;
        private readonly string savedTitle;
        private readonly string unsavedTitle;
        private Visibility isPrintEnabled;
        private string selectedTab = string.Empty;

        public MainWindowViewModel()
        {
            CurrentEmployee = new Employee();
            MaterialsList = App.DBContext.Materials.ToList();
            EmployeesList = App.DBContext.Employees.ToList();
            CustomersList = App.DBContext.Customers.ToList();
            ProvidersList = App.DBContext.Providers.ToList();
            TradesList = App.DBContext.Trades.ToList();
            TTNList = App.DBContext.TTNs.ToList();
            AccountsList = App.DBContext.Accounts.ToList();
            ContractsList = App.DBContext.Contracts.ToList();

            IsPrintEnabled = Visibility.Collapsed;
            Settings = new Settings();
            savedTitle = "Учет строительных материалов в магазине";
            unsavedTitle = "Учет строительных материалов в магазине*";
        }

        public MainWindowViewModel(Employee employee) : this()
        {
            CurrentEmployee = employee;
        }

        public List<Employee> SearchEmployees(string text)
        {
            List<Employee> employees = new List<Employee>(128);
            using(MySqlConnection _connection = new MySqlConnection(App.DBContext.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Employee WH"))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {

                    }
                }
            }
            return employees;
        }

        public IQueryable<T> CreateSearchQuery<T>(DbSet<T> db_set, string value) where T : class
        {
            IQueryable<T> query = db_set;

            List<System.Linq.Expressions.Expression> expressions = new List<System.Linq.Expressions.Expression>();

            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "p");

            MethodInfo contains_method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string)))
            {
                MemberExpression member_expression = System.Linq.Expressions.Expression.PropertyOrField(parameter, prop.Name);

                ConstantExpression value_expression = System.Linq.Expressions.Expression.Constant(value, typeof(string));

                MethodCallExpression contains_expression = System.Linq.Expressions.Expression.Call(member_expression, contains_method, value_expression);

                expressions.Add(contains_expression);
            }

            if (expressions.Count == 0)
                return query;

            System.Linq.Expressions.Expression or_expression = expressions[0];

            for (int i = 1; i < expressions.Count; i++)
            {
                or_expression = System.Linq.Expressions.Expression.OrElse(or_expression, expressions[i]);
            }

            Expression<Func<T, bool>> expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                or_expression, parameter);

            return query.Where(expression);
        }

        public void Search(string text)
        {
            try
            {
                switch (selectedTab)
                {
                    case "materialsTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                MaterialsList = App.DBContext.Materials.ToList();
                                break;
                            }
                            MaterialsList = CreateSearchQuery<Material>(App.DBContext.Materials, text).ToList();
                            break;
                        }
                    case "employersTab":
                        {

                            if (text.Equals(string.Empty))
                            {
                                EmployeesList = App.DBContext.Employees.ToList();
                                break;
                            }
                            EmployeesList = CreateSearchQuery<Employee>(App.DBContext.Employees, text).ToList();
                            break;
                        }
                    case "customersTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                CustomersList = App.DBContext.Customers.ToList();
                                break;
                            }
                            CustomersList = CreateSearchQuery<Customer>(App.DBContext.Customers, text).ToList();
                            break;
                        }
                    case "postavTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                ProvidersList = App.DBContext.Providers.ToList();
                                break;
                            }
                            ProvidersList = CreateSearchQuery<Provider>(App.DBContext.Providers, text).ToList();
                            break;
                        }
                    case "uchetTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                TradesList = App.DBContext.Trades.ToList();
                                break;
                            }
                            TradesList = CreateSearchQuery<Trade>(App.DBContext.Trades, text).ToList();
                            break;
                        }
                    case "ttnTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                TTNList = App.DBContext.TTNs.ToList();
                                break;
                            }
                            TTNList = CreateSearchQuery<TTN>(App.DBContext.TTNs, text).ToList();
                            break;
                        }
                    case "accountTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                AccountsList = App.DBContext.Accounts.ToList();
                                break;
                            }
                            AccountsList = CreateSearchQuery<Account>(App.DBContext.Accounts, text).ToList();
                            break;
                        }
                    case "contractTab":
                        {
                            if (text.Equals(string.Empty))
                            {
                                ContractsList = App.DBContext.Contracts.ToList();
                                break;
                            }
                            ContractsList = CreateSearchQuery<Contract>(App.DBContext.Contracts, text).ToList();
                            break;
                        }
                }
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
        }

        private void PrintContract()
        {
            if (SelectedRowIndex.Equals(-1))
            {                
                return;
            }
            using (PrinterConnect print = new PrinterConnect())
            {
                try
                {
                    bool result = false;
                    switch (selectedTab)
                    {
                        case "contractTab":
                            {
                                Contract selectedContract = ContractsList[SelectedRowIndex];
                                result = print.Print(selectedContract);
                                break;
                            }
                        case "accountTab":
                            {
                                Account selectedAccount = AccountsList[SelectedRowIndex];
                                result = print.Print(selectedAccount);
                                break;
                            }
                        case "ttnTab":
                            {
                                TTN selectedttn = TTNList[SelectedRowIndex];
                                result = print.Print(selectedttn);
                                break;
                            }
                        case "uchetTab":
                            {
                                Trade selected = TradesList[SelectedRowIndex];
                                result = print.Print(selected);
                                break;
                            }
                        case "materialsTab":
                            {
                                Material selected = MaterialsList[SelectedRowIndex];
                                result = print.Print(selected);
                                break;
                            }
                        case "employersTab":
                            {
                                Employee selected = EmployeesList[SelectedRowIndex];
                                result = print.Print(selected);
                                break;
                            }
                        case "customersTab":
                            {
                                Customer selected = CustomersList[SelectedRowIndex];
                                result = print.Print(selected);
                                break;
                            }
                    }
                    if (result)
                    {
                        MessageBox.Show("Печать успешно заверешена!", "Печать", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Печать заверешена с ошибкой: "+ex.Message,"Печать",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }

        private void SaveChanges()
        {
            App.DBContext.SaveChanges();
            IsSaved = true;
        }

        private void OpenAboutProgram()
        {
            AboutProgramView aboutWindow = new AboutProgramView();
            aboutWindow.ShowDialog();
        }

        private void OpenSettings()
        {
            SettingsView settingsWindow = new SettingsView();
            settingsWindow.ShowDialog();
        }

        public void OnTabChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count.Equals(0))
            {
                return;
            }
            if (e.AddedItems[0] is TabItem)
            {
                string tabName = (e.AddedItems[0] as TabItem)!.Name;
                selectedTab = tabName;
                SearchText = string.Empty;
            }
            App.DBContext.SaveChangesAsync();
        }

        private void DeleteRow()
        {
            if (SelectedRowIndex == -1)
            {
                return;
            }
            try
            {
                switch (selectedTab)
                {
                    case "materialsTab":
                        {
                            if (MaterialsList.Count.Equals(0))
                            {
                                return;
                            }
                            Material buf = App.DBContext.Materials.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Materials.Remove(buf);
                            App.DBContext.SaveChanges();
                            MaterialsList = App.DBContext.Materials.ToList();
                            break;
                        }
                    case "employersTab":
                        {
                            if (EmployeesList.Count.Equals(0))
                            {
                                return;
                            }
                            Employee buf = App.DBContext.Employees.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Employees.Remove(buf);
                            App.DBContext.SaveChanges();
                            EmployeesList = App.DBContext.Employees.ToList();
                            break;
                        }
                    case "customersTab":
                        {
                            if (CustomersList.Count.Equals(0))
                            {
                                return;
                            }
                            Customer buf = App.DBContext.Customers.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Customers.Remove(buf);
                            App.DBContext.SaveChanges();
                            CustomersList = App.DBContext.Customers.ToList();
                            break;
                        }
                    case "postavTab":
                        {
                            if (ProvidersList.Count.Equals(0))
                            {
                                return;
                            }
                            Provider buf = App.DBContext.Providers.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Providers.Remove(buf);
                            App.DBContext.SaveChanges();
                            ProvidersList = App.DBContext.Providers.ToList();
                            break;
                        }
                    case "uchetTab":
                        {
                            if (TradesList.Count.Equals(0))
                            {
                                return;
                            }
                            Trade buf = App.DBContext.Trades.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Trades.Remove(buf);
                            App.DBContext.SaveChanges();
                            TradesList = App.DBContext.Trades.ToList();
                            break;
                        }
                    case "ttnTab":
                        {
                            if (TTNList.Count.Equals(0))
                            {
                                return;
                            }
                            TTN buf = App.DBContext.TTNs.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.TTNs.Remove(buf);
                            App.DBContext.SaveChanges();
                            TTNList = App.DBContext.TTNs.ToList();
                            break;
                        }
                    case "accountTab":
                        {
                            if (AccountsList.Count.Equals(0))
                            {
                                return;
                            }
                            Account buf = App.DBContext.Accounts.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Accounts.Remove(buf);
                            App.DBContext.SaveChanges();
                            AccountsList = App.DBContext.Accounts.ToList();
                            break;
                        }
                    case "contractTab":
                        {
                            if (ContractsList.Count.Equals(0))
                            {
                                return;
                            }
                            Contract buf = App.DBContext.Contracts.Local.ElementAt(SelectedRowIndex);
                            App.DBContext.Contracts.Remove(buf);
                            App.DBContext.SaveChanges();
                            ContractsList = App.DBContext.Contracts.ToList();
                            break;
                        }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void AddRow()
        {
            switch (selectedTab)
            {
                case "materialsTab":
                    {
                        AddMaterialView addMaterial = new AddMaterialView();
                        if (addMaterial.ShowDialog() == true)
                        {
                            IsSaved = false;
                            MaterialsList = App.DBContext.Materials.ToList();
                        }
                        break;
                    }
                case "employersTab":
                    {
                        AddEmployeeView add = new AddEmployeeView();
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            EmployeesList = App.DBContext.Employees.ToList();
                        }
                        break;
                    }
                case "customersTab":
                    {
                        AddCustomerView add = new AddCustomerView();
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            CustomersList = App.DBContext.Customers.ToList();
                        }
                        break;
                    }
                case "postavTab":
                    {
                        AddProviderView add = new AddProviderView();
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            ProvidersList = App.DBContext.Providers.ToList();
                        }
                        break;
                    }
                case "uchetTab":
                    {
                        AddTradeView add = new AddTradeView();
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            TradesList = App.DBContext.Trades.ToList();
                        }
                        break;
                    }
                case "ttnTab":
                    {
                        AddTTNView add = new AddTTNView(CustomersList, ProvidersList);
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            TTNList = App.DBContext.TTNs.ToList();
                        }
                        break;
                    }
                case "accountTab":
                    {
                        AddAccountView add = new AddAccountView();
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            AccountsList = App.DBContext.Accounts.ToList();
                        }
                        break;
                    }
                case "contractTab":
                    {
                        AddContractView add = new AddContractView();
                        if (add.ShowDialog() == true)
                        {
                            IsSaved = false;
                            ContractsList = App.DBContext.Contracts.ToList();
                        }
                        break;
                    }
            }
        }

        public void SaveChanges(CancelEventArgs e)
        {
            if (IsSaved.Equals(false))
            {
                MessageBoxResult result = MessageBox.Show("Сохранить внесенные измения перед выходом?", savedTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        {
                            e.Cancel = true;
                            break;
                        }
                    case MessageBoxResult.Yes:
                        {
                            App.DBContext.SaveChanges();
                            break;
                        }
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}