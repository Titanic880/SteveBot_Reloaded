using System.ComponentModel.DataAnnotations;


namespace SB_Content.Payday.PD2
{
    public enum Category
    {
        Primary = 0,
        Secondary,
        Melee,
        Throwable,
        Armor,
        Deployable
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
