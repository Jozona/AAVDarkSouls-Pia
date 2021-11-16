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

        //Conseguir la pregunta para recordar
        public List<Users> getRemember(string user_name)
        {
            string query = String.Format("SELECT * FROM USERS_REMEMBER WHERE USER_NAME='{0}';", user_name);
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Users> users = mapper.Fetch<Users>(query);
            return users.ToList();

        }

        //Desactivar un usuario
        public void userBan(string user_name)
        {
            string query = String.Format("UPDATE USERS_REMEMBER SET ACTIVE = false WHERE USER_NAME = '{0}' IF EXISTS;", user_name);
            session.Execute(query);

            query = String.Format("SELECT * FROM USERS_REMEMBER WHERE USER_NAME='{0}';", user_name);
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Users> users = mapper.Fetch<Users>(query);
            string password = "";
            foreach (var usuario in users) {
                password = usuario.password;
            }

            query = String.Format("UPDATE USERS_LOGIN SET ACTIVE = false WHERE USER_NAME = '{0}' AND PASSWORD = '{1}' IF EXISTS;", user_name, password);
            session.Execute(query);
        }

        //Volver a activar un usuario
        public void userUnban(string user_name)
        {
            string query = String.Format("UPDATE USERS_REMEMBER SET ACTIVE = true WHERE USER_NAME = '{0}' IF EXISTS;", user_name);
            session.Execute(query);

            query = String.Format("SELECT * FROM USERS_REMEMBER WHERE USER_NAME='{0}';", user_name);
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Users> users = mapper.Fetch<Users>(query);
            string password = "";
            foreach (var usuario in users)
            {
                password = usuario.password;
            }

            query = String.Format("UPDATE USERS_LOGIN SET ACTIVE = true WHERE USER_NAME = '{0}' AND PASSWORD = '{1}' IF EXISTS;", user_name, password);
            session.Execute(query);
        }

        //Registra el usuario.
        //Hace falta registrarlo en la tabla general de USERS.
        public bool registerUser(string username, string password, int type, string pregunta, string respuesta) {
            string queryValidar = "SELECT COUNT(*) FROM USERS_REMEMBER WHERE USER_NAME ='"+username+"';";
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Existentes> existe = mapper.Fetch<Existentes>(queryValidar);
            foreach (var count in existe) {
                if (count.count == 1) {

                    return false;
                }
            }

            string query = String.Format("INSERT INTO USERS_LOGIN(USER_NAME, PASSWORD, USER_TYPE, ACTIVE, USER_ID, QUESTION, ANSWER)" +
                            " VALUES('{0}','{1}', {2}, true, uuid(),'" + pregunta + "','" + respuesta + "')", username, password, type);
            session.Execute(query);
            query = String.Format("INSERT INTO USERS_REMEMBER(USER_NAME, PASSWORD, USER_TYPE, ACTIVE, USER_ID, QUESTION, ANSWER)" +
                            " VALUES('{0}','{1}', {2}, true, uuid(), '"+pregunta+"','"+respuesta+"')", username, password, type);
            session.Execute(query);
            return true;
        }

        //Registra a los empleados
        public void registerEmployee(string username, string password, string nombre, string apellidoPaterno, string apellidoMaterno, string CURP, string RFC, string nacimiento, Guid user_id, string pregunta, string respuesta)
        {

            string query2 = "INSERT INTO EMPLOYEES (USER_ID,USER, PASSWORD, NAME, LAST_NAME, MOTHER_LAST_NAME, CLAVES_UNICAS, CREATION_DATE, DATE_OF_BIRTH, MODIFICATION_DATE, EMPLOYEE_ID, QUESTION, ANSWER)"
                                        + " VALUES(" + user_id + ",'" + username + "', '" + password + "', '" + nombre + "', '" + apellidoPaterno + "', '" + apellidoMaterno + "', {'CURP' : '" + CURP + "', 'RFC' : '" + RFC + "' }, now(), '" + nacimiento + "', [toDate(now())], uuid(), '"+pregunta+"', '"+respuesta+"');";
            session.Execute(query2);


        }

        //Recordar empleados

        //Enviar lista con todos los empleados
        public List<Employees> GetEmployees() {
            string query = "SELECT * FROM EMPLOYEES";
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Employees> employees = mapper.Fetch<Employees>(query);
            return employees.ToList();
        }

        //Actualizar un empleado
        public void updateEmployee(string username, string password, string nombre, string apellidoPaterno, string apellidoMaterno, string CURP, string RFC, string nacimiento, string employee_id, string pregunta, string respuesta)
        {
            string query2 = "UPDATE EMPLOYEES SET USER = '" + username + "', PASSWORD = '" + password + "', NAME = '" + nombre  + "', LAST_NAME = '" + apellidoPaterno + "', MOTHER_LAST_NAME = '" + apellidoMaterno + "', CLAVES_UNICAS = {'CURP' : '" + CURP + "', 'RFC' : '" + RFC + "'},  DATE_OF_BIRTH = '" + nacimiento + "', QUESTION = '"+pregunta+"', ANSWER = '"+respuesta+"' ,MODIFICATION_DATE = MODIFICATION_DATE + [todate(now())] WHERE EMPLOYEE_ID= " + employee_id + " "
                            + " IF EXISTS;";
            session.Execute(query2);
        }

        //Borrar un empleado
        public void eraseEmployee(string employee_id, string user, string password) {
            string query = "DELETE FROM EMPLOYEES WHERE EMPLOYEE_ID = " + employee_id + ";";
            session.Execute(query);

            query = "DELETE FROM USERS_LOGIN WHERE USER_NAME = '" + user + "' AND PASSWORD = '" + password + "';";
            session.Execute(query);
        }

        //Registrar un usuario
        public void registerClient(string nombre, string apellidoP, string apellidoM, string email, string CURP, string genero, string nacimiento, string ciudad, string calle, string colonia, string estado, string tipoContrato, string usuario, string password, Guid user_id) {
            string today = DateTime.Today.ToString();
            string query2 = "INSERT INTO CLIENTS (CLIENT_ID, USER_ID, CONTRACT_TYPE, CREATION_DATE, MODIFICATION_TIMES, MONTHLY_PAYMENTS, EMAIL, GENDER, MEASURER, SERVICE_NUMBER, CONTRACTS,CURP, USER, PASSWORD, NAME, LAST_NAME, MOTHER_LAST_NAME, AUTHOR, STREET, COLONY, CITY, STATE, DATE_OF_BIRTH)"
                             + " VALUES(uuid() ," + user_id + ", '"+tipoContrato+ "', now(), { toDate(now()) }, {'" + today + "' : 13.0 } , '"+email+"', '"+genero+"', 0, 0, {"+user_id+" : '"+tipoContrato+ "'}, '"+CURP+"', '"+usuario+"', '"+password+"', '"+nombre+"', '"+apellidoP+"', '"+apellidoM+"', "+Form1.currentUserId+", '"+calle+"', '"+colonia+"', '"+ciudad+"', '"+estado+"', '"+nacimiento+"');";
            session.Execute(query2);
        }

        //Obtener todos los clientes
        public List<Clientes> GetClients()
        {
            string query = "SELECT * FROM CLIENTS";
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Clientes> clientes = mapper.Fetch<Clientes>(query);
            return clientes.ToList();
        }

        //Actualizar un cliente
        public void updateClient(string nombre, string apellidoP, string apellidoM, string email, string CURP, string genero, string nacimiento, string ciudad, string calle, string colonia, string estado, string tipoContrato, string usuario, string password, string id_cliente)
        {
            string query2 = "UPDATE CLIENTS SET USER = '"+usuario+"', PASSWORD = '"+password+"', NAME= '"+nombre+"' ,LAST_NAME = '"+apellidoP+"', MOTHER_LAST_NAME= '"+apellidoM+"', EMAIL = '"+email+"', CURP = '"+CURP+"', GENDER = '"+genero+"', DATE_OF_BIRTH = '"+nacimiento+"', CITY= '"+ciudad+"', STREET = '"+calle+"', COLONY ='"+colonia+"', STATE= '"+estado+"', CONTRACT_TYPE= '"+tipoContrato+"' WHERE CLIENT_ID= "+ id_cliente +" "
                            + " IF EXISTS;";
            session.Execute(query2);
        }

        //Funcion para borrar un cliente
        public void eraseClient(string client_id, string user, string password)
        {
            string query = "DELETE FROM CLIENTS WHERE CLIENT_ID = " + client_id + ";";
            session.Execute(query);

            query = "DELETE FROM USERS_LOGIN WHERE USER_NAME = '" + user + "' AND PASSWORD = '" + password + "';";
            session.Execute(query);
        }


        //Definir las tarifas:
        public void crearTarifa(string tipo, string basica, string intermedia, string excedente) {

            string query2 = "INSERT INTO TARIFAS(TIPO, BASICO, INTERMEDIO, EXCEDENTE)" + " VALUES('" + tipo + "', " + basica + ", " + intermedia + ", " + excedente + ")";
            session.Execute(query2);
        }

        //Obetener tarifas
        public List<Tarifas> GetTarifas()
        {
            string query = "SELECT * FROM TARIFAS";
            session = cluster.Connect(keyspace);
            IMapper mapper = new Mapper(session);
            IEnumerable<Tarifas> tarifas = mapper.Fetch<Tarifas>(query);
            return tarifas.ToList();
        }


    }
}
