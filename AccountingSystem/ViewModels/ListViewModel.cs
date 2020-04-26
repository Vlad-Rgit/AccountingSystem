using AccountingSystem.Commands;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public abstract class ListViewModel<TEntity> : ViewModelBase
    {

		private bool _isPopup;

		public bool IsPopup
		{
			get { return _isPopup; }
			set 
			{
				if (IsToEdit)
				{
					_isPopup = value;
					OnPropertyChanged();
				}
			}
		}


		public bool IsToEdit { get; }

		private bool _isClosed = true;

		public bool IsClosed
		{
			get { return _isClosed; }
			set { _isClosed = value; }
		}


		public ObservableCollection<TEntity> Entities { get; protected set; }

		private TEntity _selectedEntity;

		public TEntity SelectedEntity
		{
			get { return _selectedEntity; }
			set { IsPopup = false; _selectedEntity = value; IsPopup = true; OnPropertyChanged(); }
		}



		public RelayCommand<TEntity> OpenAddCommand { get; protected set; }
		public RelayCommand<TEntity> DeleteCommand { get; protected set; }


		public ListViewModel(bool isTEdit)
		{
			IsToEdit = isTEdit;
		}


		public abstract void Filter();



		public override void Close()
		{
			DialogHost.CloseDialogCommand.Execute(this, null);
		}
	}
}
