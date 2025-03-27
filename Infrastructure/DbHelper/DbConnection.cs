namespace Infrastructure.DbHelper;

public abstract class DbConnection<T> 
{
   public abstract T OpenConnection();
}