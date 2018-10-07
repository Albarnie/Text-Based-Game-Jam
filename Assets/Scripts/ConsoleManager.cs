using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public enum PrintType
    {
        Info,
        Error,
        Question,
        User
    }

    [System.Serializable]
    public struct Command
    {
        public string commandName;
        public string help;
        public string useage;
        public bool authorised;
    }

    [System.Serializable]
    public struct Login
    {
        public string username, password;
        public bool[] priviledges;
    }

    [System.Serializable]
    public struct Email
    {
        public string username, password;
        [Multiline]
        public string[] emails;
    }

    [Header("References")]
    public TextMeshProUGUI backlog;

    [Header("Console Settings")]
    public char separator = ' ';
    public string starterText = "Welcome to the console, type 'Help' for help";
    public Command[] commands;
    public Login[] logins;
    public Email[] emails;

    [Header("Parameters")]
    public bool open;

    private void Awake()
    {
        backlog.text = "";
    }

    private void Start()
    {
        GameManager.manager.consoleManager = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown("`") && open)
        {
            open = false;
            GameManager.manager.menuManager.ChangeMenu(-1);
        }
        else if (Input.GetKeyDown("`"))
        {
            open = true;
            GameManager.manager.menuManager.ChangeMenu(1);
        }
    }

    public void Parse(string text)
    {
        string[] arguments = text.ToLower().Split(separator);

        string debug = "Command: ";
        foreach (string argument in arguments)
        {
            debug = debug + argument + ", ";
        }

        Print(text, PrintType.User);

        Debug.Log(debug);

        bool cont = true;
        int i = 0;
        while (cont && i < commands.Length)
        {
            if (arguments[0] == commands[i].commandName.ToLower())
            {
                if (!commands[i].authorised)
                {
                    Print("Insufficient Privelidges. Obtain administrator access: " + arguments[0], PrintType.Error);
                    return;
                }
            }
            i++;
        }

        switch (arguments[0])
        {
            case "door":
                Door(arguments);
                break;
            case "help":
                Help(arguments);
                break;
            case "say":
                Say(arguments);
                break;
            case "login":
                LogIn(arguments);
                break;
            case "camera":
                Camera(arguments);
                break;
            case "alarm":
                Alarm(arguments);
                break;
            case "email":
                OpenEmail(arguments);
                break;
            default:
                Print("Unknown command: " + arguments[0], PrintType.Error);
                break;
        }

    }

    public void Print(string text, PrintType type)
    {
        switch ((int)type)
        {
            //Info
            case 0:
                backlog.text += "[INFO]: " + text + "\n";
                break;

            //Error
            case 1:
                backlog.text += "[ERROR]: " + text + "\n";
                break;

            //Question
            case 2:
                backlog.text += "[INPUT]: " + text + "\n";
                break;

            //User
            case 3:
                backlog.text += "[USER]: " + text + "\n";
                break;
        }
    }

    #region Commands
    void Door(string[] arguments)
    {
        int id = int.Parse(arguments[2]) - GameManager.manager.playerData.idPrefix;
        if (GameManager.manager.electronics[id] is Door && id < GameManager.manager.electronics.Count)
        {
            switch (arguments[1])
            {
                case "open":
                    GameManager.manager.electronics[id].Disable();
                    break;
                case "close":
                    GameManager.manager.electronics[id].Enable();
                    break;
            }
        }
        else
        {
            Print("Invalid ID.", PrintType.Error);
        }
    }

    void Help(string[] arguments)
    {
        if (arguments.Length <= 1)
        {
            string help = "Help:";
            foreach (Command command in commands)
            {
                help += "\n" + command.commandName + ": " + command.help;
                help += "\n USEAGE: " + command.useage;
            }
            Print(help, PrintType.Info);
        }
        else
        {
            bool cont = true;
            int i = 0;
            while (cont && i < commands.Length)
            {
                if (arguments[1] == commands[i].commandName.ToLower())
                {
                    string help = commands[i].commandName + ": " + commands[i].help;
                    Print(help + "\n USEAGE: " + commands[i].useage, PrintType.Info);
                    cont = false;
                }
                i++;
            }
        }
    }

    void Say(string[] arguments)
    {
        if (arguments.Length >= 2)
        {
            string say = "";
            for (int i = 1; i < arguments.Length; i++)
            {
                say += arguments[i] + " ";
            }
            Print(say, PrintType.Info);
        }
        else
        {
            Print("Not enough arguments.", PrintType.Error);
        }
    }

    void LogIn(string[] arguments)
    {
        if (arguments.Length >= 3)
        {
            bool cont = true;
            int i = 0;
            while (cont && i < logins.Length)
            {
                if (arguments[1] == logins[i].username.ToLower())
                {
                    string password = "";
                    for (int p = 2; p < arguments.Length; p++)
                    {
                        password += arguments[p];
                        if (p < arguments.Length - 1)
                            password += " ";
                    }

                    if (password == logins[i].password.ToLower())
                    {
                        Print("Successfully logged in as '" + logins[i].username + "'.", PrintType.Info);
                        for (int ii = 0; ii < logins[i].priviledges.Length; ii++)
                        {
                            if (logins[i].priviledges[ii])
                            {
                                commands[ii].authorised = true;
                            }
                        }
                        Print("Granted necessary priviledges.", PrintType.Info);
                    }
                    else
                    {
                        Print("Incorrect password.", PrintType.Error);
                    }
                }
                i++;
            }
        }
        else if (arguments.Length < 3)
        {
            Print("Not enough arguments.", PrintType.Error);
        }
    }

    void Camera(string[] arguments)
    {
        int id = int.Parse(arguments[2]) - GameManager.manager.playerData.idPrefix;
        if (GameManager.manager.electronics[id] is SecurityCamera && id < GameManager.manager.electronics.Count)
        {
            switch (arguments[1])
            {
                case "enable":
                    GameManager.manager.electronics[id].Enable();
                    break;
                case "disable":
                    GameManager.manager.electronics[id].Disable();
                    break;
            }
        }
    }

    void Alarm(string[] arguments)
    {
        int id = int.Parse(arguments[2]) - GameManager.manager.playerData.idPrefix;
        if (GameManager.manager.electronics[id] is Alarm && id < GameManager.manager.electronics.Count)
        {
            switch (arguments[1])
            {
                case "disable":
                    GameManager.manager.electronics[id].Disable();
                    break;
                case "enable":
                    GameManager.manager.electronics[id].Enable();
                    break;
                case "activate":
                    ((Alarm)GameManager.manager.electronics[id]).SetOff();
                    break;
            }
        }
        else
        {
            Print("Invalid ID.", PrintType.Error);
        }
    }

    void OpenEmail(string[] arguments)
    {
        int id = int.Parse(arguments[3]);
        if (arguments.Length == 4)
        {
            bool cont = true;
            int i = 0;
            while (cont && i < emails.Length)
            {
                if (arguments[1] == emails[i].username.ToLower())
                {
                    string password = "";
                    for (int p = 2; p < arguments.Length -1; p++)
                    {
                        password += arguments[p];
                        if (p < arguments.Length - 2)
                            password += " ";
                    }

                    if (password == emails[i].password.ToLower())
                    {
                        Print("Successfully logged in as '" + emails[i].username + "'.", PrintType.Info);
                        Print(emails[i].emails[id], PrintType.Info);
                    }
                    else
                    {
                        Print("Incorrect password.", PrintType.Error);
                    }
                }
                i++;
            }
        }
        else if (arguments.Length < 4)
        {
            Print("Not enough arguments.", PrintType.Error);
        }
    }

    #endregion
}
