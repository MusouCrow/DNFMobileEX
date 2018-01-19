using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNFAtlasAsset : MonoBehaviour {
    [Serializable]
    public struct Rectf {
        public float x;
        public float y;
        public float width;
        public float height;
    }

    [Serializable]
    public struct DNFAtlasElement {
        public string name;
        public int originIndex;
        public int referenceIndex;
        public int originWidth;
        public int originHeight;
        public int offsetX;
        public int offsetY;
        public Rectf rect;
    }

    [Serializable]
    public struct DNFAtlasSlot {
        public int matType;
        public DNFAtlasElement[] elementList;
    }

    public string atalsName;
    public DNFAtlasSlot[] atlasSlotList;
}
