using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class NPKBundle {
    public static string outputPath = Application.dataPath + "/Output/";

    public AssetBundle assetBundle;
    private Dictionary<Texture2D, Texture2D> texureMap = new Dictionary<Texture2D, Texture2D>();

    public NPKBundle(string path) {
        this.assetBundle = AssetBundle.LoadFromFile(path);
        this.assetBundle.name = this.ToName(this.assetBundle.name) + "/";
    }

    private string ToName(string path) {
        var pos = path.LastIndexOf('.');

        return path.Substring(0, pos);
    }

    private string ToNumber(string name) {
        var pos = name.LastIndexOf('_');

        return Convert.ToInt32(name.Substring(pos + 1)).ToString();
    }

    private Texture2D GetTexture(Texture2D tex){
        if (!this.texureMap.ContainsKey(tex)) {
            if (tex.width == 0 || tex.height == 0) {
                return null;
            }

            var copyTex = new Texture2D(tex.width, tex.height, tex.format, tex.mipmapCount > 1);
            copyTex.LoadRawTextureData(tex.GetRawTextureData());
            copyTex.Apply();
            var writeTex = new Texture2D(copyTex.width, copyTex.height);
            writeTex.SetPixels32(copyTex.GetPixels32());
            writeTex.Apply();
            writeTex.name = this.ToName(tex.name);

            this.texureMap[tex] = writeTex;
        }

        return this.texureMap[tex];
    }

    private string GetPath() {
        return NPKBundle.outputPath + this.assetBundle.name;
    }

    private void CreateDirectory(string path) {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }

    public void WriteTexure() {
        if (this.assetBundle.isStreamedSceneAssetBundle) {
            return;
        }

        var texs = this.assetBundle.LoadAllAssets<Texture2D>();

        foreach (var tex in texs) {
            var writeTex = this.GetTexture(tex);

            if (writeTex != null) {
                var path = this.GetPath();
                var bytes = writeTex.EncodeToPNG();
                this.CreateDirectory(path);
                File.WriteAllBytes(path + writeTex.name + ".png", bytes);
            }
        }
    }

    public void WriteSprite() {
        if (this.assetBundle.isStreamedSceneAssetBundle) {
            return;
        }

        var assets = this.assetBundle.LoadAllAssets<DNFAtlasAsset>();

        foreach (var asset in assets) {            
            for (int i = 0; i < asset.atlasSlotList.Length; i++) {
                var tex = this.assetBundle.LoadAsset<Texture2D>(asset.name + "_" + i.ToString());
                if (tex == null) {
                    continue;
                }
                tex = this.GetTexture(tex);
                if (tex == null) {
                    continue;
                }

                foreach (var element in asset.atlasSlotList[i].elementList) {
                    int width = (int)element.rect.width;
                    int height = (int)element.rect.height;

                    if (width == 0 || height == 0) {
                        continue;
                    }

                    var colors = tex.GetPixels((int)element.rect.x, (int)element.rect.y, width, height);
                    var newTex = new Texture2D(width, height, tex.format, tex.mipmapCount > 1);
                    newTex.SetPixels(colors);
                    newTex.Apply();
                    
                    var path = this.GetPath() + tex.name + "/";
                    var bytes = newTex.EncodeToPNG();
                    var json = JsonUtility.ToJson(element);
                    var name = this.ToNumber(element.name);

                    this.CreateDirectory(path);
                    File.WriteAllText(path + name + ".json", json);
                    File.WriteAllBytes(path + name + ".png", bytes);
                }
            }
        }
    }
}
