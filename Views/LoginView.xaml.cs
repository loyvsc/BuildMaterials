﻿using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BuildMaterials.Views
{
    public partial class LoginView : Window
    {
        private readonly ViewModels.LoginViewModel viewModel;
        public LoginView()
        {
            viewModel = new ViewModels.LoginViewModel(this);
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.EnteredPassword = ((PasswordBox)sender).Password;
        }

        private void PasswordBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }
    }
}