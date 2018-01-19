using UnityEngine;
using System.IO;

public class Test : MonoBehaviour {
    public string outputPath;
    public TextAsset file;

    protected void Start() {
        //BundleLoader.outputPath = outputPath;
        var assetBundle = BundleLoader.LoadAssetBundle(file.bytes);
        //var assets = assetBundle.LoadAsset<DNFAtlasAsset>("assets/bundle/sprite/monster/bwanga/bwanga.img.asset");
    } 
}
