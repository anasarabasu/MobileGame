using TMPro;
using UnityEngine;

public class Cutscene : MonoBehaviour, ISaveLoad {
    [SerializeField] TextMeshProUGUI screenText;
    
    private bool newGame;
    private string[] dialogueList = {
        "introductory cutscene/ dialogue",
        "Hey",
        "Go collect some stones or minerals or gems",
        "or whatever",
        "I don't know where they are so you have to find it yourself hehe :D"
    };
    private int dialogueIndex;

    private void Update() {
        screenText.text = dialogueList[dialogueIndex];
    }

    public void _Continue() { //dialogue step incrementor TEMP
        //TODO: if newgame -> intro cutscene | else -> lastsaved scene
        if(dialogueIndex+1 < dialogueList.Length) {
            dialogueIndex++;
        }
        else 
            _FinishCutscene();
        
        Debug.Log(dialogueIndex);
    }

    public void _FinishCutscene() { //toggles newgame to false TEMP
        newGame = false;
        DataManager.instance.SaveGame();
        SceneManager.instance.LoadScene("Overworld");
    }

    public void Save(ref DataRoot data) {
        data.gameData.newGame = this.newGame;
        data.gameData.dialogueIndex = this.dialogueIndex;
    }

    public void Load(DataRoot data) {
        this.newGame = data.gameData.newGame;
        this.dialogueIndex = data.gameData.dialogueIndex;
    }
}
