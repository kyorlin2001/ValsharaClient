using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ValsharaPlatform.Code
{
    [Activity(Label = "OptionsMenu")]
    public class OptionsMenu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_options);

            NumberPicker fontsizepicker = FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            Button mudHgtBtn = FindViewById<Button>(Resource.Id.mudHeightBTN);
            TextView screenheight = FindViewById<TextView>(Resource.Id.mudHeight);
            Button saveButton = FindViewById<Button>(Resource.Id.saveButton);
            UserData myuser = UserData.Init();
            ToggleButton autologonBtn = FindViewById<ToggleButton>(Resource.Id.AutoLogonBtn);
            Button LogonInfoBtn = FindViewById<Button>(Resource.Id.UpdateLgnBtn);
            ToggleButton GraphicsMdeBtn = FindViewById<ToggleButton>(Resource.Id.GraphicModeBtn);

            Button MacroBtn1 = FindViewById<Button>(Resource.Id.Macrobutton1);
            Button MacroBtn2 = FindViewById<Button>(Resource.Id.Macrobutton2);
            Button MacroBtn3 = FindViewById<Button>(Resource.Id.Macrobutton3);
            Button MacroBtn4 = FindViewById<Button>(Resource.Id.Macrobutton4);
            Button MacroBtn5 = FindViewById<Button>(Resource.Id.Macrobutton5);
            Button MacroBtn6 = FindViewById<Button>(Resource.Id.Macrobutton6);

            TextView MacroTxt1 = FindViewById<TextView>(Resource.Id.MacroString1);
            TextView MacroTxt2 = FindViewById<TextView>(Resource.Id.MacroString2);
            TextView MacroTxt3 = FindViewById<TextView>(Resource.Id.MacroString3);
            TextView MacroTxt4 = FindViewById<TextView>(Resource.Id.MacroString4);
            TextView MacroTxt5 = FindViewById<TextView>(Resource.Id.MacroString5);
            TextView MacroTxt6 = FindViewById<TextView>(Resource.Id.MacroString6);

            Button MudIPAddressBTN = FindViewById<Button>(Resource.Id.MudIPAddressBtn);
            Button MudPortBTN = FindViewById<Button>(Resource.Id.MudPortBtn);
            TextView MudIPAddressTxt = FindViewById<TextView>(Resource.Id.MudIPAddressTxt);
            TextView MudPortTxt = FindViewById<TextView>(Resource.Id.MudPortTxt);

            MudIPAddressTxt.Text = myuser.getMudIPAddress();
            MudPortTxt.Text = myuser.getMudPort().ToString();

            MacroTxt1.Text = myuser.GetMacro1();
            MacroTxt2.Text = myuser.GetMacro2();
            MacroTxt3.Text = myuser.GetMacro3();
            MacroTxt4.Text = myuser.GetMacro4();
            MacroTxt5.Text = myuser.GetMacro5();
            MacroTxt6.Text = myuser.GetMacro6();

            GraphicsMdeBtn.Checked = myuser.GetGraphicMode();
            autologonBtn.Checked = myuser.GetAutoLogonEnabled();
            fontsizepicker.MaxValue = 50;
            fontsizepicker.MinValue = 1;
            fontsizepicker.Value = myuser.Getmudfontsize();
            screenheight.Text = myuser.Getmudheight().ToString();
            View initialItem = fontsizepicker.GetChildAt(0);

            if (!myuser.GetAutoLogonEnabled())
                LogonInfoBtn.Visibility = ViewStates.Invisible;

            if(initialItem != null)
            {
                initialItem.Visibility = ViewStates.Invisible;
            }

            autologonBtn.Click += (sender, e) =>
            {
                if (autologonBtn.Checked == true)
                    LogonInfoBtn.Visibility = ViewStates.Visible;
                else
                    LogonInfoBtn.Visibility = ViewStates.Invisible;
            };

            LogonInfoBtn.Click += (sender, ev) =>
            {
                myuser.SetAutoLogonEnabled(true);
                var mainintent = new Intent(this, typeof(AALoginActivity));
                StartActivity(mainintent);
            };

            MacroBtn1.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText MacroTxt = new EditText(this);
                MCBTxt.SetMessage("Enter Macro Text. Enter %T for inserting target.");
                MCBTxt.SetTitle("Macro One");
                MCBTxt.SetView(MacroTxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MacroTxt1.Text = MacroTxt.Text;
                    myuser.SetMacro1(MacroTxt.Text);
                });
                MCBTxt.Show();
            };
            
            MacroBtn2.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText MacroTxt = new EditText(this);
                MCBTxt.SetMessage("Enter Macro Text. Enter %T for inserting target.");
                MCBTxt.SetTitle("Macro Two");
                MCBTxt.SetView(MacroTxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MacroTxt2.Text = MacroTxt.Text;
                    myuser.SetMacro2(MacroTxt.Text);
                });
                MCBTxt.Show();
            };

            MacroBtn3.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText MacroTxt = new EditText(this);
                MCBTxt.SetMessage("Enter Macro Text. Enter %T for inserting target.");
                MCBTxt.SetTitle("Macro Three");
                MCBTxt.SetView(MacroTxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MacroTxt3.Text = MacroTxt.Text;
                    myuser.SetMacro3(MacroTxt.Text);
                });
                MCBTxt.Show();
            };

            MacroBtn4.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText MacroTxt = new EditText(this);
                MCBTxt.SetMessage("Enter Macro Text. Enter %T for inserting target.");
                MCBTxt.SetTitle("Macro Four");
                MCBTxt.SetView(MacroTxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MacroTxt4.Text = MacroTxt.Text;
                    myuser.SetMacro4(MacroTxt.Text);
                });
                MCBTxt.Show();
            };

            MacroBtn5.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText MacroTxt = new EditText(this);
                MCBTxt.SetMessage("Enter Macro Text. Enter %T for inserting target.");
                MCBTxt.SetTitle("Macro Five");
                MCBTxt.SetView(MacroTxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MacroTxt5.Text = MacroTxt.Text;
                    myuser.SetMacro5(MacroTxt.Text);
                });
                MCBTxt.Show();
            };

            MacroBtn6.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText MacroTxt = new EditText(this);
                MCBTxt.SetMessage("Enter Macro Text. Enter %T for inserting target.");
                MCBTxt.SetTitle("Macro Six");
                MCBTxt.SetView(MacroTxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MacroTxt6.Text = MacroTxt.Text;
                    myuser.SetMacro6(MacroTxt.Text);
                });
                MCBTxt.Show();
            };

            MudIPAddressBTN.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText mipadd = new EditText(this);
                MCBTxt.SetMessage("Enter IP address to connect to.");
                MCBTxt.SetTitle("Mud IP Address");
                MCBTxt.SetView(mipadd);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MudIPAddressTxt.Text = mipadd.Text;
                    myuser.setMudIPAddres(mipadd.Text);
                });
                MCBTxt.Show();
            };

            MudPortBTN.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText mPort = new EditText(this);
                mPort.SetRawInputType(Android.Text.InputTypes.ClassNumber);
                MCBTxt.SetMessage("Enter port number to connect to.");
                MCBTxt.SetTitle("Port Number");
                MCBTxt.SetView(mPort);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    MudPortTxt.Text = mPort.Text;
                    myuser.setMudPort(int.Parse(mPort.Text));
                });
                MCBTxt.Show();
            };

            mudHgtBtn.Click += (sender, ev) =>
            {
                AlertDialog.Builder MCBTxt = new AlertDialog.Builder(this);
                EditText Hgttxt = new EditText(this);
                Hgttxt.SetRawInputType(Android.Text.InputTypes.ClassNumber);
                MCBTxt.SetMessage("Enter Height of Mud Window");
                MCBTxt.SetTitle("Mud Height");
                MCBTxt.SetView(Hgttxt);
                MCBTxt.SetNegativeButton("Cancel", (dbox, e) =>
                { });
                MCBTxt.SetPositiveButton("Save", (dbox, e) =>
                {
                    screenheight.Text = Hgttxt.Text;
                });
                MCBTxt.Show();
            };

            saveButton.Click += (sender, e) =>
            {
                myuser.Setmudfontsize(fontsizepicker.Value);
                myuser.Setmudheight(int.Parse(screenheight.Text));
                myuser.SetAutoLogonEnabled(autologonBtn.Checked);
                myuser.SetGraphicMode(GraphicsMdeBtn.Checked);
                myuser.SaveUserData();
                //save mysuser to file once its added.
                var mainintent = new Intent(this, typeof(MainActivity));
                StartActivity(mainintent);
            };
        }
    }
}