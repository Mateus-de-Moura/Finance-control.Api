namespace finance_control.Api.Commom.ViewModels
{
    public class GetPagedTransactionsViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
