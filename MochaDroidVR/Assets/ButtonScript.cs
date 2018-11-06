using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public string EventName;

	public Button btn;

	// Use this for initialization
	void Start () {
		btn.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick()
	{
		EventManager.TriggerEvent (EventName);
	}

	// Update is called once per frame
	void Update () {

	}
}
