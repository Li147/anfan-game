using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{

    private static KeybindManager instance;

    public static KeybindManager MyInstance {

        get {

            if (instance == null) {
                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;


        }
    }

    public Dictionary<string, KeyCode> Keybinds { get; set; }

    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    private string bindName;




    // Start is called before the first frame update
    void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();

        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);


    }

    public void BindKey(string newKey, KeyCode keyBind) {

        // default sets current dictionary to keybinds
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (newKey.Contains("ACT")) {

            currentDictionary = ActionBinds;

        }

        // if the key DOES NOT contain the binding
        if (!currentDictionary.ContainsKey(newKey)) {

            currentDictionary.Add(newKey, keyBind);
            UIManager.MyInstance.UpdateKeyText(newKey, keyBind);


        } 
        // if the key is ALREADY BOUND
        else if (currentDictionary.ContainsValue(keyBind)) {

            // find the key that is already bound to that keybind
            string oldKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            // unbind the old key
            currentDictionary[oldKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(newKey, KeyCode.None);





        }
        currentDictionary[newKey] = keyBind;
        UIManager.MyInstance.UpdateKeyText(newKey, keyBind);

        bindName = string.Empty;

    }

    public void KeyBindOnClick(string bindName) {

        this.bindName = bindName;

        
    }

    // checks for a key press and change binding for us
    private void OnGUI() {
        
        if (bindName != string.Empty) {

            Event e = Event.current;

            if (e.isKey) {
                BindKey(bindName, e.keyCode);
            }

        }

    }

}
