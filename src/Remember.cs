using Highlighter.Core;

namespace Highlighter
{
    public static class Remember
    {
        public static BlurIntensity LastBlur = 0;
        public static TagShape LastShape = 0;
        public static bool LastGlobal = true;
    }
}