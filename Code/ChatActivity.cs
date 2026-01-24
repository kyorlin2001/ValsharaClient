using System.ComponentModel;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Views.InputMethods;
using System.Collections.Generic;

namespace ValsharaPlatform.Code
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity
    {
        private static ArcaneAges AAChatConnection;
        private static string msgtxt; //needed to store text on screen.
        LinkedList<Button> moblistBTN = new LinkedList<Button>(); //used to store buttons in targeted macro alert window.

        //add popup to let user know connection was lost then go to main menu
        public void ConnectionFailure(string errorMsg)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog connectionAlert = dialog.Create();
            connectionAlert.SetTitle("Network Connection Failure");
            connectionAlert.SetTitle("Connection to Arcane Ages " + errorMsg);
            connectionAlert.SetButton("OK", (c, ev) =>
            {
                AAChatConnection = null;
                var mainintent = new Intent(this, typeof(MainActivity));
                StartActivity(mainintent);
            });
            connectionAlert.Show();
        }

        protected void parseTargets(string macrotxt)
        {
            string message = "scan\n";
            if (!macrotxt.Contains("%T"))
                AAChatConnection.SendData(AAChatConnection.GetConnection().GetStream(), macrotxt, this);
            else
            {
                //do a scan wait for result then pop up window to make selection. Set variable so that read function knows not to post output to screen.  
                UserData myuser = UserData.Init();
                myuser.setCurrentMacroTxt(macrotxt);
                AAChatConnection.SendData(AAChatConnection.GetConnection().GetStream(), message, this);
                myuser.SetScanUpdate(true);
            }
        }

      
        protected void sendTargetedMacro()
        {
            UserData myuser = UserData.Init();
           
            string textToSend;
            //create popup here, it may take time to load all data so maybe make a thread for it.
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog targetlistAlert = dialog.Create();
            GridLayout mlistview = new GridLayout(this);
            mlistview.LayoutParameters = new GridLayout.LayoutParams();
            GridLayout.LayoutParams mlistviewparams = (GridLayout.LayoutParams) mlistview.LayoutParameters;
            mlistviewparams.SetGravity(GravityFlags.Center);
            mlistview.LayoutParameters = mlistviewparams;

            //use linked list to create buttons and events.
            foreach (string mob in myuser.GetMobList())
            {
                Button mobbtn = new Button(this);
                mobbtn.SetWidth(300);
                mobbtn.Text = mob;
                mobbtn.Click += (sender, ev) =>
                {
                    textToSend = myuser.getCurrentMacroTxt().Replace("%T", mob) + "\n";
                    AAChatConnection.SendData(AAChatConnection.GetConnection().GetStream(), textToSend, this);
                    targetlistAlert.Hide();
                };
                moblistBTN.AddLast(mobbtn);
                mlistview.AddView(moblistBTN.Last.Value);
            }
            targetlistAlert.SetView(mlistview,50,0,50,0);

           
            mlistview.ColumnCount = 1;
            mlistview.VerticalScrollBarEnabled = true;
           
       

            targetlistAlert.SetTitle("    Choose a Target");
            targetlistAlert.SetButton("Cancel", (c, ev) =>
            { 
                targetlistAlert.Hide();
            });
            targetlistAlert.Show();
            targetlistAlert.Window.SetLayout(400, 500);

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            UserData myuser = UserData.Init();

            BackgroundWorker aaThread = new BackgroundWorker();
            
            string message = "";
            SpannableStringBuilder ColorizedMessage;
            string macrotxt;
            

            aaThread.WorkerReportsProgress = true;

            SetContentView(Resource.Layout.activity_chat);

            Button SendMSGBTN = FindViewById<Button>(Resource.Id.SendButton);
            Button BackBTN = FindViewById<Button>(Resource.Id.BackButton);

            ImageButton ChatBtn = FindViewById<ImageButton>(Resource.Id.ChatButton);
            ImageButton NorthBtn = FindViewById<ImageButton>(Resource.Id.NorthBtn);
            ImageButton EastBtn = FindViewById<ImageButton>(Resource.Id.EastBtn);
            ImageButton SouthBtn = FindViewById<ImageButton>(Resource.Id.SouthBtn);
            ImageButton WestBtn = FindViewById<ImageButton>(Resource.Id.WestBtn);
            ImageButton NorthWestBtn = FindViewById<ImageButton>(Resource.Id.NorthWestBtn);
            ImageButton NorthEastBtn = FindViewById<ImageButton>(Resource.Id.NorthEastBtn);
            ImageButton SouthWestBtn = FindViewById<ImageButton>(Resource.Id.SouthWestBtn);
            ImageButton SouthEastBtn = FindViewById<ImageButton>(Resource.Id.SouthEastBtn);
            ImageButton UpBtn = FindViewById<ImageButton>(Resource.Id.UpBtn);
            ImageButton DownBtn = FindViewById<ImageButton>(Resource.Id.DownBtn);
            ImageButton EyeBtn = FindViewById<ImageButton>(Resource.Id.EyeBtn);

            Button MacroBtn1 = FindViewById<Button>(Resource.Id.Macro1Btn);
            Button MacroBtn2 = FindViewById<Button>(Resource.Id.Macro2Btn);
            Button MacroBtn3 = FindViewById<Button>(Resource.Id.Macro3Btn);
            Button MacroBtn4 = FindViewById<Button>(Resource.Id.Macro4Btn);
            Button MacroBtn5 = FindViewById<Button>(Resource.Id.Macro5Btn);
            Button MacroBtn6 = FindViewById<Button>(Resource.Id.Macro6Btn);

            TextView InTxtBox = FindViewById<TextView>(Resource.Id.ChatWindow);
            EditText OutTxtBox = FindViewById<EditText>(Resource.Id.editText2);

            //add image to chat button

            InTxtBox.MovementMethod =new ScrollingMovementMethod();
            InTxtBox.LayoutParameters.Height = myuser.Getmudheight();
            InTxtBox.SetTextSize(Android.Util.ComplexUnitType.Dip,(float) myuser.Getmudfontsize());

            //create connection to mud
            if (AAChatConnection == null)
                AAChatConnection = new ArcaneAges(this);
            else
                InTxtBox.Text = msgtxt;
            NetworkStream AAStream = AAChatConnection.GetConnection().GetStream();
            //Insert code to parse logon if autologon is on
            if (myuser.GetAutoLogonEnabled())
            {
                AAChatConnection.OnRecievedData(AAStream);
                AAChatConnection.SendData(AAStream, myuser.GetAAUsername() + '\n', this);
                AAChatConnection.OnRecievedData(AAStream);
                AAChatConnection.SendData(AAStream, myuser.GetAAPassword() + '\n', this);
            }


            //perform logon and receive messages in another thread
            aaThread.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    while(AAChatConnection.GetConnection().Connected)
                    {
                        message = Encoding.ASCII.GetString(AAChatConnection.OnRecievedData(AAStream));
                        aaThread.ReportProgress(1);
                    }
                });
                aaThread.ProgressChanged += new ProgressChangedEventHandler(
                delegate (object o, ProgressChangedEventArgs args)
                {
                    ColorizedMessage = AAChatConnection.MudDataParse(message);
                    if (myuser.GetScanUpdate())
                    {
                        AAChatConnection.MobileParse(ColorizedMessage.ToString());
                        myuser.SetScanUpdate(false);
                        sendTargetedMacro();
                    }
                    else
                    {
                        InTxtBox.Append(ColorizedMessage);
                    }
                    msgtxt = InTxtBox.Text;
                    if(myuser.GetExitsUpdated())
                    {
                        if (myuser.GetNorthExit())
                            NorthBtn.SetImageResource(Resource.Drawable.NorthBtnIcon);
                        else
                            NorthBtn.SetImageResource(Resource.Drawable.NorthBtnIconInactive);
                        if (myuser.GetNorthEastExit())
                            NorthEastBtn.SetImageResource(Resource.Drawable.NorthEastBtnIcon);
                        else
                            NorthEastBtn.SetImageResource(Resource.Drawable.NorthEastBtnIconInactive);
                        if (myuser.GetEastExit())
                            EastBtn.SetImageResource(Resource.Drawable.EastBtnIcon);
                        else
                            EastBtn.SetImageResource(Resource.Drawable.EastBtnIconInactive);
                        if (myuser.GetSouthEastExit())
                            SouthEastBtn.SetImageResource(Resource.Drawable.SouthEastBtnIcon);
                        else
                            SouthEastBtn.SetImageResource(Resource.Drawable.SouthEastBtnIconInactive);
                        if (myuser.GetSouthExit())
                            SouthBtn.SetImageResource(Resource.Drawable.SouthBtnIcon);
                        else
                            SouthBtn.SetImageResource(Resource.Drawable.SouthBtnIconInactive);
                        if (myuser.GetSouthWestExit())
                            SouthWestBtn.SetImageResource(Resource.Drawable.SouthWestBtnIcon);
                        else
                            SouthWestBtn.SetImageResource(Resource.Drawable.SouthWestBtnIconInactive);
                        if (myuser.GetWestExit())
                            WestBtn.SetImageResource(Resource.Drawable.WestBtnIcon);
                        else
                            WestBtn.SetImageResource(Resource.Drawable.WestBtnIconInactive);
                        if (myuser.GetNorthWestExit())
                            NorthWestBtn.SetImageResource(Resource.Drawable.NorthWestBtnIcon);
                        else
                            NorthWestBtn.SetImageResource(Resource.Drawable.NorthWestBtnIconInactive);
                        if (myuser.GetUpExit())
                            UpBtn.SetImageResource(Resource.Drawable.UpBtnIcon);
                        else
                            UpBtn.SetImageResource(Resource.Drawable.UpBtnIconInactive);
                        if (myuser.GetDownExit())
                            DownBtn.SetImageResource(Resource.Drawable.DownBtnIcon);
                        else
                            DownBtn.SetImageResource(Resource.Drawable.DownBtnIconInactive);
                    }
                });
            aaThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    InTxtBox.Text = InTxtBox.Text + Encoding.ASCII.GetString(AAChatConnection.OnRecievedData(AAStream));
                });
            aaThread.RunWorkerAsync();

            BackBTN.Click += (sender, ev) =>
            {
                var mainintent = new Intent(this, typeof(MainActivity));
                StartActivity(mainintent);
            };

            SendMSGBTN.Click += (sender, ev) =>
            {
                    AAChatConnection.SendData(AAStream, OutTxtBox.Text + "\n", this);
                    OutTxtBox.Text = "";
            };

            NorthBtn.Click += (sender, ev) =>
            {
                    AAChatConnection.SendData(AAStream, "North\n", this);  
            };

            EastBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "East\n", this);
            };

            SouthBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "South\n", this);
            };

            WestBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "West\n", this);
            };

            SouthEastBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "SouthEast\n", this);
            };

            SouthWestBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "SouthWest\n", this);
            };

            NorthEastBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "NorthEast\n", this);
            };

            NorthWestBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "NorthWest\n", this);
            };

            UpBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "Up\n", this);
            };

            DownBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "Down\n", this);
            };

            EyeBtn.Click += (sender, ev) =>
            {
                AAChatConnection.SendData(AAStream, "look\n", this);
            };

            MacroBtn1.Click += (sender, ev) =>
            {
                macrotxt = myuser.GetMacro1();
                parseTargets(macrotxt);
            };

            MacroBtn2.Click += (sender, ev) =>
            {
                macrotxt = myuser.GetMacro2();
                parseTargets(macrotxt);
            };

            MacroBtn3.Click += (sender, ev) =>
            {
                macrotxt = myuser.GetMacro3();
                parseTargets(macrotxt);
            };

            MacroBtn4.Click += (sender, ev) =>
            {
                macrotxt = myuser.GetMacro4();
                parseTargets(macrotxt);
            };

            MacroBtn5.Click += (sender, ev) =>
            {
                macrotxt = myuser.GetMacro5();
                parseTargets(macrotxt);
            };

            MacroBtn6.Click += (sender, ev) =>
            {
                macrotxt = myuser.GetMacro6();
                parseTargets(macrotxt);
            };

            ChatBtn.Click += (sender, ev) =>
            {
                if (OutTxtBox.Enabled == false)
                {
                    OutTxtBox.Visibility = ViewStates.Visible;
                    OutTxtBox.Enabled = true;
                    OutTxtBox.RequestFocus();
                    InputMethodManager myimm = (InputMethodManager)GetSystemService(InputMethodService);
                    myimm.ToggleSoftInput(ShowFlags.Forced, 0);
                }
                else
                {
                    OutTxtBox.Visibility = ViewStates.Invisible;
                    OutTxtBox.Enabled = false;
                }
            };
        }
    }
}