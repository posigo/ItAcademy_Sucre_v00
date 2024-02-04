using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Data;
using Microsoft.Extensions.Configuration;
using Sucre_DataAccess.Services.IServices;
using Microsoft.Extensions.Logging;
using Sucre_Core.LoggerExternal;
using Sucre_Utility;

namespace Sucre_DataAccess.Services
{
    public class InitApplicattionDbContext: IUtilApplicattionDbContext
    {
        private ApplicationDbContext _db;
        private IConfiguration _configuration;

        private List<AppRole> _appRoles;
        private List<AppUser> _appUsers;
        private List<AsPaz> _asPazs;
        private List<Canal> _cannales;
        private List<Cex> _cexs;
        private List<Energy> _energies;
        private List<GroupUser> _groupUsers;
        private List<ParameterType> _parameterTypes;
        private List<Point> _points;        
        
        //private readonly ILogger<InitApplicattionDbContext> _log;

        public InitApplicattionDbContext(ApplicationDbContext db, 
            IConfiguration configuration)//,
            //ILogger<InitApplicattionDbContext> log)
        {
            _db = db;
            _configuration = configuration;
            //_log = log;
        }

        /// <summary>
        /// Name of the connected database
        /// </summary>
        public string DatabaseName
        {
            get
            {
                try
                {
                    //var s = 0;
                    //var i = 1 / s;
                    //var connstr = _configuration.GetConnectionString("DefaultConnection").ToString();
                    //string strDatabase = connstr.Split(';').ToList().FirstOrDefault(item => item.Contains("Database"));
                    //string result = strDatabase.Split('=').Last().ToString();
                    string dbName = _configuration.GetConnectionString("DefaultConnection").ToString().
                            Split(';').ToList().FirstOrDefault(item => item.Contains("Database")).
                                Split('=').Last().ToString();

                    LoggerExternal.LoggerEx.Information("*->InitApplicattionDbContext->get DatabaseName");
                    //_log.LogInformation("InitApplicattionDbContext->get DatabaseName");
                    return dbName;
                }
                catch(Exception ex) 
                {
                    LoggerExternal.LoggerEx.Error(ex, "InitApplicattionDbContext->get DatabaseName ERROR!!!. Result EMPTY");
                    return string.Empty;
                }
            }

        }
        /// <summary>
        /// Set begin value to Db
        /// </summary>
        private void SetValueDb()
        {
            _parameterTypes = new List<ParameterType>()
            {
                new ParameterType()
                {
                    Name = "Давление",
                    Mnemo = "P",
                    UnitMeas = "МПа"
                },
                new ParameterType()
                {
                    Name = "Давление",
                    Mnemo = "P",
                    UnitMeas = "кПа"
                },
                new ParameterType()
                {
                    Name = "Давление",
                    Mnemo = "P",
                    UnitMeas = "bar"
                },
                new ParameterType()
                {
                    Name = "Температура",
                    Mnemo = "T",
                    UnitMeas = "°C"
                },
                new ParameterType()
                {
                    Name = "Температура",
                    Mnemo = "T",
                    UnitMeas = "K"
                },
                new ParameterType()
                {
                    Name = "Расход в м3",
                    Mnemo = "Q",
                    UnitMeas = "м3"
                },
                new ParameterType()
                {
                    Name = "Расход в тоннах",
                    Mnemo = "Q",
                    UnitMeas = "т"
                }
            };
            _db.ParameterTypes.AddRange(_parameterTypes);
            _energies = new List<Energy>()
            {
                new Energy()
                {
                    EnergyName = "Газ"
                },
                new Energy()
                {
                    EnergyName = "Вода"
                },
                new Energy()
                {
                    EnergyName = "Пар"
                },
                new Energy()
                {
                    EnergyName = "Азот"
                },
                new Energy()
                {
                    EnergyName = "Сжатый воздух"
                }
            };
            _db.Energies.AddRange(_energies);
            _cexs = new List<Cex>()
            {
                new Cex()
                {
                    CexName = "КОЦ и ЭГУ",
                    Device = "Установка генераторов"
                },
                new Cex()
                {
                    Management = "ОГЭ",
                    Device = "УООС"
                },
                new Cex()
                {
                    CexName = "ОВиТ",
                    Device = "БРВ"
                },
                new Cex()
                {
                    Management = "УАМИТ",
                    CexName = "ЦЭОПЕК",
                    Location = "Сан Узел №5"
                },
                new Cex()
                {
                    CexName = "КИПиА",
                    Area = "уч электронной лабаратории"
                }
            };
            _db.Cexs.AddRange(_cexs);
            _db.SaveChanges();
            _points = new List<Point>()
            {
                new Point()
                {
                    Name = "Учёт газа ЭГУ1",
                    Description = "Учёт расхода газа на электрогенераторной установке. Комерческий!",
                    EnergyId = _energies.FirstOrDefault(item => item.EnergyName == "Газ").Id,
                    Energy = _energies.FirstOrDefault(item => item.EnergyName == "Газ"),
                    CexId = _cexs.FirstOrDefault(item => item.CexName == "КОЦ и ЭГУ").Id,
                    Cex = _cexs.FirstOrDefault(item => item.CexName == "КОЦ и ЭГУ"),
                    ServiceStaff = "Цех КИПиА"
                },
                new Point()
                {
                    Name = "Учёт газа ЭГУ2",
                    EnergyId = _energies.FirstOrDefault(item => item.EnergyName == "Газ").Id,
                    Energy = _energies.FirstOrDefault(item => item.EnergyName == "Газ"),
                    CexId = _cexs.FirstOrDefault(item => item.CexName == "КОЦ и ЭГУ").Id,
                    Cex = _cexs.FirstOrDefault(item => item.CexName == "КОЦ и ЭГУ")

                },
                new Point()
                {
                    Name = "Учёт воды на ТП2",
                    EnergyId = _energies.FirstOrDefault(item => item.EnergyName == "Вода").Id,
                    Energy = _energies.FirstOrDefault(item => item.EnergyName == "Вода"),
                    CexId = _cexs.FirstOrDefault(item => item.CexName == "ОВиТ" && item.Device == "БРВ").Id,
                    Cex = _cexs.FirstOrDefault(item => item.CexName == "ОВиТ" && item.Device == "БРВ"),
                    ServiceStaff = "Служба ОВИТ"
                },
                new Point()
                {
                    Name = "Metering Test",
                    EnergyId = _energies.FirstOrDefault(item => item.EnergyName == "Сжатый воздух").Id,
                    Energy = _energies.FirstOrDefault(item => item.EnergyName == "Сжатый воздух"),
                    CexId = _cexs.FirstOrDefault(item => item.CexName == "КОЦ и ЭГУ").Id,
                    Cex = _cexs.FirstOrDefault(item => item.CexName == "КОЦ и ЭГУ")
                }
            };
            _db.Points.AddRange(_points);
            _cannales = new List<Canal>()
            {
                new Canal()
                {
                    Name = "Давление газа ЭГУ1",
                    Description = "TestCanalDescription",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Давление" && item.Mnemo == "P" && item.UnitMeas == "МПа").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Давление" && item.Mnemo == "P" && item.UnitMeas == "МПа"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = true
                },
                new Canal()
                {
                    Name = "Давление воды ТП2",
                    Description = "TestCanalDescr2",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Давление" && item.Mnemo == "P" && item.UnitMeas == "кПа").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Давление" && item.Mnemo == "P" && item.UnitMeas == "кПа"),
                    Reader = true,
                    SourceType = 1,
                    AsPazEin = true
                },
                new Canal()
                {
                    Name = "Давление газа ЭГУ2",
                    Description = "Descr3",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Давление" && item.Mnemo == "P" && item.UnitMeas == "МПа").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Давление" && item.Mnemo == "P" && item.UnitMeas == "МПа"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = true
                },
                new Canal()
                {
                    Name = "Температура газа на ЭГУ",
                    Description = "Температура газа на на общей магистрали ЭГУ",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Температура" && item.Mnemo == "T" && item.UnitMeas == "°C").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Температура" && item.Mnemo == "T" && item.UnitMeas == "°C"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = true
                },
                new Canal()
                {
                    Name = "Температура воды на ТП2",
                    Description = "Температура воды на ТП2",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Температура" && item.Mnemo == "T" && item.UnitMeas == "°C").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Температура" && item.Mnemo == "T" && item.UnitMeas == "°C"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = false
                },
                new Canal()
                {
                    Name = "Расход газа на ЭГУ1",
                    Description = "Расход газа на ЭГУ1",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Расход в м3" && item.Mnemo == "Q" && item.UnitMeas == "м3").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Расход в м3" && item.Mnemo == "Q" && item.UnitMeas == "м3"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = false
                },
                new Canal()
                {
                    Name = "Расход газа на ЭГУ2",
                    Description = "Расход газа на ЭГУ2",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Расход в м3" && item.Mnemo == "Q" && item.UnitMeas == "м3").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Расход в м3" && item.Mnemo == "Q" && item.UnitMeas == "м3"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = false
                },
                new Canal()
                {
                    Name = "Расход воды",
                    Description = "Расход воды на ТП2",
                    ParameterTypeId = _parameterTypes.FirstOrDefault(item => item.Name == "Расход в тоннах" && item.Mnemo == "Q" && item.UnitMeas == "т").Id,
                    ParameterType = _parameterTypes.FirstOrDefault(item => item.Name == "Расход в тоннах" && item.Mnemo == "Q" && item.UnitMeas == "т"),
                    Reader = true,
                    SourceType = 0,
                    AsPazEin = false
                }
            };
            _db.Canals.AddRange(_cannales);
            _db.SaveChanges();
            _asPazs = new List<AsPaz>()
            {
                new AsPaz()
                {
                    High = 100,
                    Low = 0,
                    A_HighEin = true,
                    A_HighType = false,
                    A_High = 95,
                    W_HighEin = true,
                    W_HighType = false,
                    W_High = 90,
                    W_LowEin = true,
                    W_LowType = true,
                    W_Low = 10,
                    A_LowEin = true,
                    A_LowType = false,
                    A_Low = 5,
                    CanalId = _cannales.FirstOrDefault(item => item.Name == "Давление воды ТП2").Id
                },
                new AsPaz()
                {
                    High = 50,
                    Low = 5,
                    A_HighEin = true,
                    A_HighType = false,
                    A_High = 45,
                    W_HighEin = false,
                    W_HighType = false,
                    W_High = 0,
                    W_LowEin = false,
                    W_LowType = false,
                    W_Low = 0,
                    A_LowEin = true,
                    A_LowType = false,
                    A_Low = 10,
                    CanalId = _cannales.FirstOrDefault(item => item.Name == "Давление газа ЭГУ2").Id
                },
                new AsPaz()
                {
                    High = 200,
                    Low = -50,
                    A_HighEin = true,
                    A_HighType = false,
                    A_High = 180,
                    W_HighEin = false,
                    W_HighType = false,
                    W_High = 0,
                    W_LowEin = false,
                    W_LowType = false,
                    W_Low = 0,
                    A_LowEin = false,
                    A_LowType = false,
                    A_Low = 0,
                    CanalId = _cannales.FirstOrDefault(item => item.Name == "Температура газа на ЭГУ").Id
                }
            };
            _db.AsPazs.AddRange(_asPazs);
            _db.SaveChanges();
            foreach (var asPaz in _asPazs)
            {
                Canal canal = _cannales.FirstOrDefault(item => item.Id == asPaz.CanalId);
                _cannales.IndexOf(canal);
                _cannales[_cannales.IndexOf(canal)].AsPaz = asPaz;
                asPaz.Canal = canal;
            }
            _db.UpdateRange(_asPazs);
            foreach (var paramType in _parameterTypes)
            {
                ICollection<Canal> cans = new HashSet<Canal>();
                cans = _cannales.Where(item => item.ParameterType == paramType).ToList();
                paramType.Canals = new HashSet<Canal>();
                foreach (var can in cans)
                {
                    paramType.Canals.Add(can);
                }
            }
            _db.ParameterTypes.UpdateRange(_parameterTypes);
            foreach (var energy in _energies)
            {
                ICollection<Point> pointss = new HashSet<Point>();
                pointss = _points.Where(item => item.Energy == energy).ToList();
                energy.Points = new HashSet<Point>();
                foreach (var ppoint in pointss)
                {
                    energy.Points.Add(ppoint);
                }
            }
            _db.Energies.UpdateRange(_energies);
            foreach (var cex in _cexs)
            {
                ICollection<Point> pointss = new HashSet<Point>();
                pointss = _points.Where(item => item.Cex == cex).ToList();
                cex.Points = new HashSet<Point>();
                foreach (var ppoint in pointss)
                {
                    cex.Points.Add(ppoint);
                }
            }
            _db.Cexs.UpdateRange(_cexs);
            _db.SaveChanges();
            //point 1
            Point pointCannales = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ1");
            Canal cannale = _cannales.FirstOrDefault(item => item.Name == "Давление газа ЭГУ1");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Температура газа на ЭГУ");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Расход газа на ЭГУ1");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Расход воды");
            pointCannales.Canals.Add(cannale);
            //point 2
            pointCannales = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ2");
            cannale = _cannales.FirstOrDefault(item => item.Name == "Давление газа ЭГУ2");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Температура газа на ЭГУ");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Расход газа на ЭГУ2");
            pointCannales.Canals.Add(cannale);
            //point 3
            pointCannales = _points.FirstOrDefault(item => item.Name == "Учёт воды на ТП2");
            cannale = _cannales.FirstOrDefault(item => item.Name == "Давление воды ТП2");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Температура воды на ТП2");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Расход воды");
            pointCannales.Canals.Add(cannale);
            //point 4
            pointCannales = _points.FirstOrDefault(item => item.Name == "Metering Test");
            cannale = _cannales.FirstOrDefault(item => item.Name == "Давление газа ЭГУ1");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Температура газа на ЭГУ");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Расход газа на ЭГУ2");
            pointCannales.Canals.Add(cannale);
            cannale = _cannales.FirstOrDefault(item => item.Name == "Расход воды");
            pointCannales.Canals.Add(cannale);
            _db.Points.UpdateRange(_points);
            _db.SaveChanges();
            //canale 1
            Canal cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Давление газа ЭГУ1");
            Point point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ1");
            cannalePoints.Points.Add(point);
            point = _points.FirstOrDefault(item => item.Name == "Metering Test");
            cannalePoints.Points.Add(point);
            //canale 2
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Давление воды ТП2");
            point = _points.FirstOrDefault(item => item.Name == "Учёт воды на ТП2");
            cannalePoints.Points.Add(point);
            //canale 3
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Давление газа ЭГУ2");
            point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ2");
            cannalePoints.Points.Add(point);
            //canale 4
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Температура газа на ЭГУ");
            point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ1");
            cannalePoints.Points.Add(point);
            point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ2");
            cannalePoints.Points.Add(point);
            point = _points.FirstOrDefault(item => item.Name == "Metering Test");
            cannalePoints.Points.Add(point);
            //canale 5
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Температура воды на ТП2");
            point = _points.FirstOrDefault(item => item.Name == "Учёт воды на ТП2");
            cannalePoints.Points.Add(point);
            //canale 6
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Расход газа на ЭГУ1");
            point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ1");
            cannalePoints.Points.Add(point);
            //canale 7
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Расход газа на ЭГУ2");
            point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ2");
            cannalePoints.Points.Add(point);
            point = _points.FirstOrDefault(item => item.Name == "Metering Test");
            cannalePoints.Points.Add(point);
            //canale 8
            cannalePoints = _cannales.FirstOrDefault(item => item.Name == "Расход воды");
            point = _points.FirstOrDefault(item => item.Name == "Учёт газа ЭГУ1");
            cannalePoints.Points.Add(point);
            point = _points.FirstOrDefault(item => item.Name == "Учёт воды на ТП2");
            cannalePoints.Points.Add(point);
            point = _points.FirstOrDefault(item => item.Name == "Metering Test");
            cannalePoints.Points.Add(point);
            _db.Canals.UpdateRange(_cannales);
            _db.SaveChanges();
            //add groupUser 999
            _groupUsers = new List<GroupUser>()
            {
                new GroupUser()
                {
                    Number = 999,
                    Description = "Доступ ко всем отчётам"
                },
                new GroupUser()
                {
                    Number = 99,
                    Description = "Нет доступа к отчётам"
                }
            };
            _db.GroupUsers.AddRange(_groupUsers);
            _db.SaveChanges();
            //add roles
            _appRoles = new List<AppRole>()
            {
                new AppRole()
                {
                    Name = "Supervisor",
                    Value = "SUPERVISOR"
                },
                new AppRole()
                {
                    Name = "Admin",
                    Value = "ADMIN"
                },
                new AppRole()
                {
                    Name = "User",
                    Value = "USER"
                },
                new AppRole()
                {
                    Name = "Guest",
                    Value = "GUEST"
                }
            };
            _db.AppRoles.AddRange(_appRoles);
            //add users
            _appUsers = new List<AppUser>()
            {
                new AppUser()
                {
                    Name = "Admin",
                    Description = "Test admin application",
                    Email = "admin@admin.him",
                    PasswordHash = WM.GenerateMD5Hash("admin",_configuration["AppSettings:PasswordSalt"]),
                    GroupNumber = 999
                    //GroupUser = _groupUsers.FirstOrDefault(item => item.Name == "999")
                },
                new AppUser()
                {
                    Name = "User",
                    Description = "Test user application",
                    Email = "user@user.him",
                    PasswordHash = WM.GenerateMD5Hash("user",_configuration["AppSettings:PasswordSalt"]),
                    GroupNumber = 99
                    //GroupUser = _groupUsers.FirstOrDefault(item => item.Name == "99")
                }
            };
            _db.AppUsers.AddRange(_appUsers);
            _db.SaveChanges();
            AppRole role = _appRoles.FirstOrDefault(item => item.Name == "Admin");
            AppUser user = _appUsers.FirstOrDefault(item => item.Email == "admin@admin.him");
            role.AppUsers.Add(user);
            user.AppRoles.Add(role);
            _db.AppRoles.Update(role);
            _db.AppUsers.Update(user);
            _db.SaveChanges();
            role = _appRoles.FirstOrDefault(item => item.Name == "User");
            user = _appUsers.FirstOrDefault(item => item.Email == "user@user.him");
            role.AppUsers.Add(user);
            user.AppRoles.Add(role);
            _db.AppRoles.Update(role);
            _db.AppUsers.Update(user);
            _db.SaveChanges();
        }
        /// <summary>
        /// Init DB begin value
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InitDbValue(out string errMsg)
        {
            var sds = _db.ContextId;
            var ddd = _db.Database;
            try
            {
                LoggerExternal.LoggerEx.Information("*->InitApplicattionDbContext->InitDbvalue...BEGIN");
                _db.Database.EnsureDeleted();
                _db.Database.EnsureCreated();
                LoggerExternal.LoggerEx.Information("*->InitApplicattionDbContext->InitDbvalue->SetValueDb...START");
                SetValueDb();
                LoggerExternal.LoggerEx.Information("*->InitApplicattionDbContext->InitDbvalue->SetValueDb...END. Result OK");
                errMsg = "";
                
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                LoggerExternal.LoggerEx.Error(ex, "*->InitApplicattionDbContext->InitDbvalue...ERROR!!!");
                return false;
            }
            LoggerExternal.LoggerEx.Information("*->InitApplicattionDbContext->InitDbvalue...END. Result OK (TRUE)");
            return true;

        }
    }
}
