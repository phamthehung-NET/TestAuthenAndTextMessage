namespace TestAuthenAndTextMessage.Utilities
{
    public class Pagination<T>
    {
        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int? ItemPerPage { get; set; }

        public int? PageIndex { get; set; }

        public int? NextPage { get; set; }

        public int? PrevPage { get; set; }

        public List<T> Items { get; set; }

        public Pagination(int totalItems, int? pageIndex, int? itemPerPage, IQueryable<T> items)
        {
            if (itemPerPage <= 0)
            {
                itemPerPage = 1;
            }

            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)itemPerPage);

            var nextPage = pageIndex + 1;

            var prevPage = pageIndex - 1;


            if (pageIndex < 1 || totalPages == 0)
            {
                pageIndex = 1;
                prevPage = pageIndex;
            }
            else if (pageIndex == totalPages)
            {
                nextPage = pageIndex;
            }
            else if (pageIndex > totalPages)
            {
                pageIndex = totalPages;
                nextPage = totalPages;
                prevPage = totalPages - 1;
            }
            else if (pageIndex == 1)
            {
                prevPage = pageIndex;
            }

            items = items.Skip((int)((pageIndex - 1) * itemPerPage)).Take((int)itemPerPage);

            TotalItems = totalItems;
            TotalPages = totalPages;
            PageIndex = pageIndex;
            ItemPerPage = itemPerPage;
            NextPage = nextPage;
            PrevPage = prevPage;
            Items = items.ToList();
        }

        public Pagination()
        {
            TotalItems = 0;
            TotalPages = 0;
            PageIndex = 1;
            ItemPerPage = 10;
            NextPage = 1;
            PrevPage = 1;
            Items = new();
        }
    }
}
