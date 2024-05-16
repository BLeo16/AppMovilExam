namespace AppMovilExam.Pages.UserView;
using APIConsumer;
using AppMovilExam.Utilities;
using AppMovilExam.Models;

public partial class UserListPage : ContentPage
{
    private string ApiUrl = APIConstants.ApiUser;
	public UserListPage()
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
        LoadUsers();
    }

    private async Task LoadUsers()
    {
        try
        {
            // Clear existing children
            UsersStackLayout.Children.Clear();

            var users = await Crud<User>.Read(ApiUrl);
            foreach (var user in users)
            {
                var labelId = new Label { Text = user.id.ToString() };
                var label = new Label { Text = user.username };
                UsersStackLayout.Children.Add(labelId);
                UsersStackLayout.Children.Add(label);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void btnCreate_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new UserCreatePage());
    }

    private async void BtnSearch_Clicked(object sender, EventArgs e)
    {
        try
        {

            string username = txtSearch.Text;


            if (string.IsNullOrEmpty(username))
            {
                await DisplayAlert("Error", "Please enter a username.", "OK");
                return;
            }


            User user = await Crud<User>.Read_byName(ApiUrl, username);


            if (user != null)
            {

                lblUserId.Text = user.id.ToString();
                lblUsername.Text = user.username;


                userInfoLayout.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", "User not found.", "OK");

                lblUserId.Text = string.Empty;
                lblUsername.Text = string.Empty;
                userInfoLayout.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private async void BtnEdit_Clicked(object sender, EventArgs e)
    {

        int userId = Convert.ToInt32(lblUserId.Text);
        await Navigation.PushAsync(new UserEditPage(userId));
        lblUserId.Text = string.Empty;
        lblUsername.Text = string.Empty;
        userInfoLayout.IsVisible = false;
    }

    private async void BtnDelete_Clicked(object sender, EventArgs e)
    {
        try
        {

            int userId = Convert.ToInt32(lblUserId.Text);

            await Crud<User>.Delete(ApiUrl, userId);

            await DisplayAlert("Success", "User deleted successfully", "OK");

            lblUserId.Text = string.Empty;
            lblUsername.Text = string.Empty;
            userInfoLayout.IsVisible = false;

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}