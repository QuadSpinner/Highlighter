![](images/cover-git.png)

# QuadSpinner Highlighter for Visual Studio 2022

An arbitrary highlighter that lets you pick individual class, parameter, or other element names to highlight across your projects. It makes it easier to find and identify objects inside your project. You can also use it for comment annotations, dividers, and more.

Free download on [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=DaxPandhi.Highlighter1)

**Features**
- Highlight any arbitrary term
- 4 different highlight shapes
- Toggle highlights easily
- Performant even on older machines and VMs

> Requires Visual Studio 2022 Community Edition or higher. It should work in all text editor surfaces.

![](images/example_code.png)

**You can pick any word. Highlighter does not care about its meaning to the programming language, just what is on the screen.**

## Creating Highlights

![](images/menu.png)

To create a highlight, select the desired word or phrase and press `Ctrl + Num Plus`. Or you can right-click the selection and click `Create/Edit Highlight`.

If the selection is an existing highlight, then you will be able to edit it. If it is a new one, you get to define its shape.

![](images/editor.png)

You can choose the tag shape, optional blur, and color. The Highlight Editor window shows you a preview to help you choose the right shape.

For an existing rule, you can also uncheck "Rule is Active" to disable the rule without deleting it. To delete a rule, see Options below.

## Highlight Shapes

Highlighter can create 4 different shapes, 4 option blur levels, using 40 different colors. You can also enter a hex value manually in the Options. These colors are chosen specifically to give you decent visibility whether using a dark or light theme.

There are 12 built in rules for creating dividers. `//1` through `//6` and `//#` through `//######` create full line dividers. You modify these rules or make your own.

| Dark | Light|
| --- | --- |
| ![](images/example_dark.png) | ![](images/example_light.png) | 
| ![](images/dividers_dark.png) | ![](images/dividers_light.png) |

## Options

![](images/options.png)

The Options let you edit the rules en mass, as well as use color values not in the default palette. You can delete highlights here.

# Coming Soon

## Solution-scoped rules

Highlight rules scoped to the current solution and made portable for transport.

More options.

