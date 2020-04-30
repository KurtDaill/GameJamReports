using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    LinkedList<DialogueChunk> formattedScript;
    Dictionary<string, string[]> header;
    Dictionary<string, string[]> dialgoue;
    StreamReader reader;
    string line;
    int nextLine;
    bool lastLine;

    public string[] closingDialogueLines;
    public Text[] dialogueAndOpts;
    public Button[] buttons;
    public GameObject mainView, askView;

    string[] currentDialogue, currentOptions;
    DialogueChunk currentChunk;

    public TextAsset script;
    // Start is called before the first frame update
    void Start()
    {
        /*
         * Loads the Replies from a text file and puts in an array 
         */
        header = new Dictionary<string, string[]>();
        dialgoue = new Dictionary<string, string[]>();
        formattedScript = new LinkedList<DialogueChunk>();
        reader = new StreamReader("Assets/Call Script 1.txt", true);
        lastLine = false;


        formatScriptLine(header, reader.ReadLine());

        while ((line = reader.ReadLine()) != null)
        {
            if (line == "^")
            {
                formattedScript.AddLast(new DialogueChunk(header, dialgoue));
                print(formattedScript.First.Value.ToString());
                header = new Dictionary<string, string[]>();
                dialgoue = new Dictionary<string, string[]>();
                if ((line = reader.ReadLine()) != null)
                {
                    formatScriptLine(header, line);
                }
                continue;
            }
            formatScriptLine(dialgoue, line);
        }

        tryNewChunk("start");
    }

    // Update is called once per frame
    void Update()
    {
        if (nextLine < currentDialogue.Length && Input.GetMouseButtonDown(0))
        {
            dialogueAndOpts[0].text = formatNewLine(currentDialogue[nextLine]);
            nextLine++;
        }    
        else if (Input.GetMouseButtonDown(0) && lastLine)
        {
            //TO DO: Close the Scene
        }
        else if (Input.GetMouseButtonDown(0))
        {
            EnableInteraction();
            if (currentChunk.Equals(formattedScript.Last.Value))
            {
                for(int i = 0; i < currentChunk.chosenOptions.Length; i++)
                {
                    if(currentChunk.chosenOptions[i] == false)
                    {
                        break;
                    }
                }
                //If the code gets here, it means we're on the last chunk, and there are no options left. In general, the last chuck should have no options, so this just indicates we should be going to the closer.
                lastLine = true;
                currentDialogue = closingDialogueLines;
                DisableInteraction();
            }
        }
    }

    public void formatScriptLine(Dictionary<string, string[]> parentDictionary, string line)
    {
        int seperatorIndex, lineCount, askCount, temp;
        string[] answers, questions;

        seperatorIndex = line.IndexOf(":");
        askCount = (int)char.GetNumericValue(line[seperatorIndex - 1]);
        lineCount = (int)char.GetNumericValue(line[seperatorIndex + 1]);
        lineCount = (int)char.GetNumericValue(line[seperatorIndex + 1]);
        answers = new string[lineCount];
        questions = new string[askCount];
        temp = 0;

        for (int i = 0; i < askCount; i++)
        {
            questions[i] = line.Substring(temp, line.IndexOf("/"));
            print("new question: " + questions[i]);
            line = line.Remove(0, line.IndexOf("/") + 1);
        }
        temp = 0;
        line = line.Remove(0, 4);

        for (int i = 0; i < lineCount; i++)
        {
            answers[i] = line.Substring(temp, line.IndexOf("/"));
            print("new response: " + answers[i]);
            line = line.Remove(0, line.IndexOf("/") + 1);
        }

        foreach (string question in questions)
        {
            parentDictionary.Add(question, answers);
        }
    }

    public void acceptButtonPush(int id)
    {
        if(id == 4)
        {
            mainView.SetActive(false);
            askView.SetActive(true);
            return;
        }
        if(id == 5)
        {
            //TO DO: Hang Up
            return;
        }
        print("Got to Normal Button Code");
        if(currentChunk.dialogueOptions.TryGetValue(currentOptions[id], out currentDialogue))
        {
            print("Passphrase Accepted: " + currentOptions[id]);
            print("Text Should Read: " + currentDialogue[0]);
            dialogueAndOpts[0].text = formatNewLine(currentDialogue[0]);
            currentChunk.chosenOptions[id] = true;
            buttons[id].interactable = false;
            nextLine = 1;
        }else print("Passphrase Not Working my Dude..");
        DisableInteraction();
    }

    //Used to interpret and remove any command characters (i.e. speaker indicator characters at start of line), as well as set up the next line of dialogue to be read.
    public string formatNewLine(string line)
    {
        if (line.Substring(0,1) == "P")
        {
            dialogueAndOpts[5].gameObject.SetActive(false);
        }
        else
        {
            dialogueAndOpts[5].gameObject.SetActive(true);
        }
        return line.Remove(0, 1);
        
    }

    //Called when the player uses the "ask about" box, to see if they inputed a valid keyword. If so, set up the related chunk.
    public void tryNewChunk(string check)
    {
        string[] buffer;
        foreach(DialogueChunk chunk in formattedScript)
        {
            if (chunk.headerDialogue.TryGetValue(check.ToLower(), out buffer))
            {
                returnToMainView();
                currentDialogue = buffer;
                foreach(Button button in buttons)
                {
                    button.gameObject.SetActive(false);
                }
                buttons[4].gameObject.SetActive(true);
                buttons[5].gameObject.SetActive(true);
                currentChunk = chunk;
                currentOptions = new string[currentChunk.chosenOptions.Length];
                int i = 0;

                EnableInteraction();

                foreach (string key in currentChunk.dialogueOptions.Keys)
                {
                    currentOptions[i] = key;
                    buttons[i].interactable = true;
                    dialogueAndOpts[i + 1].text = key;
                    i++;
                }

                EnableInteraction();
                dialogueAndOpts[0].text = formatNewLine(currentDialogue[0]);
                if(currentDialogue.Length > 1)
                {
                    DisableInteraction();
                    nextLine = 1;
                }
                return;
            }
        }
        print("New Chunk Failed, we'll get em next time");
    }

    public void EnableInteraction()
    {
        for(int i = 0; i < currentOptions.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
        buttons[4].gameObject.SetActive(true);
        buttons[5].gameObject.SetActive(true);
    }

    public void DisableInteraction()
    {
        for (int i = 0; i < currentOptions.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        buttons[4].gameObject.SetActive(false);
        buttons[5].gameObject.SetActive(false);
    }

    public void returnToMainView()
    {
        mainView.SetActive(true);
        askView.SetActive(false);
    }
}

public struct DialogueChunk
{
    public DialogueChunk(Dictionary<string, string[]> header, Dictionary<string, string[]> mainDialogue)
    {
        headerDialogue = header;
        dialogueOptions = mainDialogue;
        chosenOptions = new bool[mainDialogue.Count];
        for(int i = 0; i > mainDialogue.Count; i++)
        {
            chosenOptions[i] = false;
        }
    }
    public bool[] chosenOptions { get; set; }
    public Dictionary<string, string[]> headerDialogue { get; }
    public Dictionary<string, string[]> dialogueOptions { get; }

    public override string ToString()
    {
        string script = "[Header]";
        foreach(string question in headerDialogue.Keys)
        {
            script += (" \n Question: " + question);
        }
        script += " \n [Response] ";
        foreach(string[] lines in headerDialogue.Values)
        {
            foreach(string line in lines) script += (" \n " + line);
            break;
        }
        return script;
    }
}
