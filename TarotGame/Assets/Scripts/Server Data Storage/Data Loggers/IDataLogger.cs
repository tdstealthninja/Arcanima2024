using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataLogger
{
    /// <summary>
    /// Stores data
    /// </summary>
    public void Log();

    /// <summary>
    /// Returns logged data
    /// </summary>
    /// <returns></returns>
    public IData Return();
}
