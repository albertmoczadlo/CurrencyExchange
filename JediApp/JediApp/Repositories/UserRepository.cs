using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public class UserRepository
    {
        private readonly string fileName = "..//..//..//..//Users.csv"; //może przeniść do klasy statycznej ???
        //public User AddUser (User user)
        //{
        //    var id = Guid.NewGuid();
        //    var registereduser = new User { Id = id, Login = user.Login, Password = user.Password, Role = user.Role };
        //    using (StreamWriter file = new StreamWriter(fileName, true))
        //    {
        //        file.WriteLine($"{id};{registereduser.Login};{registereduser.Password};{registereduser.Role};{registereduser.Wallet.Id}");
        //    }

        //    return registereduser; /*new User{ Id = id, Login = user.Login, Password = user.Password, Role = user.Role }*/;
        //}

        //public List<User> GetAllUsers()
        //{
        //    List<User> users = new List<User>();

        //    if (!File.Exists(fileName))
        //        return new List<User>();

        //    var usersFromFile = File.ReadAllLines(fileName);
        //    foreach (var line in usersFromFile)
        //    {
        //        var columns = line.Split(';');
        //        Guid.TryParse(columns[0], out var newGuid);
        //        Enum.TryParse(columns[3], out UserRole userRole);
        //        var wallet = new Wallet(columns[4]);
        //        users.Add(new User { Id = newGuid, Login = columns[1], Password = columns[2], Role = userRole, Wallet = wallet});
        //    }
        //    return users;
        //}

        //public User GetUserById(Guid id)
        //{
        //    List<User> users = GetAllUsers();

        //    return users.SingleOrDefault(x => x.Id == id);
        //}

        //public User GetUserByLogin(string login)
        //{
        //    List<User> users = GetAllUsers();

        //    return users.SingleOrDefault(x => x.Login.ToLowerInvariant().Contains(login.ToLowerInvariant()));
        //}

        //public List<User> BrowseUsers(string query)
        //{
        //    List<User> currencies = GetAllUsers();

        //    return currencies.Where(x => x.Login.ToLowerInvariant().Contains(query.ToLowerInvariant())).ToList();
        //}

        //public User GetLoginPassword(string login, string password)
        //{
        //    List<User> users = GetAllUsers();

            //var usersFromFile = File.ReadAllLines(fileName);

            //foreach (var line in usersFromFile)
            //{
            //    var columns = line.Split(';');
            //    Enum.TryParse(columns[3], out UserRole userRole);
            //    if (columns.Length == 5)
            //    users.Add(new User { Login = columns[1], Password = columns[2], Id = Guid.Parse(columns[0]),  Role = userRole });
            //}

        //    User user = users.FirstOrDefault(x => x.Login == login && x.Password == password );

        //    return user;
        //}
    }
}
