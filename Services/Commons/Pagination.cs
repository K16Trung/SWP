namespace Services.Commons
{
    public class Pagination<T>
    {
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPagesCount
        {
            get
            {
                var temp = TotalItemCount / PageSize;
                if (TotalItemCount % PageSize == 0)
                {
                    return temp;
                }
                return temp + 1;
            }
        }
        public int PageIndex { get; set; }
        public bool Next => PageIndex + 1 < TotalPagesCount;
        public bool Previous => PageIndex > 0;
        public ICollection<T> Items { get; set; }
    }
}
