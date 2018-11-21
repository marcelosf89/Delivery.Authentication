using System.Text;

namespace Delivery.Authentication.Infrastructure.Cassandra.Constants
{
    public static class UserConstant
    {
        public static readonly string TABLE_NAME = "users";

        public static string ALL_COLUMNS_EXCEPT_CLAIMS => new StringBuilder().AppendJoin(", ",
                                                            COLUMNS_ID, COLUMNS_CREATION, COLUMNS_DELETION_DATE,
                                                            COLUMNS_EMAIL, COLUMNS_FIRST_NAME, COLUMNS_LAST_ACCESS,
                                                            COLUMNS_LAST_NAME, COLUMNS_PHONE, COLUMNS_USERNAME, COLUMNS_CLAIMS).ToString();

        public static readonly string COLUMNS_ID = "id";
        public static readonly string COLUMNS_CREATION = "creation";
        public static readonly string COLUMNS_DELETION_DATE = "deletiondate";
        public static readonly string COLUMNS_EMAIL = "email";
        public static readonly string COLUMNS_FIRST_NAME = "firstname";
        public static readonly string COLUMNS_LAST_ACCESS = "lastaccess";
        public static readonly string COLUMNS_LAST_NAME = "lastname";
        public static readonly string COLUMNS_PHONE = "phone";
        public static readonly string COLUMNS_USERNAME = "username";
        public static readonly string COLUMNS_PASSWORD = "password";
        public static readonly string COLUMNS_CLAIMS = "claims";
    }
}
