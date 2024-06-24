using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HISK {
    public class InteractableObject : MonoBehaviour {
        public bool IsInteractable = false;

        public virtual void Interact() {
            if (IsInteractable == false) return;
		}
    }
}