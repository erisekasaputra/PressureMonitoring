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
        private string _signed;

        public ReportDocument(
            ExportData exportData,
            string titleSection1,
            string titleSection2,
            List<TitleContent> titleContentsSection1,
            List<TitleContent> titleContentsSection2,
            string signed)
        {
            _exportData = exportData;
            _titleSection1 = titleSection1;
            _titleSection2 = titleSection2;
            _titleContentsSection1 = titleContentsSection1;
            _titleContentsSection2 = titleContentsSection2;
            _signed = signed;
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
                // LOGO di kiri (tetap ukurannya fixed)
                row.ConstantItem(150).Column(column =>
                {
                    column.Item().Height(50).Image("TAMFINDO_LOGO.jpeg").FitArea();

                    column.Item().Text("PT. Tamfindo Mitra Mandiri").FontSize(9);

                    column.Item().Text("https://www.infotamfindo.com/")
                        .FontSize(9)
                        .FontColor(Colors.Blue.Medium)
                        .Underline();
                });

                // TITLE di kanan (fleksibel)
                row.RelativeItem().Column(column =>
                {
                    column.Item()
                        .AlignRight() // agar teks rata kanan
                        .Text($"Chart Record Test")
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium); 
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


                column.Item().PaddingTop(30).Element(ComposeSignature);
            });
        }

        void ComposeSignature(IContainer container)
        {
            container.AlignRight().Width(200).Column(column =>
            {
                column.Item().BorderBottom(1).Height(20); // garis tanda tangan tanpa ExtendVertical()
                column.Item().Text($"Signed : {_signed}").FontSize(10).AlignCenter(); // nama
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