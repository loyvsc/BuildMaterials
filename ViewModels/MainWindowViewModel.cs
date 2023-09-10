using BuildMaterials.BD;
using BuildMaterials.Models;
using BuildMaterials.Other;
using BuildMaterials.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<Material> materials = null!;
        private List<Employee> employees = null!;
        private List<Customer> customers = null!;
        private List<Provider> providers = null!;
        private List<Trade> trades = null!;
        private List<TTN> ttns = null!;
        private List<Account> accounts = null!;
        private List<Contract> contracts = null!;

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

        public EmployeeFIO[] SellersFIO
        {
            get
            {
                List<EmployeeFIO> fio = new List<EmployeeFIO>(32);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    _connection.OpenAsync().Wait();
                    using (MySqlCommand _command = new MySqlCommand("SELECT Name, Surname, pathnetic FROM Employees;", _connection))
                    {
                        using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                            while (reader.Read())
                            {
                                fio.Add(new EmployeeFIO(reader.GetString(1), reader.GetString(0), reader.GetString(2)));
                            }
                    }
                    _connection.CloseAsync().Wait();
                }
                return fio.ToArray();
            }
        }

        public ICommand AboutProgrammCommand => new RelayCommand((sender) => OpenAboutProgram());
        public ICommand ExitCommand => new RelayCommand((sener) => System.Windows.Application.Current.MainWindow.Close());
        public ICommand SettingsCommand => new RelayCommand((sener) => OpenSettings());
        public ICommand AddRowCommand => new RelayCommand((sender) => AddRow());
        public ICommand DeleteRowCommand => new RelayCommand((sender) => DeleteRow());
        public ICommand PrintCommand => new RelayCommand((sender) => PrintContract());
        public ICommand SaveChangesCommand => new RelayCommand((sender) => ExitFromProgramm(new CancelEventArgs()));

        private string _searchtext = string.Empty;
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

        public bool? CanAdd_EditConfidentional => CurrentEmployee.AccessLevel.Equals(3);
        public Visibility IsConfidentionNotView => CurrentEmployee.AccessLevel < 2 ? Visibility.Collapsed : Visibility.Visible;        
        public Visibility IsPrintEnabled
        {
            get => isPrintEnabled;
            set
            {
                isPrintEnabled = value;
                OnPropertyChanged(nameof(IsPrintEnabled));
            }
        }

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
        }

        public MainWindowViewModel(Employee employee) : this()
        {
            CurrentEmployee = employee;
        }

        public void Search(string text)
        {
            try
            {
                if (text.Equals(string.Empty))
                {
                    switch (selectedTab)
                    {
                        case "materialsTab":
                            {
                                MaterialsList = App.DBContext.Materials.ToList();
                                break;
                            }
                        case "employersTab":
                            {
                                EmployeesList = App.DBContext.Employees.ToList();
                                break;
                            }
                        case "customersTab":
                            {
                                CustomersList = App.DBContext.Customers.ToList();
                                break;
                            }
                        case "postavTab":
                            {
                                ProvidersList = App.DBContext.Providers.ToList();
                                break;
                            }
                        case "uchetTab":
                            {
                                TradesList = App.DBContext.Trades.ToList();
                                break;
                            }
                        case "ttnTab":
                            {
                                TTNList = App.DBContext.TTNs.ToList();
                                break;
                            }
                        case "accountTab":
                            {
                                AccountsList = App.DBContext.Accounts.ToList();
                                break;
                            }
                        case "contractTab":
                            {
                                ContractsList = App.DBContext.Contracts.ToList();
                                break;
                            }
                    }
                    return;
                }
                switch (selectedTab)
                {
                    case "materialsTab":
                        {
                            MaterialsList = App.DBContext.Materials.Search(text);
                            break;
                        }
                    case "employersTab":
                        {
                            EmployeesList = App.DBContext.Employees.Search(text);
                            break;
                        }
                    case "customersTab":
                        {
                            CustomersList = App.DBContext.Customers.Search(text);
                            break;
                        }
                    case "postavTab":
                        {
                            ProvidersList = App.DBContext.Providers.Search(text);
                            break;
                        }
                    case "uchetTab":
                        {
                            TradesList = App.DBContext.Trades.Search(text);
                            break;
                        }
                    case "ttnTab":
                        {
                            TTNList = App.DBContext.TTNs.Search(text);
                            break;
                        }
                    case "accountTab":
                        {
                            AccountsList = App.DBContext.Accounts.Search(text);
                            break;
                        }
                    case "contractTab":
                        {
                            ContractsList = App.DBContext.Contracts.Search(text);
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
            if (SelectedTableItem == null)
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
                        case "postavTab":
                            {
                                Provider selectedProvider = (Provider)SelectedTableItem;
                                result = print.Print(selectedProvider);
                                break;
                            }
                        case "contractTab":
                            {
                                Contract selectedContract = (Contract)SelectedTableItem;
                                result = print.Print(selectedContract);
                                break;
                            }
                        case "accountTab":
                            {
                                Account selectedAccount = (Account)SelectedTableItem;
                                result = print.Print(selectedAccount);
                                break;
                            }
                        case "ttnTab":
                            {
                                TTN selectedttn = (TTN)SelectedTableItem;
                                result = print.Print(selectedttn);
                                break;
                            }
                        case "uchetTab":
                            {
                                Trade selected = (Trade)SelectedTableItem;
                                result = print.Print(selected);
                                break;
                            }
                        case "materialsTab":
                            {
                                Material selected = (Material)SelectedTableItem;
                                result = print.Print(selected);
                                break;
                            }
                        case "employersTab":
                            {
                                Employee selected = (Employee)SelectedTableItem;
                                result = print.Print(selected);
                                break;
                            }
                        case "customersTab":
                            {
                                Customer selected = (Customer)SelectedTableItem;
                                result = print.Print(selected);
                                break;
                            }
                    }
                    if (result)
                    {
                        System.Windows.MessageBox.Show("Печать успешно заверешена!", "Печать", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Печать заверешена с ошибкой: " + ex.Message, "Печать", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                finally
                {
                    SelectedTableItem = null;
                }
            }
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
        }

        public ITable? SelectedTableItem { get; set; }

        private void DeleteRow()
        {
            try
            {
                if (SelectedTableItem == null) return;
                switch (selectedTab)
                {
                    case "providersTab":
                        {
                            if (ProvidersList.Count.Equals(0))
                            {
                                return;
                            }
                            Provider buf = (Provider)SelectedTableItem;
                            App.DBContext.Providers.Remove(buf);
                            ProvidersList = App.DBContext.Providers.ToList();
                            break;
                        }
                    case "materialsTab":
                        {
                            if (MaterialsList.Count.Equals(0))
                            {
                                return;
                            }
                            Material buf = (Material)SelectedTableItem;
                            App.DBContext.Materials.Remove(buf);
                            MaterialsList = App.DBContext.Materials.ToList();
                            break;
                        }
                    case "employersTab":
                        {
                            if (EmployeesList.Count.Equals(0))
                            {
                                return;
                            }
                            Employee buf = (Employee)SelectedTableItem;
                            if (CurrentEmployee.Equals(buf))
                            {
                                System.Windows.MessageBox.Show("Нельзя удалять пользователя под которым был выполнен вход!");
                                return;
                            }
                            App.DBContext.Employees.Remove(buf);
                            EmployeesList = App.DBContext.Employees.ToList();
                            break;
                        }
                    case "customersTab":
                        {
                            if (CustomersList.Count.Equals(0))
                            {
                                return;
                            }
                            Customer buf = (Customer)SelectedTableItem;
                            App.DBContext.Customers.Remove(buf);
                            CustomersList = App.DBContext.Customers.ToList();
                            break;
                        }
                    case "postavTab":
                        {
                            if (ProvidersList.Count.Equals(0))
                            {
                                return;
                            }
                            Provider buf = (Provider)SelectedTableItem;
                            App.DBContext.Providers.Remove(buf);
                            ProvidersList = App.DBContext.Providers.ToList();
                            break;
                        }
                    case "uchetTab":
                        {
                            if (TradesList.Count.Equals(0))
                            {
                                return;
                            }
                            Trade buf = (Trade)SelectedTableItem;
                            App.DBContext.Trades.Remove(buf);
                            TradesList = App.DBContext.Trades.ToList();
                            break;
                        }
                    case "ttnTab":
                        {
                            if (TTNList.Count.Equals(0))
                            {
                                return;
                            }
                            TTN buf = (TTN)SelectedTableItem;
                            App.DBContext.TTNs.Remove(buf);
                            TTNList = App.DBContext.TTNs.ToList();
                            break;
                        }
                    case "accountTab":
                        {
                            if (AccountsList.Count.Equals(0))
                            {
                                return;
                            }
                            Account buf = (Account)SelectedTableItem;
                            App.DBContext.Accounts.Remove(buf);
                            AccountsList = App.DBContext.Accounts.ToList();
                            break;
                        }
                    case "contractTab":
                        {
                            if (ContractsList.Count.Equals(0))
                            {
                                return;
                            }
                            Contract buf = (Contract)SelectedTableItem;
                            App.DBContext.Contracts.Remove(buf);
                            ContractsList = App.DBContext.Contracts.ToList();
                            break;
                        }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
            finally
            {
                SelectedTableItem = null;
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
                            MaterialsList = App.DBContext.Materials.ToList();
                        }
                        break;
                    }
                case "employersTab":
                    {
                        AddEmployeeView add = new AddEmployeeView();
                        if (add.ShowDialog() == true)
                        {
                            EmployeesList = App.DBContext.Employees.ToList();
                        }
                        break;
                    }
                case "customersTab":
                    {
                        AddCustomerView add = new AddCustomerView();
                        if (add.ShowDialog() == true)
                        {
                            CustomersList = App.DBContext.Customers.ToList();
                        }
                        break;
                    }
                case "postavTab":
                    {
                        AddProviderView add = new AddProviderView();
                        if (add.ShowDialog() == true)
                        {
                            ProvidersList = App.DBContext.Providers.ToList();
                        }
                        break;
                    }
                case "uchetTab":
                    {
                        AddTradeView add = new AddTradeView();
                        if (add.ShowDialog() == true)
                        {
                            TradesList = App.DBContext.Trades.ToList();
                        }
                        break;
                    }
                case "ttnTab":
                    {
                        AddTTNView add = new AddTTNView(ProvidersList);
                        if (add.ShowDialog() == true)
                        {
                            TTNList = App.DBContext.TTNs.ToList();
                        }
                        break;
                    }
                case "accountTab":
                    {
                        AddAccountView add = new AddAccountView();
                        if (add.ShowDialog() == true)
                        {
                            AccountsList = App.DBContext.Accounts.ToList();
                        }
                        break;
                    }
                case "contractTab":
                    {
                        AddContractView add = new AddContractView();
                        if (add.ShowDialog() == true)
                        {
                            ContractsList = App.DBContext.Contracts.ToList();
                        }
                        break;
                    }
            }
        }

        public void ExitFromProgramm(CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Выйти из программы?", "АРМ Менеджера Строительной Компании", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result== MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}