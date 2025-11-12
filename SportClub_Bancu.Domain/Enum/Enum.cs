using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.Domain.Enum
{
    public enum UserRole
    {
        [Display(Name = "Пользователь")]
        User = 0,

        [Display(Name = "Тренер")]
        Trainer = 1,

        [Display(Name = "Кладовщик")]
        Storekeeper = 2,

        [Display(Name = "Администратор")]
        Admin = 3
    }

    public enum OrderStatus
    {
        [Display(Name = "Запланировано")]
        Planned = 0,

        [Display(Name = "Выдано")]
        Issued = 1,

        [Display(Name = "Частично возвращено")]
        PartiallyReturned = 2,

        [Display(Name = "Возвращено")]
        Returned = 3,

        [Display(Name = "Просрочено")]
        Overdue = 4,

        [Display(Name = "Отменено")]
        Cancelled = 5
    }

    public enum InventoryCondition
    {
        [Display(Name = "Новое")]
        New = 0,

        [Display(Name = "Хорошее")]
        Good = 1,

        [Display(Name = "Удовлетворительное")]
        Fair = 2,

        [Display(Name = "Требует ремонта")]
        NeedsRepair = 3,

        [Display(Name = "Неисправно")]
        Broken = 4,

        [Display(Name = "Списано")]
        Scrapped = 5
    }
}
