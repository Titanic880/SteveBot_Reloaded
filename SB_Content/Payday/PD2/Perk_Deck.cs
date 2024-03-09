using System.ComponentModel.DataAnnotations;


namespace SB_Content.Payday.PD2 {
    public enum Category {
        NaN = -1,
        Primary = 0,
        Secondary,
        Melee,
        Throwable,
        Armor,
        Deployable
    }

    public class ListObject {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string TableName { get; set; } = "";
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public bool HasOptional { get; set; } = false;
        public bool OptionalData { get; set; } = false;

        public string ExtendedInfo { get; set; } = "";
    }
    public class Perk_Deck
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public required string Name { get; set; }
        [Required]
        public required bool HasThrowable { get; set; }
        public string ThrowableName { get; set; } = "";
    }
    public class Equipment
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public required string Name { get; set; }
        [Required]
        public required Category EquipmentCategory  { get; set; }
    }
    public class Difficulty
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(15)]
        public required string Name { get; set; }
        public bool IsOneDown { get; set; }
    }
}
