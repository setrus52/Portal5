using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum Gender
{
    [Display(Name = "Женский")]
    Female = 0,

    [Display(Name = "Мужской")]
    Male = 1
}