
namespace Delivery.Authentication.Infrastructure.Cassandra.Constants
{
    public static  class IdentityClientConstant
    {
        public static readonly string TABLE_NAME = "identityclient";

        public static readonly string COLUMNS_CLIENT_ID = "clientid";
        public static readonly string COLUMNS_CLIENT_DESCRIPTION = "clientdescription";
        public static readonly string COLUMNS_TIME_LIFE = "timelife";
        public static readonly string COLUMNS_GRANT_TYPES = "granttypes";
        public static readonly string COLUMNS_REQUIRE_CLIENT_SECRET = "requireclientsecret";
        public static readonly string COLUMNS_CLIENT_SECRET = "clientsecret";
        public static readonly string COLUMNS_ALLOW_ACCESS_IN_BROWSER = "allowaccessinbrowser";
        public static readonly string COLUMNS_SCOPES = "scopes";
        public static readonly string COLUMNS_REDIRECT_URIS = "redirecturis";
        public static readonly string COLUMNS_POST_LOGOUT_REDIRECT_URIS = "postlogoutredirecturis";
        public static readonly string COLUMNS_AUTHORITY = "authority";
        public static readonly string COLUMNS_ALLOW_OFFLINE_ACCESS = "allowofflineaccess";
        public static readonly string COLUMNS_CLAIMS = "claims";
    }
}
