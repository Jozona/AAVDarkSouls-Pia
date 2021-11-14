using AAVD.Entidades;
using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAVD.Base_de_datos
{
    class DatabaseManagement
    {   
        static public string cassandraHome { get; set; }
        static public string keyspace { get; set; }
        static private Cluster cluster;
        static private ISession session;
        static private DatabaseManagement _instance;

        private DatabaseManagement() {
            Connect();
            session = cluster.Connect(keyspace);
        }

        static public DatabaseManagement getInstance() {
            if (_instance == null) {
                _instance = new DatabaseManagement();
            }
            return _instance;
        }

        static private void Connect() {
            cassandraHome = ConfigurationManager.AppSettings["cassandra_home"].ToString();
            keyspace = ConfigurationManager.AppSettings["keyspace"].ToString();
            cluster = Cluster.Builder().AddContactPoint(cassandraHome).Build();
        }

        //Regresa en una lista a el usuario resultante de la query. 
        //Se tiene que utilizar el USER_NAME y PASSWORD, son una compund key en cql. Para una busqueda rapida
        //AUn no descubro como regresar el usuario solo, sin una lista. Pero esto jala.
        public List<Users> getLogin(string user_name, string password) {
            string query = String.Format("SELECT * FROM USERS_LOGIN WHERE USER_NAME='{0}' AND PASSWORD='{1}';", user_name, password);
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Users> users = mapper.Fetch<Users>(query);
            return users.ToList();

        }

        //Registra el usuario.
        //Hace falta registrarlo en la tabla general de USERS.
        public void registerUser(string username, string password, int type) {
            string query = String.Format("INSERT INTO USERS_LOGIN(USER_NAME, PASSWORD, USER_TYPE, ACTIVE, USER_ID)" +
                            " VALUES('{0}','{1}', {2}, true, uuid())", username, password, type);
            session.Execute(query);

        }

        //Registra a los empleados
        public void registerEmployee(string username, string password, string nombre, string apellidoPaterno, string apellidoMaterno, string CURP, string RFC, string nacimiento, Guid user_id)
        {

            string query2 = "INSERT INTO EMPLOYEES (USER_ID,USER, PASSWORD, NAME, LAST_NAME, MOTHER_LAST_NAME, CLAVES_UNICAS, CREATION_DATE, DATE_OF_BIRTH, MODIFICATION_DATE, EMPLOYEE_ID)"
                                        + " VALUES(" + user_id + ",'" + username + "', '" + password + "', '" + nombre + "', '" + apellidoPaterno + "', '" + apellidoMaterno + "', {'CURP' : '" + CURP + "', 'RFC' : '" + RFC + "' }, now(), '" + nacimiento + "', [toDate(now())], uuid());";
            session.Execute(query2);
        }

        //Enviar lista con todos los empleados
        public List<Employees> GetEmployees() {
            string query = "SELECT * FROM EMPLOYEES";
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Employees> employees = mapper.Fetch<Employees>(query);
            return employees.ToList();
        }

        //Actualizar un empleado
        public void updateEmployee(string username, string password, string nombre, string apellidoPaterno, string apellidoMaterno, string CURP, string RFC, string nacimiento, string employee_id)
        {
            string query2 = "UPDATE EMPLOYEES SET USER = '" + username + "', PASSWORD = '" + password + "', NAME = '" + nombre  + "', LAST_NAME = '" + apellidoPaterno + "', MOTHER_LAST_NAME = '" + apellidoMaterno + "', CLAVES_UNICAS = {'CURP' : '" + CURP + "', 'RFC' : '" + RFC + "'},  DATE_OF_BIRTH = '" + nacimiento + "', MODIFICATION_DATE = MODIFICATION_DATE + [todate(now())] WHERE EMPLOYEE_ID= " + employee_id + " "
                            + " IF EXISTS;";
            session.Execute(query2);
        }

        public void eraseEmployee(string employee_id, string user, string password) {
            string query = "DELETE FROM EMPLOYEES WHERE EMPLOYEE_ID = " + employee_id + ";";
            session.Execute(query);

            query = "DELETE FROM USERS_LOGIN WHERE USER_NAME = '" + user + "' AND PASSWORD = '" + password + "';";
            session.Execute(query);
        }
    }
}
