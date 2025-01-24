using Microsoft.AspNetCore.Mvc;

public abstract class BaseController : Controller
{
    protected void SetBreadcrumb(params (string Text, string Url)[] items)
    {
        ViewData["Breadcrumb"] = items.ToList();
    }
} 