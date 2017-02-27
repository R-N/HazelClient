using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Security.Cryptography;

using Hazel;
using Hazel.Tcp;

using UnityEngine;

using UnityEngine.UI;

public class HazelTest : MonoBehaviour {
	 Connection connection;
	HashAlgorithm md5;
	 Dictionary<byte, Action<Connection, HazelReader>> DataHandlers = new Dictionary<byte, Action<Connection, HazelReader>> ();
	string sessionkey = null;

	public GameObject logWindow = null;

	public Text logText = null;

	public InputField usernameField = null;
	public InputField passwordField = null;

	public List<RoomInfo> rooms = new List<RoomInfo>();

	public RectTransform roomListEntryHolder = null;

	public GameObject roomListEntryPrefab = null;

	public GameObject loginPanel = null;

	public GameObject roomListPanel = null;

	public static HazelTest singleton = null;

	public int room = 0;

	public void Start()
	{
		singleton = this;
		md5 = ((HashAlgorithm)CryptoConfig.CreateFromName ("MD5"));

		AddDataHandler (0, LoginReply);
		//1 reserved for register reply
		AddDataHandler (2, RoomListReply);
		AddDataHandler (3, MoveRoom);
		//4 for playerlist
		//5 for playercount, for anonymous rooms
		//6 for playerlist by team
		//7 for user info

		Connect ();
	}

	void Connect(){

		string serverurl = "linearch.ddns.net";

		IPAddress[] addresslist = Dns.GetHostAddresses(serverurl);

		if (addresslist.Length == 0) {
			throw(new Exception ("Unable to connect to server"));
			return;
		}


		string serverIP = addresslist [0].ToString ();



		string internalIP = Network.player.ipAddress;

		WWW checkip = new WWW ("http://linearch.16mb.com/ipcheck.php");
		System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew ();
		while (!checkip.isDone && sw.Elapsed.Seconds < 10) {

		}
		sw.Stop ();
		string externalIP;

		if (checkip.isDone) {
			externalIP = checkip.text;
		}else{
			UnityEngine.Debug.LogError ("Unable to get ip. Assuming..");
			externalIP = "NOIP";
		}



		NetworkEndPoint endPoint;

		if (externalIP == serverIP) {
			if (internalIP == "192.168.1.134")
				endPoint = new NetworkEndPoint ("127.0.0.1", 2570);
			else
				endPoint = new NetworkEndPoint ("192.168.1.134", 2570);
		}else
			endPoint = new NetworkEndPoint (addresslist [0], 2570);

		if (connection != null) {
			connection.Close ();
			connection.Dispose ();
		}
		connection = new TcpConnection(endPoint);


		connection.DataReceived += DataReceived;
		connection.Disconnected += DisconnectionHandler;

		Debug.Log("Connecting");

		try{
		connection.Connect();
		}catch(Exception ex){
			PopupLog (ex.ToString ());
			Debug.LogError(ex);
			return;
		}
		StartCoroutine ("WaitForConnection");

	}

	IEnumerator WaitForConnection(){
		while (connection.State == ConnectionState.Connecting) {
			yield return new WaitForEndOfFrame ();
		}
		if (connection.State == ConnectionState.Connected) {
			yield return new WaitForEndOfFrame ();
			Debug.Log ("Connected!");
			if (!string.IsNullOrEmpty (sessionkey))
				Relog ();
		} else {
			Debug.Log ("Connection failed : " + connection.State.ToString());
		}
	}

	public void Login(){
		if (connection.State != ConnectionState.Connected) {
			PopupLog ("Not connected");
			return;
		}
		if (!string.IsNullOrEmpty (sessionkey)) {
			PopupLog ("Already logged in");
			return;
		}
		string uname = usernameField.text;
		string pass = passwordField.text;
		if (string.IsNullOrEmpty (uname) || string.IsNullOrEmpty (pass)) {
			PopupLog ("Both username and password can't be empty");
			return;
		}
		HazelWriter writer = new HazelWriter ();
		writer.WriteByte(0);
		writer.Write (uname);
		writer.Write (MD5 (pass + "134"));
		connection.SendBytes (writer.bytes, SendOption.Reliable);
	}

	void Relog(){
		HazelWriter writer = new HazelWriter ();
		writer.WriteByte(1);
		writer.Write (sessionkey);
		connection.SendBytes (writer.bytes, SendOption.Reliable);
	}

