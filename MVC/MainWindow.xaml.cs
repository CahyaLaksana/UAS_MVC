using MVC.Controller;
using MVC.Model;
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

namespace MVC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,
        OnPenawaranChangedListener,
        OnPaymentChangedListener,
        OnKeranjangBelanjaChangedListener,
        OnDiskonChangedListener

    {
        MainWindowController controller;
        Payment payment;
        DiskonController Discount;
        public MainWindow()
        {
            InitializeComponent();

            payment = new Payment(this);
            payment.setBalance(9000000);
            payment.setDeliveryFee(15000);
            payment.setPromo(5000);

            KeranjangBelanja keranjangBelanja = new KeranjangBelanja(payment, this);

            controller = new MainWindowController(keranjangBelanja);

            listBoxPesanan.ItemsSource = controller.getSelectedItems();


            initializeView();
        }

        private void initializeView()
        {
            labelSubtotal.Content = 0;
            labelGrantTotal.Content = 0;
            labelBalance.Content = payment.getBalance();
            labelPromoFee.Content = payment.getPromo();
            labelDeliveryFee.Content = payment.getDeliveryFee();
        }

        public void onPenawaranSelected(Item item)
        {
            controller.addItem(item);
        }



        private void onButtonAddItemClicked(object sender, RoutedEventArgs e)
        {
            Penawaran penawaranWindow = new Penawaran();
            penawaranWindow.SetOnItemSelectedListener(this);
            penawaranWindow.Show();
        }

        private void listBoxPesanan_ItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Kamu ingin menghapus item ini?", "Konfirmasi", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListBox listBox = sender as ListBox;
                Item item = listBox.SelectedItem as Item;
                controller.deleteSelectedItem(item);
            }
        }

        public void onSelectedPenawaranDeleted()
        {
            listBoxPesanan.Items.Refresh();
        }

        public void onPriceUpdated(double subtotal, double grantTotal, double balance)
        {
            labelSubtotal.Content = subtotal;
            labelBalance.Content = balance;
            labelGrantTotal.Content = grantTotal;
        }

        public void removeItemSucceed()
        {
            listBoxPesanan.Items.Refresh();
        }

        public void addItemSucceed()
        {
            listBoxPesanan.Items.Refresh();
        }

        public void onDiskonSelected(Promo promo)
        {
            Discount.addPromo(promo);
        }
        private void onButtonDiskonClicked(object sender, RoutedEventArgs e)
        {
            Diskon diskonWindow = new Diskon();
            diskonWindow.SetOnDiskonSelectedListener(this);
            diskonWindow.Show();
        }
    }
}
