using System.Text.Json.Serialization;

namespace Suzan.Domain.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    User,
    Admin,
    Moderator
}