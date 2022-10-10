using Highlighter.Core;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

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
        private string path = null;

        public Options()
        {
            //Debug.WriteLine("LOADED!");
            VS.Events.SolutionEvents.OnBeforeOpenSolution += SolutionEvents_OnBeforeOpenSolution;
            VS.Events.SolutionEvents.OnAfterCloseSolution += SolutionEvents_OnAfterCloseSolution;

            Saved += Options_Saved;
        }

        private void SolutionEvents_OnAfterCloseSolution()
        {
            // Remove any loaded solution tags
            SolutionTags = Array.Empty<HighlightTag>();

            // Remove the path
            path = null;
        }

        private void Options_Saved(Options obj)
        {
            if (path == null)
                return;

            try
            {
                if (SolutionTags != null && SolutionTags.Any())
                {
                    var xs = new XmlSerializer(SolutionTags.GetType());
                    // Serialize
                    using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    xs.Serialize(fs, SolutionTags);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SolutionEvents_OnBeforeOpenSolution(string obj)
        {
            // Assign path to the local Highlighter file
            path = $"{Path.GetDirectoryName(obj)}\\.vs\\Highlighter.xml";

            if (File.Exists(path))
            {
                // Load into SolutionTags
                var xs = new XmlSerializer(SolutionTags.GetType());

                // Deserialize
                using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                SolutionTags = xs.Deserialize(fs) as HighlightTag[];
            }
            else
            {
                // If none exist, clear SolutionTags
                SolutionTags = Array.Empty<HighlightTag>();
            }
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