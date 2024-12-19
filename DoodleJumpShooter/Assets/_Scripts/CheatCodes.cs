using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatCodes : MonoBehaviour
{
    public List<Keys> Codes;

    void Update()
    {
        if (Input.anyKeyDown) {
            for (int i = 0; i < Codes.Count; i++) {
                if (Input.GetKeyDown(Codes[i].Code[Codes[i].CurrentCode])) 
                    Codes[i].CurrentCode++;
                else {
                    Codes[i].CurrentCode = 0;
                } 
            }
        }
    }
}
[System.Serializable]
public class Keys {
    public List<KeyCode> Code;
    public UnityEvent SuccesAction = new UnityEvent();
    private int currentCode;
    public int CurrentCode {
        get { return currentCode; }
        set { 
            currentCode = value; 
            if (currentCode >= Code.Count) {
                currentCode = 0;
                SuccesAction.Invoke();
            }
        }
    }
}
