using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Td = Telegram.Td;
using TdApi = Telegram.Td.Api;
using System.Threading;
using System.Collections;
using TMDbLib;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;

namespace xTelegram
{

    public partial class MainFrom : Form
    {
        string strLastMessage = string.Empty;
        int iCnt1 = 0;
        int iCnt3 = 0;
        int iCnt6 = 0;
        //Globle Object List
        objChat[] gobjChats = new objChat[1000];
        long glngChatIndex = 0;
        long glngOffsetChatID = 0;
        long glngOffsetOrder = long.MaxValue;

        objMessage[] gobjMessages = new objMessage[1000];
        long glngMessageIndex = 0;

        //Create OMDB API Client
        TMDbLib.Client.TMDbClient objTMDBClient = new TMDbLib.Client.TMDbClient("ef1422d351866bc45e21c29a52f3e3d4");
        //TMDbLib.Client.TMDbClient objTMDBClient = new TMDbLib.Client.TMDbClient("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJlZjE0MjJkMzUxODY2YmM0NWUyMWMyOWE1MmYzZTNkNCIsInN1YiI6IjVjZGUyZjc3YzNhMzY4MGFkNWMxODdmNCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.68PapeEvaj1vDLX41CtA0n6QnjG9hguUUQvgNBXKw9o");
        SearchMovie objSearchResult = null;

        //Define data source for download grid
        System.Data.DataTable dtDownloads = new System.Data.DataTable();
        Dictionary<string, TdApi.File> lstDownloadFiles = new Dictionary<string, TdApi.File>();





        public MainFrom()
        {
            InitializeComponent();
        }

        private void MainFrom_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();
            tdEngine.StartEngine();
            showmessage("Started now");

            //Define datasource for donwload list
            dtDownloads.Columns.Add("ID", typeof(int));
            dtDownloads.Columns.Add("Name", typeof(string));
            dtDownloads.Columns.Add("Percent", typeof(double));

            dtDownloads.Rows.Add(new object[] { 1,"Test", 12.4 });
            radGridDownloads.DataSource = dtDownloads;



        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageLabel.Text = "Sending Logon Code..";
            tdEngine.strAuthCode = CodeTxt.Text;

        }

        public void showmessage(string pstr)
        {
            MessageLabel.Text = (tdEngine.strEngStatus);
            LogText.Text += pstr + Environment.NewLine;
            LogText.SelectionStart = LogText.Text.Length;
            lblChatCounter.Text = glngChatIndex.ToString();
            lblMessageCounter.Text = glngMessageIndex.ToString();
            this.Refresh();
            Application.DoEvents();

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (tdEngine.strMessagae != strLastMessage)
            {
                showmessage(tdEngine.strMessagae);
                strLastMessage = tdEngine.strMessagae;
            }
            if (tdEngine.strEngStatus != EngStatusLable.Text) { EngStatusLable.Text = tdEngine.strEngStatus; }

            if (tdEngine.strEngStatus == "AutorizationDone")
            {
                LoadChatsButton.Enabled = true;
            }

            //check update object
            if (tdEngine.objUpdated != null && tdEngine.objUpdated.Count > 0)
            {
                //We got some update here.
                TdApi.BaseObject objReturned = tdEngine.objUpdated.Dequeue();
                if (objReturned.GetType().FullName == "Telegram.Td.Api.UpdateFile" )
                {
                    Td.Api.UpdateFile objUpdateFile = (Td.Api.UpdateFile)objReturned;
                    dtDownloads.Select("id = " + objUpdateFile.File.Id.ToString())[0].SetField("percent",  (double) objUpdateFile.File.Local.DownloadedSize / (double) objUpdateFile.File.ExpectedSize);

                }
                else { 
                showmessage(objReturned.GetType().FullName);
                showmessage(objReturned.ToString());
                }
            }


        }

        private void Button2_Click(object sender, EventArgs e)
        {

            while (XGetChats())
            {
                MessageLabel.Text = "Getting chats";
            }
            Array.Resize(ref gobjChats, (int)glngChatIndex);
            this.radGridView1.DataSource = gobjChats;
            this.radGridView1.BestFitColumns();
            this.radGridView1.Refresh();
            this.Refresh();
            Application.DoEvents();


            MessageLabel.Text = "All Chat received";

        }


