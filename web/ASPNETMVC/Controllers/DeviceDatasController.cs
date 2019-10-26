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
using PagedList;
using PagedList.Mvc;


namespace WebAppMedTechTest.Controllers
{
    public class DeviceDatasController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        // GET: DeviceDatas
        public ActionResult Index(string searchString, int? page)
        {
            #region Default Code
            //var deviceDatas = db.DeviceDatas.Include(d => d.Device);
            //return View(deviceDatas.ToList());

            #endregion End of Default Code

            #region List
            //var _model = db.DeviceDatas.Include(d => d.Device);
            //var _viewModel = new List<DeviceDataViewModel>();

            //foreach (var item in _model)
            //{
            //    _viewModel.Add(DeviceDataViewModel.GetViewModelDatas(item));
            //}

            //return View(_viewModel);
            #endregion // end of List 

            #region LINQ Expression
            //var qData = from q in db.Devices.Include(d => d.DeviceDocument) select q;

            var qData = (from q in db.DeviceDatas.Include(d => d.Device) select q).OrderBy(s => s.DeviceID).Skip(0);
            if (!string.IsNullOrEmpty(searchString))
            {
                qData = qData.Where(s => s.Device.InventoryNumber.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var _viewModel = new List<DeviceDataViewModel>();
            foreach (var item in qData)
            {
                _viewModel.Add(DeviceDataViewModel.GetViewModelDatas(item));
            }

            return View("Index", _viewModel.ToPagedList(pageNumber, pageSize));
            //return View(_listViewModel.ToPagedList(pageNumber, pageSize));

            #endregion // end of LINQ Expression

        }

        // GET: DeviceDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DeviceData _model = db.DeviceDatas.Where(s=>s.DeviceID==id).FirstOrDefault();
            List<DeviceData> _model = db.DeviceDatas.Where(s => s.DeviceID == id).ToList();
            if (_model == null)
            {
                //return HttpNotFound();
                return View("ErrorView");
               
            }
            return View(_model);
        }

        // GET: DeviceDatas/Create
        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceData _model)
        {
            if (ModelState.IsValid)
            {
                db.DeviceDatas.Add(_model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);
            return View(_model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceData _model = db.DeviceDatas.Find(id);
            var _viewModel = DeviceDataViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);
            return View(_viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceDataViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                var _model = GetModelDatas(_viewModel);
                db.Entry(_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _viewModel.DeviceID);
            return View(_viewModel);
        }

        DeviceData GetModelDatas(DeviceDataViewModel _viewModel)
        {
            DeviceData _model = new DeviceData()
            {
                DeviceDataID = _viewModel.DeviceDataID,
                DeviceID = _viewModel.DeviceID,
                DatePuttingOperation = _viewModel.DatePuttingOperation,
                DateWithdrawalOperation = _viewModel.DateWithdrawalOperation,
                DateBeginningGuarantee = _viewModel.DateBeginningGuarantee,
                DateCompletionGuarantee = _viewModel.DateCompletionGuarantee,
                WarrantyDuration = _viewModel.WarrantyDuration,
                RepairType = _viewModel.RepairType,
                DateRequestRepair = _viewModel.DateRequestRepair,
                FaultsDescription = _viewModel.FaultsDescription,
                RepairDescription = _viewModel.RepairDescription,
                RepairParts = _viewModel.RepairParts,
                DateDelivery = _viewModel.DateDelivery,
                CompanyContractorDD = _viewModel.CompanyContractorDD,
                SurnameContractorDD = _viewModel.SurnameContractorDD,
                NameContractorDD = _viewModel.NameContractorDD,
                PatronymicContractorDD = _viewModel.PatronymicContractorDD,
                PhoneContractorDD = _viewModel.PhoneContractorDD,
                MobilePhoneContractorDD = _viewModel.MobilePhoneContractorDD,
                EmailContractorDD = _viewModel.EmailContractorDD,
                DeviceStatus = _viewModel.DeviceStatus,
                CertificateCompletionTechFileName = _viewModel.CertificateCompletionTechFileName,
                CertificateCompletionTechBinData = _viewModel.CertificateCompletionTechBinData,
                ClaimsServiceFileName = _viewModel.ClaimsServiceFileName,
                ClaimsServiceBinData = _viewModel.ClaimsServiceBinData,
                NoteDevData = _viewModel.NoteDevData,
                CreateDateTime = _viewModel.CreateDateTime,
                ChangeDateTime = _viewModel.ChangeDateTime,
                Device = _viewModel.Device
            };
            return _model;
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceData deviceData = db.DeviceDatas.Find(id);
            if (deviceData == null)
            {
                return HttpNotFound();
            }
            return View(deviceData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceData deviceData = db.DeviceDatas.Find(id);
            db.DeviceDatas.Remove(deviceData);
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
