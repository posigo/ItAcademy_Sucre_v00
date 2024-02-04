using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface ISelectListItemObj
    {
        /// <summary>
        /// Получить список элементов объекта
        /// </summary>
        /// <param name="AddFirstSelect">Добавить первый элемент</param>
        /// <param name="valueFirstSelect">Название первого элемента</param>
        /// <returns>список элементов</returns>
        IEnumerable<SelectListItem> GetAllDropdownList(bool addFirstSelect = true, string valueFirstSelect = null);

        /// <summary>
        /// Получить имя объекта из полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <returns>имя объекта</returns>
        string GetStringName(object obj);

    }
}
