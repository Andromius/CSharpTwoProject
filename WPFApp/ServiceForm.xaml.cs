using BusinessLayer.Services.FormValidationService;
using BusinessLayer;
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
using System.Windows.Shapes;
using DataLayer;
using Microsoft.AspNet.Identity;
using C_Projekt.Validation;

namespace C_Projekt
{
	/// <summary>
	/// Interaction logic for ServiceForm.xaml
	/// </summary>
	public delegate void AddService();
	public partial class ServiceForm : Window
	{
		public event AddService OnFormSubmit;
		private IFormValidationService<ServiceForm> _formValidationService;
		public Service Service { get; set; }
		public ServiceForm()
		{
			_formValidationService = new ServiceFormValidation();
			Service = new Service();
			InitializeComponent();
			ConfigureAdd();
		}
		public ServiceForm(Service service)
		{
			_formValidationService = new ServiceFormValidation();
			Service = service;
			InitializeComponent();
			ConfigureEdit(service);
		}
		private void ConfigureEdit(Service service)
		{
			Grid grid = Content as Grid;
			StackPanel stackPanel = grid.Children[0] as StackPanel;
			TextBox textBox = stackPanel.Children[2] as TextBox;
			textBox.Text = Service.Name;
			Button add = grid.Children[1] as Button;
			Button edit = grid.Children[2] as Button;
			add.Visibility = Visibility.Hidden;
			edit.Visibility = Visibility.Visible;
		}
		private void ConfigureAdd()
		{
			Grid grid = Content as Grid;
			Button add = grid.Children[1] as Button;
			Button edit = grid.Children[2] as Button;
			add.Visibility = Visibility.Visible;
			edit.Visibility = Visibility.Hidden;
		}
		private async void Add(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_formValidationService.Validate(this)))
			{
				await new DataMapper<Service>().Insert(Service);
				OnFormSubmit.Invoke();
				Close();
			}
			return;
		}

		private async void Edit(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_formValidationService.Validate(this)))
			{
				await new DataMapper<Service>().Update(Service);
				OnFormSubmit.Invoke();
				Close();
			}
			return;
		}
	}
}
