using System.Data;

namespace Article.Application.Data
{
    public interface ISqlConnection
    {
        IDbConnection ConnectionBuild();
    }
}
