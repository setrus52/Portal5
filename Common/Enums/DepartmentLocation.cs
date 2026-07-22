using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum DepartmentLocation
{
    Unknown = 0,

    [Display(Name = "Вышестоящий")]
    Superior = 1,

    [Display(Name = "Текущий")]
    Current = 2,

    [Display(Name = "Подчинённый")]
    Subordinate = 3
}