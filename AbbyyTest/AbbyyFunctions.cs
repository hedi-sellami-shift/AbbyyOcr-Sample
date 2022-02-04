using System;
using System.Collections.Generic;
using System.Linq;
using FREngine;


public static class AbbyyFunctions
{
    public static void ProcessDocument(IEngine engine)
    {
        var document = engine.CreateFRDocument();
        document.AddImageFile("Page_01.jpg");
        document.Process();
        document.Close();
    }

    public static void ProcessAndParseDocument(IEngine engine)
    {
        var document = engine.CreateFRDocument();
        document.AddImageFile("Page_01.jpg");
        document.Process();
        foreach (FRPage page in document.Pages)
        {
            foreach (IBlock block in page.Layout.Blocks)
            {
                if (block.Type != BlockTypeEnum.BT_Text)
                    continue;

                ITextBlock textBlock = block.GetAsTextBlock();

                foreach (IParagraph paragraph in textBlock.Text.Paragraphs)
                {
                    var wordsInCurrentLine = new List<string>();
                    var lines = paragraph.Lines.GetEnumerator();
                    lines.MoveNext();
                    var line = (IParagraphLine) lines.Current;
                    for (var i = 0; i < paragraph.Words.Count; i++)
                    {
                        var word = paragraph.Words[i];
                        if (word.Region.IsEmpty)
                            continue;
                        while (word.FirstSymbolPosition >= line.FirstCharIndex + line.CharactersCount)
                        {
                            //We need to get lineBox and lineText
                            int[] lineBox = new int[4]
                                {line.Left, line.Top, line.Right, line.Bottom};
                            var lineText = String.Join(" ", wordsInCurrentLine);
                            lines.MoveNext();
                            line = (IParagraphLine) lines.Current;
                            wordsInCurrentLine = new List<string>();
                        }

                        wordsInCurrentLine.Add(word.Text);
                    }
                }
            }
        }

        document.Close();
    }

}