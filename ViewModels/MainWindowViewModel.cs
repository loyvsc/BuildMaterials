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
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Lists
        public bool CanUserEditEmployeeConf => CurrentEmployee?.AccessLevel == 3;
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
        public List<Seller> SellersList
        {
            get => sellers;
            set
            {
                sellers = value;
                OnPropertyChanged(nameof(SellersList));
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
        public List<MaterialResponse> MaterialResponsesList
        {
            get => materialResponses;
            set
            {
                materialResponses = value;
                OnPropertyChanged(nameof(MaterialResponsesList));
            }
        }
        public List<Seller> ProvidersList
        {
            get => provlist;
            set
            {
                provlist = value;
                OnPropertyChanged(nameof(ProvidersList));
            }
        }
        public List<Seller> CustomersList
        {
            get => custlist;
            set
            {
                custlist = value;
                OnPropertyChanged(nameof(CustomersList));
            }
        }
        #endregion

        #region Commands
        public ICommand AboutProgrammCommand => new RelayCommand((sender) => OpenAboutProgram());
        public ICommand ExitCommand => new RelayCommand((sener) => System.Windows.Application.Current.MainWindow.Close());
        public ICommand SettingsCommand => new RelayCommand((sener) => OpenSettings());
        public ICommand AddRowCommand => new RelayCommand((sender) => AddRow());
        public ICommand DeleteRowCommand => new RelayCommand((sender) => DeleteRow());
        public ICommand PrintCommand => new RelayCommand((sender) => PrintContract());
        #endregion

        public bool CanViewConfidentional => CurrentEmployee?.AccessLevel == 3;
        public string SearchText
        {
            get => _searchtext;
            set
            {
                if (value.Trim() != _searchtext)
                {
                    _searchtext = value.Trim();
                    OnPropertyChanged(nameof(SearchText));
                    Search(_searchtext);
                }
            }
        }

        public Employee? CurrentEmployee
        {
            get => currentEmployee;
            set
            {
                currentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
                OnPropertyChanged(nameof(CanViewConfidentional));
            }
        }
        public Settings Settings { get; private set; }

        public bool? CanAdd_EditConfidentional => CurrentEmployee?.AccessLevel.Equals(3);
        public Visibility IsConfidentionNotView => CurrentEmployee?.AccessLevel < 2 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPrintEnabled
        {
            get => isPrintEnabled;
            set
            {
                isPrintEnabled = value;
                OnPropertyChanged(nameof(IsPrintEnabled));
            }
        }
        public ITable? SelectedTableItem { get; set; }

        #region Private vars
        private Visibility isPrintEnabled;
        private string selectedTab = string.Empty;
        private List<Seller> provlist;
        private List<Seller> custlist;
        private string _searchtext = string.Empty;
        private Employee? currentEmployee;
        private List<Material> materials = null!;
        private List<Employee> employees = null!;
        private List<Seller> customers = null!;
        private List<Seller> sellers = null!;
        private List<Trade> trades = null!;
        private List<TTN> ttns = null!;
        private List<Account> accounts = null!;
        private List<Contract> contracts = null!;
        private List<MaterialResponse> materialResponses = null!;
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            CurrentEmployee = new Employee();
            CustomersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER = TRUE;");
            ProvidersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER = false;");
            MaterialsList = App.DBContext.Materials.ToList();
            EmployeesList = App.DBContext.Employees.ToList();
            SellersList = App.DBContext.Sellers.ToList();
            TradesList = App.DBContext.Trades.ToList();
            TTNList = App.DBContext.TTNs.ToList();
            AccountsList = App.DBContext.Accounts.ToList();
            ContractsList = App.DBContext.Contracts.ToList();
            MaterialResponsesList = App.DBContext.MaterialResponse.ToList();

            IsPrintEnabled = Visibility.Collapsed;
            Settings = new Settings();
        }

        public MainWindowViewModel(Employee employee) : this()
        {
            CurrentEmployee = employee;
        }
        #endregion

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
                        case "materialResponsibleTab":
                            {
                                MaterialResponse materialResponse = (MaterialResponse)SelectedTableItem;
                                result = print.Print(materialResponse);
                                break;
                            }
                        case "postavTab":
                            {
                                Seller selectedProvider = (Seller)SelectedTableItem;
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
                                Seller selected = (Seller)SelectedTableItem;
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

        #region ApplicationFunctions
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
        public void ExitFromProgramm(CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Выйти из программы?", "АРМ Менеджера Строительной Компании", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        #endregion

        public void OnTabChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is TabItem)
            {
                string tabName = (e.AddedItems[0] as TabItem)!.Name;
                selectedTab = tabName;
                SearchText = string.Empty;
            }
        }

        private void DeleteRow()
        {
            try
            {
                if (SelectedTableItem == null) return;
                switch (selectedTab)
                {
                    case "materialResponsibleTab":
                        {
                            if (MaterialResponsesList.Count.Equals(0))
                            {
                                return;
                            }
                            MaterialResponse buf = (MaterialResponse)SelectedTableItem;
                            App.DBContext.MaterialResponse.Remove(buf);
                            MaterialResponsesList = App.DBContext.MaterialResponse.ToList();
                            break;
                        }
                    case "providersTab":
                        {
                            if (SellersList.Count.Equals(0))
                            {
                                return;
                            }
                            Seller buf = (Seller)SelectedTableItem;
                            App.DBContext.Sellers.Remove(buf);
                            SellersList = App.DBContext.Sellers.ToList();
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
                            if (CurrentEmployee == buf)
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
                            Seller buf = (Seller)SelectedTableItem;
                            App.DBContext.Sellers.Remove(buf);
                            CustomersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER = TRUE");
                            break;
                        }
                    case "postavTab":
                        {
                            if (SellersList.Count.Equals(0))
                            {
                                return;
                            }
                            Seller buf = (Seller)SelectedTableItem;
                            App.DBContext.Sellers.Remove(buf);
                            SellersList = App.DBContext.Sellers.ToList();
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
                case "materialResponsibleTab":
                    {
                        AddMaterialResponseView addMaterial = new AddMaterialResponseView();
                        if (addMaterial.ShowDialog() == true)
                        {
                            MaterialResponsesList = App.DBContext.MaterialResponse.ToList();
                        }
                        break;
                    }
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
                            CustomersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER == TRUE;");
                        }
                        break;
                    }
                case "postavTab":
                    {
                        AddProviderView add = new AddProviderView();
                        if (add.ShowDialog() == true)
                        {
                            ProvidersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER == FALSE;");
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
                        AddTTNView add = new AddTTNView();
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
                                SellersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER = TRUE;");
                                break;
                            }
                        case "postavTab":
                            {
                                SellersList = App.DBContext.Sellers.Select("SELECT * FROM SELLERS WHERE ISCUSTOMER = false;");
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
                            SellersList = App.DBContext.Sellers.Search(text);
                            break;
                        }
                    case "postavTab":
                        {
                            SellersList = App.DBContext.Sellers.Search(text);
                            break;
                        }
                }
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
        }

    }
}