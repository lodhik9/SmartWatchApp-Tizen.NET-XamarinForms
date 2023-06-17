using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TizenDotNet4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {

        public LandingPage()
        {
            InitializeComponent();

            // Create a list of items for the bullet point list
            List<BulletItem> bulletItems = new List<BulletItem>
            {
                new BulletItem { ItemText = "Item 1" },
                new BulletItem { ItemText = "Item 2" },
                new BulletItem { ItemText = "Item 3" }
            };

            // Bind the list to the ListView
            bulletList.ItemsSource = bulletItems;

            // Set the default selected item
            SelectedBulletItem = bulletItems.Last();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            // Perform the necessary operations based on the selected item

            // Navigate to MainPage using Navigation.PushAsync
            await Navigation.PushModalAsync(new MainPage());

            // Deselect the selected item
            ((ListView)sender).SelectedItem = null;
        }

        // Define the property for the selected item
        public BulletItem SelectedBulletItem { get; set; }
    }



    // Define a class for the bullet point list items
    public class BulletItem
    {
        public string ItemText { get; set; }
    }
}
