using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecoilPattern : ScriptableObject
{
    public List<Vector2> recoilDeltas;
    public bool staticPattern;
    public float recoilRandomness;
    public AnimationCurve recoilRandomnessSpread;

    public int currentIndex;

    public Vector2 GetNext()
    {
        if (recoilDeltas.Count <= 0) return new Vector2();
        if (!staticPattern) currentIndex = Random.Range(0, recoilDeltas.Count);
        if (currentIndex >= recoilDeltas.Count - 1)
            currentIndex = 0;
        else
            currentIndex += 1;
        return recoilDeltas[currentIndex]*recoilRandomnessSpread.Evaluate(Random.Range(0, recoilRandomness));
    }
}
