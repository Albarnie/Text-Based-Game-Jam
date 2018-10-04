using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
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
        public bool authorised;
    }

    [System.Serializable]
    public struct AdminLogin
    {
        public string username, password;
        public bool[] priviledges;
    }

    [Header("References")]
    public GameObject console;
    public TextMeshProUGUI backlog;
    public TMP_InputField input;

    [Header("Console Settings")]
    public char separator = ' ';
    public string starterText = "Welcome to the console, type 'Help' for help";
    public Command[] commands;
    public AdminLogin[] logins;

    [Header("Parameters")]
    public bool open;

    private void Awake()
    {
        backlog.text = "";
    }

    private void Update()
    {
        if(Input.GetKeyDown("`") && open)
        {
            open = false;
            HideConsole();
        }
        else if (Input.GetKeyDown("`"))
        {
            open = true;
            ShowConsole();
        }
    }

    public void Parse (string text)
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
                Login(arguments);
                break;
            case "camera":
                Camera(arguments);
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

    public void HideConsole ()
    {
        console.SetActive(false);
    }

    public void ShowConsole ()
    {
        console.SetActive(true);
        Debug.Log(starterText + "\n");
        backlog.text += starterText + "\n";
    }

    #region Commands
    void Door (string[] arguments)
    {
        switch (arguments[1])
        {
            case "open":
                if (int.Parse(arguments[2]) < GameManager.manager.doors.Count)
                {
                    GameManager.manager.doors[int.Parse(arguments[2])].Open();
                }
                else
                {
                    Print("Invalid id.", PrintType.Error);
                }
                break;
            case "close":
                if (int.Parse(arguments[2]) < GameManager.manager.doors.Count)
                {
                    GameManager.manager.doors[int.Parse(arguments[2])].Close();
                }
                else
                {
                    Print("Invalid id.", PrintType.Error);
                }
                break;
        }
    }

    void Help (string[] arguments)
    {
        if(arguments.Length <= 1)
        {
            string help = "Help:";
            foreach (Command command in commands)
            {
                help += "\n" + command.commandName + ": " + command.help;
            }
            Print(help, PrintType.Info);
        }
        else
        {
            bool cont = true;
            int i = 0;
            while (cont && i < commands.Length)
            {
                if(arguments[1] == commands[i].commandName.ToLower())
                {
                    string help = commands[i].commandName + ": " + commands[i].help;
                    Print(help, PrintType.Info);
                    cont = false;
                }
                i++;
            }
        }
    }

    void Say (string[] arguments)
    {
        if(arguments.Length >=2)
        {
            string say = "";
            for (int i = 1; i < arguments.Length; i ++)
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

    void Login (string[] arguments)
    {
        if(arguments.Length >= 3)
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
                            if(logins[i].priviledges[ii])
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
        else if(arguments.Length < 3)
        {
            Print("Not enough arguments.", PrintType.Error);
        }
    }

    void Camera (string[] arguments)
    {
        switch (arguments[1])
        {
            case "enable":
                if (int.Parse(arguments[2]) < GameManager.manager.doors.Count)
                {
                    GameManager.manager.cameras[int.Parse(arguments[2])].Enable();
                }
                else
                {
                    Print("Invalid id.", PrintType.Error);
                }
                break;
            case "disable":
                if (int.Parse(arguments[2]) < GameManager.manager.doors.Count)
                {
                    GameManager.manager.cameras[int.Parse(arguments[2])].Disable();
                }
                else
                {
                    Print("Invalid id.", PrintType.Error);
                }
                break;
        }
    }

    #endregion
}
