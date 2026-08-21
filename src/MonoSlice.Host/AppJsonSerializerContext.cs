using System.Text.Json.Serialization;
using MonoSlice.Modules.Catalog.Features.CreateProduct;
using MonoSlice.Modules.Catalog.Features.DeleteProduct;
using MonoSlice.Modules.Catalog.Features.GetProduct;
using MonoSlice.Modules.Catalog.Features.ListProducts;
using MonoSlice.Modules.Catalog.Features.UpdateProduct;
using MonoSlice.Modules.Users.Features.AssignRole;
using MonoSlice.Modules.Users.Features.GetProfile;
using MonoSlice.Modules.Users.Features.Login;
using MonoSlice.Modules.Users.Features.RefreshToken;
using MonoSlice.Modules.Users.Features.Register;
using MonoSlice.Shared.Abstractions.Common;

namespace MonoSlice.Host;

[JsonSerializable(typeof(ApiResponse))]
[JsonSerializable(typeof(ApiResponse<string>))]
[JsonSerializable(typeof(ApiResponse<UserResponseDto>))]
[JsonSerializable(typeof(ApiResponse<LoginResponseDto>))]
[JsonSerializable(typeof(ApiResponse<RefreshTokenResponseDto>))]
[JsonSerializable(typeof(ApiResponse<ProductDto>))]
[JsonSerializable(typeof(ApiResponse<PaginatedList<ProductDto>>))]
[JsonSerializable(typeof(PaginatedList<ProductDto>))]
[JsonSerializable(typeof(RegisterCommand))]
[JsonSerializable(typeof(UserResponseDto))]
[JsonSerializable(typeof(LoginCommand))]
[JsonSerializable(typeof(LoginResponseDto))]
[JsonSerializable(typeof(UserInfoDto))]
[JsonSerializable(typeof(RefreshTokenCommand))]
[JsonSerializable(typeof(RefreshTokenResponseDto))]
[JsonSerializable(typeof(AssignRoleCommand))]
[JsonSerializable(typeof(GetProfileQuery))]
[JsonSerializable(typeof(CreateProductCommand))]
[JsonSerializable(typeof(ProductDto))]
[JsonSerializable(typeof(UpdateProductCommand))]
[JsonSerializable(typeof(DeleteProductCommand))]
[JsonSerializable(typeof(GetProductQuery))]
[JsonSerializable(typeof(ListProductsQuery))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(IReadOnlyList<string>))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
