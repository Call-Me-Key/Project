using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Normal
}

public class DamageTypeStruct
{
    public Dictionary<DamageType, int> savedDamage = new Dictionary<DamageType, int>();

    public int Normalized()
    {
        int ret = 0;
        foreach (KeyValuePair<DamageType, int> item in savedDamage)
        {
            ret += item.Value;
        }
        return ret;
    }



    public static DamageTypeStruct operator - (DamageTypeStruct dt1, DamageTypeStruct dt2)
    {
        DamageTypeStruct ret = new DamageTypeStruct();
        foreach (KeyValuePair<DamageType, int> item in dt2.savedDamage)
        {
            if(dt1.savedDamage.ContainsKey(item.Key))
                ret.savedDamage.Add(item.Key, dt1.savedDamage[item.Key] - dt2.savedDamage[item.Key]);
            else
                ret.savedDamage.Add(item.Key, dt2.savedDamage[item.Key]);
        }
        return ret;
    }


    public static DamageTypeStruct operator +(DamageTypeStruct dt1, DamageTypeStruct dt2)
    {
        DamageTypeStruct ret = new DamageTypeStruct();
        foreach (KeyValuePair<DamageType, int> item in dt1.savedDamage)
        {
            ret.savedDamage.Add(item.Key, item.Value);
        }
        foreach (KeyValuePair<DamageType, int> item in dt2.savedDamage)
        {
            if (ret.savedDamage.ContainsKey(item.Key))
                ret.savedDamage.Add(item.Key, dt1.savedDamage[item.Key] - dt2.savedDamage[item.Key]);
        }
        return ret;
    }
}
