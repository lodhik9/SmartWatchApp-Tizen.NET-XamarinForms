using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TizenDotNet4
{
    /// <summary>
    /// Represents the landing page of the application.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the LandingPage class.
        /// </summary>
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

        /// <summary>
        /// Event handler for the item selected event of the ListView.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
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
        /// <summary>
        /// Gets or sets the selected bullet item.
        /// </summary>
        public BulletItem SelectedBulletItem { get; set; }
    }



    /// <summary>
    /// Represents a bullet point list item.
    /// </summary>
    public class BulletItem
    {
        /// <summary>
        /// Gets or sets the text of the bullet item.
        /// </summary>
        public string ItemText { get; set; }
    }
}
