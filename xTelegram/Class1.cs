using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Td = Telegram.Td;
using TdApi = Telegram.Td.Api;
using System.Threading;

namespace xTelegram
{
    public static class tdEngine  
    {
        public static Td.Client _client = null;
        private readonly static Td.ClientResultHandler _defaultHandler = new DefaultHandler();

        public static TdApi.AuthorizationState _authorizationState = null;
        public static volatile bool _haveAuthorization = false;
        private static volatile bool _quiting = false;
        public static string strAuthCode = string.Empty;

        public static string strMessagae { get; set; }
        public static TdApi.BaseObject objReturned { get; set; }
        public static string strEngStatus = "None";

        private static volatile AutoResetEvent _gotAuthorization = new AutoResetEvent(false);

        private static readonly string _newLine = Environment.NewLine;
        private static readonly string _commandsLine = "Enter command (gc <chatId> - GetChat, me - GetMe, sm <chatId> <message> - SendMessage, lo - LogOut, r - Restart, q - Quit): ";
        private static volatile string _currentPrompt = null;
        
        public static void SendAuthCode()
        {
            _client.Send(new TdApi.CheckAuthenticationCode(strAuthCode, "", ""), new AuthorizationRequestHandler());
            strMessagae = "Authorizaton code sent";
        }

        public static void GeMessagesFromChat(long plngChatID, long plngFromMessageID, int pintOffset, int pintLimit )
        {
            strEngStatus = "getmessagesfromchat";
            _client.Send(new TdApi.GetChatHistory(plngChatID, plngFromMessageID,pintOffset,pintLimit,false), _defaultHandler);
            strEngStatus = "getmessagesfromchatsent";

        }
        public static void GetChatByID(long plngChartID)
        {
            strEngStatus = "getchatbyidentered";
            _client.Send(new TdApi.GetChat(GetChatId(plngChartID.ToString())), _defaultHandler);
            strEngStatus = "getchatbyidsent";

        }

        public static void GetChatList(long plngOffsetOrder, long plngOffsetChatID)
        {
            strMessagae = "Getting chat lists";
            if (tdEngine._haveAuthorization)
            {
                strEngStatus = "getchatsentered";
                tdEngine._client.Send(new TdApi.GetChats(plngOffsetOrder, plngOffsetChatID, 10), _defaultHandler); // preload chat list
                strEngStatus = "getchatssent";
            }
        }
       
