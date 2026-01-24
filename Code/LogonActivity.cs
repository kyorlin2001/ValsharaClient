using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;


namespace ValsharaPlatform.Code
{
    [Activity(Label = "LogonActivity")]
    public class LogonActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_login);

            UserData myuser = UserData.Init();

            Button CreatUserBTN = FindViewById<Button>(Resource.Id.CreateAccountButton);
            Button submitButton = FindViewById<Button>(Resource.Id.submitButton);
            EditText UserTextBox = FindViewById<EditText>(Resource.Id.UserNameText);
            EditText PasswordTextBox = FindViewById<EditText>(Resource.Id.PasswordText);
            EditText ErrorMessage = FindViewById<EditText>(Resource.Id.ErrorMessage);

            ErrorMessage.Text = myuser.GetPassword();
            ErrorMessage.Visibility = ViewStates.Visible;
            submitButton.Click += (sender, e) =>
            {

                //  UserData.UpdateDB(UserTextBox.Text, PasswordTextBox.Text);
                myuser.DBLogin(this, typeof(LogonActivity));
                //Save user file to local storage as well as db.
                myuser.SetPassword(PasswordTextBox.Text);
                myuser.SetUserName(UserTextBox.Text);
                myuser.SaveUserData();

                var mainintent = new Intent(this, typeof(MainActivity));
                StartActivity(mainintent);
            };
            CreatUserBTN.Click += (sender, e) =>
            {
                UserData.DBSignup(UserTextBox.Text, PasswordTextBox.Text, this);
                var mainintent = new Intent(this, typeof(MainActivity));
                StartActivity(mainintent);
            };

        }
    }
}