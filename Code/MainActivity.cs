using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Parse;
using ValsharaPlatform.Code;
using Android.Content;
using Android.Gms.Ads;

// addmob app id REMOVED_FOR_SECURITY
namespace ValsharaPlatform
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            MobileAds.Initialize(this, "REMOVED_FOR_SECURITY");
            //Code to initialize Buddy Parse
            ParseClient.Initialize( new ParseClient.Configuration {
                ApplicationId = "REMOVED_FOR_SECURITY",
                WindowsKey = "REMOVED_FOR_SECURITY",
                Server = "https://parse.buddy.com/parse/" });
          

            ParseAnalytics.TrackAppOpenedAsync();

            base.OnCreate(savedInstanceState);


            // create new instance of userdata called current user here
            UserData currentUser = UserData.Init();

            //check that user is verified in parse 
            //this is causing infinite loop.  May need to check to see if user is logged in before this.  But need to verify that user will not be logged in until after it is checked once

            currentUser.DBLogin(this, typeof(LogonActivity));


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Button functions
            Button OptionsButton = FindViewById<Button>(Resource.Id.optionsbutton);
            Button ConnectButton = FindViewById<Button>(Resource.Id.connectbutton);
            AdView myAD = FindViewById<AdView>(Resource.Id.adView1);
            AdRequest adrequest = new AdRequest.Builder().Build();
            myAD.LoadAd(adrequest);

            //Hide keyboard, focus needs to be set first.
           

            ConnectButton.Click += (sender, e) =>
            {
                var mainintent = new Intent(this, typeof(ChatActivity));
                StartActivity(mainintent);
            };

            OptionsButton.Click += (sender, e) =>
            {
                var mainintent = new Intent(this, typeof(OptionsMenu));
                StartActivity(mainintent);
            };
        }

      
    }
}

