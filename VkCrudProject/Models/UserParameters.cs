namespace VkCrudProject.Models
{
    public class UserParameters
    {
        const int maxPageSize = 200;
        public int PageNumber { get; set; }
        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
