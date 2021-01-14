using MVC.Controller;
using MVC.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace MVC
{
    /// <summary>
    /// Interaction logic for Diskon.xaml
    /// </summary>
    public partial class Diskon : Window
    {
        DiskonController Discount;
        OnDiskonChangedListener listener;
        public Diskon()
        {
            InitializeComponent();

            Discount = new DiskonController();
            listDiskon.ItemsSource = Discount.getPromo();
            generateContentDiskon();

        }
        public void updateTotal(double subtotal)
        {
          
        }
        public void SetOnDiskonSelectedListener(OnDiskonChangedListener listener)
        {
            this.listener = listener;
        }
        private void generateContentDiskon()
        {

            Promo diskon1 = new Promo("Promo Awal Tahun", "Diskon 25%");
            Promo diskon2 = new Promo("Promo Tebus Murah","Diskon 30% Maximal 30.000" );
            Promo diskon3 = new Promo("Promo Natal","Diskon 10.000" );

            Discount.addPromo(diskon1);
            Discount.addPromo(diskon2);
            Discount.addPromo(diskon3);

            listDiskon.Items.Refresh();

        }
        private void listDiskon_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Label text = sender as Label;
            Promo promo = text.Content as Promo;
            Debug.WriteLine(promo.nama);

            this.listener.onDiskonSelected(promo);
        }
    }
}

public interface OnDiskonChangedListener
{
    void onDiskonSelected(Promo promo);
}
