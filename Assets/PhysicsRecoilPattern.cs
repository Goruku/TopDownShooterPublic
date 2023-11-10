using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PhysicsRecoilPattern : ScriptableObject
{
    public float recoilRandomness;
    public List<Vector2> recoils;
    public AnimationCurve recoilRandomnessSpread;
    public List<float> torques;
    public AnimationCurve torqueRandomnessSpread;
    
    public int currentIndex;
    
    public struct PhysicsRecoil
    {
        public Vector2 force;
        public float torque;
    }
    
    public PhysicsRecoil GetNext()
    {
        if (currentIndex >= recoils.Count - 1 && currentIndex >= torques.Count - 1)
            currentIndex = 0;
        else
            currentIndex += 1;
        return new PhysicsRecoil(){
            force = currentIndex < recoils.Count ? recoils[currentIndex]
                                                    * recoilRandomnessSpread.Evaluate(Random.Range(0, recoilRandomness)) : new Vector2(),
            torque = currentIndex < torques.Count ? torques[currentIndex]
                                                    * torqueRandomnessSpread.Evaluate(Random.Range(0, recoilRandomness)): 0
        };
    }
}
