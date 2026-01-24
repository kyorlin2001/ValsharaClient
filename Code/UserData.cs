using System;
using Android.App;
using System.IO;
using Parse;
using System.Collections.Generic;
using Android.Content.PM;
using Android.OS;
using Android.Content;

namespace ValsharaPlatform.Code
{
    //User data class for storing data retrieved from db for logging in and out of games.
    //Uses Singleton pattern as there should only be one instance of this ever and most of the functions are required to be performed atomically.

    class UserData
    {
        private static UserData Singleton;

        //Mudconnection information
        private string mudIpAddress;
        private int mudPort;
        //Variables to track user related information
        private string ParseuserName;
        private string ParsepassWord;
        private int mudheight;
        private int mudfontsize;
        private bool autologonenabled;
        private static string userFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Valshara.dat");
        private string ArcaneAgesUsername;
        private string ArcaneAgesPassword;
        private bool graphicsMode = true;
        private bool loggedIn = true;
        public bool isDBRead = false;

        //Future code simplification, make this a linked list of macros so that it can be any size requires more overhead to manage but less memory is needed for having many macros.
        private string macro1 = "";
        private string macro2 = "";
        private string macro3 = "";
        private string macro4 = "";
        private string macro5 = "";
        private string macro6 = "";

        //exit directions need to be stored so that highlighting of exit directionals can be done per room.
        //may need to move these to a new datastructure so they can be reinitialized without setting each individually.  May not matter.
        private bool northExit;
        private bool northEastExit;
        private bool eastExit;
        private bool southEastExit;
        private bool southExit;
        private bool southWestExit;
        private bool westExit;
        private bool northWestExit;
        private bool upExit;
        private bool downExit;
        private bool exitsUpdated = false;
        private bool scanUpdate = false;
        //Need to track mobs and maybe objects in a room using scan
        private LinkedList<string> moblist;
        private string currentMacroTxt;

        public void setMudIPAddres(string txt)
        {
            mudIpAddress = txt;
        }

        public void setMudPort(int port)
        {
            mudPort = port;
        }

        public string getMudIPAddress()
        {
            return mudIpAddress;
        }

        public int getMudPort()
        {
            return mudPort;
        }

        public void setCurrentMacroTxt(string txt)
        {
            currentMacroTxt = txt;
        }

        public string getCurrentMacroTxt()
        {
            return currentMacroTxt;
        }

        

        public LinkedList<string>GetMobList()
        {
            return moblist;
        }

        public void SetMobList(LinkedList<string> mlist )
        {
            moblist = mlist;
        }

        public void SetGraphicMode(bool gmode)
        {
            graphicsMode = gmode;
        }

        public bool GetGraphicMode()
        {
            return graphicsMode;
        }

        public void SetExitsUpdated(bool yes)
        {
            exitsUpdated = yes;
        }

        public bool GetExitsUpdated()
        {
            return exitsUpdated;
        }

        public void SetScanUpdate(bool scstat)
        {
            scanUpdate = scstat;
        }

        public bool GetScanUpdate()
        {
            return scanUpdate;
        }

        public void SetNorthExit(bool exitExists)
        {
            northExit = exitExists;
        }

        public void SetNorthEastExit(bool exitExists)
        {
            northEastExit = exitExists;
        }

        public void SetEastExit(bool exitExists)
        {
            eastExit = exitExists;
        }

        public void SetSouthEastExit(bool exitExists)
        {
            southEastExit = exitExists;
        }

        public void SetSouthExit(bool exitExists)
        {
            southExit = exitExists;
        }

        public void SetSouthWestExit(bool exitExists)
        {
            southWestExit = exitExists;
        }

        public void SetWestExit(bool exitExists)
        {
            westExit = exitExists;
        }

        public void SetNorthWestExit(bool exitExists)
        {
            northWestExit = exitExists;
        }

        public void SetUpExit(bool exitExists)
        {
            upExit = exitExists;
        }

        public void SetDownExit(bool exitExists)
        {
            downExit = exitExists;
        }

        public bool GetNorthExit()
        {
            return northExit;
        }

        public bool GetNorthEastExit()
        {
            return northEastExit;
        }
        
        public bool GetEastExit()
        {
            return eastExit;
        }

        public bool GetSouthEastExit()
        {
            return southEastExit;
        }

        public bool GetSouthExit()
        {
            return southExit;
        }

        public bool GetSouthWestExit()
        {
            return southWestExit;
        }

        public bool GetWestExit()
        {
            return westExit;
        }

