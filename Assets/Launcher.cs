using System.IO;
using UnityEngine;
using UnityEditor;

public class Launcher : MonoBehaviour {
    private bool willOutputTexture = true;
    private string outputPath;

    private string SaveFolderPanel() {
        var path = EditorUtility.SaveFolderPanel("Select Saving Folder", "", "");
        NPKBundle.outputPath = path + "/";

        return path;
    }

    private void Result() {
        EditorUtility.DisplayDialog("Message", "Completed!", "OK");
    }

    private void Save(string path) {
        var bundle = new NPKBundle(path);
        
        try {
            if (this.willOutputTexture) {
                bundle.WriteTexure();
            }

            bundle.WriteSprite();
        }
        catch {
            Debug.Log("error: " + path);
        }
    }

    protected void OnGUI() {
        if (GUILayout.Button("Open Bundle")) {
            var filePath = EditorUtility.OpenFilePanel("Select Assets Bundle", "", "");
            
            if (filePath.Length != 0 && this.SaveFolderPanel().Length != 0) {
                this.Save(filePath);
                this.Result();
            }
        }
        
        if (GUILayout.Button("Open Bundles")) {
            var filePath = EditorUtility.OpenFolderPanel("Select Assets Bundle Folder", "", "");
        
            if (filePath.Length != 0 && this.SaveFolderPanel().Length != 0) {
                var info = new DirectoryInfo(filePath);

                foreach (var file in info.GetFiles()) {
                    this.Save(file.FullName);
                }

                this.Result();
            }
        }

        this.willOutputTexture = GUILayout.Toggle(this.willOutputTexture, "Output Texture");
    }
}
