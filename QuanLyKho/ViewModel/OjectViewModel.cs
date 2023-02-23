using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class OjectViewModel : BaseViewModel
    {
        private ObservableCollection<Model.Object> _List;
        public ObservableCollection<Model.Object> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Unit> _Unit;
        public ObservableCollection<Model.Unit> Unit { get { return _Unit; } set { _Unit = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Suplier> _Suplier;
        public ObservableCollection<Model.Suplier> Suplier { get { return _Suplier; } set { _Suplier = value; OnPropertyChanged(); } }

        private Model.Object _SelectedItem;
        public Model.Object SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    QRCode = SelectedItem.QRCode;
                    BarCode = SelectedItem.BarCode;
                    SelectedUnit = SelectedItem.Unit;
                    SelectedSuplier = SelectedItem.Suplier;
                }
            }
        }

        private Model.Unit _SelectedUnit;
        public Model.Unit SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                _SelectedUnit = value;
                OnPropertyChanged();             
            }
        }

        private Model.Suplier _SelectedSuplier;
        public Model.Suplier SelectedSuplier
        {
            get => _SelectedSuplier;
            set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
            }
        }



        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _QRCode;
        public string QRCode { get => _QRCode; set { _QRCode = value; OnPropertyChanged(); } }

        private string _BarCode;
        public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _MoreInfo;
        public string MoreInfo { get => _MoreInfo; set { _MoreInfo = value; OnPropertyChanged(); } }

        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public OjectViewModel()
        {
            List = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            Unit = new ObservableCollection<Model.Unit>(DataProvider.Ins.DB.Units);
            Suplier = new ObservableCollection<Model.Suplier>(DataProvider.Ins.DB.Supliers);

            AddCommand = new RelayCommand<object>(
                p => { 
                    if (SelectedSuplier == null || SelectedItem == null)
                    {
                        return false;
                    }
                    return true; 
                },
                p => {
                    var Object = new Model.Object() { Id = Guid.NewGuid().ToString() , DisplayName = DisplayName, QRCode = QRCode, BarCode = BarCode, IdSuplier = SelectedSuplier.Id, IdUnit = SelectedUnit.Id };

                    DataProvider.Ins.DB.Objects.Add(Object);
                    DataProvider.Ins.DB.SaveChanges();

                    List.Add(Object);
                });

            EditCommand = new RelayCommand<object>(
                p => {
                    if (SelectedItem == null || SelectedSuplier == null || SelectedItem == null)
                    {
                        return false;
                    }

                    var displayList = DataProvider.Ins.DB.Objects.Where(x => x.Id == SelectedItem.Id);
                    if (displayList != null && displayList.Count() != 0)
                    {
                        return true;
                    }
                    return false;
                },
                p => {
                    var Object = DataProvider.Ins.DB.Objects.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();

                    Object.Id = Guid.NewGuid().ToString();
                    Object.DisplayName = DisplayName;
                    Object.QRCode = QRCode;
                    Object.BarCode = BarCode;
                    Object.IdSuplier = SelectedSuplier.Id;
                    Object.IdUnit = SelectedUnit.Id;
                    DataProvider.Ins.DB.SaveChanges();

                    SelectedItem.DisplayName = DisplayName;
                });
        }
    }
}
