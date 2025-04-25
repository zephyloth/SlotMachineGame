using System.Text;
using UnityEngine;

public class LogManager : Singleton<LogManager>
{
    //Used SB for memory efficiency
    private StringBuilder SB = new StringBuilder();

    public void Log(object Message, LogLevel Level = LogLevel.Info)
    {
        SB.Clear();
        switch (Level)
        {
            case LogLevel.Info:
                SB.Append(GenericUtils.ColorizeText("[INFO] ", "white"));
                SB.Append(Message);

                Debug.Log(SB.ToString());
                break;
            case LogLevel.Warning:
                SB.Append(GenericUtils.ColorizeText("[WARN] ", "yellow"));
                SB.Append(Message);

                Debug.LogWarning(SB.ToString());
                break;
            case LogLevel.Error:
                SB.Append(GenericUtils.ColorizeText("[ERROR] ", "red"));
                SB.Append(Message); 
                
                Debug.LogError(SB.ToString());
                break;
        }
    }

    public void LogInfo(object message) => Log(message, LogLevel.Info);
    public void LogWarning(object message) => Log(message, LogLevel.Warning);
    public void LogError(object message) => Log(message, LogLevel.Error);
}

public enum LogLevel
{
    Info,
    Warning,
    Error
}