namespace BackgroundService.Entity
{
    public class ExpensesComprovant : BaseEntity
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
    }
}