        public bool GetNorthWestExit()
        {
            return northWestExit;
        }

        public bool GetUpExit()
        {
            return upExit;
        }

        public bool GetDownExit()
        {
            return downExit;
        }

        public void SetMacro1(string Mstr)
        {
            macro1 = Mstr;
        }

        public void SetMacro2(string Mstr)
        {
            macro2 = Mstr;
        }

        public void SetMacro3(string Mstr)
        {
            macro3 = Mstr;
        }

        public void SetMacro4(string Mstr)
        {
            macro4 = Mstr;
        }

        public void SetMacro5(string Mstr)
        {
            macro5 = Mstr;
        }

        public void SetMacro6(string Mstr)
        {
            macro6 = Mstr;
        }

        public string GetMacro1()
        {
            return macro1;
        }

        public string GetMacro2()
        {
            return macro2;
        }

        public string GetMacro3()
        {
            return macro3;
        }

        public string GetMacro4()
        {
            return macro4;
        }

        public string GetMacro5()
        {
            return macro5;
        }

        public string GetMacro6()
        {
            return macro6;
        }

        public bool GetAutoLogonEnabled()
        {
            return autologonenabled;
        }

        public void SetAutoLogonEnabled(bool Value)
        {
            autologonenabled = Value;
        }

        public void Setmudheight(int mheight)
        {
            mudheight = mheight;
        }

        public int Getmudheight()
        {
            return mudheight;
        }

        public void Setmudfontsize(int fsize)
        {
            mudfontsize = fsize;
        }

        public int Getmudfontsize()
        {
            return mudfontsize;
        }

        public static UserData Init()
        {
            if(Singleton == null)
            {
                Singleton = new UserData();
                if (!File.Exists(userFile))
                {
                    Singleton.loggedIn = false;
                }
                else
                {
                    Singleton.LoadUserData();
                }
            }
            return Singleton;
        }

        private UserData()
        {
            mudIpAddress = "75.101.147.27";
            mudPort = 4000;
            mudheight = 800;
            mudfontsize = 12;
            autologonenabled = false;
        }

        //I do not want this changed outside of class control only creating getter, no setter.
        public bool IsLoggedIn()
        {
            return loggedIn;
        }

        public string GetUserName()
        {
            return ParseuserName;
        }

        public void SetUserName( string uname )
        {
            ParseuserName = uname;
        }

        //signup function
        static public async void DBSignup(string username, string password, Activity myact )
        {
            string errorMessage = null;
            var user = new ParseUser()
            {
                Username = username,
                Password = password,
                Email = "no@email.com"
            };
            try
            {
                await user.SignUpAsync();
            }
            catch (Exception error)
            {
                errorMessage = error.Message;
                /*EditText ErrorMessageBox = FindViewByID()
                FindViewById( = errorMessage;*/
            }
        }

        //log into parse
        async public void DBLogin(Activity myactivity,Type mytype)
        {
            //update list of games in userData including username/password (needs variable)
            string retrievedVersion = "";
            // No data stored yet so do not actually login.  For now we just need to retrieve the version number.
            try
            {
                ParseQuery<ParseObject> versionQuery = ParseObject.GetQuery("ClientVersion");
                ParseObject currentVersion = await versionQuery.GetAsync("JwSacr13lR");
                retrievedVersion = currentVersion.Get<string>("CurrentVersion");
                //await ParseUser.LogInAsync(ParseuserName, ParsepassWord);
                loggedIn = true;
            }
            catch (Exception)
            {
                loggedIn = false;
            }
            PackageInfo mypkginfo;
            mypkginfo = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);

            if (retrievedVersion != mypkginfo.VersionName )
            {
                //add code to deal with needing update.
                AlertDialog.Builder dialog = new AlertDialog.Builder(myactivity);
                AlertDialog UpdateAlert = dialog.Create();
                UpdateAlert.SetTitle("Update Needed");
                UpdateAlert.SetButton("Exit", (c, ev) =>
                {
                    Process.KillProcess(Process.MyPid());
                });
                UpdateAlert.SetButton2("Update Now", (c, ev) =>
                {
                    string appPackageName = Application.Context.PackageName;
                    //cannot be tested until beta is live.  Seems to work though.
                    try
                    {
                        var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + appPackageName));
                        intent.AddFlags(ActivityFlags.NewTask);

                        Application.Context.StartActivity(intent);
                    }
                    catch (ActivityNotFoundException)
                    {
                        var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + appPackageName));
                        intent.AddFlags(ActivityFlags.NewTask);

