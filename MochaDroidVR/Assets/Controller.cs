using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller : MonoBehaviour {

	ClientSocketConnection client = null;

	// Use this for initialization
	void Start () {
		client = new ClientSocketConnection();
		client.Connect ("192.168.1.51", "8027");

		EventManager.StartListening("RightButton", new UnityAction(RightButton));
		EventManager.StartListening("LeftButton", new UnityAction(LeftButton));
		EventManager.StartListening("ForwardButton", new UnityAction(ForwardButton));
		EventManager.StartListening("ReverseButton", new UnityAction(ReverseButton));
		EventManager.StartListening("StopButton", new UnityAction(StopButton));
	}

	public void RightButton()
	{
		Debug.Log ("Right Button");
		client.SendMessage("4|0");
	}

	public void LeftButton()
	{
		Debug.Log ("Left Button");
		client.SendMessage("3|0");
	}

	public void ForwardButton()
	{
		Debug.Log ("Forward Button");
		client.SendMessage("1|0");
	}

	public void ReverseButton()
	{
		Debug.Log ("Reverse Button");
		client.SendMessage("2|0");
	}

	public void StopButton()
	{
		Debug.Log ("Stop Button");
		client.SendMessage("0|0");
	}

	// Update is called once per frame
	void Update () {

	}
}
