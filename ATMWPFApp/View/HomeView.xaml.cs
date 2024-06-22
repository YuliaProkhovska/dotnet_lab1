using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATMWPFApp.View
{
	/// <summary>
	/// Логика взаимодействия для HomeView.xaml
	/// </summary>
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();
		}

		private void CloseWithdraw(object sender, RoutedEventArgs e)
		{
			WithdrawBorder.Visibility = Visibility.Collapsed;
		}
		
		private void WithdrawMyCount_Click(object sender, RoutedEventArgs e)
		{
			WithdrawMyCount.Visibility = Visibility.Collapsed;
		}
		private void OpenTopUp(object sender, RoutedEventArgs e)
		{

			TopUpBorder.Visibility = Visibility.Visible;
		}

		private void CloseTopUp(object sender, RoutedEventArgs e)
		{
			TopUpBorder.Visibility = Visibility.Collapsed;
		}

		private void OpenWithdraw(object sender, RoutedEventArgs e)
		{
			WithdrawBorder.Visibility = Visibility.Visible;
		}
		private void OpenTransferBorder(object sender, RoutedEventArgs e)
		{

			TransferBorder.Visibility = Visibility.Visible;
		}
		private void CloseTransferBorder(object sender, RoutedEventArgs e)
		{
			TransferBorder.Visibility = Visibility.Collapsed;
		}
	}
}