                        Application.Context.StartActivity(intent);
                    }
                });
                UpdateAlert.Show();
            }
            /*if (!IsLoggedIn())
            {
                var logonintent = new Android.Content.Intent(myactivity, mytype);
                myactivity.StartActivity(logonintent);
            }*/  
        }
        public void SaveUserData()
        {
            //need to delete existing file and create new if it exists.
            if(File.Exists(userFile))
            {
                File.Delete(userFile);
            }
            StreamWriter mystream = new StreamWriter(userFile);
            mystream.WriteLine("Username " + Singleton.GetUserName());
            mystream.WriteLine("Password " + Singleton.GetPassword()); //yes password is plain text, but device is fairly secure anyway as it is saved with only access to this app.
            mystream.WriteLine("ArcaneAgesUsername " + ArcaneAgesUsername);
            mystream.WriteLine("ArcaneAgesPassword " + ArcaneAgesPassword);
            mystream.WriteLine("ArcaneAgesFontSize " + Singleton.Getmudfontsize().ToString());
            mystream.WriteLine("ArcaneAgesMudHeight " + Singleton.Getmudheight().ToString());
            if (GetAutoLogonEnabled() == true)
                mystream.WriteLine("AutologonEnabled " + Singleton.GetAutoLogonEnabled().ToString());
            mystream.WriteLine("Macro1 " + macro1);
            mystream.WriteLine("Macro2 " + macro2);
            mystream.WriteLine("Macro3 " + macro3);
            mystream.WriteLine("Macro4 " + macro4);
            mystream.WriteLine("Macro5 " + macro5);
            mystream.WriteLine("Macro6 " + macro6);
            mystream.WriteLine("GraphicMode " + graphicsMode);
            
            mystream.Close();
            loggedIn = true;
        }
        
        public void LoadUserData()
        {
            string filedata;
            StreamReader mystream = new StreamReader(userFile);
            while (!mystream.EndOfStream)
            {
                filedata = mystream.ReadLine();
                if (filedata.Substring(0, 7) == "Macro1 ")
                    macro1 = filedata.Substring(7);
                if (filedata.Substring(0, 7) == "Macro2 ")
                    macro2 = filedata.Substring(7);
                if (filedata.Substring(0, 7) == "Macro3 ")
                    macro3 = filedata.Substring(7);
                if (filedata.Substring(0, 7) == "Macro4 ")
                    macro4 = filedata.Substring(7);
                if (filedata.Substring(0, 7) == "Macro5 ")
                    macro5 = filedata.Substring(7);
                if (filedata.Substring(0, 7) == "Macro6 ")
                    macro6 = filedata.Substring(7);
                if (filedata.Length > 9)
                {
                    if (filedata.Substring(0, 9) == "Username ")
                        ParseuserName = filedata.Substring(9);
                    if (filedata.Substring(0, 9) == "Password ")
                        ParsepassWord = filedata.Substring(9);
                }
                if (filedata.Length > 11 && filedata.Substring(0, 11) == "GraphicMode ")
                        graphicsMode = bool.Parse(filedata.Substring(11));
                if (filedata.Length > 13 && filedata.Contains("AutologonEnabled "))
                    Singleton.SetAutoLogonEnabled(bool.Parse(filedata.Substring(17)));
                if (filedata.Length > 19)
                {
                    if (filedata.Substring(0, 19) == "ArcaneAgesUsername ")
                        ArcaneAgesUsername = filedata.Substring(19);
                    if (filedata.Substring(0, 19) == "ArcaneAgesPassword ")
                        ArcaneAgesPassword = filedata.Substring(19);
                    if (filedata.Substring(0, 18) == "ArcaneAgesFontSize ")
                        Singleton.Setmudfontsize(int.Parse(filedata.Substring(19)));
                    if (filedata.Substring(0, 19) == "ArcaneAgesMudHeight ")
                        Singleton.Setmudheight(int.Parse(filedata.Substring(20)));
                }
            }
            mystream.Close();
        }
        //needs to encrypt the password in this function, get working first then add later.
        public void SetPassword(string newpass)
        {
            ParsepassWord = newpass;
        }

        public string GetPassword()
        {
            return ParsepassWord;
        }

        public void SetAAUsername(string name)
        {
            ArcaneAgesUsername = name;
        }

        public void SetAAPassword(string newpass)
        {
            ArcaneAgesPassword = newpass;
        }

        public string GetAAUsername()
        {
            return ArcaneAgesUsername;
        }

        public string GetAAPassword()
        {
            return ArcaneAgesPassword;
        }
    }
}
    