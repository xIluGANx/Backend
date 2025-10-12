var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ���������� ��������� ������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // �������� �� ���������������� �������� ������
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        // �������� �� �������� 404
        response.Redirect("/Error/404");
    }
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
