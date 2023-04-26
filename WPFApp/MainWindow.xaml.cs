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
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			GetReservations();
		}

		private async void GetReservations()
		{
			List<Reservation> reservations = await new DataMapper<Reservation>().SelectAll();
			Reservations = new ObservableCollection<Reservation>(reservations);
		}

		private async void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Reservation reservation = (Reservation)button.DataContext;
			if (await new DataMapper<Reservation>().Delete(reservation) > 0)
			{ 
				Reservations.Remove(reservation); 
			}
        }

		private async void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGrid grid = (DataGrid)sender;
			var cells = grid.SelectedCells;
			Reservation r = (Reservation)cells[0].Item;
			Grid windowGrid = (Grid)Content;
			Grid detailGrid = (Grid)windowGrid.Children[1];
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

		private void Save_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Clear_Click(object sender, RoutedEventArgs e)
		{
			Grid windowGrid = (Grid)Content;
			Grid detailGrid = (Grid)windowGrid.Children[1];
			StackPanel sp = (StackPanel)detailGrid.Children[0];
			IEnumerable<TextBox> textboxes = sp.Children.OfType<TextBox>();
			foreach (TextBox textbox in textboxes)
			{
				textbox.Text = "";
			}
		}

		public void Dg_Selected(object sender, RoutedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;
		}

		private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Grid windowGrid = (Grid)Content;
			Grid detailGrid = (Grid)windowGrid.Children[1];
			StackPanel sp = (StackPanel)detailGrid.Children[0];
			IEnumerable<TextBox> textboxes = sp.Children.OfType<TextBox>();
			foreach (TextBox textbox in textboxes)
			{
				textbox.Text = "";
			}
		}
	}
}
