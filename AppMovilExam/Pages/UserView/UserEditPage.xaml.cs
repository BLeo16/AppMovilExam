namespace AppMovilExam.Pages.UserView;
using APIConsumer;
using AppMovilExam.Utilities;
using AppMovilExam.Models;

public partial class UserEditPage : ContentPage
{
    private string ApiUrl = APIConstants.ApiUser;
    private int UserId { get; set; }
    public UserEditPage(int userId)
	{
		InitializeComponent();
        UserId = userId;
        LoadUserData();
    }

    private async void LoadUserData()
    {
        try
        {
            var user = await Crud<User>.Read_ById(ApiUrl, UserId);
            if (user != null)
            {
                txtUsername.Text = user.username;
                txtPassword.Text = user.password;
            }
            else
            {
                await DisplayAlert("Error", "User not found.", "OK");
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
            // Verificar si el nuevo nombre de usuario ya existe
            bool usernameExists = await CheckIfUsernameExists(txtUsername.Text.ToString());

            if (usernameExists && txtUsername.Text.ToString() != txtUsername.Text.ToString())
            {
                await DisplayAlert("Error", "Username already exists. Please choose another one.", "OK");
                return;
            }

            User user = new User
            {
                id = UserId,
                username = txtUsername.Text.ToString(),
                password = txtPassword.Text.ToString()
            };

            // Actualizar el usuario
            await Crud<User>.Update(ApiUrl, UserId, user);
            await DisplayAlert("Success", "User updated successfully.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",
"Username must be unique [5-10 characters]\nPassword must be between 8 and 15 characters.",
"OK");

        }
    }

    private async Task<bool> CheckIfUsernameExists(string username)
    {
        try
        {
            var existingUser = await Crud<User>.Read_byName(ApiUrl, username);
            return existingUser != null;
        }
        catch (Exception)
        {

            return false;
        }
    }
}
