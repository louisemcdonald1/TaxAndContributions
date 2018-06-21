using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MethodReturn
/// </summary>
public class MethodReturn
{
    public double returnValue = 0;
    public string errorMessage = "";

    public MethodReturn() { }

    public MethodReturn(double returnValue, string errorMessage)
    {
        this.returnValue = returnValue;
        this.errorMessage = errorMessage;
    }
}