using System.ComponentModel.DataAnnotations.Schema;

namespace TaskAPI.Data {
    [Table("tasks")]
    public record Task (int Id, string Name, bool IsFinished);
}
