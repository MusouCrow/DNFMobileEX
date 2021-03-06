using System;
using System.IO;
using System.Threading;
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
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("Message", "Completed!", "OK");
    }

    private void ShowBar(float process, int now, int max) {
        EditorUtility.DisplayProgressBar("", string.Format("Loading...({0}/{1})", now, max), process);
    }

    private void Save(string path) {
        try {
            var bundle = new NPKBundle(path);
            
            if (this.willOutputTexture) {
                bundle.WriteTexure();
            }

            bundle.WriteSprite();
            bundle.Destroy();
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
                var files = info.GetFiles();
                this.ShowBar(0, 0, files.Length);

                for (int i = 0; i < files.Length; i++) {
                    /*
                    if (files[i].Name.Substring(0, 1) == ".") {
                        continue;
                    }
                    */
                    this.ShowBar((float)i / (float)files.Length, i, files.Length);
                    this.Save(files[i].FullName);

                    if (i % 100 == 0) {
                        GC.Collect();
                        Thread.Sleep(1000);
                    }
                }

                this.Result();
            }
        }

        this.willOutputTexture = GUILayout.Toggle(this.willOutputTexture, "Output Texture");
    }

    protected void OnDestroy() {
        EditorUtility.ClearProgressBar();
    }
}