        private static Td.Client CreateTdClient()
        {
            Td.Client result = Td.Client.Create(new UpdatesHandler());
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                result.Run();
            }).Start();
            return result;
        }

        private static void Print(string str)
        {
            if (_currentPrompt != null)
            {
                Console.WriteLine();
            }
            Console.WriteLine(str);
            if (_currentPrompt != null)
            {
                Console.Write(_currentPrompt);
            }
        }

        private static string ReadLine(string str)
        {
            Console.Write(str);
            _currentPrompt = str;
            var result = Console.ReadLine();
            _currentPrompt = null;
            return result;




        }

        public static void OnAuthorizationStateUpdated(TdApi.AuthorizationState authorizationState)
        {
            if (authorizationState != null)
            {
                _authorizationState = authorizationState;
            }
            if (_authorizationState is TdApi.AuthorizationStateWaitTdlibParameters)
            {
                TdApi.TdlibParameters parameters = new TdApi.TdlibParameters();
                parameters.DatabaseDirectory = "tdlib";
                parameters.UseMessageDatabase = true;
                parameters.UseSecretChats = true;
                parameters.ApiId = 94575;
                parameters.ApiHash = "a3406de8d171bb422bb6ddf3bbd800e2";
                parameters.SystemLanguageCode = "en";
                parameters.DeviceModel = "Desktop";
                parameters.SystemVersion = "Unknown";
                parameters.ApplicationVersion = "1.0";
                parameters.EnableStorageOptimizer = true;

                _client.Send(new TdApi.SetTdlibParameters(parameters), new AuthorizationRequestHandler());
            }
            else if (_authorizationState is TdApi.AuthorizationStateWaitEncryptionKey)
            {
                _client.Send(new TdApi.CheckDatabaseEncryptionKey(), new AuthorizationRequestHandler());
            }
            else if (_authorizationState is TdApi.AuthorizationStateWaitPhoneNumber)
            {
                //Need to send phone number automatically..
                string phoneNumber = "+919998823941";
                _client.Send(new TdApi.SetAuthenticationPhoneNumber(phoneNumber, false, false), new AuthorizationRequestHandler());
            }
            else if (_authorizationState is TdApi.AuthorizationStateWaitCode)
            {
                //Need to send authorization code
                //Wait for authorization code to be set by fornt ui for 30 sec
                strMessagae = "Enter Authorization Code";
                
                        if (strAuthCode != string.Empty)
                        {
                            string code = strAuthCode;
                            _client.Send(new TdApi.CheckAuthenticationCode(code, "", ""), new AuthorizationRequestHandler());
                            strEngStatus = "AuthorizationTokenSent";
                        }

                
            }
            else if (_authorizationState is TdApi.AuthorizationStateWaitPassword)
            {
                string password = ReadLine("Please enter password: ");
                _client.Send(new TdApi.CheckAuthenticationPassword(password), new AuthorizationRequestHandler());
            }
            else if (_authorizationState is TdApi.AuthorizationStateReady)
            {
                _haveAuthorization = true;
                _gotAuthorization.Set();
                strEngStatus = "AutorizationDone";
            }
            else if (_authorizationState is TdApi.AuthorizationStateLoggingOut)
            {
                _haveAuthorization = false;
                Print("Logging out");
            }
            else if (_authorizationState is TdApi.AuthorizationStateClosing)
            {
                _haveAuthorization = false;
                Print("Closing");
            }
            else if (_authorizationState is TdApi.AuthorizationStateClosed)
            {
                Print("Closed");
                _client.Dispose(); // _client is closed and native resources can be disposed now
                if (!_quiting)
                {
                    _client = CreateTdClient(); // recreate _client after previous has closed
                }
            }
            else
            {
                Print("Unsupported authorization state:" + _newLine + _authorizationState);
            }
        }

        private static long GetChatId(string arg)
        {
            long chatId = 0;
            try
            {
                chatId = Convert.ToInt64(arg);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return chatId;
        }

        private static void GetCommand()
        {
            string command = ReadLine(_commandsLine);
            string[] commands = command.Split(new char[] { ' ' }, 2);
            try
            {
                switch (commands[0])
                {
                    case "gc":
                        _client.Send(new TdApi.GetChat(GetChatId(commands[1])), _defaultHandler);
                        break;
                    case "me":
                        _client.Send(new TdApi.GetMe(), _defaultHandler);
                        break;
                    case "sm":
                        string[] args = commands[1].Split(new char[] { ' ' }, 2);
                        sendMessage(GetChatId(args[0]), args[1]);
                        break;
                    case "lo":
                        _haveAuthorization = false;
                        _client.Send(new TdApi.LogOut(), _defaultHandler);
                        break;
                    case "r":
                        _haveAuthorization = false;
                        _client.Send(new TdApi.Close(), _defaultHandler);
                        break;
                    case "q":
                        _quiting = true;
                        _haveAuthorization = false;
                        _client.Send(new TdApi.Close(), _defaultHandler);
                        break;
                    default:
                        Print("Unsupported command: " + command);
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Print("Not enough arguments");
            }
        }

        private static void sendMessage(long chatId, string message)
        {
            // initialize reply markup just for testing
            TdApi.InlineKeyboardButton[] row = { new TdApi.InlineKeyboardButton("https://telegram.org?1", new TdApi.InlineKeyboardButtonTypeUrl()), new TdApi.InlineKeyboardButton("https://telegram.org?2", new TdApi.InlineKeyboardButtonTypeUrl()), new TdApi.InlineKeyboardButton("https://telegram.org?3", new TdApi.InlineKeyboardButtonTypeUrl()) };
            TdApi.ReplyMarkup replyMarkup = new TdApi.ReplyMarkupInlineKeyboard(new TdApi.InlineKeyboardButton[][] { row, row, row });

            TdApi.InputMessageContent content = new TdApi.InputMessageText(new TdApi.FormattedText(message, null), false, true);
            _client.Send(new TdApi.SendMessage(chatId, 0, false, false, replyMarkup, content), _defaultHandler);
        }

        public static void StartEngine()
        {
            // disable TDLib log
            Td.Log.SetVerbosityLevel(0);
            if (!Td.Log.SetFilePath("tdlib.log"))
            {
                throw new System.IO.IOException("Write access to the current directory is required");
            }

            // create Td.Client
            _client = CreateTdClient();

            // test Client.Execute
            _defaultHandler.OnResult(_client.Execute(new TdApi.GetTextEntities("@telegram /test_command https://telegram.org telegram.me @gif @test")));

                _gotAuthorization.Reset();
                _gotAuthorization.WaitOne();

            strMessagae = "Waiting for Code";

        }

        private class DefaultHandler : Td.ClientResultHandler
        {
            void Td.ClientResultHandler.OnResult(TdApi.BaseObject @object)
            {
                if (strEngStatus == "getchatssent") { 
                    objReturned = @object;
                    strEngStatus = "getchatsreceived";
                }
                if (strEngStatus == "getchatbyidsent")
                {
                    objReturned = @object;
                    strEngStatus = "getchatbyidreceived";
                }
                if (strEngStatus == "getmessagesfromchatsent")
                {
                    objReturned = @object;
                    strEngStatus = "getmessagesfromchatrecevied";
                }


            }
        }

        private class UpdatesHandler : Td.ClientResultHandler
        {
            void Td.ClientResultHandler.OnResult(TdApi.BaseObject @object)
            {
                if (@object is TdApi.UpdateAuthorizationState)
                {
                    OnAuthorizationStateUpdated((@object as TdApi.UpdateAuthorizationState).AuthorizationState);
                }
                else
                {
                    // Print("Unsupported update: " + @object);
                }
            }
        }

        private class AuthorizationRequestHandler : Td.ClientResultHandler
        {
            void Td.ClientResultHandler.OnResult(TdApi.BaseObject @object)
            {
                if (@object is TdApi.Error)
                {
                    Print("Receive an error:" + _newLine + @object);
                    OnAuthorizationStateUpdated(null); // repeat last action
                }
                else
                {
                    // result is already received through UpdateAuthorizationState, nothing to do
                }
            }
        }
    }
}
