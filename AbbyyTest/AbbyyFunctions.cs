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
                    for (var i = 0; i < paragraph.Words.Count; i++)
                    {
                        var word = paragraph.Words[i];
                        for (var iChar = word.FirstSymbolPosition;
                            iChar < word.FirstSymbolPosition + word.Text.Length;
                            iChar++)
                        {
                            paragraph.GetCharParams(iChar,engine.CreateCharParams());
                        }
                    }
                }
            }
        }

        document.Close();
    }
}