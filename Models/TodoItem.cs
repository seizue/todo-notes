namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
    public required string Title { get; set; }
    public bool IsComplete { get; set; }
    public string? DueDate { get; set; }
    public string? Tags { get; set; }
    public string? Priority { get; set; }
    public int Order { get; set; }
    }
}
