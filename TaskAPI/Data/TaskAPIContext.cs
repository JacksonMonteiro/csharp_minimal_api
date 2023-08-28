using System.Data;

namespace TaskAPI.Data {
    public class TaskAPIContext {
        public delegate Task<IDbConnection> GetConnection();
    }
}
