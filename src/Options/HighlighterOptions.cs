using System.ComponentModel;
using System.Runtime.InteropServices;
using Highlighter.Core;

namespace Highlighter
{
    [ComVisible(true)]
    internal class OptionsProvider
    {
        [ComVisible(true)]
        public class HighlighterOptions : BaseOptionPage<Options>
        {
        }
    }

    [ComVisible(true)]
    public class Options : BaseOptionModel<Options>
    {

        public Options()
        {
            //Debug.WriteLine("LOADED!");
            //VS.Events.SolutionEvents.OnBeforeOpenSolution += SolutionEvents_OnBeforeOpenSolution;

            //Saved += Options_Saved;
        }

        //private void Options_Saved(Options obj)
        //{
        //  string path = Path.GetDirectoryName(VS.Solutions.) + "\\.vs\\Highlighter.xml";
        //  Debug.WriteLine(path);
        //}

        private void SolutionEvents_OnBeforeOpenSolution(string obj)
        {
            //string path = Path.GetDirectoryName(obj) + "\\.vs\\Highlighter.xml";
            //Debug.WriteLine(path);
        }

        [Category("Tags")]
        [DisplayName("Global Rules")]
        [Description("These rules are applied across all projects.")]
        public HighlightTag[] ColorTags { get; set; }

        [Category("Tags")]
        [DisplayName("Solution Rules")]
        [Description("These rules are applied only to the current solution.")]
        public HighlightTag[] SolutionTags { get; set; }


        [Category("Appearance")]
        [DisplayName("Performance")]
        [Description("Choose the performance level.")]
        [DefaultValue(Performance.Normal)]
        [TypeConverter(typeof(EnumConverter))]
        public Performance Performance { get; set; } = Performance.Normal;

    }

}