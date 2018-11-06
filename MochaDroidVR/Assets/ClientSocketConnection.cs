using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class ClientSocketConnection
{
	//exposed to hook into server status changes
	public event EventHandler<ConnectionStatusChangedEventArgs> ConnectStatusChanged;
	private ConnectionStatus _status = ConnectionStatus.Idle;

	public ConnectionStatus Status
	{
		get { return _status; }
		set
		{
			if (value != _status)
			{
				_status = value;
				var args = new ConnectionStatusChangedEventArgs { Status = _status };
				var handler = ConnectStatusChanged;

				if (handler != null)
					handler.Invoke(Status, args);

			}
		}
	}

	private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private BinaryReader nwRead;
	private StreamWriter nwWrite;

	public void Connect(string serverIP, string serverPort)
	{
		try
		{
			Status = ConnectionStatus.Connecting;

			sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			sock.Connect(serverIP, int.Parse(serverPort));

			NetworkStream ns = new NetworkStream(sock);
			nwRead = new BinaryReader(ns);
			nwWrite = new StreamWriter(ns);

			Status = ConnectionStatus.Connected;
		}
		catch (Exception e)
		{
			Debug.Log (e.ToString ());
			Status = ConnectionStatus.Failed;
			//todo:report errors via event to be consumed by UI thread
		}
	}

	public void SendMessage(string message)
	{
		Debug.Log ("SendMessage");
		try
		{
			nwWrite.WriteLine(message);
			nwWrite.Flush();
			//await _writer2.FlushAsync();
		}
		catch (Exception exc)
		{
			Debug.Log ("Error");
			Debug.Log (exc.ToString ());
			Status = ConnectionStatus.Failed;
		}
	}

	public void GetData()
	{
		try
		{
			while (true)
			{
				int j = nwRead.ReadInt32();

				byte[] data = nwRead.ReadBytes(j);

				Message = System.Text.ASCIIEncoding.ASCII.GetString(data);

			}
		}
		catch (Exception e)
		{
			Status = ConnectionStatus.Failed;
			//TODO:send a connection status message with error, then try to reconnect
		}
	}

	public event EventHandler<MessageSentEventArgs> NewMessageReady;
	private string _message;

	public string Message
	{
		get { return _message; }
		set
		{
			//System.Diagnostics.Debug.WriteLine("Robot Received: " + _message);
			_message = value;
			var args = new MessageSentEventArgs { Message = _message };
			var handler = NewMessageReady;
			if (handler != null)
				handler.Invoke(Message, args);
		}
	}
}

public class ConnectionStatusChangedEventArgs : EventArgs
{
	public ConnectionStatus Status;
}

public class MessageSentEventArgs : EventArgs
{
	public string Message;
}

public enum ConnectionStatus
{
	Idle = 0,
	Connecting,
	Listening,
	Connected,
	Failed = 99
}
