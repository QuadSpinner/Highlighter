namespace Highlighter.Core
{
    [Serializable]
    public enum TagShape
    {
        Tag,
        TagUnder,
        Line,
        LineUnder
    }

    [Serializable]
    public enum BlurIntensity
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Ultra = 4
    }
    
    public enum Performance
    {
        Normal,
        Fast,
        NoEffects
    }
}