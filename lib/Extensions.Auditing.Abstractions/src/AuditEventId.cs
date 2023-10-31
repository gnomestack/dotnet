namespace GnomeStack.Extensions.Auditing;

public struct AuditEventId
{
    public AuditEventId(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public static AuditEventId Error { get; } = new AuditEventId(1, "error");

    public static AuditEventId Add { get; } = new AuditEventId(2, "add");

    public static AuditEventId Update { get; } = new AuditEventId(3, "update");

    public static AuditEventId Delete { get; } = new AuditEventId(4, "delete");

    public static AuditEventId Query { get; } = new AuditEventId(5, "query");

    public static AuditEventId Login { get; } = new AuditEventId(6, "login");

    public static AuditEventId Logout { get; } = new AuditEventId(7, "logout");

    public static AuditEventId AccessDenied { get; } = new AuditEventId(8, "access-denied");

    public static AuditEventId UserAdded { get; } = new AuditEventId(9, "user-created");

    public static AuditEventId UserUpdated { get; } = new AuditEventId(10, "user-updated");

    public static AuditEventId UserRemoved { get; } = new AuditEventId(11, "user-removed");

    public static AuditEventId UserRoleAdded { get; } = new AuditEventId(12, "user-role-added");

    public static AuditEventId UserRoleRemoved { get; } = new AuditEventId(13, "user-role-removed");

    public int Id { get; }

    public string Name { get; set; } = string.Empty;
}