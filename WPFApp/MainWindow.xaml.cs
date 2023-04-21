using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private async void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataGrid grid = (DataGrid)sender;
			var cells = grid.SelectedCells;
			Reservation r = (Reservation)cells[0].Item;
			Grid windowGrid = (Grid)Content;
			StackPanel sp = (StackPanel)windowGrid.Children[1];
			TextBox[] textBoxes = sp.Children.OfType<TextBox>().ToArray();
			if (r != null)
			{
				User u = await r.GetReservee(new DataMapper<User>());
				Service s = await r.GetService(new DataMapper<Service>());
				textBoxes[0].Text = u.Name;
				textBoxes[1].Text = u.Surname;
				textBoxes[2].Text = s.Name;
				return;
			}
			textBoxes[0].Text = "";
			textBoxes[1].Text = "";
			textBoxes[2].Text = "";
		}
	}
}
