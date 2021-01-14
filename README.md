<h1> UAS PEMROGRAMAN LANJUT</h1>
<h2>Model</h2>
<h3>Item</h3>

```csharp
 public class Item
    {
        public string title { get; set; }
        public double price { get; set; }

        public Item(string title,  double price)
        {
            this.title = title;
            this.price = price;
        }
    }
```
digunakan untuk pendeklarasian variabel dari window penawaran

<h3>Keranjang Belanja</h3>

```csharp
class KeranjangBelanja
    {
        List<Item> itemBelanja;
        Payment payment;
        OnKeranjangBelanjaChangedListener callback;

        public KeranjangBelanja(Payment payment, OnKeranjangBelanjaChangedListener callback)
        {
            this.payment = payment;
            this.itemBelanja = new List<Item>();
            this.callback = callback;
        }

        public List<Item> getItems()
        {
            return this.itemBelanja;
        }

        public void addItem(Item item)
        {
            this.itemBelanja.Add(item);
            this.callback.addItemSucceed();
            calculateSubTotal();
        }

        public void removeItem(Item item)
        {
            this.itemBelanja.Remove(item);
            this.callback.removeItemSucceed();
            calculateSubTotal();
        }

        private void calculateSubTotal()
        {
            double subtotal = 0;
            foreach(Item item in itemBelanja)
            {
                subtotal += item.price;
            }
            payment.updateTotal(subtotal);
        }
    }
    interface OnKeranjangBelanjaChangedListener
    {
        void removeItemSucceed();
        void addItemSucceed();
    }
```
tempat dihitungnya total harga item yang sudah dipilih, yang berada di listbox utama

<h3>Payment</h3>

```csharp
class Payment
    {
        private double deliveryFee = 0;
        private double promo = 0;
        private double balance = 0;
        private OnPaymentChangedListener paymentCallback;
        
        public Payment(OnPaymentChangedListener paymentCallback)
        {
            this.paymentCallback = paymentCallback;
        }

        public void setBalance (double balance)
        {
            this.balance = balance; 
        }

        public double getBalance()
        {
            return this.balance;
        }

        public void setDeliveryFee(double deliveryFee)
        {
            this.deliveryFee = deliveryFee;
        }

        public double getDeliveryFee()
        {
            return this.deliveryFee;
        }

        public double getPromo()
        {
            return this.promo;
        }

        public void setPromo(double promo)
        {
            this.promo = promo;
        }

        public void updateTotal(double subtotal)
        {
            double total = subtotal + deliveryFee - promo;
            this.balance = this.balance - total;
            this.paymentCallback.onPriceUpdated(subtotal, total, balance);
        }
    }
    interface OnPaymentChangedListener
    {
        void onPriceUpdated(double subtotal, double grantTotal, double balance);
    }
```
menghitung total harga yang harus dibayar

<h3>Promo</h3>

```csharp
public class Promo
    {
        public string nama { get; set; }
        public string promo { get; set; }
        
        public Promo(string title, string promo)
        {
            this.nama = title;
            this.promo = promo;
        }
    }
```
tempat pendklarasian variabel pada window promo

<h2>Controller</h2>

<h3>DiskonController</h3>

```csharp
 class DiskonController
    {
        private List<Promo> promo;
        public DiskonController()
        {
            promo = new List<Promo>();
        }

        public void addPromo(Promo promo)
        {
            this.promo.Add(promo);
        }

        public List<Promo> getPromo()
        {
            return this.promo;
        }
    }
```
digunakan untuk membuat daftar list pada promo


<h3>MainWindowController</h3>

```csharp
class MainWindowController
    {
        KeranjangBelanja keranjangBelanja;
        
        public MainWindowController(KeranjangBelanja keranjangBelanja)
        {
            this.keranjangBelanja = keranjangBelanja;
        }

        public void addItem(Item item)
        {
            this.keranjangBelanja.addItem(item);
        }

        public List<Item> getSelectedItems()
        {
            return this.keranjangBelanja.getItems();
        }

        public void deleteSelectedItem(Item item)
        {
          this.keranjangBelanja.removeItem(item);
        }
    }
```
digunakan untuk mendapatkan data item dari window penawaran, dan menempatkan pada listbox mainwindow

<h3>PenawaranController</h3>

```csharp
class PenawaranController
    {
        private List<Item> items;
        public PenawaranController()
        {
            items = new List<Item>();
        }
        
        public void addItem(Item item)
        {
            this.items.Add(item);
        }

        public List<Item> getItems()
        {
            return this.items;
        }
    }
```
digunakan untuk membuat list item pada window penawaran dan mainwindow

<h2>View</h2>
<h3>MainWindow</h3>

```csharp
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
```
jendela utama (MainWindow)

<h3>Penawaran</h3>

```csharp
namespace MVC
{

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
```
jendela penawaran 

<h3>MainWindow</h3>

```csharp
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
```
Jendela Diskon