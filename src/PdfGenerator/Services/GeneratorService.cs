﻿using PdfGenerator.DTOs;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;
public interface IGeneratorService
{
  byte[] Generate(GenerationDTO data);
}

public class GeneratorService : IGeneratorService
{
  public byte[] Generate(GenerationDTO data)
  {
    return Document.Create(c =>
    {
      c.Page(p =>
      {
        p.Margin(50);
        p.Size(data.Width, data.Height, Unit.Point);
        p.DefaultTextStyle(t => t.FontSize(30));

        p.Content().Column(c =>
        {
          foreach (var i in Enumerable.Range(0, data.PageCount))
          {
            c.Item().Text(Placeholders.Sentence());
            if (i < data.PageCount - 1)
              c.Item().PageBreak();
          }
        });
        p.Footer().AlignCenter().Text(t =>
        {
          t.CurrentPageNumber();
          t.Span(" / ");
          t.TotalPages();
        });
      });
    }).WithMetadata(new DocumentMetadata()
    {
      DocumentLayoutExceptionThreshold = 2500,
    }).GeneratePdf();
  }
}
