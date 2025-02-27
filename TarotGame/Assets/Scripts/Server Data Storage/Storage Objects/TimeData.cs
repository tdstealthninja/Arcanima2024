using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimeData : IData
{
    [SerializeField]
    private float seconds;

    public float Seconds
    {
        get { return seconds; }
        set { seconds = value; }
    }

    public float Minutes
    {
        get { return Seconds / 60; }
        set { Seconds = value * 60; }
    }

    public float Hours
    {
        get { return Minutes / 60; }
        set { Minutes = value * 60; }
    }

    public float Days
    {
        get { return Hours / 24; }
        set { Hours = value * 24; }
    }

    public TimeData(float seconds)
    {
        Seconds = seconds;
    }

    public override string ToString()
    {
        return "(TimeData)[seconds: " + seconds + " ]";
    }

    /// <summary>
    /// dd hh:mm:ss
    /// </summary>
    /// <param name="format"></param>
    /// <returns>Formatted time.</returns>
    public string ToFormatedString()
    {
        // set up format
        string format = "dd hh:mm:ss";

        // get correct values
        int seconds = Mathf.FloorToInt(Seconds % 60);
        int minutes = Mathf.FloorToInt(Minutes % 60);
        int hours = Mathf.FloorToInt(Hours % 24);
        int days = Mathf.FloorToInt(Days);

        // convert to strings
        string secondString = seconds.ToString();
        string minuteString = minutes.ToString();
        string hourString = hours.ToString();
        string dayString = days.ToString();

        // clean up format
        secondString = (secondString.Length == 1 ? "0" + secondString : secondString);
        minuteString = (minuteString.Length == 1 ? "0" + minuteString : minuteString);
        hourString = (hourString.Length == 1 ? "0" + hourString : hourString);

        // add to format
        format.Replace("dd", dayString);
        format.Replace("hh", hourString);
        format.Replace("mm", minuteString);
        format.Replace("ss", secondString);

        // return
        return format;
    }
}
