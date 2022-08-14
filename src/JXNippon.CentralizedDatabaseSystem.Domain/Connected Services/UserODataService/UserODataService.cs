﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generation date: 14/8/2022 2:34:00 PM
namespace UserODataService.Affra.Service.User.Domain.Users
{
    /// <summary>
    /// There are no comments for UserSingle in the schema.
    /// </summary>
    public partial class UserSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<User>
    {
        /// <summary>
        /// Initialize a new UserSingle object.
        /// </summary>
        public UserSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new UserSingle object.
        /// </summary>
        public UserSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new UserSingle object.
        /// </summary>
        public UserSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<User> query)
            : base(query) {}

    }
    /// <summary>
    /// There are no comments for User in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class User : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new User object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="createdDateTime">Initial value of CreatedDateTime.</param>
        /// <param name="status">Initial value of Status.</param>
        /// <param name="xmin">Initial value of xmin.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public static User CreateUser(global::System.Guid ID, global::System.DateTimeOffset createdDateTime, global::UserODataService.Affra.Service.User.Domain.Users.UserStatus status, long xmin)
        {
            User user = new User();
            user.Id = ID;
            user.CreatedDateTime = createdDateTime;
            user.Status = status;
            user.xmin = xmin;
            return user;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::System.Guid Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::System.Guid _Id;
        partial void OnIdChanging(global::System.Guid value);
        partial void OnIdChanged();
        /// <summary>
        /// There are no comments for Property Username in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                this.OnUsernameChanging(value);
                this._Username = value;
                this.OnUsernameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Username;
        partial void OnUsernameChanging(string value);
        partial void OnUsernameChanged();
        /// <summary>
        /// There are no comments for Property Email in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this.OnEmailChanging(value);
                this._Email = value;
                this.OnEmailChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Email;
        partial void OnEmailChanging(string value);
        partial void OnEmailChanged();
        /// <summary>
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// There are no comments for Property CreatedDateTime in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::System.DateTimeOffset CreatedDateTime
        {
            get
            {
                return this._CreatedDateTime;
            }
            set
            {
                this.OnCreatedDateTimeChanging(value);
                this._CreatedDateTime = value;
                this.OnCreatedDateTimeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::System.DateTimeOffset _CreatedDateTime;
        partial void OnCreatedDateTimeChanging(global::System.DateTimeOffset value);
        partial void OnCreatedDateTimeChanged();
        /// <summary>
        /// There are no comments for Property Role in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                this.OnRoleChanging(value);
                this._Role = value;
                this.OnRoleChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Role;
        partial void OnRoleChanging(string value);
        partial void OnRoleChanged();
        /// <summary>
        /// There are no comments for Property Personalization in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Personalization
        {
            get
            {
                return this._Personalization;
            }
            set
            {
                this.OnPersonalizationChanging(value);
                this._Personalization = value;
                this.OnPersonalizationChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Personalization;
        partial void OnPersonalizationChanging(string value);
        partial void OnPersonalizationChanged();
        /// <summary>
        /// There are no comments for Property Status in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::UserODataService.Affra.Service.User.Domain.Users.UserStatus Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this.OnStatusChanging(value);
                this._Status = value;
                this.OnStatusChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::UserODataService.Affra.Service.User.Domain.Users.UserStatus _Status;
        partial void OnStatusChanging(global::UserODataService.Affra.Service.User.Domain.Users.UserStatus value);
        partial void OnStatusChanged();
        /// <summary>
        /// There are no comments for Property xmin in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public long xmin
        {
            get
            {
                return this._xmin;
            }
            set
            {
                this.OnxminChanging(value);
                this._xmin = value;
                this.OnxminChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private long _xmin;
        partial void OnxminChanging(long value);
        partial void OnxminChanged();
    }
    /// <summary>
    /// There are no comments for UserActivitySingle in the schema.
    /// </summary>
    public partial class UserActivitySingle : global::Microsoft.OData.Client.DataServiceQuerySingle<UserActivity>
    {
        /// <summary>
        /// Initialize a new UserActivitySingle object.
        /// </summary>
        public UserActivitySingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new UserActivitySingle object.
        /// </summary>
        public UserActivitySingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new UserActivitySingle object.
        /// </summary>
        public UserActivitySingle(global::Microsoft.OData.Client.DataServiceQuerySingle<UserActivity> query)
            : base(query) {}

    }
    /// <summary>
    /// There are no comments for UserActivity in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class UserActivity : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new UserActivity object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="createdDateTime">Initial value of CreatedDateTime.</param>
        /// <param name="activityType">Initial value of ActivityType.</param>
        /// <param name="xmin">Initial value of xmin.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public static UserActivity CreateUserActivity(long ID, global::System.DateTimeOffset createdDateTime, global::UserODataService.Affra.Service.User.Domain.Users.ActivityType activityType, long xmin)
        {
            UserActivity userActivity = new UserActivity();
            userActivity.Id = ID;
            userActivity.CreatedDateTime = createdDateTime;
            userActivity.ActivityType = activityType;
            userActivity.xmin = xmin;
            return userActivity;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// There are no comments for Property Username in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                this.OnUsernameChanging(value);
                this._Username = value;
                this.OnUsernameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Username;
        partial void OnUsernameChanging(string value);
        partial void OnUsernameChanged();
        /// <summary>
        /// There are no comments for Property Site in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string Site
        {
            get
            {
                return this._Site;
            }
            set
            {
                this.OnSiteChanging(value);
                this._Site = value;
                this.OnSiteChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _Site;
        partial void OnSiteChanging(string value);
        partial void OnSiteChanged();
        /// <summary>
        /// There are no comments for Property IpAddress in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public string IpAddress
        {
            get
            {
                return this._IpAddress;
            }
            set
            {
                this.OnIpAddressChanging(value);
                this._IpAddress = value;
                this.OnIpAddressChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private string _IpAddress;
        partial void OnIpAddressChanging(string value);
        partial void OnIpAddressChanged();
        /// <summary>
        /// There are no comments for Property CreatedDateTime in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::System.DateTimeOffset CreatedDateTime
        {
            get
            {
                return this._CreatedDateTime;
            }
            set
            {
                this.OnCreatedDateTimeChanging(value);
                this._CreatedDateTime = value;
                this.OnCreatedDateTimeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::System.DateTimeOffset _CreatedDateTime;
        partial void OnCreatedDateTimeChanging(global::System.DateTimeOffset value);
        partial void OnCreatedDateTimeChanged();
        /// <summary>
        /// There are no comments for Property ActivityType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::UserODataService.Affra.Service.User.Domain.Users.ActivityType ActivityType
        {
            get
            {
                return this._ActivityType;
            }
            set
            {
                this.OnActivityTypeChanging(value);
                this._ActivityType = value;
                this.OnActivityTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::UserODataService.Affra.Service.User.Domain.Users.ActivityType _ActivityType;
        partial void OnActivityTypeChanging(global::UserODataService.Affra.Service.User.Domain.Users.ActivityType value);
        partial void OnActivityTypeChanged();
        /// <summary>
        /// There are no comments for Property xmin in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public long xmin
        {
            get
            {
                return this._xmin;
            }
            set
            {
                this.OnxminChanging(value);
                this._xmin = value;
                this.OnxminChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private long _xmin;
        partial void OnxminChanging(long value);
        partial void OnxminChanged();
    }
    /// <summary>
    /// There are no comments for UserStatus in the schema.
    /// </summary>
    public enum UserStatus
    {
        Active = 0,
        Suspended = 1,
        SoftDelete = 2
    }
    /// <summary>
    /// There are no comments for ActivityType in the schema.
    /// </summary>
    public enum ActivityType
    {
        Login = 0,
        Logout = 1
    }
    /// <summary>
    /// Class containing all extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get an entity of type global::UserODataService.Affra.Service.User.Domain.Users.User as global::UserODataService.Affra.Service.User.Domain.Users.UserSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::UserODataService.Affra.Service.User.Domain.Users.UserSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.User> _source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::UserODataService.Affra.Service.User.Domain.Users.UserSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::UserODataService.Affra.Service.User.Domain.Users.User as global::UserODataService.Affra.Service.User.Domain.Users.UserSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::UserODataService.Affra.Service.User.Domain.Users.UserSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.User> _source,
            global::System.Guid id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::UserODataService.Affra.Service.User.Domain.Users.UserSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::UserODataService.Affra.Service.User.Domain.Users.UserActivity as global::UserODataService.Affra.Service.User.Domain.Users.UserActivitySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::UserODataService.Affra.Service.User.Domain.Users.UserActivitySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.UserActivity> _source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::UserODataService.Affra.Service.User.Domain.Users.UserActivitySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::UserODataService.Affra.Service.User.Domain.Users.UserActivity as global::UserODataService.Affra.Service.User.Domain.Users.UserActivitySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::UserODataService.Affra.Service.User.Domain.Users.UserActivitySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.UserActivity> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::UserODataService.Affra.Service.User.Domain.Users.UserActivitySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, keys)));
        }
    }
}
namespace UserODataService.Default
{
    /// <summary>
    /// There are no comments for Container in the schema.
    /// </summary>
    public partial class Container : global::Microsoft.OData.Client.DataServiceContext
    {
        /// <summary>
        /// Initialize a new Container object.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public Container(global::System.Uri serviceRoot) : 
                base(serviceRoot, global::Microsoft.OData.Client.ODataProtocolVersion.V4)
        {
            this.ResolveName = new global::System.Func<global::System.Type, string>(this.ResolveNameFromType);
            this.ResolveType = new global::System.Func<string, global::System.Type>(this.ResolveTypeFromName);
            this.OnContextCreated();
            this.Format.LoadServiceModel = GeneratedEdmModel.GetInstance;
            this.Format.UseJson();
        }
        partial void OnContextCreated();
        /// <summary>
        /// Since the namespace configured for this service reference
        /// in Visual Studio is different from the one indicated in the
        /// server schema, use type-mappers to map between the two.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        protected global::System.Type ResolveTypeFromName(string typeName)
        {
            global::System.Type resolvedType = this.DefaultResolveType(typeName, "Affra.Service.User.Domain.Users", "UserODataService.Affra.Service.User.Domain.Users");
            if ((resolvedType != null))
            {
                return resolvedType;
            }
            resolvedType = this.DefaultResolveType(typeName, "Default", "UserODataService.Default");
            if ((resolvedType != null))
            {
                return resolvedType;
            }
            return null;
        }
        /// <summary>
        /// Since the namespace configured for this service reference
        /// in Visual Studio is different from the one indicated in the
        /// server schema, use type-mappers to map between the two.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        protected string ResolveNameFromType(global::System.Type clientType)
        {
            if (clientType.Namespace.Equals("UserODataService.Affra.Service.User.Domain.Users", global::System.StringComparison.Ordinal))
            {
                return string.Concat("Affra.Service.User.Domain.Users.", clientType.Name);
            }
            if (clientType.Namespace.Equals("UserODataService.Default", global::System.StringComparison.Ordinal))
            {
                return string.Concat("Default.", clientType.Name);
            }
            return clientType.FullName;
        }
        /// <summary>
        /// There are no comments for User in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.User> User
        {
            get
            {
                if ((this._User == null))
                {
                    this._User = base.CreateQuery<global::UserODataService.Affra.Service.User.Domain.Users.User>("User");
                }
                return this._User;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.User> _User;
        /// <summary>
        /// There are no comments for UserActivity in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.UserActivity> UserActivity
        {
            get
            {
                if ((this._UserActivity == null))
                {
                    this._UserActivity = base.CreateQuery<global::UserODataService.Affra.Service.User.Domain.Users.UserActivity>("UserActivity");
                }
                return this._UserActivity;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::UserODataService.Affra.Service.User.Domain.Users.UserActivity> _UserActivity;
        /// <summary>
        /// There are no comments for User in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public void AddToUser(global::UserODataService.Affra.Service.User.Domain.Users.User user)
        {
            base.AddObject("User", user);
        }
        /// <summary>
        /// There are no comments for UserActivity in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        public void AddToUserActivity(global::UserODataService.Affra.Service.User.Domain.Users.UserActivity userActivity)
        {
            base.AddObject("UserActivity", userActivity);
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
        private abstract class GeneratedEdmModel
        {
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
            private static global::Microsoft.OData.Edm.IEdmModel ParsedModel = LoadModelFromString();
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
            private const string Edmx = @"<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""Affra.Service.User.Domain.Users"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""User"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Guid"" Nullable=""false"" />
        <Property Name=""Username"" Type=""Edm.String"" />
        <Property Name=""Email"" Type=""Edm.String"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <Property Name=""CreatedDateTime"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""Role"" Type=""Edm.String"" />
        <Property Name=""Personalization"" Type=""Edm.String"" />
        <Property Name=""Status"" Type=""Affra.Service.User.Domain.Users.UserStatus"" Nullable=""false"" />
        <Property Name=""xmin"" Type=""Edm.Int64"" Nullable=""false"" />
      </EntityType>
      <EntityType Name=""UserActivity"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Int64"" Nullable=""false"" />
        <Property Name=""Username"" Type=""Edm.String"" />
        <Property Name=""Site"" Type=""Edm.String"" />
        <Property Name=""IpAddress"" Type=""Edm.String"" />
        <Property Name=""CreatedDateTime"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""ActivityType"" Type=""Affra.Service.User.Domain.Users.ActivityType"" Nullable=""false"" />
        <Property Name=""xmin"" Type=""Edm.Int64"" Nullable=""false"" />
      </EntityType>
      <EnumType Name=""UserStatus"">
        <Member Name=""Active"" Value=""0"" />
        <Member Name=""Suspended"" Value=""1"" />
        <Member Name=""SoftDelete"" Value=""2"" />
      </EnumType>
      <EnumType Name=""ActivityType"">
        <Member Name=""Login"" Value=""0"" />
        <Member Name=""Logout"" Value=""1"" />
      </EnumType>
    </Schema>
    <Schema Namespace=""Default"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"">
        <EntitySet Name=""User"" EntityType=""Affra.Service.User.Domain.Users.User"" />
        <EntitySet Name=""UserActivity"" EntityType=""Affra.Service.User.Domain.Users.UserActivity"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
            public static global::Microsoft.OData.Edm.IEdmModel GetInstance()
            {
                return ParsedModel;
            }
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
            private static global::Microsoft.OData.Edm.IEdmModel LoadModelFromString()
            {
                global::System.Xml.XmlReader reader = CreateXmlReader(Edmx);
                try
                {
                    global::System.Collections.Generic.IEnumerable<global::Microsoft.OData.Edm.Validation.EdmError> errors;
                    global::Microsoft.OData.Edm.IEdmModel edmModel;
                    
                    if (!global::Microsoft.OData.Edm.Csdl.CsdlReader.TryParse(reader, false, out edmModel, out errors))
                    {
                        global::System.Text.StringBuilder errorMessages = new global::System.Text.StringBuilder();
                        foreach (var error in errors)
                        {
                            errorMessages.Append(error.ErrorMessage);
                            errorMessages.Append("; ");
                        }
                        throw new global::System.InvalidOperationException(errorMessages.ToString());
                    }

                    return edmModel;
                }
                finally
                {
                    ((global::System.IDisposable)(reader)).Dispose();
                }
            }
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "2.7.0")]
            private static global::System.Xml.XmlReader CreateXmlReader(string edmxToParse)
            {
                return global::System.Xml.XmlReader.Create(new global::System.IO.StringReader(edmxToParse));
            }
        }
    }
}