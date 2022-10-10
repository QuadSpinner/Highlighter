using EnvDTE;
using Highlighter.Core;
using System.Linq;
using System.Windows;

namespace Highlighter.Commands
{
    [Command(PackageIds.CreateHighlight)]
    internal sealed class CreateHighlight : BaseCommand<CreateHighlight>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE));
            string selection = (dte.ActiveDocument.Selection as TextSelection).Text;

            if (string.IsNullOrEmpty(selection) || string.IsNullOrWhiteSpace(selection))
                return;

            var options = Options.GetLiveInstanceAsync().Result;
            options.ColorTags ??= Helper.GetFillerTags().ToArray();

            var tags = options.ColorTags.ToList();
            var slntags = options.SolutionTags?.ToList();
            var found = tags.FirstOrDefault(x => x.Criteria == selection);
            bool isNew = false;
            bool notGlobal = false;

            // If tag is null, do a second search in Solution-level tags
            if (found == null)
            {
                found = slntags.FirstOrDefault(x => x.Criteria == selection);

                if (found != null)
                    notGlobal = true;
            }

            // No tags were found, so make a new tag
            isNew = found == null;

            found ??= new HighlightTag(selection)
            {
                Blur = Remember.LastBlur,
                Shape = Remember.LastShape
            };

            // Prepare the editor
            var editor = new EditColor
            {
                // The tag to edit
                TagToEdit = found,
                // Is it an existing rule?
                btnModify = { Content = isNew ? "Create" : "Modify" },
                // Allow deleting of existing rule
                btnDelete = { Visibility = isNew ? Visibility.Collapsed : Visibility.Visible },
                // Appropriate title
                Title = isNew ? "Create Highlight Rule" : "Modify Highlight Rule",
                // Whether rule is global or not, disallow editing of existing rule
                chkGlobal = { IsChecked = isNew ? Remember.LastGlobal : !notGlobal, IsEnabled = isNew }
            };

            bool result = editor.ShowDialog().Value;

            if (result)
            {
                if (isNew)
                {
                    if (editor.chkGlobal.IsChecked == true)
                    {
                        // CREATE GLOBAL TAG
                        tags.Add(editor.TagToEdit);
                        options.ColorTags = tags.ToArray();
                    }
                    else
                    {
                        // CREATE SOLUTION TAG
                        slntags.Add(editor.TagToEdit);
                        options.SolutionTags = slntags.ToArray();
                    }

                    // Remember user preference
                    Remember.LastGlobal = editor.chkGlobal.IsChecked == true;
                    Remember.LastBlur = found.Blur;
                    Remember.LastShape = found.Shape;
                }

                // Save to options
                await options.SaveAsync();
            }
            else
            {
                // User wants to delete the rule
                if (editor.delete)
                {
                    // Check if it is a global rule
                    if (notGlobal)
                    {
                        // It is a solution-level rule
                        slntags.Remove(found);
                        options.SolutionTags = slntags.ToArray();
                    }
                    else
                    {
                        // It is a global rule
                        tags.Remove(found);
                        options.ColorTags = tags.ToArray();
                    }

                    // Save to options
                    await options.SaveAsync();
                }
            }
        }
    }
}