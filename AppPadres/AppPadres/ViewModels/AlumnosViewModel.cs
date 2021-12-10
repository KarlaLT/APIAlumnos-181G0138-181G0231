using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using AppPadres.Models;
using AppPadres.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AppPadres.ViewModels
{
    public class AlumnosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public AlumnosViewModel()
        {
            Alumno = new Alumno();
            EntrarCommand = new Command(Entrar);
            SalirCommand = new Command(Salir);
        }

        CalificacionesView calif;

        public ICommand EntrarCommand { get; set; }
        public ICommand SalirCommand { get; set; }
    
        public HttpClient client = new HttpClient();
        public string Url = "http://192.168.1.67:45458/api/padres/";
        private Alumno alumno;
        public Alumno Alumno
        {
            get { return alumno; }
            set
            {
                alumno = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Alumno"));
            }
        }

        private string user;
        public string User
        {
            get { return user; }
            set
            {
                user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User"));
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }
        private string error;
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }
        private async void Entrar()
        {
            HttpResponseMessage result = await client.GetAsync(Url + user + "-" + password);


            if (result.IsSuccessStatusCode)
            {
                var json = result.Content.ReadAsStringAsync().Result;
                Alumno = JsonConvert.DeserializeObject<Alumno>(json);

                calif = new CalificacionesView() { BindingContext = this };
                await Application.Current.MainPage.Navigation.PushModalAsync(calif);
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                Error = "No se encontró alumno con su usuario y contraseña, verifique que los datos sean correctos.";
            }
            else
            {
                Error = "Verifique que los campos no estén vacíos (No pueden llevar espacios)";
            }

        }

        private void Salir()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
