using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    public enum LabelType
    {
        Electronic,
        Password,
        Note,
        Email
    }


    public LabelType type;
    public float mouseOverDistance = 2;
    public Electronic parent;

    private void Awake()
    {
        parent = transform.parent.GetComponent<Electronic>();
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        if (Vector2.Distance(mousePos, transform.position) < mouseOverDistance)
        {
            switch ((int)type)
            {
                case 0:
                    GameManager.manager.guiManager.UpdateWorldNotify("Name: '" + parent.deviceName + "' \nATID: " + parent.id, mousePos);
                    Debug.DrawLine(transform.position, mousePos);
                    break;
                case 1:
                    GameManager.manager.guiManager.UpdateWorldNotify("Username: '" + GameManager.manager.consoleManager.logins[parent.id].username + "' \nPassword: '" + GameManager.manager.consoleManager.logins[parent.id].password + "'", mousePos);
                    Debug.DrawLine(transform.position, mousePos);
                    break;
                case 2:
                    GameManager.manager.guiManager.UpdateWorldNotify(parent.deviceName, mousePos);
                    Debug.DrawLine(transform.position, mousePos);
                    break;
                case 3:
                    GameManager.manager.guiManager.UpdateWorldNotify(parent.deviceName + "\nUsername: '" + GameManager.manager.consoleManager.emails[parent.id].username + "' \nPassword: '" + GameManager.manager.consoleManager.emails[parent.id].password + "'", mousePos);
                    Debug.DrawLine(transform.position, mousePos);
                    break;
            }
        }
    }

}
