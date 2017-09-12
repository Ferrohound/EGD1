using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the player to interact with the object

public interface Interactable {
	void Interact(PlayerController pc, int flag);
}
