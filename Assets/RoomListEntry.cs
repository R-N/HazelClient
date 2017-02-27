using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomListEntry : MonoBehaviour, IPointerClickHandler {
	public int roomNumber = 0;
	public Text name = null;
	public Text playerCount = null;
	public GameObject playing = null;
	public GameObject hasPassword = null;

	public void OnPointerClick(PointerEventData data){
		HazelTest.singleton.JoinRoom (roomNumber);
	}
}
