using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace C_Projekt
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<Reservation>? Reservations { get; set; }
		public ObservableCollection<Service>? Services { get; set; }
		public MainWindow()
		{
			FillCollections();
			InitializeComponent();
		}

		private async void FillCollections()
		{
			List<Reservation> reservations = await new DataMapper<Reservation>().SelectAll();
			Reservations = new ObservableCollection<Reservation>(reservations);
			List<Service> services = await new DataMapper<Service>().SelectAll();
			Services = new ObservableCollection<Service>(services);
		}

		private async void Delete_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if(button.DataContext is Reservation r)
			{
				if (await new DataMapper<Reservation>().Delete(r) > 0)
				{
					Reservations.Remove(r);
				}
			}
			else if(button.DataContext is Service s)
			{
				if (await new DataMapper<Service>().Delete(s) > 0)
				{
					Services.Remove(s);
				}
			}
        }

		private async void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Service service = (Service)button.DataContext;
			if (await new DataMapper<Service>().Delete(service) > 0)
			{
				Services.Remove(service);
			}
		}

		private async void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGrid grid = (DataGrid)sender;
			var cells = grid.SelectedCells;
			Reservation r = (Reservation)cells[0].Item;
			Grid windowGrid = (Grid)Content;
			Grid detailGrid = (Grid)windowGrid.Children[3];
			StackPanel sp = (StackPanel)detailGrid.Children[0];
			TextBox[] textBoxes = sp.Children.OfType<TextBox>().ToArray();
			if (r != null)
			{
				User u = await r.GetReservee(new DataMapper<User>());
				Service s = await r.GetService(new DataMapper<Service>());
				textBoxes[0].Text = u.Name;
				textBoxes[1].Text = u.Surname;
				textBoxes[2].Text = s.Name;
				textBoxes[3].Text = r.ReservationStart.ToString();
				textBoxes[4].Text = r.ReservationEnd.ToString();
				return;
			}
			IEnumerable<TextBox> textboxes = sp.Children.OfType<TextBox>();
			foreach (TextBox textbox in textboxes)
			{
				textbox.Text = "";
			}
		}
		private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Grid windowGrid = (Grid)Content;
			Grid detailGrid = (Grid)windowGrid.Children[3];
			StackPanel sp = (StackPanel)detailGrid.Children[0];
			IEnumerable<TextBox> textboxes = sp.Children.OfType<TextBox>();
			foreach (TextBox textbox in textboxes)
			{
				textbox.Text = "";
			}
		}
		private void AddReservation_Click(object sender, RoutedEventArgs e)
		{
			ReservationForm form = new ReservationForm();
			form.OnFormSubmit += async () => { 
				Reservations.Clear();
                foreach (var item in await new DataMapper<Reservation>().SelectAll())
                {
                    Reservations.Add(item);
                }
            };
			form.Show();
		}
		private void AddService_Click(object sender, RoutedEventArgs e)
		{
			ServiceForm form = new ServiceForm();
			form.OnFormSubmit += async () => {
				Services.Clear();
				foreach (var item in await new DataMapper<Service>().SelectAll())
				{
					Services.Add(item);
				}
			};
			form.Show();
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			if(btn.DataContext is Reservation r)
			{
				ReservationForm form = new ReservationForm(r);
				form.OnFormSubmit += async () => {
					Reservations.Clear();
					foreach (var item in await new DataMapper<Reservation>().SelectAll())
					{
						Reservations.Add(item);
					}
				};
				form.Show();
			}
			else if(btn.DataContext is Service s)
			{
				ServiceForm form = new ServiceForm(s);
				form.OnFormSubmit += async () => {
					Services.Clear();
					foreach (var item in await new DataMapper<Service>().SelectAll())
					{
						Services.Add(item);
					}
				};
				form.Show();
			}

		}
	}
}
