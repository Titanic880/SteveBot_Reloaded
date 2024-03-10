using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Stevebot_DB.DB_Content {
    public enum LoggingType {
        Info = 0,
        Warning = 1,
        Error = 2
    }
    internal class DataLogging {
        [Key]
        public int Id { get; set; }
        [Required]
        public required DateTime Time { get; set; }
        [Required]
        public required LoggingType Type { get; set; }
        [Required]
        [NotNull]
        public required string Message { get; set; } = "";
        public string AdditionalInfo { get; set; } = "";
    }
}
