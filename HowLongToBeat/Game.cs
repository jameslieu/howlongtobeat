namespace HowLongToBeat
{
    public class Game
    {
        public string Title { get; }
        public string ImgURL { get; }
        public string Main { get; }
        public string MainAndExtras { get; }
        public string Completionist { get; }

        public Game(string title, string main, string mainandextras, string completionist, string imgURL)
        {
            Title = title;
            ImgURL = imgURL;
            Main = main;
            MainAndExtras = mainandextras;
            Completionist = completionist;
        }
    }
}