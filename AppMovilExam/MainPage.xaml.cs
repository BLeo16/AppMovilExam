

using AppMovilExam.Pages.PostView;
using AppMovilExam.Pages.UserView;

namespace AppMovilExam
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

       private void BtnListUsers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserListPage());
        }

        private void BtnListPosts_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PostListPage());
        }
    }

}
