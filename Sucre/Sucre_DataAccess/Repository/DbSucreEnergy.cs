using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class DbSucreEnergy : DbSucre<Energy, int>, IDbSucreEnergy
    {
        private readonly ApplicationDbContext _db;

        public DbSucreEnergy(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        /// <summary>
        /// Получить список элементов объекта
        /// </summary>
        /// <param name="AddFirstSelect">Добавить первый элемент</param>
        /// <param name="valueFirstSelect">Название первого элемента</param>
        /// <returns>список элементов</returns>
        public IEnumerable<SelectListItem> GetAllDropdownList(bool addFirstSelect = true, string valueFirstSelect = null)
        {
            List<SelectListItem> returnValues = new List<SelectListItem>();
            SelectListItem value;
            //"--Select energy type--"
            if (addFirstSelect)
            {
                value = new SelectListItem(
                    text: (valueFirstSelect == null || valueFirstSelect.Trim() == "") ?
                            "--Select from the list--" :
                            valueFirstSelect,
                    value: "0",
                    selected: true,
                    disabled: true);

                returnValues.Add(value);
            }
            foreach (var item in dbSet)
            {
                value = new SelectListItem();
                string textValue = GetStringName(item);
                value.Text = textValue;
                value.Value = item.Id.ToString();
                returnValues.Add(value);
            };

            return returnValues;
        }

        /// <summary>
        /// Получить имя объекта из полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <returns>имя объекта</returns>
        public string GetStringName(object obj)
        {
            Energy energy = (Energy)obj;
            List<string> listText = new List<string>();
            if (energy.EnergyName != null && energy.EnergyName.Trim() != "")
                listText.Add(energy.EnergyName);
            return String.Join("->", listText.ToArray());
        }

        public void Update(Energy energy)
        {
            _db.Update(energy);
        }
        public async Task UpdateAsync(Energy energy)
        {
            await Task.Run(() => Update(energy));
        }
    }
}
