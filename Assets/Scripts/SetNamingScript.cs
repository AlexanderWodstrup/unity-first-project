using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetNamingScript : MonoBehaviour
{
    [SerializeField] public GameObject ProjectNaming;
    [SerializeField] public TextMeshProUGUI GroupName;
    public FirebaseGroups _FirebaseGroups;
    public UserData UserDataComponent;

    private Toggle m_Toggle;
    // Start is called before the first frame update
    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(delegate { enableNaming(); });
    }
    
    public void enableNaming()
    {
        Debug.Log("Enable - " + gameObject.name);
        var txt = gameObject.name.Replace("Toggle", "").Replace("(","").Replace(")","");
        GroupName.text = "Group: " + txt;
        ProjectNaming.gameObject.SetActive(true);
        UserDataComponent.storeFirebaseGroup(_FirebaseGroups);
    }
}
