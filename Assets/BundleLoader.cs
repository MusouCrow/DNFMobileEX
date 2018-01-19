using System.IO;
using UnityEngine;
using Unity_Studio;
using U_MonoBehaviour = Unity_Studio.MonoBehaviour;

public class BundleLoader {
    public static string outputPath = Application.dataPath + "/Output/";

    public static AssetBundle LoadAssetBundle(byte[] bytes) {
        var assetBundle = AssetBundle.LoadFromMemory(bytes);
        var pos = assetBundle.name.LastIndexOf('.');
        assetBundle.name = assetBundle.name.Substring(0, pos) + "/";

        return assetBundle;
    }

    public static void Test(string path) {
        UnityStudio.LoadBundleFile(path);
        
        foreach (var assetsfile in UnityStudio.assetsfileList) {
            foreach (var asset in assetsfile.preloadTable.Values) {
                if (asset.Type2 == 114) {
                    U_MonoBehaviour mb = new U_MonoBehaviour(asset, true);
                    Debug.Log(mb.serializedText);
                }
            }
        }
    }

    public static void WriteTexure(AssetBundle assetBundle) {
        var texs = assetBundle.LoadAllAssets<Texture2D>();

        for (int i = 0; i < texs.Length; i++) {
            var copyTex = new Texture2D(texs[i].width, texs[i].height, texs[i].format, texs[i].mipmapCount > 1);
            copyTex.LoadRawTextureData(texs[i].GetRawTextureData());
            copyTex.Apply();
            var writeTex = new Texture2D(copyTex.width, copyTex.height);
            writeTex.SetPixels32(copyTex.GetPixels32());
            writeTex.Apply();

            var path = BundleLoader.outputPath + assetBundle.name;
            var bytes = writeTex.EncodeToPNG();

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes(path + texs[i].name + ".png", bytes);
        }
    }

    public static void WriteSprite(AssetBundle assetBundle) {
        
    }
}

