using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IWebHostEnvironment _webHostEnvironment;
    public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        return View(objCompanyList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Company obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Company.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }


    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        if (companyFromDb == null)
        {
            return NotFound();
        }
        return View(companyFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Company obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Company.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company updated successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        if (companyFromDb == null)
        {
            return NotFound();
        }
        return View(companyFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }
        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Company deleted successfully";
        return RedirectToAction("Index");
    }
}