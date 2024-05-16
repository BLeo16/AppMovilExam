namespace AppMovilExam.Pages.PostView;
using APIConsumer;
using AppMovilExam.Utilities;
using AppMovilExam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class PostEditPage : ContentPage
{
    private string ApiUrl = APIConstants.ApiPost;
    private string ApiUserUrl = APIConstants.ApiUser;
    private int PostId { get; set; }

    public PostEditPage(int postId)
    {
        InitializeComponent();
        PostId = postId;
        LoadUsersAndPostData();
    }

    private async void LoadUsersAndPostData()
    {
        try
        {
            var users = await Crud<User>.Read(ApiUserUrl);
            userPicker.ItemsSource = users;
            userPicker.ItemDisplayBinding = new Binding("username");

            var post = await Crud<Post>.Read_ById(ApiUrl, PostId);
            if (post != null)
            {
                txtTitle.Text = post.title;
                txtContent.Text = post.content;
                userPicker.SelectedItem = users.FirstOrDefault(u => u.id == post.userId);
            }
            else
            {
                await DisplayAlert("Error", "Post not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void BtnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void BtnUpdate_Clicked(object sender, EventArgs e)
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
                id = PostId,
                title = txtTitle.Text,
                content = txtContent.Text,
                userId = selectedUser.id
            };

            await Crud<Post>.Update(ApiUrl, PostId, post);
            await DisplayAlert("Success", "Post updated successfully.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
