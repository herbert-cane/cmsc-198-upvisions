using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string mapBoundary;

    // Add all player stats to be saved
    public string playerName;
    public string org;
    public string academicProgram;
    public float energy;
    public float sanity;
    public float stress;
    public float focus;
    public float knowledge;
    public float socialLife;
    public float finances;
    public float health;
    public float sleep;
    public float motivation;
    public float luck;
    public float procrastinationResistance;
}