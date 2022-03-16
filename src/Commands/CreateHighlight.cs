using System.Linq;
using EnvDTE;
using Highlighter.Core;

namespace Highlighter.Commands
{
    [Command(PackageIds.CreateHighlight)]
    internal sealed class CreateHighlight : BaseCommand<CreateHighlight>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var dte = (DTE) Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE));
            string selection = (dte.ActiveDocument.Selection as TextSelection).Text;

            if (string.IsNullOrEmpty(selection) || string.IsNullOrWhiteSpace(selection))
                return;

            var options = Options.GetLiveInstanceAsync().Result;
            options.ColorTags ??= Helper.GetFillerTags().ToArray();

            var tags = options.ColorTags.ToList();
            var found = tags.FirstOrDefault(x => x.Criteria == selection);
            bool isNew = false;

            if (found == null)
            {
                isNew = true;
                // CREATE NEW
                tags.Add(new HighlightTag(selection));
                found = tags.FirstOrDefault(x => x.Criteria == selection);
            }

            var editor = new EditColor
            {
                TagToEdit = found,
                btnModify = {Content = isNew ? "Create" : "Modify"},
                Title = isNew ? "Create Highlight Rule" : "Modify Highlight Rule"
            };

            bool result = editor.ShowDialog().Value;

            if (result)
            {
                options.ColorTags = tags.ToArray();
                await options.SaveAsync();
            }
        }
    }
}