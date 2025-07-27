namespace PressureTest.Domains
{
    public class TitleContent
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public TitleContent(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}