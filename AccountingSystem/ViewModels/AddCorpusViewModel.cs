using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Repositories;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class AddCorpusViewModel : ViewModelBase
    {
        private bool _isToEdit = false;

        public Corpus Corpus { get; set; }

        private Address _address;

        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }


        private City _city;

        public City City
        {
            get => _city;
            set => _city = value;
        }

        private Street _street;

        public Street Street
        {
            get { return _street; }
            set { _street = value; }
        }


        public RelayCommand AcceptCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AddCorpusViewModel(CorpusView corpus)
        { 
            if(corpus == null)
            {
                Corpus = new Corpus();
                Address = new Address();
                City = new City();
                Street = new Street();
            }
            else 
            {
                _isToEdit = true;

                using (var repo = new Repository())
                {
                    Corpus = repo.Get<Corpus>(corpus.Id);

                    Address = Corpus.Address;
                    City = Address.City;
                    Street = Address.Street;
                }
            }


            AcceptCommand = new RelayCommand(Accept);

            CancelCommand = new RelayCommand(() => DialogHost.CloseDialogCommand.Execute(this, null));
        }


        public void Accept()
        {
            using(var repo = new Repository())
            {

                repo.AddOrAttach<City>(ref _city, (c) => c.CityName == City.CityName);
                repo.AddOrAttach<Street>(ref _street, (c) => c.StreetName == Street.StreetName);

                Address.City = City;
                Address.Street = Street;

                repo.AddOrAttach<Address>(ref _address, (a) => a.StreetId == Street.StreetId
                                                     && a.CityId == City.CityId
                                                     && a.HausNumber == Address.HausNumber);
            


                if (_isToEdit)
                {
                    Corpus.Address = Address;
                    repo.Update(Corpus);
                }
                else
                {
                    Corpus.Address = Address;
                    repo.Add(Corpus);
                }

                repo.SaveChanges();

                DialogHost.CloseDialogCommand.Execute(this, null);
            }
        }

    }
}
