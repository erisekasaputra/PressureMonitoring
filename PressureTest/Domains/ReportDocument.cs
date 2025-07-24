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
        public ReportDocument(ExportData exportData)
        {
            _exportData = exportData;
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
                        .Text($"Invoice #123")
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"123");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Due date: ").SemiBold();
                        text.Span($"123");
                    });
                });

                row.ConstantItem(100).Height(50).Placeholder();
            });
        }


        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new AddressComponent("From"));
                    row.ConstantItem(50);
                    row.RelativeItem().Component(new AddressComponent("For"));
                });

                column.Item().Element(ComposeTable);

                var totalPrice = 1000;
                column.Item().AlignRight().Text($"Grand total: {totalPrice}$").FontSize(14);

                if (!string.IsNullOrWhiteSpace("Koment"))
                    column.Item().PaddingTop(25).Element(ComposeComments);
            });
        }


        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Product");
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                    header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                for (int i = 0; i < 10; i ++)
                {
                    table.Cell().Element(CellStyle).Text((i + 1).ToString());
                    table.Cell().Element(CellStyle).Text("Eris");
                    table.Cell().Element(CellStyle).AlignRight().Text($"1000$");
                    table.Cell().Element(CellStyle).AlignRight().Text(10.ToString());
                    table.Cell().Element(CellStyle).AlignRight().Text($"1000$");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Comments").FontSize(14);
                column.Item().Text("Coment");
            });
        }
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }

        public AddressComponent(string title)
        {
            Title = title;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                column.Item().Text("COMPANY NAME");
                column.Item().Text("STREET");
                column.Item().Text($"Bekasi");
                column.Item().Text("Email");
                column.Item().Text("Phoe");
            });
        }
    }
}