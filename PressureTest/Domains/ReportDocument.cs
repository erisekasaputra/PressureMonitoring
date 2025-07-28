using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PressureTest.Domains
{
    public class ReportDocument : IDocument
    {
        private readonly ExportData _exportData;
        private string _titleSection1;
        private string _titleSection2;
        private List<TitleContent> _titleContentsSection1;
        private List<TitleContent> _titleContentsSection2;

        public ReportDocument(
            ExportData exportData,
            string titleSection1,
            string titleSection2,
            List<TitleContent> titleContentsSection1,
            List<TitleContent> titleContentsSection2)
        {
            _exportData = exportData;
            _titleSection1 = titleSection1;
            _titleSection2 = titleSection2;
            _titleContentsSection1 = titleContentsSection1;
            _titleContentsSection2 = titleContentsSection2;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item()
                        .Text($"Test Certificate")
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    //column.Item().Text(text =>
                    //{
                    //    text.Span("Issue date: ").SemiBold();
                    //    text.Span($"123");
                    //}); 
                });

                row.ConstantItem(100).Column(column =>
                {
                    column.Item().Height(50).Image("TAMFINDO_LOGO.jpeg").FitArea(); // Your image
                    //column.Item().Text("Teks di bawah gambar").FontSize(8).AlignCenter(); // Your text below the image
                });
            });
        }


        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {

                if (!string.IsNullOrEmpty(_titleSection1))
                {
                    column.Spacing(20);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Component(new AddressComponent(_titleSection1, [.. _titleContentsSection1]));
                    });
                }

                if (!string.IsNullOrEmpty(_titleSection2))
                {
                    column.Spacing(20);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Component(new AddressComponent(_titleSection2, [.. _titleContentsSection2]));
                    });
                }  

                column.Item().Element(ComposeImageContent); 
            });
        }

        void ComposeImageContent(IContainer container)
        {
            container
               .AlignCenter() // Center the content horizontally
               .AlignMiddle()  // Center the content vertically (optional, depends on overall layout)
               .Image(Path.Combine("Plots", _exportData.ChartImagePath)) // Replace with your actual image path
               .FitWidth();
        } 
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private TitleContent[] TitleContents { get; set; } 

        public AddressComponent(string title, params TitleContent[] titleContents)
        {
            Title = title;
            TitleContents = titleContents; 
        }

        public void Compose(IContainer container)
        { 
            if (string.IsNullOrEmpty(Title))
            {
                return;
            }

            container.Column(column =>
            {
                if (TitleContents.Length > 0) 
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                    foreach(var content in TitleContents)
                    {
                        if (!string.IsNullOrEmpty(content.Title.Trim()))
                        {
                            column.Item().Text($"{content.Title.Trim()}: {content.Content}");
                        }    
                    } 
                }
            });
        }
    }
}