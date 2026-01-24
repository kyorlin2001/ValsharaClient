using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Android.Text;
using Android.Graphics;
using Android.Text.Style;
using System.IO;
using Android.Content;
using System;
using System.Collections.Generic;

/* ArcaneAges specific functions, some of these may evolve into general mud specific functions but different muds may be different. When multimud client is made this may need individual files for different mud types.
 * to simplify future development these functions are being kept seperate.
 */

namespace ValsharaPlatform.Code
{
    class ArcaneAges
    {
        UserData myUser = UserData.Init();
        const int BufferSize = 8192; //TCP Packet size
        byte[] inStream = new byte[BufferSize]; //one packet

        private TcpClient AASocket = new TcpClient();

        public ArcaneAges(ChatActivity ca)
        {
            try
            {
                AASocket.Connect(myUser.getMudIPAddress(),myUser.getMudPort());
            }
            catch (Exception)
            {
                ca.ConnectionFailure("could not be established.");
            }
        }

        public byte[] OnRecievedData(NetworkStream AAConnection)
        {
            inStream = new byte[BufferSize];
            while (!AAConnection.DataAvailable)
            {
                 System.Threading.Thread.Sleep(200);
            }
            AAConnection.Read(inStream, 0, BufferSize);
            return inStream;

        }
        public void SendData(NetworkStream AAConnection, string message, ChatActivity ca)
        {
            try
            {
                AAConnection.Write(Encoding.ASCII.GetBytes(message), 0, Encoding.ASCII.GetBytes(message).Length);
            }
            catch (IOException)
            {
                ca.ConnectionFailure("lost.");
            }
        }

        public TcpClient GetConnection()
        {
            return AASocket;
        }


        public SpannableStringBuilder MudDataParse(string message)
        {
            SpannableStringBuilder ColorizedMessage = new SpannableStringBuilder();
            MatchCollection AnsiCodes;
            int startpos = 0;
            int CountofMatches;
            int CurrentMatch = 1; //offset needs to include the string size of the filter.
            int MatchIndex;

            Regex Filter = new Regex (@"\[\d;\d\d;\d\dm");

            //Maybe instead of split, use matches to get the location in the string and set the colors.  Each ansi code from tera is 9 characters long.  For second stage, check mozart and a merc based mud as well as a moo or a mush to see if ansi works..
            //The string size is known so using Regex.split the string can be split into an array of strings using foreach to visit, this will alow each individual to be converted to color codes correctly once color is done with window
            //It appears the regex filter is being removed from the split, however the original string is still in message.  Due to this and we know the size of the filter, the original string can be used for color codes to apply to the final string
            //initial code is null or a continuation of the previous color.  I'm going to assume null for now for simplicities sake and see how it looks.
            AnsiCodes = Filter.Matches(message);
            CountofMatches = AnsiCodes.Count;
            ColorizedMessage.Append(Filter.Replace(message,""));
            foreach (Match ansicode in AnsiCodes)
            {
                //may need an object and a recursive function to go through the collection rather than a foreach.  Must determine if it is possible to iterate, need location of next match to determine length of span.
                if (CurrentMatch < CountofMatches)
                    MatchIndex = ansicode.NextMatch().Index - 9*CurrentMatch;
                else
                    MatchIndex = ColorizedMessage.Length();
                ColorizedMessage.SetSpan(new ForegroundColorSpan(ConvertAnsi(ansicode.Value.Substring(startpos+1, 1), ansicode.Value.Substring(startpos + 3, 2))), ansicode.Index - 9*(CurrentMatch-1), MatchIndex, SpanTypes.ExclusiveExclusive);
                ColorizedMessage.SetSpan(new BackgroundColorSpan(ConvertBackgroundAnsi(ansicode.Value.Substring(startpos + 6, 2))), ansicode.Index - 9 * (CurrentMatch - 1), MatchIndex, SpanTypes.ExclusiveExclusive);
                CurrentMatch++;
            }
            //Need to parse exits and set variables, probably on userdata for current room.  These will not be saved to datafile, but will be used to determine color and activity of directional arrows.
            if(message.Contains("Exits: "))
                ExitParse(message);
            return ColorizedMessage;
        }

