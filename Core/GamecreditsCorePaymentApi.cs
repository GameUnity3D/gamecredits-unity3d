/* Gamecredits Cryptocurrency Gaming Network Unity3D Payment Plugin
 * https://gamecredits.com or https://gamecredits.org
 * 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * © Gametoschi Minato-ku, Tokyo, Japan
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ZXing;
using ZXing.QrCode;

namespace GamecreditsGaming.QT
{
    public enum _AddNodeOption { Add, Remove, OneTry };

    public class GamecreditsCorePaymentApi : MonoBehaviour
    {
        public bool useDebug = true;//Enable debug logs
        public JObject ResponseObject;//Just to hold the last object
        public JObject CoinMarketCapData;//Json Objects for market data
        public const string CoinMarketCap = "https://api.coinmarketcap.com/v2/ticker/";
        private string URI_;
        private string User_;
        private string Pass_;
        private int Port_;
        private bool MarketInitiated = false;
        public void QTInitialize(string URI, string RPCUser, string RPCPassword, int RPCPort)
        {
            URI_ = URI;
            User_ = RPCUser;
            Pass_ = RPCPassword;
            Port_ = RPCPort;
        }

        public void TestConnection(){StartCoroutine(IE_TestConnection());}

        IEnumerator IE_TestConnection()
        {
            yield return StartCoroutine(SendInvokeMethod("ping"));
            if (ResponseObject["result"].ToString() == "Success")
            {
                if (useDebug) Debug.Log("Server is reachable");
            }

            if(ResponseObject["data"].ToString()== "Empty reply from server")
            {
                if(useDebug)
                {
                    Debug.LogError("Communication with your wallet return an empty string. If you are confident that your login credential is correct then check the wallet .config file.");
                }
            }
        }

        #region Delegates and Events
        public delegate void OnCompleteVoid(JObject json);
        public delegate string OnCompleteString(JObject json);

        public delegate void _SendRawTransactionCompleteComplete(JObject json);
        public event _SendRawTransactionCompleteComplete qtSendRawTransactionComplete;

        public delegate void _GetInfoComplete(JObject json);
        public event _GetInfoComplete qtGetInfoComplete;

        public delegate void _MoveComplete(JObject json);
        public event _MoveComplete qtMoveComplete;

        public delegate void _GetRawChangedAddress(JObject json);
        public event _GetRawChangedAddress qtGetRawChangedAddress;

        public delegate void _GetRawTransaction(JObject json);
        public event _GetRawTransaction qtGetRawTransactionComplete;

        public delegate void _GetTxOut(JObject json);
        public event _GetTxOut qtGetTxOutComplete;

        public delegate void _Help(JObject json);
        public event _Help qtHelpComplete;

        public delegate void _InvalidateBlock(JObject json);
        public event _InvalidateBlock qtInvalidateBlockComplete;

        public delegate void _BackupWallet(JObject json);
        public event _BackupWallet qtBackupWalletComplete;

        public delegate void _EncryptWallet(JObject json);
        public event _EncryptWallet qtEncryptWalletComplete;

        public delegate void _GetAccount(JObject json);
        public event _GetAccount qtGetAccountComplete;

        public delegate void _GetAccountAddress(JObject json);
        public event _GetAccountAddress qtGetAccountAddressComplete;

        public delegate void _GetBalance(JObject json);
        public event _GetBalance qtGetBalanceComplete;

        public delegate void _GetAddressByAccount(JObject json);
        public event _GetAddressByAccount qtGetAddressByAccountComplete;

        public delegate void _GetRecieveByAccount(JObject json);
        public event _GetRecieveByAccount qtGetRecieveByAccountComplete;

        public delegate void _GetRecieveByAddress(JObject json);
        public event _GetRecieveByAddress qtGetRecieveByAddressComplete;

        public delegate void _GetTransaction(JObject json);
        public event _GetTransaction qtGetTransactionComplete;

        public delegate void _ListRecieveByAccount(JObject json);
        public event _ListRecieveByAccount qtListRecieveByAccountComplete;

        public delegate void _ListRecieveByAddress(JObject json);
        public event _ListRecieveByAddress qtListRecieveByAddressComplete;

        public delegate void _ListTransaction(JObject json);
        public event _ListTransaction qtListTransactionComplete;

        public delegate void _ListUnSpent(JObject json);
        public event _ListUnSpent qtListUnSpentComplete;

        public delegate void _SetAccount(JObject json);
        public event _SetAccount qtSetAccountComplete;

        public delegate void _SetTXfees(JObject json);
        public event _SetTXfees qtSetTXfeesComplete;

        public delegate void _VarifyMessage(JObject json);
        public event _VarifyMessage qtVarifyMessageComplete;

        public delegate void _ValidateAddress(JObject json);
        public event _ValidateAddress qtValidateAddressComplete;

        public delegate void _WalletPassphraseChange(JObject json);
        public event _WalletPassphraseChange qtWalletPassphraseChangeComplete;

        public delegate void _WalletPassphrase(JObject json);
        public event _WalletPassphrase qtWalletPassphraseComplete;

        public delegate void _WalletLock(JObject json);
        public event _WalletLock qtWalletLockComplete;

        public delegate void _Stop(JObject json);
        public event _Stop qtStopComplete;

        public delegate void _ListAddressGrouping(JObject json);
        public event _ListAddressGrouping qtListAddressGroupingComplete;

        public delegate void _ListAccounts(JObject json);
        public event _ListAccounts qtListAccountsComplete;

        public delegate void _GetTXoutSetInfo(JObject json);
        public event _GetTXoutSetInfo qtGetTXoutSetInfoComplete;

        public delegate void _GetRawMemPool(JObject json);
        public event _GetRawMemPool qtGetRawMemPoolComplete;

        public delegate void _GetBestBlockHash(JObject json);
        public event _GetBestBlockHash qtGetBestBlockHashComplete;

        public delegate void _GetBlockCount(JObject json);
        public event _GetBlockCount qtGetBlockCountComplete;

        public delegate void _GetHashesPerSec(JObject json);
        public event _GetHashesPerSec qtGetHashesPerSecComplete;

        public delegate void _GetGenerate(JObject json);
        public event _GetGenerate qtGetGenerateComplete;

        public delegate void _GetDifficulty(JObject json);
        public event _GetDifficulty qtGetDifficultyComplete;

        public delegate void _GetConnectionCount(JObject json);
        public event _GetConnectionCount qtGetConnectionCountComplete;

        public delegate void _GetMiningInfo(JObject json);
        public event _GetMiningInfo qtGetMiningInfoComplete;

        public delegate void _GetPeerInfo(JObject json);
        public event _GetPeerInfo qtGetPeerInfoComplete;

        public delegate void _GetNewAddress(JObject json);
        public event _GetNewAddress qtGetNewAddressComplete;

        public delegate void _GetNetworkInfo(JObject json);
        public event _GetNetworkInfo qtGetNetworkInfoComplete;

        public delegate void _AddMultiSigAddress(JObject json);
        public event _AddMultiSigAddress qtAddMultiSigAddressComplete;

        public delegate void _AddNode(JObject json);
        public event _AddNode qtAddNodeComplete;

        public delegate void _UpdateMarket(JObject json);
        public event _UpdateMarket qtUpdateMarketComplete;
        #endregion
        #region Commands
        public void SendRawTransaction(string HexString) { StartCoroutine(IE_SendRawTransaction(HexString, qtSendRawTransactionComplete)); }
        public void GetInfo() { StartCoroutine(IE_Getinfo(qtGetInfoComplete)); }
        public void Help() { StartCoroutine(IE_Help(qtHelpComplete)); }
        public void Help(string Command) { StartCoroutine(IE_Help(Command,qtHelpComplete)); }
        public void Move(string FromWalletAccount, string ToWalletAccount, float Amount, int MinimumConfirmation, string Comments){StartCoroutine(IE_Move(FromWalletAccount, ToWalletAccount, Amount, MinimumConfirmation, Comments, qtMoveComplete));}
        public void GetRawChangedAddress(string WalletAccount) { StartCoroutine(IE_Getrawchangeaddress(WalletAccount, qtGetRawChangedAddress));}
        public void GetRawTransaction(string TXid) { StartCoroutine(IE_Getrawtransaction(TXid,qtGetRawTransactionComplete)); }
        public void GetTxOut(string TXid,int Amount,bool IncludeMemPool) { StartCoroutine(IE_Gettxout(TXid, Amount, IncludeMemPool, qtGetTxOutComplete)); }
        public void InvalidateBlock(string Hash) {StartCoroutine(IE_Invalidateblock(Hash,qtInvalidateBlockComplete)); }
        public void BackupWallet(string Destination) {StartCoroutine(IE_Backupwallet(Destination,qtBackupWalletComplete)); }
        public void EncryptWallet(string PassPhrase) {StartCoroutine(IE_Encryptwallet(PassPhrase,qtEncryptWalletComplete)); }
        public void GetAccount(string WalletAccount) {StartCoroutine(IE_Getaccount(WalletAccount,qtGetAccountComplete));}
        public void GetAccountaddress(string WalletAccountOrLabel) {StartCoroutine(IE_Getaccountaddress(WalletAccountOrLabel,qtGetAccountAddressComplete)); }
        public void GetBalance() {StartCoroutine(IE_Getbalance(qtGetBalanceComplete)); }
        public void GetBalance(string WalletAccount) {StartCoroutine(IE_Getbalance(WalletAccount,qtGetBalanceComplete)); }
        public void GetAddressesByAccount(string WalletAccount) {StartCoroutine(IE_Getaddressesbyaccount(WalletAccount,qtGetAddressByAccountComplete)); }
        public void GetreceivedByAccount(string WalletAccount) {StartCoroutine(IE_Getreceivedbyaccount(WalletAccount,qtGetRecieveByAccountComplete)); }
        public void GetreceivedByAddress(string WalletAddress) {StartCoroutine(IE_Getreceivedbyaddress(WalletAddress,qtGetRecieveByAddressComplete)); }
        public void GetReceivedByAddress(string WalletAddress) {StartCoroutine(IE_Getreceivedbyaddress(WalletAddress, qtGetRecieveByAddressComplete)); }
        public void GetTransaction(string TxID) {StartCoroutine(IE_Gettransaction(TxID, qtGetTransactionComplete)); }
        public void ListReceivedByAccount(int MinimumConfirmation, bool ListEmptyAddress) { StartCoroutine(IE_Listreceivedbyaccount(MinimumConfirmation, ListEmptyAddress, qtListRecieveByAccountComplete)); }
        public void ListReceivedByAddress(int MinimumConfirmation, bool ListEmptyAddress) {StartCoroutine(IE_Listreceivedbyaddress(MinimumConfirmation, ListEmptyAddress,qtListRecieveByAddressComplete)); }
        public void ListTransactions(string WalletAddress) {StartCoroutine(IE_Listtransactions(WalletAddress,qtListTransactionComplete)); }
        public void ListTransactions() {StartCoroutine(IE_Listtransactions(qtListTransactionComplete)); }
        public void ListunSpent(int Min, int Max) {StartCoroutine(IE_Listunspent(Min, Max,qtListUnSpentComplete)); }
        public void SetAccount(string WalletAddress, string DeviceID) {StartCoroutine(IE_Setaccount( WalletAddress, DeviceID,qtSetAccountComplete)); }
        public void SetTXfee(float Amount) {StartCoroutine(IE_Settxfee(Amount,qtSetTXfeesComplete)); }
        public void VerifyMessage(string WalletAddress, string Signature, string Message) {StartCoroutine(IE_Verifymessage(WalletAddress, Signature, Message,qtVarifyMessageComplete)); }
        public void ValidateAddress(string WalletAddress) {StartCoroutine(IE_Validateaddress(WalletAddress,qtValidateAddressComplete)); }
        public void WalletPassphrasechange(string OldPassPhrase, string NewPassPhrase) {StartCoroutine(IE_Walletpassphrasechange(OldPassPhrase, NewPassPhrase,qtWalletPassphraseChangeComplete)); }
        public void WalletPassphrase(string PassPhrase, float TimeOut) {StartCoroutine(IE_Walletpassphrase(PassPhrase, TimeOut,qtWalletPassphraseComplete)); }
        public void WalletLock() {StartCoroutine(IE_Walletlock(qtWalletLockComplete)); }
        public void Stop() {StartCoroutine(IE_Stop(qtStopComplete)); }
        public void ListAddressgroupings() {StartCoroutine(IE_Listaddressgroupings(qtListAddressGroupingComplete)); }
        public void ListAccounts(int Amount) { StartCoroutine(IE_Listaccounts(Amount,qtListAccountsComplete)); }
        public void GetTXoutSetInfo() {StartCoroutine(IE_Gettxoutsetinfo(qtGetTXoutSetInfoComplete)); }
        public void GetRawMemPool() {StartCoroutine(IE_Getrawmempool(qtGetRawMemPoolComplete)); }
        public void GetBestBlockHash() {StartCoroutine(IE_Getbestblockhash(qtGetBestBlockHashComplete)); }
        public void GetBlockCount() {StartCoroutine(IE_Getblockcount(qtGetBlockCountComplete)); }
        public void GetJashesPerSec() {StartCoroutine(IE_Gethashespersec(qtGetHashesPerSecComplete)); }
        public void GetGenerate() {StartCoroutine(IE_Getgenerate(qtGetGenerateComplete)); }
        public void GetDifficulty() {StartCoroutine(IE_Getdifficulty(qtGetDifficultyComplete)); }
        public void GetConnectionCount() {StartCoroutine(IE_Getconnectioncount(qtGetConnectionCountComplete)); }
        public void GetMiningInfo() {StartCoroutine(IE_Getmininginfo(qtGetMiningInfoComplete)); }
        public void GetPeerInfo() {StartCoroutine(IE_Getpeerinfo(qtGetPeerInfoComplete)); }
        public void GetNewAddress(string UseSomethingUniqueLikeDeviceID) {StartCoroutine(IE_Getnewaddress(UseSomethingUniqueLikeDeviceID,qtGetNewAddressComplete)); }
        public void GetNewAddress() { StartCoroutine(IE_Getnewaddress(qtGetNewAddressComplete)); }
        public void GetNetworkInfo() {StartCoroutine(IE_Getnetworkinfo(qtGetNetworkInfoComplete)); }
        public void AddMultiSigAddress(Dictionary<string, string> ListOfAddresses) {StartCoroutine(IE_Addmultisigaddress(ListOfAddresses,qtAddMultiSigAddressComplete)); }
        public void AddNode(string NodeIPAndPort, _AddNodeOption addNodeOption) { StartCoroutine(IE_Addnode(NodeIPAndPort, addNodeOption, qtAddNodeComplete)); }
        public void RetrieveMarketData(string TickerID = "gamecredits",string ConverCurrancySymbol = "USD", bool AutoRefresh = true,int UpdateEverySeconds = 120){StartCoroutine(IE_UpdateCoinMarket(TickerID,ConverCurrancySymbol,AutoRefresh,UpdateEverySeconds,qtUpdateMarketComplete));}
        public void CancelMarketData() { StopCoroutine("IE_UpdateCoinMarket"); MarketInitiated = false; }
        #endregion

        //List of commands that does not need the wallet to be unlock
        IEnumerator IE_Getinfo(_GetInfoComplete SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getinfo")); if (qtGetInfoComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_SendRawTransaction(string HexString, _SendRawTransactionCompleteComplete SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("sendrawtransaction", HexString)); if(qtSendRawTransactionComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Help(_Help SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("help")); if(qtHelpComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Help(string HelpCommand, _Help SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("help", HelpCommand)); if(qtHelpComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Move(string FromWalletAccount, string ToWalletAccount, float Amount, int MinimumConfirmation, string Comments, _MoveComplete SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("move", FromWalletAccount, ToWalletAccount, Amount, MinimumConfirmation, Comments)); if(qtMoveComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getrawchangeaddress(string WalletAccount, _GetRawChangedAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getrawchangeaddress", WalletAccount)); if(qtGetRawChangedAddress != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getrawtransaction(string TXid, _GetRawTransaction SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getrawtransaction", 0)); if(qtGetRawTransactionComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Gettxout(string TxID, int Amount, bool IncludeMemPool, _GetTxOut SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("gettxout", TxID, Amount, IncludeMemPool)); if(qtGetTxOutComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Invalidateblock(string Hash, _InvalidateBlock SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("invalidateblock", Hash)); if (qtInvalidateBlockComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Backupwallet(string Destination, _BackupWallet SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("backupwallet", Destination)); if (qtBackupWalletComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Encryptwallet(string PassPhrase, _EncryptWallet SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("encryptwallet", PassPhrase)); if (qtEncryptWalletComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getaccount(string WalletAccount, _GetAccount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getaccount", WalletAccount)); if (qtGetAccountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getaccountaddress(string WalletAccountOrLabel, _GetAccountAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getaccountaddress", WalletAccountOrLabel)); if (qtGetAccountAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getbalance(_GetBalance SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getbalance")); if (qtGetBalanceComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getbalance(string WalletAccount, _GetBalance SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getbalance", WalletAccount)); if (qtGetBalanceComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getaddressesbyaccount(string WalletAccount, _GetAddressByAccount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getaddressesbyaccount", WalletAccount)); if (qtGetAddressByAccountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getreceivedbyaccount(string WalletAccount, _GetRecieveByAccount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getreceivedbyaccount", WalletAccount)); if (qtGetRecieveByAccountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getreceivedbyaddress(string WalletAddress, _GetRecieveByAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getreceivedbyaddress", WalletAddress)); if (qtGetRecieveByAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Gettransaction(string TxID, _GetTransaction SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("gettransaction", TxID)); if (qtGetTransactionComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listreceivedbyaccount(int MinimumConfirmation, bool ListEmptyAddress, _ListRecieveByAccount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listreceivedbyaccount", MinimumConfirmation, ListEmptyAddress)); if (qtListRecieveByAccountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listreceivedbyaddress(int MinimumConfirmation, bool ListEmptyAddress, _ListRecieveByAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listreceivedbyaddress", MinimumConfirmation, ListEmptyAddress)); if (qtListRecieveByAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listtransactions(string WalletAddress, _ListTransaction SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listtransactions", WalletAddress)); if (qtListTransactionComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listtransactions(_ListTransaction SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listtransactions")); if (qtListTransactionComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listunspent(int Min, int Max, _ListUnSpent SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listunspent", Min, Max)); if (qtListUnSpentComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Setaccount(string WalletAddress, string DeviceID, _SetAccount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("setaccount", WalletAddress, DeviceID)); if (qtSetAccountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Settxfee(float Amount, _SetTXfees SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("settxfee", Amount)); if (qtSetTXfeesComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Verifymessage(string WalletAddress, string Signature, string Message, _VarifyMessage SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("verifymessage", WalletAddress, Signature, Message)); if (qtVarifyMessageComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Validateaddress(string WalletAddress, _ValidateAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("validateaddress", WalletAddress)); if (qtValidateAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Walletpassphrasechange(string OldPassPhrase, string NewPassPhrase, _WalletPassphraseChange SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("walletpassphrasechange", OldPassPhrase, NewPassPhrase)); if (qtWalletPassphraseChangeComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Walletpassphrase(string PassPhrase, float TimeOut, _WalletPassphrase SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("walletpassphrase", PassPhrase, TimeOut)); if (qtWalletPassphraseComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Walletlock(_WalletLock SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("walletlock")); if (qtWalletLockComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Stop(_Stop SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("stop")); if (qtStopComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listaddressgroupings(_ListAddressGrouping SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listaddressgroupings")); if (qtListAddressGroupingComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Listaccounts(int Amount, _ListAccounts SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("listaccounts", Amount)); if (qtListAccountsComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Gettxoutsetinfo(_GetTXoutSetInfo SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("gettxoutsetinfo")); if (qtGetTXoutSetInfoComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getrawmempool(_GetRawMemPool SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getrawmempool")); if (qtGetRawMemPoolComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getbestblockhash(_GetBestBlockHash SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getbestblockhash")); if (qtGetBestBlockHashComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getblockcount(_GetBlockCount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getblockcount")); if (qtGetBlockCountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Gethashespersec(_GetHashesPerSec SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("gethashespersec")); if (qtGetHashesPerSecComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getgenerate(_GetGenerate SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getgenerate")); if (qtGetGenerateComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getdifficulty(_GetDifficulty SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getdifficulty")); if (qtGetDifficultyComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getconnectioncount(_GetConnectionCount SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getconnectioncount")); if (qtGetConnectionCountComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getmininginfo(_GetMiningInfo SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getmininginfo")); if (qtGetMiningInfoComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getpeerinfo(_GetPeerInfo SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getpeerinfo")); if (qtGetPeerInfoComplete != null) SendOnComplete(ResponseObject); }                                                    
        IEnumerator IE_Getnewaddress(string UseSomethingUniqueLikeDeviceID, _GetNewAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getnewaddress", UseSomethingUniqueLikeDeviceID)); if (qtGetNewAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getnewaddress(_GetNewAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getnewaddress")); if (qtGetNewAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Getnetworkinfo(_GetNetworkInfo SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("getnetworkinfo")); if (qtGetNetworkInfoComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Addmultisigaddress(Dictionary<string, string> ListOfAddresses, _AddMultiSigAddress SendOnComplete) { yield return StartCoroutine(SendInvokeMethod("addmultisigaddress", ListOfAddresses)); if (qtAddMultiSigAddressComplete != null) SendOnComplete(ResponseObject); }
        IEnumerator IE_Addnode(string NodeIPAndPort, _AddNodeOption addNodeOption, _AddNode SendOnComplete)
        {
            string s = "";

            switch (addNodeOption)
            {
                case _AddNodeOption.Add:
                    s = "add";
                    break;
                case _AddNodeOption.Remove:
                    s = "remove";
                    break;
                case _AddNodeOption.OneTry:
                    s = "onetry";
                    break;
                default:
                    s = "add";
                    break;
            }

            yield return StartCoroutine(SendInvokeMethod("addnode", NodeIPAndPort, s));
            if (qtAddNodeComplete != null) SendOnComplete(ResponseObject);
        }

        public void CancelCommand() { this.StopAllCoroutines(); }

        IEnumerator IE_UpdateCoinMarket(string TickerID,string ConvertCurrancySymbol, bool AutoUpdate,int EverySec, _UpdateMarket SendOnComplete)
        {
            //Safey flag to prevent multiple calls
            if(!MarketInitiated)
            {
                MarketInitiated = true;
                while (MarketInitiated)
                {
                    if (EverySec <= 119) { Debug.LogError("You are not allowed to update this function in less than 2 minutes. I've force your update to every 2 minutes. It's to prevent your IP from being ban as a false DDOS attack. You should however edit EverySec timer."); EverySec = 120; }

                    UnityWebRequest webrequest = new UnityWebRequest();
                    string ConcatValue = CoinMarketCap + TickerID + "/?convert=" + ConvertCurrancySymbol;

                    if (useDebug) Debug.Log(ConcatValue);

                    webrequest = UnityWebRequest.Get(ConcatValue);
                    CoinMarketCapData = null;
                    webrequest.SendWebRequest();
                    //webrequest.timeout = 30;

                    //yield return webrequest;
                    while (!webrequest.isDone) yield return null;

                    DownloadHandler GetResult = webrequest.downloadHandler;
                    Debug.Log("StringValue:" + GetResult.text);
                    CoinMarketCapData = JsonConvert.DeserializeObject<JObject>(GetResult.text);
                    webrequest.Dispose();
                    GetResult.Dispose();

                    if (qtUpdateMarketComplete != null) SendOnComplete(CoinMarketCapData);

                    if (!AutoUpdate)
                    {
                        MarketInitiated = false;
                        break;
                    }
                    else
                    {
                        if(useDebug) Debug.Log("Price will be updated in the next " + EverySec + " seconds.");
                        yield return new WaitForSeconds(EverySec);
                    }
                }
            }
            else
            {
                Debug.LogError("WARNING: You already called this function!");
            }
        }

        //QR Implementation
        public Texture2D GenerateQR(string textToGenerate)
        {
            var encoded = new Texture2D(256, 256);
            var color32 = Encode(textToGenerate, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            return encoded;
        }

        public Texture2D GenerateQR(string textToGenerate, bool CopyToClipboard)
        {
            var encoded = new Texture2D(256, 256);
            var color32 = Encode(textToGenerate, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            if (CopyToClipboard)
            {
                TextEditor te = new TextEditor();
                te.SelectAll();
                te.Copy();
            }
            return encoded;
        }

        private static Color32[] Encode(string textForEncoding,int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }

        IEnumerator SendInvokeMethod(string method, params object[] a_params)
        {
            UnityWebRequest webrequest = new UnityWebRequest();
            string b_params = "";
            var form = new WWWForm();
            form.AddField("method", method);

            if (a_params != null)
            {
                if (a_params.Length > 0)
                {
                    foreach (var p in a_params)
                    {
                        //form.AddField("params", p.ToString());
                        if (b_params == "") b_params = p.ToString();
                        else b_params = b_params + "," + p.ToString();
                    }
                }
            }

            form.AddField("params", b_params);
            form.AddField("rpcusername", User_);
            form.AddField("rpcpassword", Pass_);
            form.AddField("port", Port_);

            webrequest = UnityWebRequest.Post(URI_+"QTCoreCalls.php", form);
            webrequest.timeout = 30;

            yield return webrequest.SendWebRequest();
            //while (!webrequest.isDone) yield return null;

            if (webrequest.isNetworkError)
            {
                if (useDebug) Debug.Log(webrequest.error);
            }
            else
            {
                DownloadHandler GetResult = webrequest.downloadHandler;
                ResponseObject = JsonConvert.DeserializeObject<JObject>(GetResult.text);
                if (useDebug) Debug.Log("ResponseObject holds the lastest json result: " + GetResult.text);
                webrequest.Dispose();
                GetResult.Dispose();
            }
        }
    }
}
