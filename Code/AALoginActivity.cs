using System;
using System.Collections.Generic;
using System.Linq;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ValsharaPlatform.Code
{
    [Activity(Label = "AALoginActivity")]
    public class AALoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_aalogin);

            double WidthRatio;
            double HeightRatio;

            double calculatedX;

            UserData myUser = UserData.Init();

            TextView UserNameTxt = FindViewById<TextView>(Resource.Id.AAUserNameTxt);
            TextView PasswordTxt = FindViewById<TextView>(Resource.Id.AAPasswordTxt);

            EditText AAUsername = FindViewById<EditText>(Resource.Id.AAUsername);
            EditText AAPassword = FindViewById<EditText>(Resource.Id.AAPassword);

            Button SaveButton = FindViewById<Button>(Resource.Id.AASaveButton);

            View BackGroundView = FindViewById<View>(Resource.Id.BackgroundView);

            DisplayMetrics dmetrics = Resources.DisplayMetrics;

            //first screen (main) is generalized so button resolution doesn't matter.  Have that screen set userdata screen height and width then use here maybe.

            WidthRatio = dmetrics.WidthPixels / 384.0;
            HeightRatio = dmetrics.HeightPixels / 589.0;

            //Set position of Fields by Width and Height ratio May also need to adjust font

            calculatedX = (UserNameTxt.GetX() * WidthRatio);

            RelativeLayout.LayoutParams UserNameTxtParams = (RelativeLayout.LayoutParams) UserNameTxt.LayoutParameters;
            UserNameTxtParams.LeftMargin = (int)(WidthRatio * 75);
            UserNameTxtParams.TopMargin = (int)(HeightRatio * 200);
            UserNameTxtParams.Width = (int)(WidthRatio * 300);
            UserNameTxtParams.Height = (int)(HeightRatio * 40);
            UserNameTxt.SetTextSize(ComplexUnitType.Dip, 6 * (float)HeightRatio);
            UserNameTxt.LayoutParameters = UserNameTxtParams;

            RelativeLayout.LayoutParams AAUsernameParams = (RelativeLayout.LayoutParams) AAUsername.LayoutParameters;
            AAUsernameParams.LeftMargin = (int)(WidthRatio * 75);
            AAUsernameParams.TopMargin = (int)(HeightRatio * 200)+40;
            AAUsernameParams.Width = (int)(WidthRatio * 200);
            AAUsernameParams.Height = (int)(HeightRatio * 40);
            AAUsername.SetTextSize(ComplexUnitType.Dip, 6 * (float) HeightRatio);
            AAUsername.LayoutParameters = AAUsernameParams;

            RelativeLayout.LayoutParams PasswordTxtParams = (RelativeLayout.LayoutParams)PasswordTxt.LayoutParameters;
            PasswordTxtParams.LeftMargin = (int)(WidthRatio * 75);
            PasswordTxtParams.TopMargin = (int)(HeightRatio * 300);
            PasswordTxtParams.Width = (int)(WidthRatio * 300);
            PasswordTxtParams.Height = (int) (HeightRatio * 40);
            PasswordTxt.SetTextSize(ComplexUnitType.Dip, 6 * (float)HeightRatio);
            PasswordTxt.LayoutParameters = PasswordTxtParams;

            RelativeLayout.LayoutParams AAPasswordParams = (RelativeLayout.LayoutParams)AAPassword.LayoutParameters;
            AAPasswordParams.LeftMargin = (int)(WidthRatio * 75);
            AAPasswordParams.TopMargin = (int)(HeightRatio * 300)+40;
            AAPasswordParams.Width = (int)(WidthRatio * 200);
            AAPasswordParams.Height = (int)(HeightRatio * 40);
            AAPassword.SetTextSize(ComplexUnitType.Dip, 6 * (float)HeightRatio);
            AAPassword.LayoutParameters = AAPasswordParams;

            RelativeLayout.LayoutParams SaveButtonParams = (RelativeLayout.LayoutParams)SaveButton.LayoutParameters;
            SaveButtonParams.LeftMargin = (int)(WidthRatio * 129.5);
            SaveButtonParams.TopMargin = (int)(HeightRatio * 500);
            SaveButtonParams.Width = (int)(WidthRatio*125);
            SaveButtonParams.Height = (int)(HeightRatio * 40);
            SaveButton.SetTextSize(ComplexUnitType.Px, 8 * (float)HeightRatio);
            SaveButton.LayoutParameters = SaveButtonParams;

            SaveButton.Click += (sender, e) =>
            {
                myUser.SetAAUsername(AAUsername.Text);
                myUser.SetAAPassword(AAPassword.Text);
                myUser.SaveUserData();

                var mainintent = new Intent(this, typeof(OptionsMenu));
                StartActivity(mainintent);
            };
        }
    }
}