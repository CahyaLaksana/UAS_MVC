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
    /// Interaction logic for Penawaran.xaml
    /// </summary>
    public partial class Penawaran : Window
    {
        PenawaranController controller;
        OnPenawaranChangedListener listener;

        public Penawaran()
        {
            InitializeComponent();

            controller = new PenawaranController();
            listPenawaran.ItemsSource = controller.getItems();

            generateContentPenawaran();
      
        }
       
        public void SetOnItemSelectedListener(OnPenawaranChangedListener listener)
        {
            this.listener = listener;
        }

        private void generateContentPenawaran()
        {
            Item drink1 = new Item("Coffe Late", 30000);
            Item drink2 = new Item("Black Tea", 20000);
            Item drink3 = new Item("Water Melon Juice", 7000);
            Item drink4 = new Item("Milkshake", 15000);
            Item drink5 = new Item("Lemon Squash", 30000);
            Item food6 = new Item("Pizza", 75000);
            Item food7 = new Item("Fried Frice Special", 45000);

            controller.addItem(drink1);
            controller.addItem(drink2);
            controller.addItem(drink3);
            controller.addItem(drink4);
            controller.addItem(drink5);
            controller.addItem(food6);
            controller.addItem(food7);

            listPenawaran.Items.Refresh();
        }
        private void listPenawaran_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            Item item = listbox.SelectedItem as Item;
            Debug.WriteLine(item.title);

            this.listener.onPenawaranSelected(item);
        }

       
    }
    public interface OnPenawaranChangedListener
    {
        void onPenawaranSelected(Item item);
    }
}