using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldInstructionContainer
{
    private static Dictionary<float, WaitForSeconds> _waitForSecondsDic = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (_waitForSecondsDic.TryGetValue(time, out WaitForSeconds result))
            return result;

        result = new WaitForSeconds(time);
        _waitForSecondsDic.Add(time, result);
        return result;
    }
}
