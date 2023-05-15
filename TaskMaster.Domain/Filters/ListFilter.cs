namespace TaskMaster.Domain.Filters
{
    public class ListFilter
    {
        public int Page { get; set; }
        public int PageLimit { get; set; }

        public int GetPage()
        {
            return (Page < 1) ? 1 : Page;
        }

        public int GetPageLimit()
        {
            if (PageLimit > 50)
            {
                return 50;
            }

            if (PageLimit < 1)
            {
                return 30;
            }

            return PageLimit;
        }
    }
}
