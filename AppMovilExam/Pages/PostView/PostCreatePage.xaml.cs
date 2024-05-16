namespace AppMovilExam.Pages.PostView;
using APIConsumer;
using AppMovilExam.Utilities;
using AppMovilExam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class PostCreatePage : ContentPage
{
    private string ApiUrl = APIConstants.ApiPost;
    private string ApiUserUrl = APIConstants.ApiUser;

    public PostCreatePage()
    {
        InitializeComponent();
        LoadUsers();
    }

    private async void LoadUsers()
    {
        try
        {
            var users = await Crud<User>.Read(ApiUserUrl);
            userPicker.ItemsSource = users;
            userPicker.ItemDisplayBinding = new Binding("username");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to load users", "OK");
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnCreate_Clicked(object sender, EventArgs e)
    {
        try
        {
            var selectedUser = userPicker.SelectedItem as User;
            if (selectedUser == null)
            {
                await DisplayAlert("Error", "Please select a user", "OK");
                return;
            }

            Post post = new Post
            {
                title = txtTitle.Text,
                content = txtContent.Text,
                userId = selectedUser.id
            };

            var result = await Crud<Post>.Create(ApiUrl, post);
            if (result != null)
            {
                await DisplayAlert("Success", "Post created successfully", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to create post", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while creating the post", "OK");
        }
    }
}
