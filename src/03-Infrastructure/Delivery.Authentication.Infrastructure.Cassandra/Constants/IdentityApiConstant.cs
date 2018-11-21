using System.Text;

namespace Delivery.Authentication.Infrastructure.Cassandra.Constants
{
    public static  class IdentityApiConstant
    {
        public static readonly string TABLE_NAME = "identityapi";

        public static readonly string COLUMNS_CODE = "code";
        public static readonly string COLUMNS_DESCRIPTION = "description";
        public static readonly string COLUMNS_CLAIMS = "claims";
    }
}
