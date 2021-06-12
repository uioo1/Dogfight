using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct UdpPack
{
    public int cmd; //this is drivven from stati variable
    public Transform transform;
    public bool isfliped;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
    public string textString; // incase of a Text Addon 
}