        //function to parse mobiles for buttons, initial version may pick up objects too.  Uses scan command to get list.  Scan command is issued then this will be called from background, maybe merge with ansi parse.
        public void MobileParse(string message)
        {
            UserData myuser = UserData.Init();
            Regex Filter = new Regex(@"\s\w+, right here");
            MatchCollection mobiles;
            string workingMatch;

            LinkedList<string> moblist = new LinkedList<string>();
  
            mobiles = Filter.Matches(message);

            foreach (Match mobile in mobiles)
            {
                workingMatch = mobile.Value;
                workingMatch = workingMatch.Substring(0, workingMatch.Length - 12);
                moblist.AddLast(workingMatch);
            }
            //string manipulation: need to remove is right here from moblist
            myuser.SetMobList(moblist);
        }
        //function to parse exits.  Depending on graphical settings this may end up doing different things so seperated it as its own function
        private void ExitParse(string txt)
        {
            string workingtxt;
            myUser = UserData.Init();
            //since for now we don't have any other way to reset exit variables
            myUser.SetNorthExit(false);
            myUser.SetNorthEastExit(false);
            myUser.SetEastExit(false);
            myUser.SetSouthEastExit(false);
            myUser.SetSouthExit(false);
            myUser.SetSouthWestExit(false);
            myUser.SetWestExit(false);
            myUser.SetNorthWestExit(false);
            myUser.SetUpExit(false);
            myUser.SetDownExit(false);

            //Need to check to make sure this doesn't have problems with names in room.
            workingtxt = txt.Substring(txt.IndexOf("Exits: "));
            workingtxt = workingtxt.Substring(0, workingtxt.IndexOf('\n'));
            if (workingtxt.Contains("north,") || workingtxt.Contains("north\x1B"))
                myUser.SetNorthExit(true);
            if (workingtxt.Contains("northeast"))
                myUser.SetNorthEastExit(true);
            if (workingtxt.Contains(" east"))
                myUser.SetEastExit(true);
            if (workingtxt.Contains("southeast"))
                myUser.SetSouthEastExit(true);
            if (workingtxt.Contains("south,") || workingtxt.Contains("south\x1B"))
                myUser.SetSouthExit(true);
            if (workingtxt.Contains("southwest"))
                myUser.SetSouthWestExit(true);
            if (workingtxt.Contains(" west"))
                myUser.SetWestExit(true);
            if (workingtxt.Contains("northwest"))
                myUser.SetNorthWestExit(true);
            if (workingtxt.Contains("up"))
                myUser.SetUpExit(true);
            if (workingtxt.Contains("down"))
                myUser.SetDownExit(true);
            myUser.SetExitsUpdated(true);
        }

        //Function to get color tag from ansi code.
        private Color ConvertBackgroundAnsi(string backgroundcode)
        {
            Color mycolor;
            switch (backgroundcode)
            {
                case "40":
                    mycolor = Color.Black;
                    break;
                case "41":
                    mycolor = Color.Red;
                    break;
                case "42":
                    mycolor = Color.Green;
                    break;
                case "43":
                    mycolor = Color.Yellow;
                    break;
                case "44":
                    mycolor = Color.Blue;
                    break;
                case "45":
                    mycolor = Color.Magenta;
                    break;
                case "46":
                    mycolor = Color.Cyan;
                    break;
                case "47":
                    mycolor = Color.White;
                    break;
                default:
                    mycolor = Color.Black;
                    break;
            }
            return mycolor;
        }
        private Color ConvertAnsi(string light, string ansicode)
        {
            Color mycolor;
            //forground colors
            if (light == "1")
            {
                switch (ansicode)
                {
                    case "30":
                        mycolor = Color.DarkGray;
                        break;
                    case "31":
                        mycolor = Color.Red;
                        break;
                    case "32":
                        mycolor = Color.Green;
                        break;
                    case "33":
                        mycolor = Color.Yellow;
                        break;
                    case "34":
                        mycolor = Color.Blue;
                        break;
                    case "35":
                        mycolor = Color.Magenta;
                        break;
                    case "36":
                        mycolor = Color.Cyan;
                        break;
                    case "37":
                        mycolor = Color.White;
                        break;
                    default:
                        mycolor = Color.AntiqueWhite;
                        break;
                }
            }
            else
            {
                switch (ansicode)
                {
                    case "30":
                        mycolor = Color.Black;
                        break;
                    case "31":
                        mycolor = Color.DarkRed;
                        break;
                    case "32":
                        mycolor = Color.DarkGreen;
                        break;
                    case "33":
                        mycolor = Color.Goldenrod;
                        break;
                    case "34":
                        mycolor = Color.DarkBlue;
                        break;
                    case "35":
                        mycolor = Color.DarkMagenta;
                        break;
                    case "36":
                        mycolor = Color.DarkCyan;
                        break;
                    case "37":
                        mycolor = Color.LightGray;
                        break;
                    default:
                        mycolor = Color.AntiqueWhite;
                        break;
                }
            }
                return mycolor;
            }
    }
}