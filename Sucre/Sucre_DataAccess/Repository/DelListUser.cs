using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository
{
    public static class DelListUser
    {
        private static List<AppUser> _listUser=new List<AppUser>();
        private static List<AppRole> _appRole=new List<AppRole>();
        
        static DelListUser()
        {

            List<AppUser> lst = new List<AppUser>()
            {
                new AppUser()
                {
                    Id = new Guid("DA9D33F9-27FF-4A44-81A4-61461D851B0B"),
                    Name = "admin",
                    Description = "User ASUTP",
                    Email = "admin@admin.him",
                    PasswordHash ="27A34DCEA5D00E4EE9C8301CC2D84383",

                    GroupNumber = 999,
                    AppRoles = new List<AppRole>()
                },
                new AppUser()
                {
                    Id = new Guid("87947224-749C-40E1-8B34-4129387FFAAD"),
                    Email = "user@user.him",
                    PasswordHash = "43B3788992A26B46532CC11C0563C46B",

                    GroupNumber = 99,
                    AppRoles = new List<AppRole>()
                }
            };

            _listUser.AddRange(lst);

            List<AppRole> rol = new List<AppRole>()
            {
                new AppRole() {
                 Id = new Guid("A8C7F069-7955-4A80-A942-6A31FA8B6A66"),
                 Name = "Admin",
                 Value = "ADMIN",
                 AppUsers = new List<AppUser>()
                },
                new AppRole()
                {
                    Id= new Guid("2825D298-2A63-42E4-AD77-012A150DF759"),
                    Name = "User",
                    Value = "USER",
                    AppUsers=new List<AppUser>()
                }
            };
            _appRole.AddRange(rol);

            _listUser[0].AppRoles.Add(_appRole.FirstOrDefault(item => item.Name == "Admin"));
            _appRole[0].AppUsers.Add(_listUser.FirstOrDefault(item => item.Email == "admin@admin.him"));
            _listUser[1].AppRoles.Add(_appRole.FirstOrDefault(item => item.Name == "User"));
            _appRole[1].AppUsers.Add(_listUser.FirstOrDefault(item => item.Email == "user@user.him"));
            var jjj = _listUser;

        }

        public static List<AppUser> ListUser 
        { 
            get {  return _listUser; } 
        }

        public static void AddInListUser(AppUser user)
        {
            _listUser.Add(user);
        }
    }
}
