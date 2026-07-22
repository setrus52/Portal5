using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum PersonContactType
{
    Undefined = 0,

    [Display(Name = "Внутренний телефон")]
    InternalPhone,

    [Display(Name = "Мобильный телефон")]
    MobilePhone,

    [Display(Name = "Городской телефон")]
    WorkPhone,

    [Display(Name = "Домашний телефон")]
    HomePhone,

    [Display(Name = "Офис телефон")]
    OfficePhone,

    [Display(Name = "Электронная почта")]
    Email,

    [Display(Name = "Microsoft Teams")]
    Teams,

    [Display(Name = "Telegram")]
    Telegram,

    [Display(Name = "WhatsApp")]
    WhatsApp,

    [Display(Name = "Skype")]
    Skype,

    [Display(Name = "Web")]
    Web
}