	void DisconnectionHandler (object sender, DisconnectedEventArgs args){
		connection = (Connection)sender;

		PopupLog ("Disconnected");
		UnityMainThreadDispatcher.Instance().Enqueue(() => StartCoroutine("WaitReconnectCor"));
	}

	 string MD5(string input){
		// byte array representation of that string
		byte[] temp5 = new UTF8Encoding().GetBytes(input);
		// need MD5 to calculate the hash
		byte[] hash = md5.ComputeHash(temp5);

		return BitConverter.ToString(hash)
			// without dashes
				.Replace("-", string.Empty);
	}

	public void OnDestroy(){
		if (connection != null && connection.State == ConnectionState.Connected)
			connection.Close();
	}
	 bool AddDataHandler(byte id, Action<Connection, HazelReader> act){
		if (DataHandlers.ContainsKey (id))
			return false;
		DataHandlers.Add (id, act);
		return true;
	}

	private void DataReceived(object sender, DataReceivedEventArgs args)
	{
		connection = (Connection)sender;

		HazelReader reader = new HazelReader (args.Bytes);

		byte header = reader.ReadByte ();

		if (DataHandlers.ContainsKey (header))
			UnityMainThreadDispatcher.Instance ().Enqueue (() => DataHandlers [header] (connection, reader));
		args.Recycle();
	}

	public void CreateRoom(){
		if (room > 0) {
			PopupLog ("Already in room " + room);
			return;
		}
		HazelWriter writer = new HazelWriter ();
		writer.WriteByte(6);
		writer.Write ("Test room");
		writer.Write (0);
		writer.Write (4);
		writer.Write (string.Empty);
		Debug.Log ("asking to create room");

		connection.SendBytes (writer.bytes);
	}

	public void Refresh(){
		connection.SendBytes (new byte[]{ 5 });
	}

	void LoginReply(Connection conn, HazelReader reader){
		sessionkey = reader.ReadString ();
		if (string.IsNullOrEmpty (sessionkey)) {
			sessionkey = null;
			PopupLog ("Login failed");
		} else {
			Debug.Log ("Login successful");
			Refresh ();
			roomListPanel.SetActive (true);
			loginPanel.SetActive (false);
		}
	}

	void MoveRoom (Connection conn, HazelReader reader){
		int reply = reader.ReadInt ();
		if (reply < 0) {
			if (reply == -1) {
				PopupLog ("You have been kicked.");
			} else if (reply == -2) {
				PopupLog ("Room full");
			} else if (reply == -3) {
				PopupLog ("Wrong password. If the room seem to have no password, try refreshing.");
			} else if (reply == -4) {
				PopupLog ("Room not found");
			} else if (reply == -5) {
				PopupLog ("Room is either full or already playing.");
			}else{
				PopupLog ("Unknown error : " + reply + "\nTry restarting");
			}
		}
	}

	public void JoinRoom(int number){
		if (room > 0) {
			PopupLog ("Already in room " + room);
			return;
		}
		HazelWriter writer = new HazelWriter ();
		writer.WriteByte(7);
		writer.Write (number);
		connection.SendBytes (writer.bytes);
	}
	void RoomListReply(Connection conn, HazelReader reader){
		int count = reader.ReadInt ();
		Debug.Log ("there are " + count + " rooms");
		rooms.Clear ();
		while (count > 0) {
			rooms.Add (reader.ReadRoomInfo ());
			count--;
		}
		foreach (Transform t in roomListEntryHolder) {
			Destroy (t.gameObject);
		}
		foreach (RoomInfo r in rooms) {
			RoomListEntry re = ((GameObject)Instantiate (roomListEntryPrefab)).GetComponent<RoomListEntry> ();
			re.transform.SetParent (roomListEntryHolder, false);
			re.name.text = r.number + ". " + r.name;
			re.roomNumber = r.number;
			re.playerCount.text = r.curPlayers + "/" + r.maxPlayers;
			if (r.hasPassword)
				re.hasPassword.SetActive (true);
			if (r.playing)
				re.playing.SetActive (true);
		}
	}
	public IEnumerator WaitReconnectCor(){
		yield return new WaitForSecondsRealtime (1);
		if (connection.State != ConnectionState.Connected) {
			Debug.Log ("Reconnecting...");
			Connect ();
		}
	}

	public void PopupLog(string log){
		logText.text = log;
		logWindow.SetActive (true);
	}
}