        private bool XGetChats()
        {
            tdEngine.GetChatList(glngOffsetOrder, glngOffsetChatID);
            while (tdEngine.strEngStatus != "getchatsreceived" && iCnt1 < 100) { iCnt1 += 1; System.Threading.Thread.Sleep(100); Application.DoEvents(); }
            if (tdEngine.strEngStatus == "getchatsreceived")
            {
                showmessage(tdEngine.objReturned.ToString());
                //Load chartids in grid
                //TdApi.Chats[] lobjChart = (TdApi.Chats)(tdEngine.objReturned);
                Telegram.Td.Api.Chats tdCharts = (Telegram.Td.Api.Chats)(tdEngine.objReturned);
                long[] tdchats = tdCharts.ChatIds;
                if (tdCharts.ChatIds.Length == 0)
                {

                    return false;
                }

                if (glngChatIndex + tdchats.Length > gobjChats.Length)
                {
                    //Need to resize array with preseve
                    Array.Resize(ref gobjChats, gobjChats.Length + 1000);
                }

                for (int i = 0; i < tdchats.Length; i++)
                {
                    gobjChats[glngChatIndex] = new objChat();
                    gobjChats[glngChatIndex].chatid = tdchats[i];
                    gobjChats[glngChatIndex].chattitle = "";
                    gobjChats[glngChatIndex].ChatType = "";
                    gobjChats[glngChatIndex].IsChannel = "";
                    XGetSingleChatDetails(glngChatIndex, gobjChats[glngChatIndex].chatid);
                    this.Refresh();
                    Application.DoEvents();

                    glngChatIndex++;
                }
                glngOffsetOrder = gobjChats[glngChatIndex - 1].ChatOrder;
                glngOffsetChatID = gobjChats[glngChatIndex - 1].chatid;

                //TODEL:
                if (glngChatIndex > 20) { return false; }

            }
            else { showmessage("Plese resent, fail to get authorization"); return false; }
            return true;
        }

        private bool XGetSingleChatDetails(long plngChatIndex, long plngChatID)
        {

            tdEngine.GetChatByID(plngChatID);
            MessageLabel.Text = "Receiving Chat names for chatID " + plngChatID.ToString();
            while (tdEngine.strEngStatus != "getchatbyidreceived" && iCnt3 < 1000) { iCnt3 += 1; System.Threading.Thread.Sleep(100); Application.DoEvents(); }
            if (tdEngine.strEngStatus == "getchatbyidreceived")
            {
                showmessage(tdEngine.objReturned.ToString());
                Telegram.Td.Api.Chat tdChat = (Telegram.Td.Api.Chat)(tdEngine.objReturned);
                gobjChats[plngChatIndex].chattitle = tdChat.Title;
                gobjChats[plngChatIndex].ChatOrder = tdChat.Order;

                switch (tdChat.Type.GetType().FullName)
                {
                    case "Telegram.Td.Api.ChatTypeSupergroup":
                        gobjChats[plngChatIndex].ChatType = "SuperGroup";
                        break;
                    default:
                        break;
                }
                //EngStatusLable.Text = "Getting data for chat " + iCnt2.ToString() + " ID:" + list[iCnt2].chatid.ToString();
                return true;
            }
            else
            {
                showmessage("Could not get chat details");
                return false;
            }
        }

