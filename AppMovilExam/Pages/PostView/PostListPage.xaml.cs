namespace AppMovilExam.Pages.PostView;
using APIConsumer;
using AppMovilExam.Utilities;
using AppMovilExam.Models;

public partial class PostListPage : ContentPage
{
    private string ApiUrl = APIConstants.ApiPost;

    public PostListPage()
    {
        InitializeComponent();
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadPosts();
    }

    private async Task LoadPosts()
    {
        try
        {
            // Clear existing children
            PostsStackLayout.Children.Clear();

            var posts = await Crud<Post>.Read(ApiUrl);
            foreach (var post in posts)
            {
                var labelId = new Label { Text = post.id.ToString() };
                var labelTitle = new Label { Text = post.title };
                var labelContent = new Label { Text = post.content };
                PostsStackLayout.Children.Add(labelId);
                PostsStackLayout.Children.Add(labelTitle);
                PostsStackLayout.Children.Add(labelContent);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void btnCreate_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PostCreatePage());
    }

    private async void BtnSearch_Clicked(object sender, EventArgs e)
    {
        try
        {
            string title = txtSearch.Text;

            if (string.IsNullOrEmpty(title))
            {
                await DisplayAlert("Error", "Please enter a title.", "OK");
                return;
            }

            var posts = await Crud<Post>.Read(ApiUrl);
            var post = posts.FirstOrDefault(p => p.title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (post != null)
            {
                lblPostId.Text = post.id.ToString();
                lblTitle.Text = post.title;
                lblContent.Text = post.content;

                postInfoLayout.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", "Post not found.", "OK");

                lblPostId.Text = string.Empty;
                lblTitle.Text = string.Empty;
                lblContent.Text = string.Empty;
                postInfoLayout.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void BtnEdit_Clicked(object sender, EventArgs e)
    {
        int postId = Convert.ToInt32(lblPostId.Text);
        await Navigation.PushAsync(new PostEditPage(postId));
        lblPostId.Text = string.Empty;
        lblTitle.Text = string.Empty;
        lblContent.Text = string.Empty;
        postInfoLayout.IsVisible = false;
    }

    private async void BtnDelete_Clicked(object sender, EventArgs e)
    {
        try
        {
            int postId = Convert.ToInt32(lblPostId.Text);

            await Crud<Post>.Delete(ApiUrl, postId);

            await DisplayAlert("Success", "Post deleted successfully", "OK");

            lblPostId.Text = string.Empty;
            lblTitle.Text = string.Empty;
            lblContent.Text = string.Empty;
            postInfoLayout.IsVisible = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
