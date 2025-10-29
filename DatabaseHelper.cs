using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WindowsFormsApp1
{
    internal class DatabaseHelper
    {
        private string connectionString = @"Data Source=adclg1;Initial Catalog=Комфорт_Иванова;Integrated Security=True";
        
        public bool ValidateUser(string login, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM usert WHERE Логин = @Login AND Пароль = @Password";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        public User GetUser(string login)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Роль_сотрудника, ФИО, Логин FROM usert WHERE Логин = @Login";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Role = SafeGetString(reader, 0),
                                FullName = SafeGetString(reader, 1),
                                Login = SafeGetString(reader, 2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void AddLoginHistory(string login, bool success)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO History (Логин, Время_входа, Успешно) VALUES (@Login, @LoginTime, @Success)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@LoginTime", DateTime.Now);
                        command.Parameters.AddWithValue("@Success", success);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка записи в историю: {ex.Message}");
            }
        }

        public DataTable GetProducts(string sortColumn = "Наименование_продукции", bool ascending = true)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string orderDirection = ascending ? "ASC" : "DESC";

                // Простая логика выбора колонки для сортировки
                string sortField = "Наименование_продукции";
                if (sortColumn == "Артикул") sortField = "Артикул";
                else if (sortColumn == "Тип") sortField = "Тип_продукции";

                string query = $@"
                    SELECT 
                        Тип_продукции, Наименование_продукции, Артикул, Минимальная_стоимость_для_партнера, Основной_материал
                    FROM Productst 
                    WHERE Наименование_продукции IS NOT NULL
                    ORDER BY {sortField} {orderDirection}";

                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public DataTable SearchProducts(string searchText, string sortColumn = "Наименование_продукции", bool ascending = true)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string orderDirection = ascending ? "ASC" : "DESC";

                string sortField = "Наименование_продукции";
                if (sortColumn == "Артикул") sortField = "Артикул";
                else if (sortColumn == "Тип") sortField = "Тип_продукции";

                string query = $@"
                    SELECT 
                        Тип_продукции, Наименование_продукции, Артикул, Минимальная_стоимость_для_партнера, Основной_материал
                    FROM Productst 
                    WHERE (Наименование_продукции LIKE @Search OR 
                           Тип_продукции LIKE @Search OR
                           Артикул LIKE @Search OR
                           Основной_материал LIKE @Search)
                    AND Наименование_продукции IS NOT NULL
                    ORDER BY {sortField} {orderDirection}";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Search", $"%{searchText}%");

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public List<LoginHistoryRecord> GetLoginHistory(string sortBy = "Время_входа", bool ascending = false)
        {
            var history = new List<LoginHistoryRecord>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string orderDirection = ascending ? "ASC" : "DESC";

                    // Простой выбор колонки для сортировки
                    string orderColumn = sortBy == "Логин" ? "Логин" : "Время_входа";

                    string query = $@"
                        SELECT Время_входа, Логин, Успешно 
                        FROM History 
                        ORDER BY {orderColumn} {orderDirection}";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new LoginHistoryRecord
                            {
                                LoginTime = SafeGetDateTime(reader, 0),
                                Login = SafeGetString(reader, 1),
                                Success = SafeGetBoolean(reader, 2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка чтения истории: {ex.Message}");
            }

            return history;
        }

        public Product GetProductById(string article)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Тип_продукции, Наименование_продукции, Артикул, Минимальная_стоимость_для_партнера, Основной_материал
                    FROM Productst
                    WHERE Артикул = @Article";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Article", article);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                Type = SafeGetString(reader, 0),
                                Name = SafeGetString(reader, 1),
                                Article = SafeGetString(reader, 2),
                                Price = SafeGetDecimal(reader, 3),
                                Material = SafeGetString(reader, 4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool AddProduct(Product product)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Productst (
                            Тип_продукции, Наименование_продукции, Артикул, Минимальная_стоимость_для_партнера, Основной_материал
                        ) VALUES (@Type, @Name, @Article, @Price, @Material)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Type", product.Type ?? "");
                        command.Parameters.AddWithValue("@Name", product.Name ?? "");
                        command.Parameters.AddWithValue("@Article", product.Article ?? "");
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Material", product.Material ?? "");

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка добавления товара: {ex.Message}");
                return false;
            }
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        UPDATE Productst SET 
                            Тип_продукции = @Type,
                            Наименование_продукции = @Name,
                            Минимальная_стоимость_для_партнера = @Price,
                            Основной_материал = @Material
                        WHERE Артикул = @Article";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Type", product.Type ?? "");
                        command.Parameters.AddWithValue("@Name", product.Name ?? "");
                        command.Parameters.AddWithValue("@Article", product.Article ?? "");
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Material", product.Material ?? "");

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления товара: {ex.Message}");
                return false;
            }
        }

        public bool DeleteProduct(string article)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Простая проверка существования товара
                    string checkQuery = "SELECT COUNT(*) FROM Productst WHERE Артикул = @Article";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Article", article);
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count == 0) return false;
                    }

                    // Удаляем товар
                    string deleteQuery = "DELETE FROM Productst WHERE Артикул = @Article";
                    using (var command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Article", article);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления товара: {ex.Message}");
                return false;
            }
        }

        public string GetNextArticle()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MAX(TRY_CAST(Артикул AS BIGINT)) FROM Productst WHERE TRY_CAST(Артикул AS BIGINT) IS NOT NULL";

                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && long.TryParse(result.ToString(), out long maxArticle))
                    {
                        return (maxArticle + 1).ToString();
                    }
                    return "1000000";
                }
            }
        }

        public DataTable GetWorkshops()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Название_цеха, Тип_цеха, Количество_человек_для_производства FROM Workshopst WHERE Название_цеха IS NOT NULL";

                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public DataTable GetProductTypes()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Тип_продукции, Коэффициент_типа_продукции FROM Product_typet WHERE Тип_продукции IS NOT NULL";

                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
        public bool CreateOrder(Order order)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Applications (
                            Номер_заявки,
                            Логин,
                            Артикул,
                            Наименование_товара,
                            Статус,
                            Дата_создания,
                            Дата_изменения
                        ) VALUES (@OrderNumber, @ClientLogin, @ProductArticle, @ProductName, @Status, @CreateDate, @UpdateDate)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                        command.Parameters.AddWithValue("@ClientLogin", order.ClientLogin);
                        command.Parameters.AddWithValue("@ProductArticle", order.ProductArticle);
                        command.Parameters.AddWithValue("@ProductName", order.ProductName);
                        command.Parameters.AddWithValue("@Status", order.Status);
                        command.Parameters.AddWithValue("@CreateDate", order.CreateDate);
                        command.Parameters.AddWithValue("@UpdateDate", order.UpdateDate);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка создания заявки: {ex.Message}");
                return false;
            }
        }

        public List<Order> GetClientOrders(string clientLogin)
        {
            var orders = new List<Order>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            Id,
                            Номер_заявки,
                            Логин,
                            Артикул,
                            Наименование_товара,
                            Статус,
                            Дата_создания,
                            Дата_изменения
                        FROM Applications 
                        WHERE Логин = @ClientLogin
                        ORDER BY Дата_создания DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientLogin", clientLogin);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new Order
                                {
                                    Id = SafeGetInt(reader, 0),
                                    OrderNumber = SafeGetString(reader, 1),
                                    ClientLogin = SafeGetString(reader, 2),
                                    ProductArticle = SafeGetString(reader, 3),
                                    ProductName = SafeGetString(reader, 4),
                                    Status = SafeGetString(reader, 5),
                                    CreateDate = SafeGetDateTime(reader, 6),
                                    UpdateDate = SafeGetDateTime(reader, 7)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка получения заявок: {ex.Message}");
            }

            return orders;
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            Id,
                            Номер_заявки,
                            Логин,
                            Артикул,
                            Наименование_товара,
                            Статус,
                            Дата_создания,
                            Дата_изменения
                        FROM Applications 
                        ORDER BY Дата_создания DESC";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                Id = SafeGetInt(reader, 0),
                                OrderNumber = SafeGetString(reader, 1),
                                ClientLogin = SafeGetString(reader, 2),
                                ProductArticle = SafeGetString(reader, 3),
                                ProductName = SafeGetString(reader, 4),
                                Status = SafeGetString(reader, 5),
                                CreateDate = SafeGetDateTime(reader, 6),
                                UpdateDate = SafeGetDateTime(reader, 7)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка получения всех заявок: {ex.Message}");
            }

            return orders;
        }

        public bool UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        UPDATE Applications SET 
                            Статус = @Status,
                            Дата_изменения = @UpdateDate
                        WHERE Id = @OrderId";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@OrderId", orderId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления статуса заявки: {ex.Message}");
                return false;
            }
        }

        public bool CancelOrder(int orderId, string clientLogin)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        UPDATE Applications SET 
                            Статус = 'Отменена',
                            Дата_изменения = @UpdateDate
                        WHERE Id = @OrderId AND Логин = @ClientLogin AND Статус IN ('Создана', 'На согласовании')";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@ClientLogin", clientLogin);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка отмены заявки: {ex.Message}");
                return false;
            }
        }

        public string GenerateOrderNumber()
        {
            return "ORD-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
        private string SafeGetString(SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return string.Empty;

            try
            {
                return reader.GetString(columnIndex);
            }
            catch (InvalidCastException)
            {
                // Если тип не string, пробуем преобразовать в string
                return reader.GetValue(columnIndex)?.ToString() ?? string.Empty;
            }
        }

        private int SafeGetInt(SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return 0;

            try
            {
                return reader.GetInt32(columnIndex);
            }
            catch (InvalidCastException)
            {
                // Если тип не int, пробуем преобразовать
                var value = reader.GetValue(columnIndex);
                if (value != null && int.TryParse(value.ToString(), out int result))
                    return result;
                return 0;
            }
        }

        private decimal SafeGetDecimal(SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return 0;

            try
            {
                return reader.GetDecimal(columnIndex);
            }
            catch (InvalidCastException)
            {
                // Если тип не decimal, пробуем преобразовать
                var value = reader.GetValue(columnIndex);
                if (value != null && decimal.TryParse(value.ToString(), out decimal result))
                    return result;
                return 0;
            }
        }

        private DateTime SafeGetDateTime(SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return DateTime.Now;

            try
            {
                return reader.GetDateTime(columnIndex);
            }
            catch (InvalidCastException)
            {
                return DateTime.Now;
            }
        }

        private bool SafeGetBoolean(SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return false;

            try
            {
                return reader.GetBoolean(columnIndex);
            }
            catch (InvalidCastException)
            {
                var value = reader.GetValue(columnIndex);
                if (value != null && bool.TryParse(value.ToString(), out bool result))
                    return result;
                return false;
            }
        }
    }

    public class User
    {
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsAdmin => Role == "Администратор";
        public bool IsManager => Role == "Менеджер";
        public bool IsClient => Role == "Авторизированный клиент";
        public bool CanViewOrders => IsAdmin || IsManager || IsClient;
    }

    public class LoginHistoryRecord
    {
        public DateTime LoginTime { get; set; }
        public string Login { get; set; }
        public bool Success { get; set; }
        public string Status => Success ? "Успешно" : "Ошибка";
    }

    public class Product
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public decimal Price { get; set; }
        public string Material { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ClientLogin { get; set; }
        public string ProductArticle { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string StatusDisplay
        {
            get
            {
                if (Status == "Создана") return "Создана";
                else if (Status == "На согласовании") return "На согласовании";
                else if (Status == "Ожидает предоплаты") return "Ожидает предоплаты";
                else if (Status == "В производстве") return "В производстве";
                else if (Status == "Готово к отгрузке") return "Готово к отгрузке";
                else if (Status == "Выполнена") return "Выполнена";
                else if (Status == "Отменена") return "Отменена";
                else return Status;
            }
        }
    }
}