        private void RadGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
           
        }


        private void RadGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                long llngFromMessageID = 0;
                int lintOffset = 0;
                int lintLimit = 100;

                gobjMessages = new objMessage[1000];
                glngMessageIndex = 0;

                while (XGetMessages((long)e.Value, llngFromMessageID, lintOffset, lintLimit)  && glngMessageIndex < 100)
                {

                    llngFromMessageID = gobjMessages[glngMessageIndex - 1].MessageID;
                    //lintOffset = (int)glngMessageIndex;
                    showmessage("Getting messages" + glngMessageIndex.ToString());
                    radGridView2.DataSource = null;
                    radGridView2.DataSource = gobjMessages;
                    //radGridView2.MasterTemplate.Refresh();
                    radGridView2.BestFitColumns(Telerik.WinControls.UI.BestFitColumnMode.AllCells);
                    radGridView2.Refresh();
                    this.Refresh();
                    Application.DoEvents();

                }
                showmessage("Loading completed");

            }
            catch (Exception ex)
            {
                showmessage(ex.StackTrace);
            }
        }

        private bool XGetMessages(long plngChatID, long plngFromMessageID, int pintOffset, int pintLimit)
        {

            tdEngine.GeMessagesFromChat(plngChatID, plngFromMessageID, pintOffset, pintLimit);
            MessageLabel.Text = "Receiving messages from chatID " + plngChatID;
            while (tdEngine.strEngStatus != "getmessagesfromchatrecevied" && iCnt3 < 1000) { iCnt3 += 1; System.Threading.Thread.Sleep(100); Application.DoEvents(); }
            if (tdEngine.strEngStatus == "getmessagesfromchatrecevied")
            {
                showmessage(tdEngine.objReturned.ToString());
                Telegram.Td.Api.Messages tdMessages = (Telegram.Td.Api.Messages)(tdEngine.objReturned);
                if (tdMessages.MessagesValue.Length == 0) {return false;}
                if (glngMessageIndex + tdMessages.TotalCount > gobjMessages.Length)
                {
                    //Need to resize array with preseve
                    Array.Resize(ref gobjChats, gobjMessages.Length + 1000);
                }

                for (int iCnt5 = 0; iCnt5 < tdMessages.TotalCount; iCnt5++)
                {
                    gobjMessages[glngMessageIndex] = new objMessage();
                    gobjMessages[glngMessageIndex].MessageID = tdMessages.MessagesValue[iCnt5].Id;
                    
                    switch(tdMessages.MessagesValue[iCnt5].Content.GetType().FullName)
                    {
                        case "Telegram.Td.Api.MessageDocument":
                            Telegram.Td.Api.MessageDocument lobjMessageDocument = (Telegram.Td.Api.MessageDocument)tdMessages.MessagesValue[iCnt5].Content;
                            if (lobjMessageDocument.Document.MimeType == "video/x-matroska" | lobjMessageDocument.Document.MimeType == "video/mp4") {
                                gobjMessages[glngMessageIndex].MessageType = "Movie";
                                gobjMessages[glngMessageIndex].MessageTitle = lobjMessageDocument.Document.FileName;
                                gobjMessages[glngMessageIndex].FileID = lobjMessageDocument.Document.DocumentValue.Id;
                            }
                            else {
                                gobjMessages[glngMessageIndex].MessageType = lobjMessageDocument.Document.MimeType;
                                gobjMessages[glngMessageIndex].MessageTitle = lobjMessageDocument.Caption.Text;
                            }
                            glngMessageIndex++;
                            break;
                        case "Telegram.Td.Api.MessageText":
                            if (chkLoadAll.Checked)
                            {
                                Telegram.Td.Api.MessageText lobjMessageText = (Telegram.Td.Api.MessageText)tdMessages.MessagesValue[iCnt5].Content;
                                gobjMessages[glngMessageIndex].MessageType = "Text";
                                gobjMessages[glngMessageIndex].MessageTitle = lobjMessageText.Text.Text;
                            }
                            break;
                        case "Telegram.Td.Api.MessagePhoto":
                            if (chkLoadAll.Checked)
                            {
                                gobjMessages[glngMessageIndex].MessageType = "Photo";
                                Telegram.Td.Api.MessagePhoto lobjMessagePhoto = (Telegram.Td.Api.MessagePhoto)tdMessages.MessagesValue[iCnt5].Content;
                                gobjMessages[glngMessageIndex].MessageTitle = lobjMessagePhoto.Caption.Text;
                            }
                                break;
                        default:
                            if (chkLoadUnknown.Checked)
                            {

                                gobjMessages[glngMessageIndex].MessageType = "Unknown";
                                gobjMessages[glngMessageIndex].MessageTitle = tdMessages.MessagesValue[iCnt5].Content.ToString();
                                showmessage(tdMessages.MessagesValue[iCnt5].Content.GetType().FullName + "Not supported yet");
                            }
                                break;

                    }

                   

                }
                return true;
            }
            else
            {
                showmessage("Could not get chat details");
                return false;
            }




        }

        private bool GetOMDBResult(string pstrMovieName, ref SearchMovie pSearchMovieOut)
        {
            try
            {
                pstrMovieName=pstrMovieName.TrimEnd();
                SearchContainer<SearchMovie> results =  objTMDBClient.SearchMovieAsync(pstrMovieName).Result;


                //If error will catch in try block so here we will get searchlist
                if (results.Results.Count > 0)
                {
                    foreach (SearchMovie item in results.Results)
                    {
                        showmessage("Title:" + item.Title + " Type:" + item.MediaType + " IMDBID: " + item.Id);
                    }
                    pSearchMovieOut = results.Results.FirstOrDefault(t => t.Title.ToLower().Contains(pstrMovieName));
                    //pSearchMovieOut = results.Results.First();
                    return true;

                }
                else {return false;}

            }
            catch (Exception ex)
            {
                showmessage(ex.Message);
                return false;
            }
        }

        private void GetMoviewInfoFromTMDB(string pstrMovieName)
        {
            //We will get name of document. We have to find exact movie
            string lstrCell = pstrMovieName;
            //Remove filename
            lstrCell = lstrCell.Substring(0, lstrCell.Length - 3).ToLower();
            showmessage("Finding string : " + lstrCell);
            if (GetOMDBResult(lstrCell, ref objSearchResult)) { return; }

            lstrCell = lstrCell.Replace(".", " ");
            showmessage("Finding string : " + lstrCell);
            if (GetOMDBResult(lstrCell, ref objSearchResult)) { return; }

            lstrCell = lstrCell.Replace("720p", "").Replace("HDRip", "").Replace("BRRip", "").Replace("(", "").Replace(")", "");
            showmessage("Finding string : " + lstrCell);
            if (GetOMDBResult(lstrCell, ref objSearchResult)) { return; }

            //Remove word starting with @ or containing @
            string[] strChars = lstrCell.Split(' ');
            string strNewCell = string.Empty;
            foreach (var item in strChars)
            {
                if (!item.All(char.IsNumber) && !item.Contains("@") && (!item.Contains("audio")) && (!item.Contains("rip"))) { strNewCell += item + " "; }
            }
            strNewCell.Substring(1, (strNewCell.Length - 1));
            showmessage("Finding string : " + strNewCell);
            if (GetOMDBResult(strNewCell, ref objSearchResult)) { return; }

            //try to remove from back side
            strNewCell = string.Empty;
            for (int iCnt8 = strChars.Length - 1; iCnt8 > 0; iCnt8--)
            {
                strNewCell = string.Empty;
                for (int iCnt9 = 0; iCnt9 < iCnt8; iCnt9++)
                {
                    strNewCell += strChars[iCnt9] + " ";
                }
                strNewCell.Substring(1, (strNewCell.Length - 1));
                showmessage("Finding string : " + strNewCell);
                if (GetOMDBResult(strNewCell, ref objSearchResult)) { return; }
            }
        }

            private void RadGridView2_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                showmessage("Clicked on column index : " + e.ColumnIndex.ToString());
                objSearchResult = null; //Reset search result
                switch (e.ColumnIndex)
                {
                    case 0: //messageid
                        break;
                    case 1: //Type
                        break;
                    case 2: // Name
                        GetMoviewInfoFromTMDB(e.Value.ToString());
                        if (objSearchResult != null)
                        {
                            showmessage("Found movie as final : " + objSearchResult.Title);
                        }
                        break;
                    case 3: // fileid
                        tdEngine.DownloadFile((int)e.Value);
                        MessageLabel.Text = "Downloading file " + e.Value.ToString();
                        while (tdEngine.strEngStatus != "downloadfilereceived" && iCnt6 < 1000) { iCnt6 += 1; System.Threading.Thread.Sleep(100); Application.DoEvents(); }
                        if (tdEngine.strEngStatus == "downloadfilereceived")
                        {
                            //dosomething on received file...
                            TdApi.BaseObject objReturned = tdEngine.objReturned;
                            if (objReturned.GetType().FullName == "Telegram.Td.Api.File")
                            {
                                //Cast object to file
                                Td.Api.File objTdApiFile = (Td.Api.File)objReturned;
                                //Add to list of loadloading file
                                lstDownloadFiles.Add(objTdApiFile.Id.ToString(),objTdApiFile);
                                //Create raw in grid
                                dtDownloads.Rows.Add(new object[] { objTdApiFile.Id, radGridView2.Rows[e.RowIndex].Cells[2].Value ,0.0 });
                                
                            }
                            showmessage(objReturned.GetType().FullName);
                            showmessage(objReturned.ToString());
                        }
                        break;
                    default:
                        break;
                }

               

                

             }
            catch (Exception ex)
            {
                showmessage(ex.StackTrace);
            }
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            LogText.Clear();
        }

        private void RadGridView2_Click(object sender, EventArgs e)
        {

        }
    }

 

    public class objChat
    {
        public long chatid { get; set; }
        public string chattitle { get; set; }
        public string ChatType { get; set; }
        public string IsChannel { get; set; }
        public long ChatOrder { get; set; }
        public Dictionary<long, Td.Api.Chat> tdchats { get; set; }

    }

    public class objMessage
    {
        public long MessageID { get; set; }
        public string MessageType { get; set; }
        public string MessageTitle { get; set; }
        public int FileID { get; set; }
    }

    public class ValueType<T>
    {
        T item;
        public ValueType() { }
        public ValueType(T item)
        {
            this.item = item;
        }
        public T ItemProperty
        {
            get { return this.item; }
            set { this.item = value; }
        }
    }

}
