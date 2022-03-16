using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Highlighter.Core
{
    internal sealed class Adorner
    {
        private const double cornerRadius = 2.0;

        private readonly IAdornmentLayer layer;

        private readonly IWpfTextView view;

        private Thickness tBlur = new(2, -3, 2, -3);
        private Thickness tNone = new(0, 0, 0, 0);
        private Options options;
        private char[] firstChars;
        private IEnumerable<HighlightTag> tags;
        private Performance performance = Performance.Normal;

        public Adorner(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            Helper.InitDefaults();

            options = Options.GetLiveInstanceAsync().Result;

            if (options.ColorTags == null || !options.ColorTags.Any())
            {
                options.ColorTags = Helper.GetFillerTags().ToArray();
                options.Save();
            }

            Options.Saved += HighlighterOptions_Saved;

            RefreshCriteria();

            layer = view.GetAdornmentLayer("HighlighterHighlighter");

            this.view = view;
            this.view.LayoutChanged += OnLayoutChanged;
        }

        private void HighlighterOptions_Saved(Options obj) => RefreshCriteria();

        private void RefreshCriteria()
        {
            performance = options.Performance;
            tags = options.ColorTags.Where(x => x.IsActive);
            //string path = Path.GetDirectoryName(VS.Solutions.GetCurrentSolution().FullPath) + "\\.vs\\Highlighter.xml";
            firstChars = options.ColorTags.Select(k => k.Criteria[0]).Distinct().ToArray();
        }

        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (tags == null || !tags.Any())
                return;

            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                CreateVisuals(line);
            }
        }

        private void CreateVisuals(ITextViewLine line)
        {
            //# Grab a reference to the lines in the current TextView
            IWpfTextViewLineCollection textViewLines = view.TextViewLines;
            int start = line.Start;
            int end = line.End;
            List<Geometry> geometries = new();

            //## Main Loop
            for (int i = start; i < end; i++)
            {
                if (firstChars.Contains(view.TextSnapshot[i]))
                {
                    foreach (var tag in tags)
                    {
                        string keyword = tag.Criteria.Trim();

                        if (view.TextSnapshot[i] == keyword[0] &&
                            i <= end - keyword.Length &&
                            view.TextSnapshot.GetText(i, keyword.Length) == keyword &&
                            Helper.escapes.Contains(Convert.ToChar(view.TextSnapshot.GetText(Math.Max(0, i - 1), 1))) &&
                            Helper.escapes.Contains(Convert.ToChar(view.TextSnapshot.GetText(i + keyword.Length, 1))))
                        {
                            SnapshotSpan span = new(view.TextSnapshot, Span.FromBounds(i, i + keyword.Length));

                            Geometry markerGeometry = textViewLines.GetMarkerGeometry(span, true, tag.Blur == BlurIntensity.None ? tNone : tBlur);

                            if (markerGeometry != null)
                            {
                                if (!geometries.Any(g => g.FillContainsWithDetail(markerGeometry) >
                                                         IntersectionDetail.Empty))
                                {
                                    geometries.Add(markerGeometry);
                                    AddMarker(span, markerGeometry, tag);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddMarker(SnapshotSpan span, Geometry markerGeometry, HighlightTag ct)
        {
            Rectangle r = new()
            {
                Fill = new SolidColorBrush(ct.Color.ChangeAlpha(60)),
                RadiusX = cornerRadius,
                RadiusY = cornerRadius,
                Width = markerGeometry.Bounds.Width,
                Height = markerGeometry.Bounds.Height,
                Stroke = new SolidColorBrush(ct.Color.ChangeAlpha(100))
            };

            bool isLine = ct.IsLine();

            if (ct.IsUnder())
            {
                r.Height = 4.0;
            }

            if (isLine)
            {
                r.Width = view.ViewportWidth - markerGeometry.Bounds.Left;
            }

            if (performance != Performance.NoEffects && ct.Blur != BlurIntensity.None)
            {
                r.Effect = new BlurEffect
                {
                    KernelType = performance == Performance.Normal ? KernelType.Gaussian : KernelType.Box,
                    RenderingBias = RenderingBias.Performance
                };


                switch (ct.Blur)
                {
                    case BlurIntensity.Low:
                        ((SolidColorBrush)r.Fill).Color.ChangeAlpha(80);
                        ((BlurEffect)r.Effect).Radius = isLine ? 2 : 4.0;
                        break;

                    case BlurIntensity.Medium:
                        ((SolidColorBrush)r.Fill).Color.ChangeAlpha(120);
                        ((BlurEffect)r.Effect).Radius = isLine ? 4 : 7.0;
                        break;

                    case BlurIntensity.High:
                        ((SolidColorBrush)r.Fill).Color.ChangeAlpha(170);
                        ((BlurEffect)r.Effect).Radius = isLine ? 6 : 11.0;
                        break;

                    case BlurIntensity.Ultra:
                        ((SolidColorBrush)r.Fill).Color.ChangeAlpha(255);
                        ((BlurEffect)r.Effect).Radius = isLine ? 8 : 20.0;
                        break;
                }

                r.Stroke = null;

                if (r.Effect.CanFreeze)
                    r.Effect.Freeze();
            }

            // Align the image with the top of the bounds of the text geometry
            Canvas.SetLeft(r, markerGeometry.Bounds.Left);

            if (r.Fill.CanFreeze)
                r.Fill.Freeze();

            if(r.Stroke is {CanFreeze: true})
                r.Stroke.Freeze();
            
            if (ct.IsUnder())
            {
                Canvas.SetTop(r, markerGeometry.Bounds.Top + markerGeometry.Bounds.Height - 2);
            }
            else
            {
                Canvas.SetTop(r, markerGeometry.Bounds.Top);
            }

            layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, r, null);
        }
    }
}