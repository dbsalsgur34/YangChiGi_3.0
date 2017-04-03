using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
using System.IO;
using ClientSide;

public class Network_Client {
    public static string serverAddress;
	public static int PORT = 11900;
	
	private static TcpClient tcpClient;
	private static NetworkStream networkStream;
	private static StreamReader streamReader;
	private static StreamWriter streamWriter;

	private static Thread thread_connect;
	private static Thread thread_receive;

	private static int networkId = -1;

	private static DataBaseIO dataBaseIO;

	public static int NetworkId{
		get{return networkId;}
		set{
			networkId = value;
			//NetworkMessage.SenderId = networkId.ToString();
		}
	}

	private static bool isConnected = false;
	public static bool IsConnected{
		get{return isConnected;}
	}

	public static void Begin(string ip){
		dataBaseIO = new DataBaseIO();
        //ShutDown();
        serverAddress = ip;
        tcpClient = new TcpClient();
		NetworkId = -1;

		thread_connect = new Thread(BeginConnection);
		thread_connect.Start();
	}

	static bool shutDown = false;
	private static void BeginConnection(){
		int conCount = 0;
		while(isConnected == false){
			if (shutDown)
				return;
			try{
				Debug.Log("Connecting to..." + serverAddress + ":" + PORT);
				tcpClient.Connect(serverAddress, PORT);
				isConnected = true;

			}catch(SocketException e){
				//ConsoleMsgQueue.EnqueMsg("Connection Msg: " + e.SocketErrorCode.ToString());
				conCount++;
				if(conCount > 5){
					Debug.Log("Fail Connect, Exit Connecting");
					isConnected = false;
					return;
				}

				Thread.Sleep(2000);
			}catch(Exception e){
			//	ConsoleMsgQueue.EnqueMsg(e.ToString());
			}
		}
			
		Debug.Log("Connected.");

		networkStream = tcpClient.GetStream();
		streamWriter = new StreamWriter(networkStream, Encoding.UTF8);
		streamReader = new StreamReader(networkStream, Encoding.UTF8);

		thread_receive = new Thread(ReceivingOperation);
		thread_receive.Start();
	}

	public static void Send(string nm_){
		if(isConnected){
			string str = nm_.ToString();
			try{
				Debug.Log("Send: " + str);
				streamWriter.WriteLine(str);
				streamWriter.Flush();
			}catch(Exception e){
				//ConsoleMsgQueue.EnqueMsg("Send: " + e.Message);
				isConnected = false;
				networkId = -1;
			}
			dataBaseIO.push(str);
		}else{
			Debug.Log("Send: Network Disconnected.");
		}
	}

	private static void ReceivingOperation(){
		string recStr;

		try{
			while(isConnected){
				recStr = streamReader.ReadLine();

				if(recStr != null){
					Debug.Log("Received: " + recStr );
                    
					ReceiveQueue.SyncEnqueMsg(recStr);
				}else{
					isConnected = false;
				}
			}
		}catch(Exception e){
			//ConsoleMsgQueue.EnqueMsg("ReceivingOperation: " + e.Message);
		}

		isConnected = false;
		Debug.Log("Disconnected.");

		streamReader.Close();
	}

	public static void ShutDown(){
		isConnected = false;
		shutDown = true;
		if(streamReader != null)
			streamReader.Close();
		if(streamWriter != null)
			streamWriter.Close();
		if(tcpClient != null)
			tcpClient.Close();

        Network_Client.Send("SHUTDOWN");

        Debug.Log ("SHUT DOWN");
	}
}
