using BusinessLayer;
using BusinessLayer.Services.FormValidationService;
using BusinessLayer.Services.ReservationService;
using C_Projekt.Validation;
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
using System.Windows.Shapes;

namespace C_Projekt
{
	/// <summary>
	/// Interaction logic for ReservationForm.xaml
	/// </summary>
	public delegate void AddReservation();
	public partial class ReservationForm : Window
	{
		public event AddReservation OnFormSubmit;
		private IReservationService _reservationService;
		private IFormValidationService<ReservationForm> _formValidationService;
		public DateTime? Date { get; set; }
		public string TimeSpan { get; set; }
		public List<Service> Services { get; set; }
		public List<User> Users { get; set; }
		public List<string> TimeSpans { get; set; }
		public User User { get; set; }
		public Service Service { get; set; }
		public Reservation Reservation { get; set; }
		public ReservationForm()
		{
			_reservationService = new ReservationService(new DataMapper<Reservation>());
			_formValidationService = new ReservationFormValidation();
			FillCollections();
			InitializeComponent();
			ConfigureAdd();
		}

		public ReservationForm(Reservation reservation)
		{
			_formValidationService = new ReservationFormValidation();
			FillCollections();
			Reservation = reservation;
			User = reservation.GetReservee(new DataMapper<User>()).Result;
			Service = reservation.GetService(new DataMapper<Service>()).Result;
			InitializeComponent();
			ConfigureEdit(reservation);
		}
		private async void FillCollections()
		{
			TimeSpans = new List<string>() {"13:30 - 15:00",
				"15:00 - 16:30",
				"16:30 - 18:00",
				"18:00 - 19:30" };
			Users = await new DataMapper<User>().SelectAll();
			Services = await new DataMapper<Service>().SelectAll();
		}
		private void ConfigureEdit(Reservation reservation)
		{
			Grid grid = Content as Grid;
			StackPanel stackPanel = grid.Children[0] as StackPanel;
			ComboBox c1 = stackPanel.Children[2] as ComboBox;
			c1.SelectedItem = Users.Find(x => x.Id == User.Id);
			ComboBox c2 = stackPanel.Children[4] as ComboBox;
			c2.SelectedItem = Services.Find(x => x.Id == Service.Id);
			ComboBox c3 = stackPanel.Children[8] as ComboBox;
			string s = $"{Reservation.ReservationStart:HH:mm} - {Reservation.ReservationEnd:HH:mm}";
			c3.SelectedItem = TimeSpans.Find(x => x == s);
			DatePicker datePicker = stackPanel.Children[6] as DatePicker;
			datePicker.SelectedDate = reservation.ReservationStart;
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
				await _reservationService.Reserve(User, Service.Id.Value, Date.Value.Date.ToString("dd.MM.yyyy"), TimeSpan);
				OnFormSubmit.Invoke();
				Close();
			}
		}

		private async void Edit(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_formValidationService.Validate(this)))
			{
				string[] times = TimeSpan.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				Reservation.Reservee = User;
				Reservation.Service = Service;
				DateOnly date = DateOnly.Parse(Date.Value.Date.ToString("dd.MM.yyyy"));
				Reservation.ReservationStart = date.ToDateTime(TimeOnly.ParseExact(times[0], "HH:mm"));
				Reservation.ReservationEnd = date.ToDateTime(TimeOnly.ParseExact(times[1], "HH:mm"));
				await new DataMapper<Reservation>().Update(Reservation);
				OnFormSubmit.Invoke();
				Close();
			}
		}
	}
}
