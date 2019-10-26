using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppMedTechTest.Models;
using WebAppMedTechTest.ViewModel;

namespace WebAppMedTechTest.Controllers
{
    public class ContractorsController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        public ActionResult Index()
        {
            //return View(db.Contractors.ToList());
            #region List
            var _model = db.Contractors.Include(d => d.Devices);
            var _viewModel = new List<ContractorViewModel>();

            foreach (var item in _model)
            {
                _viewModel.Add(ContractorViewModel.GetViewModelDatas(item));
            }

            return View(_viewModel);
            #endregion // end of List 


        }

        // GET: Contractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = db.Contractors.Find(id);
            if (contractor == null)
            {
                //return HttpNotFound();
                return View("ErrorView");

            }
            return View(contractor);
        }

        // GET: Contractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractorID,DeviceID,OwnershipType,Company,SurnameContractPers,NameContractPers,PatronymicContractPers,PersonNumberContractPers,PositionContractPers,DepartmentContractPers,OfficeAdressCompanyContractPers,StockAdressCompanyContractPers,PhoneContractPers,InternalPnoneContractPers,MobilePhoneContractPers,EmailContractPers,SiteCompanyContractPers,NoteContractor,CreateDateTime,ChangeDateTime")] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                db.Contractors.Add(contractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contractor);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor _model = db.Contractors.Find(id);
            var _viewModel = ContractorViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            //ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);

            return View(_viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractorViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                var _model = GetModelDatas(_viewModel);
                db.Entry(_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _viewModel.DeviceID);

            return View(_viewModel);
        }

        Contractor GetModelDatas(ContractorViewModel _viewModel)
        {
            Contractor _model = new Contractor()
            {
                ContractorID = _viewModel.ContractorID,
                DeviceID = _viewModel.DeviceID,
                OwnershipType = _viewModel.OwnershipType.ToString(),
                Company = _viewModel.Company,
                SurnameContractPers = _viewModel.SurnameContractPers,
                NameContractPers = _viewModel.NameContractPers,
                PatronymicContractPers = _viewModel.PatronymicContractPers,
                PositionContractPers = _viewModel.PositionContractPers,
                DepartmentContractPers = _viewModel.DepartmentContractPers,
                OfficeAdressCompanyContractPers = _viewModel.OfficeAdressCompanyContractPers,
                StockAdressCompanyContractPers = _viewModel.StockAdressCompanyContractPers,
                PhoneContractPers = _viewModel.PhoneContractPers,
                InternalPnoneContractPers = _viewModel.InternalPnoneContractPers,
                MobilePhoneContractPers = _viewModel.MobilePhoneContractPers,
                EmailContractPers = _viewModel.EmailContractPers,
                SiteCompanyContractPers = _viewModel.SiteCompanyContractPers,
                NoteContractor = _viewModel.NoteContractor,
                CreateDateTime = _viewModel.CreateDateTime,
                ChangeDateTime = _viewModel.ChangeDateTime
            };
            return _model;
        }

        // GET: Contractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = db.Contractors.Find(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contractor contractor = db.Contractors.Find(id);
            db.Contractors.Remove(contractor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
